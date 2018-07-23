using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIstoneBehaviour : MonoBehaviour {
    public GameObject _center;
    public int _rotatespeed;
    public bool _triggered;
	// Use this for initialization
    /*OnCollisionEnter(Collision collision)
    {
        //if(collision.tag == "manette")

        
    }*/
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(GameObject.Find("BlankController_Hand2")!= null)
        {
            _triggered = true;
        }
        if(_triggered)
        {
            _center.transform.parent = GameObject.Find("BlankController_Hand2").transform;
        }

        transform.RotateAround(_center.transform.position, Vector3.left, _rotatespeed * Time.deltaTime);

	}
}
