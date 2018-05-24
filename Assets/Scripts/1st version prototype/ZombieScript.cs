using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieScript : MonoBehaviour {

    [Range(0.0f, 1.0f)]
    public float speed = 1.0f;
    public float distanceMin = 1.0f;
    public Transform playerTr;


    private Rigidbody rb;


    private void Start()
    {
        rb = this.GetComponent<Rigidbody>();

    }

    void Update ()
    {
        Vector3 playerPos = playerTr.position;
        playerPos.y = transform.position.y;

        if (Vector3.Distance(transform.position, playerPos) > distanceMin)
        { 
            transform.LookAt(playerPos);
            transform.position = Vector3.MoveTowards(transform.position, playerPos, speed * Time.deltaTime);
        }


        if (Input.GetKeyDown(KeyCode.R)) UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }


}
