using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Controlador : MonoBehaviour
{
    private Actuadores actuador;
    public float distanceFromObject; //Distancia esperada entre el dron y el objeto. No siempre puede ser garantizada
    private Transform playerObject; //Coordenadas del sujeto stalkeado. Si quieren ponerse sus monos, esto podria ir en una clase que contenga al otro sujeto.
    public GameObject objetoASeguir; //El sujeto en persona
    private Sensores sensor; //Arreglo que contiene los sensores. En teoria, deberia de poder ser facilmente modificable para agregar mas o demas, por el momento son solo 6.
    private List<int> sensorPared;
    private List<int> bloqueados;
    private float distancia;//Distancia real entre dron y a seguir.
    private Vector3 diferencias;
    private int preferencia;
    void Start(){
		distanceFromObject=2.0f;
		playerObject=objetoASeguir.GetComponent<Transform>();
        actuador = GetComponent<Actuadores>();
        sensor = GetComponent<Sensores>();
   		bloqueados=new List<int>();
    }
    void FixedUpdate(){
        if(sensor.Bateria() <= 0){
			Debug.Log("Bateria murio");
			return;
		}
		actuador.Detener();
        diferencias = playerObject.position - transform.position; //Obtiene vector de diferencia entre nuestra posicion y nuestro objetivo.
        if(sensor.TocandoPared()){
			Debug.Log("Valio merga");
		}
		seguimientoCiego(diferencias);
		actuador.Flotar();
    }
    void seguimientoCiego(Vector3 lookOnObject){
		distancia=Vector3.Distance(playerObject.position,transform.position);
        if(distancia <=distanceFromObject)return;
        evitaMuros();
        toString(bloqueados);
        if(bloqueados.Count!=0){
	        int toDeque=bloqueados[0];
				if(bloqueados.Contains(0)){
					if(!bloqueados.Contains(1)){
						actuador.Ascender();
						if(toDeque==0)bloqueados.RemoveAt(0);
					}
				}else{
					actuador.Descender();
					if(toDeque==1)bloqueados.RemoveAt(0);
				}
				if(bloqueados.Contains(5)){
					if(!bloqueados.Contains(4)){
						actuador.Izquierda();
						if(toDeque==5)bloqueados.RemoveAt(0);
					}
				}else{
					actuador.Derecha();
					if(toDeque==4)bloqueados.RemoveAt(0);
				}
				if(bloqueados.Contains(3)){
					if(!bloqueados.Contains(2)){
						actuador.Atras();
						if(toDeque==3)bloqueados.RemoveAt(0);
					}
				}else{
					actuador.Adelante();
					if(toDeque==2)bloqueados.RemoveAt(0);
				}
		}
        if(lookOnObject.y>0&&(!bloqueados.Contains(1)))actuador.Ascender();
        if(lookOnObject.y<0&&(!bloqueados.Contains(0)))actuador.Descender();
        if(lookOnObject.z>0&&(!bloqueados.Contains(3)))actuador.Adelante();
        if(lookOnObject.z<0&&(!bloqueados.Contains(2)))actuador.Atras();
        if(lookOnObject.x>0&&(!bloqueados.Contains(5)))actuador.Derecha();
        if(lookOnObject.x<0&&(!bloqueados.Contains(4)))actuador.Izquierda();
	}
	void evitaMuros(){
		sensorPared=sensor.CercaDePared();
		foreach(int sID in sensorPared){
			if(!bloqueados.Contains(sID))bloqueados.Add(sID);
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
