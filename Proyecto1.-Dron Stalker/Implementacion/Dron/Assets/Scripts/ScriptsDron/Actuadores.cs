using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actuadores : MonoBehaviour
{
    private Rigidbody rb; 
    private Bateria bateria; 
    private Sensores sensor; 
    public float speed; 
	public Vector3 zeroV;
	private Vector3 velocidad;
    void Start(){
		speed=2.0f;
		zeroV=new Vector3(0,0.2f,0);
        rb = GetComponent<Rigidbody>();
        sensor = GetComponent<Sensores>();
        bateria = GameObject.Find("Bateria").gameObject.GetComponent<Bateria>();
    }
    /**
     * Va incrementado partes de un vector de velocidad. A excepcion de flotar, por lo que siempre tiene que estar flotando el objeto. 
     * */
    public void Ascender(){
   		velocidad+=Vector3.up*speed;
   	}
    public void Descender(){
		velocidad+=Vector3.down*speed;
	}
    public void Flotar(){
        velocidad+=zeroV;
        rb.velocity=velocidad;
    }
    public void Adelante(){
		velocidad+=Vector3.forward*speed;
	}
    public void Atras(){
		velocidad+=Vector3.back*speed;
	}
    public void Derecha(){
		velocidad+=Vector3.right*speed;
	}
    public void Izquierda(){
		velocidad+=Vector3.left;
	}
    public void Detener(){
        velocidad=Vector3.zero;
    }
    public void CargarBateria(){
        bateria.Cargar();
    }
}
