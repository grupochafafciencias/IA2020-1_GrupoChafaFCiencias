using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Controlador : MonoBehaviour
{
    private Actuadores actuador;
    public float distanceFromObject; //Distancia esperada entre el dron y el objeto. No siempre puede ser garantizada
    private Transform playerObject; //Coordenadas del sujeto stalkeado. Si quieren ponerse sus monos, esto podria ir en una clase que contenga al otro sujeto.
    public GameObject objetoASeguir; //El sujeto en persona
    public Transform baseDeRecarga;
    private Sensores sensor; //Arreglo que contiene los sensores. En teoria, deberia de poder ser facilmente modificable para agregar mas o demas, por el momento son solo 6.
    private List<int> sensorPared;
    private List<int> bloqueados;
    private List<int> disponibles;
    private float distancia;//Distancia real entre dron y a seguir.
    private Vector3 diferencias;
    private bool haciaDonde;
    private List<int> lastDequed;
    private List<char> candidateToMute;
    /*Inicializa el controlador del dron.
     * */
    void Start(){
		distanceFromObject=2.0f;
		playerObject=objetoASeguir.GetComponent<Transform>();
        actuador = GetComponent<Actuadores>();
        sensor = GetComponent<Sensores>();
   		bloqueados=new List<int>();
   		lastDequed=new List<int>();
   		candidateToMute=new List<char>();
   		haciaDonde=true;
    }
    /**
     * Permite cambiar al sujeto que se esta siguiendo,de esta forma podemos simplemente cambiar entre la base de carga y 
     * el sujeto a seguir, sin necesidad de cambiar tanto. 
     * */
    void cambiaVista(){
			transform.parent=playerObject;
			Transform tmp=playerObject;
			playerObject=baseDeRecarga;
			baseDeRecarga=tmp;
			haciaDonde=!haciaDonde;
			if(haciaDonde){
				distanceFromObject=1.0f;}
			else distanceFromObject=2.0f;
	}
	/**
	 * Metodo que se llama cada frame. 
	 * */
    void FixedUpdate(){
		float bateri=sensor.Bateria();
        if(bateri <= 0){
			Debug.Log("Bateria murio");
			return;
		}
        if(bateri <= 450 &&haciaDonde)cambiaVista(); //Si la bateria llega a la mitad, entonces cambia su objetivo a la base de carga
		actuador.Detener();
        diferencias = playerObject.position - transform.position; //Obtiene vector de diferencia entre nuestra posicion y nuestro objetivo.
        if(sensor.TocandoPared()){
			Debug.Log("Choco con pared");
		}
		seguimientoCiego(diferencias);
		actuador.Flotar();
    }
    /**
     * Remueve una cara de la lista de bloqueado, mantiendo un registro de cual fue. 
     * */
    void removeAt(int index){
		lastDequed.Add(bloqueados[0]);
		bloqueados.RemoveAt(0);
	}
	/**
	 * Metodo principal. Checa si puede avanzar en su direccion, desbloquea segun sea posible y conveniente.
	 * */
    void seguimientoCiego(Vector3 lookOnObject){
		distancia=Vector3.Distance(playerObject.position,transform.position);
        if(distancia <=distanceFromObject){
			if(!haciaDonde){ //Si fuimos a cargar bateria, ya llegamos, cargamos y cambiamos vista.
				actuador.CargarBateria();
				cambiaVista();
			}
			return; //No necesitamos alterar nada, solo flota.
		}
        evitaMuros(); //Checa cuales caras estan disponibles para moverse en ese eje.
       if(bloqueados.Count!=0){
	        int toDeque=bloqueados[0];
				if(bloqueados.Contains(0)){
					if(!bloqueados.Contains(1)){
						actuador.Ascender();
						lookOnObject.y=1;
						if(candidateToMute.Contains('y')) mute(1,0);
						if(lastDequed.Contains(1)) candidateToMute.Add('y');
						if(toDeque==0)removeAt(0);
					}
				}else{
					actuador.Descender();
					if(candidateToMute.Contains('y')) mute(0,1);
					if(lastDequed.Contains(1)) candidateToMute.Add('y');
					if(toDeque==1)removeAt(0);
				}
				if(bloqueados.Contains(5)){
					if(!bloqueados.Contains(4)){
						actuador.Izquierda();
						if(candidateToMute.Contains('x')) mute(4,5);
						if(lastDequed.Contains(1)) candidateToMute.Add('x');
						if(toDeque==5)removeAt(0);
					}
				}else{
					actuador.Derecha();
					if(candidateToMute.Contains('x')) mute(5,4);
					if(lastDequed.Contains(1)) candidateToMute.Add('x');
					if(toDeque==4)removeAt(0);
				}
				if(bloqueados.Contains(3)){
					if(!bloqueados.Contains(2)){
						actuador.Atras();
						if(candidateToMute.Contains('z')) mute(2,3);
						if(lastDequed.Contains(1)) candidateToMute.Add('z');
						if(toDeque==3)removeAt(0);}
				}else{
					actuador.Adelante();
					if(candidateToMute.Contains('z')) mute(3,2);
					if(lastDequed.Contains(1)) candidateToMute.Add('z');
					if(toDeque==2)removeAt(0);
				}
		}
		//Si es conveniente se mueva en esa direccion, lo hace.
        if(lookOnObject.y>0){
			if(disponibles.Contains(1))bloqueados.Remove(1);
			if(!bloqueados.Contains(1))actuador.Ascender();}
        if(lookOnObject.y<0){
			if(disponibles.Contains(0))bloqueados.Remove(0);
			if(!bloqueados.Contains(0))actuador.Descender();}
        if(lookOnObject.z>0){
			if(disponibles.Contains(3))bloqueados.Remove(3);
			if(!bloqueados.Contains(3))actuador.Adelante();}
        if(lookOnObject.z<0){
			if(disponibles.Contains(2))bloqueados.Remove(2);
			if(!bloqueados.Contains(2))actuador.Atras();}
        if(lookOnObject.x>0){
			if(disponibles.Contains(5))bloqueados.Remove(5);
			if(!bloqueados.Contains(5))actuador.Derecha();}
        if(lookOnObject.x<0){
			if(disponibles.Contains(4))bloqueados.Remove(4);
			if(!bloqueados.Contains(4))actuador.Izquierda();}
	}
	void mute(int a, int b){
			if(!bloqueados.Contains(a))bloqueados.Add(a);
			if(!bloqueados.Contains(b))bloqueados.Add(b);
	}
	void evitaMuros(){
		sensorPared=sensor.CercaDePared();
		disponibles=new List<int>(){0,1,2,3,4,5};
		foreach(int sID in sensorPared){
			if(!bloqueados.Contains(sID))bloqueados.Add(sID);
			disponibles.Remove(sID);
		}
	}
}
