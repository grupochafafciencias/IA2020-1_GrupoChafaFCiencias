using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Controlador : MonoBehaviour
{
    private Actuadores actuador;
    private Sensores sensor;
    private int sensorPared;
    void Start(){
        actuador = GetComponent<Actuadores>();
        sensor = GetComponent<Sensores>();
        sensorPared=-1;
    }
    void FixedUpdate(){
        if(sensor.Bateria() <= 0){
			Debug.Log("Bateria murio");
			return;
		}
        actuador.Flotar();
        if(Input.GetKey(KeyCode.V))actuador.Ascender();
        if(Input.GetKey(KeyCode.B))actuador.Descender();
        if(Input.GetAxis("Vertical") > 0)actuador.Adelante();
        if(Input.GetAxis("Vertical") < 0)actuador.Atras();
        if(Input.GetAxis("Horizontal") > 0)actuador.Derecha();
        if(Input.GetAxis("Horizontal") < 0)actuador.Izquierda();
        if(sensor.TocandoPared())Debug.Log("Tocando pared!");
        sensorPared=sensor.CercaDePared();
        if(sensorPared!=-1){
			Debug.Log(sensorPared+" Esta cerca de una parad!");
			sensorPared=-1;	
		}
    }
}
