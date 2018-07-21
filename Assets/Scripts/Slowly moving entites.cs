using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slowlymovingentites : MonoBehaviour {

    public float _distancetomove, _movespeed, _distancefromplayer;
    public GameObject _player;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        _distancefromplayer = Vector3.Distance(transform.position, _player.transform.position);

        if (_distancefromplayer<_distancetomove)
        {
            transform.RotateAround(_player.transform.position, Vector3.up, _movespeed * Time.deltaTime);
        }
	}
}
