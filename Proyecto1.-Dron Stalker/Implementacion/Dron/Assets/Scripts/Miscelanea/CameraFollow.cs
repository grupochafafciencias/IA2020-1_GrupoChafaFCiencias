using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CameraFollow : MonoBehaviour
{
    public GameObject objetoASeguir; //El sujeto en persona
    private Transform playerObject;
    public Transform dron;
        public float speedH = 2.0f;
    public float speedV = 2.0f;
    private float yaw = 0.0f;
    private float pitch = 0.0f;
    void Start(){
		playerObject=objetoASeguir.GetComponent<Transform>();
	}
    void LateUpdate()
    {
        if(Input.GetKey(KeyCode.Z))cambiaVista();
        if (Input.GetMouseButton(1)) {
		yaw += speedH * Input.GetAxis("Mouse X");
        pitch -= speedV * Input.GetAxis("Mouse Y");
        transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
        }
     }
    /**
     * Al pulsar Z puedes cambiar entre si ves desde el dron o desde el humano.
     * Esta bien chido.
     * */
    void cambiaVista(){
			transform.parent=playerObject;
			Transform tmp=playerObject;
			playerObject=dron;
			dron=tmp;
			transform.position=dron.position;
	}
}

