using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    public GameObject _stone;
    public bool _go, _shrink;
    Vector3 _smallsize = Vector3.zero;
    public float _radius;
    public Collider[] _gems;
    // Use this for initialization
    /* void OnCollisionEnter (Collision col)
     {
         Debug.Log(col.gameObject.tag);

         if (col.gameObject.tag == "Collectible")
         {
             Debug.Log("Touch");


             _go = true;
             _shrink = true;
             _stone = col.gameObject;
             col.gameObject.GetComponent<MeshCollider>().enabled = false;

         }
     }*/
    void Start () {

        //_radius = GetComponent<SphereCollider>().radius;
	}
	
	// Update is called once per frame
	void Update () {

        _gems = Physics.OverlapSphere(transform.position,_radius);
        foreach (Collider c in _gems)
            
        {
            if (c.gameObject.tag == "Collectible")
            {
                
               // c.gameObject.GetComponent<SphereCollider>().enabled = false;
                c.gameObject.transform.position = Vector3.MoveTowards(c.gameObject.transform.position, transform.position, 2 * Time.deltaTime);
                if (c.gameObject.transform.localScale != _smallsize)
                {
                    c.gameObject.transform.localScale = c.gameObject.transform.localScale * 0.987f;

                }
                else
                {
                    c.gameObject.transform.localScale = _smallsize;
                    
                }

            }
        }

		/*if(_go)
        {
            _stone.transform.position = Vector3.MoveTowards(_stone.transform.position, transform.position, 2 * Time.deltaTime);
            if (_shrink)
            {
                if (_stone.gameObject.transform.localScale != _smallsize)
                {
                    _stone.gameObject.transform.localScale = _stone.gameObject.transform.localScale*0.987f;

                }
                else {
                    _stone.gameObject.transform.localScale = _smallsize;
                    _shrink = false;
                }
            }
        }*/
	}
}
