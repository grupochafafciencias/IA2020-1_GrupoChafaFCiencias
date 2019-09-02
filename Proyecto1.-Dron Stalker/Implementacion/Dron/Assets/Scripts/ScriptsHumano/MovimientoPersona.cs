using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimientoPersona : MonoBehaviour
{
	private Actua actuador;
    void Start()
    {
        actuador=GetComponent<Actua>();
    }
     void FixedUpdate(){
		actuador.Detener();
        if(Input.GetKey(KeyCode.V))actuador.Ascender();
        if(Input.GetKey(KeyCode.B))actuador.Descender();
        if(Input.GetAxis("Vertical") > 0)actuador.Adelante();
        if(Input.GetAxis("Vertical") < 0)actuador.Atras();
        if(Input.GetAxis("Horizontal") > 0)actuador.Derecha();
        if(Input.GetAxis("Horizontal") < 0)actuador.Izquierda();
    }
}
