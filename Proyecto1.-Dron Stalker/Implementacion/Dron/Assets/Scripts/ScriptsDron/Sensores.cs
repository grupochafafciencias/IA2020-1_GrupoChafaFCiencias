using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensores : MonoBehaviour
{
    public List<GameObject>radares;
    private List<Radar> radaresScripts;
    private Bateria bateria; // Componente adicional (script) que representa la batería
    private Actuadores actuador; // Componente adicional (script) para obtener información de los ac
    private bool tocandoPared; // Bandera auxiliar para mantener el estado en caso de tocar pared
    private bool cercaPared; // Bandera auxiliar para mantener el estado en caso de estar cerca de una pared
    // Asignaciones de componentes
    void Start(){
        bateria = GameObject.Find("Bateria").gameObject.GetComponent<Bateria>();
        radaresScripts=new List<Radar>();
        foreach(GameObject foo in radares){
			radaresScripts.Add(foo.GetComponent<Radar>());
		}
		int counter=0;
		foreach(Radar foo in radaresScripts){
			foo.setNumber(counter);
			counter++;
		}
        actuador = GetComponent<Actuadores>();
    }
    void OnCollisionEnter(Collision other){
        if(other.gameObject.CompareTag("Pared")){
            tocandoPared = true;
        }
    }
    void OnCollisionStay(Collision other){
        if(other.gameObject.CompareTag("Pared")){
            tocandoPared = true;
        }
        if(other.gameObject.CompareTag("BaseDeCarga")){
            actuador.CargarBateria();
        }
    }
    void OnCollisionExit(Collision other){
        if(other.gameObject.CompareTag("Pared")){
            tocandoPared = false;
        }
    }
    public bool TocandoPared(){
			return tocandoPared;
	}
    public List<int> CercaDePared(){
		List<int> cercaPared=new List<int>();
		foreach(Radar bar in radaresScripts){
			int tmp=bar.CercaDePared();
			if(tmp!=-1)	cercaPared.Add(tmp);
		}
		return cercaPared;
    }
    public float Bateria(){
        return bateria.NivelDeBateria();
    }
    public Vector3 Ubicacion(){
        return transform.position;
    }
}
