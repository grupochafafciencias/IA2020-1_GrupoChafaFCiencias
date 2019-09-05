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
    private bool preferencia;
    private bool haciaDonde;
    private int lastDequed;
    private char candidateToMute;
    void Start(){
		distanceFromObject=2.0f;
		playerObject=objetoASeguir.GetComponent<Transform>();
        actuador = GetComponent<Actuadores>();
        sensor = GetComponent<Sensores>();
   		bloqueados=new List<int>();
   		haciaDonde=true;
        preferencia=false;
    }
    void cambiaVista(){
			transform.parent=playerObject;
			Transform tmp=playerObject;
			playerObject=baseDeRecarga;
			baseDeRecarga=tmp;
			haciaDonde=!haciaDonde;
	}
    void FixedUpdate(){
		float bateri=sensor.Bateria();
        if(bateri <= 0){
			Debug.Log("Bateria murio");
			return;
		}
        if(bateri <= 50 &&haciaDonde)cambiaVista();
		actuador.Detener();
        diferencias = playerObject.position - transform.position; //Obtiene vector de diferencia entre nuestra posicion y nuestro objetivo.
        if(sensor.TocandoPared()){
			Debug.Log("Choco con pared");
		}
		seguimientoCiego(diferencias);
		actuador.Flotar();
    }
    void removeAt(int index){
		lastDequed=bloqueados[0];
		bloqueados.RemoveAt(0);
	}
    void seguimientoCiego(Vector3 lookOnObject){
		distancia=Vector3.Distance(playerObject.position,transform.position);
        if(distancia <=distanceFromObject)return;
        evitaMuros();
        toString(bloqueados);
       if(bloqueados.Count!=0&&preferencia){
	        int toDeque=bloqueados[0];
				if(bloqueados.Contains(0)){
					if(!bloqueados.Contains(1)){
						actuador.Ascender();
						if(candidateToMute=='y') mute(1,0);
						if(lastDequed==1) candidateToMute='y';
						if(toDeque==0)removeAt(0);
					}
				}else{
					actuador.Descender();
					if(candidateToMute=='y') mute(0,1);
					if(lastDequed==0) candidateToMute='y';
					if(toDeque==1)removeAt(0);
				}
				if(bloqueados.Contains(5)){
					if(!bloqueados.Contains(4)){
						actuador.Izquierda();
						if(candidateToMute=='x') mute(4,5);
						if(lastDequed==5) candidateToMute='x';
						if(toDeque==5)removeAt(0);
					}
				}else{
					actuador.Derecha();
					if(candidateToMute=='x') mute(5,4);
					if(lastDequed==4) candidateToMute='x';
					if(toDeque==4)removeAt(0);
				}
				if(bloqueados.Contains(3)){
					if(!bloqueados.Contains(2)){
						actuador.Atras();
						if(candidateToMute=='z') mute(2,3);
						if(lastDequed==3) candidateToMute='z';
						if(toDeque==3)removeAt(0);}
				}else{
					actuador.Adelante();
					if(candidateToMute=='z') mute(3,2);
					if(lastDequed==2) candidateToMute='z';
					if(toDeque==2)removeAt(0);
				}
		}
		else{
        if(lookOnObject.y>0){
			if(disponibles.Contains(1))bloqueados.Remove(1);
			if(!bloqueados.Contains(1))actuador.Ascender();		}
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
        preferencia=!preferencia;
	}
	void mute(int a, int b){
			if(!bloqueados.Contains(a))bloqueados.Add(a);
			if(!bloqueados.Contains(b))bloqueados.Add(b);
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
	void toString(List<int> foo){
			string string1="";
			foreach(int ssd in foo){
				string1+=ssd+",";
			}
			Debug.Log(string1);
	}
}
