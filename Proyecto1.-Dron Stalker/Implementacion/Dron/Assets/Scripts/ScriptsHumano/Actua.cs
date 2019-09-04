using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actua : MonoBehaviour
{
	private Rigidbody rb; 
    public float speed; 
	private Vector3 zeroV;
	private Vector3 velocidad;
    void Start(){
		speed=2.0f;
		zeroV=new Vector3(0,0.2f,0);
        rb = GetComponent<Rigidbody>();
    }
    public void Ascender(){
   		velocidad+=Vector3.up*speed;
    }
    public void Descender(){
		velocidad+=Vector3.up*-speed;
    }
    public void Adelante(){
		velocidad+=Vector3.forward*speed;
	}
    public void Atras(){
		velocidad+=Vector3.forward*-speed;
    }
    public void Derecha(){
		velocidad+=Vector3.right*speed;
    }
    public void Izquierda(){
		velocidad+=Vector3.right*-speed;
    }
    public void Detener(){
        rb.velocity= Vector3.zero;
        velocidad=Vector3.zero;
    }
    public void Flotar(){
        velocidad+=zeroV;
        rb.velocity=velocidad;
    }
}
