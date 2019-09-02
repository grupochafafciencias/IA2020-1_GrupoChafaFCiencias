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
    private int sensorPared;
    private float distancia;//Distancia real entre dron y a seguir.
    private Vector3 diferencias;
    void Start(){
		distanceFromObject=3.0f;
		playerObject=objetoASeguir.GetComponent<Transform>();
        actuador = GetComponent<Actuadores>();
        sensor = GetComponent<Sensores>();
        sensorPared=-1;
    }
    void FixedUpdate(){
        if(sensor.Bateria() <= 0){
			Debug.Log("Bateria murio");
			return;
		}
        actuador.Flotar(); //El dron inicialmente esta flotando.
        diferencias = playerObject.position - transform.position; //Obtiene vector de diferencia entre nuestra posicion y nuestro objetivo.
        seguimientoCiego(diferencias);
        sensorPared=sensor.CercaDePared();
        if(sensorPared!=-1){
			Debug.Log(sensorPared+" Esta cerca de una pared!");
			sensorPared=-1;	
		}
    }
    void seguimientoCiego(Vector3 lookOnObject){
		distancia=Vector3.Distance(playerObject.position,transform.position);
        if(distancia <=distanceFromObject)return; //Nada que hacer, esta en rango
        if(lookOnObject.y>0)actuador.Ascender();
        if(lookOnObject.y<0)actuador.Descender();
        if(lookOnObject.z>0)actuador.Adelante();
        if(lookOnObject.z<0)actuador.Atras();
        if(lookOnObject.x>0)actuador.Derecha();
        if(lookOnObject.x<0)actuador.Izquierda();
	}
}
