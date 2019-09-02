using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actuadores : MonoBehaviour
{
    private Rigidbody rb; 
    private Bateria bateria; 
    private Sensores sensor; 
    private float speed= 98.1f; 
	public Vector3 zeroV=new Vector3(0,0.2f,0);
    void Start(){
        rb = GetComponent<Rigidbody>();
        sensor = GetComponent<Sensores>();
        bateria = GameObject.Find("Bateria").gameObject.GetComponent<Bateria>();
    }
    public void Ascender(){
        rb.AddRelativeForce(Vector3.up *speed, ForceMode.Acceleration);
    }
    public void Descender(){
		rb.AddRelativeForce(Vector3.up *-speed, ForceMode.Acceleration);
    }
    public void Flotar(){
        rb.velocity=zeroV;
    }
    public void Adelante(){
		rb.AddRelativeForce(Vector3.forward*speed, ForceMode.Acceleration);
	}
    public void Atras(){
		rb.AddRelativeForce(Vector3.forward*-speed, ForceMode.Acceleration);
    }
    public void Derecha(){
		rb.AddRelativeForce(Vector3.right*speed, ForceMode.Acceleration);
    }
    public void Izquierda(){
		rb.AddRelativeForce(Vector3.right*-speed,ForceMode.Acceleration);
    }
    public void Detener(){
        rb.velocity= Vector3.zero;
    }
    public void CargarBateria(){
        bateria.Cargar();
    }
}
