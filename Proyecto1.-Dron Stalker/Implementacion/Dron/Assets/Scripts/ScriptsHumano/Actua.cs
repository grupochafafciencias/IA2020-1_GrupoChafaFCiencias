using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actua : MonoBehaviour
{
	private Rigidbody rb; 
    private float speed; 
	private Vector3 zeroV;
    void Start(){
		speed=98.1f;
		zeroV=new Vector3(0,0,0);
        rb = GetComponent<Rigidbody>();
    }
    public void Ascender(){
        rb.AddRelativeForce(Vector3.up *speed, ForceMode.Acceleration);
    }
    public void Descender(){
		rb.AddRelativeForce(Vector3.up *-speed, ForceMode.Acceleration);
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
}
