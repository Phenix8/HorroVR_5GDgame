using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIstoneBehaviour : MonoBehaviour {
    public GameObject _center;
    public int _rotatespeed;
	// Use this for initialization
    /*OnCollisionEnter(Collision collision)
    {
        //if(collision.tag == "manette")

        
    }*/
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.RotateAround(_center.transform.position, Vector3.up, _rotatespeed * Time.deltaTime);

	}
}
