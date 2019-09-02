using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CameraFollow : MonoBehaviour
{
    public GameObject objetoASeguir; //El sujeto en persona
    private Transform playerObject;
    void FixedUpdate()
    {
		playerObject=objetoASeguir.GetComponent<Transform>();
        Vector3 lookOnObject = playerObject.position - transform.position;
        transform.forward = lookOnObject.normalized;
    }
}

