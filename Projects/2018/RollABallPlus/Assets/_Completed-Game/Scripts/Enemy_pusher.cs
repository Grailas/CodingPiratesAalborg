using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_pusher : MonoBehaviour
{
    public SphereCollider zone;
    public float speed = 1f;
    public bool canMoveVertically = false; 

    private Rigidbody rb;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        // Assign the Rigidbody component to our private rb variable
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        MoveAndChase();
    }

    void MoveAndChase()
    {
        if (player != null)
        {
            rb.AddForce(PlayerDirection() * speed);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            player = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            player = null;
        }
    }

    private Vector3 PlayerDirection()
    {
        Vector3 result;

        result = player.transform.position - transform.position;

        if(!canMoveVertically)
        {
            result.y = 0;
        }

        result.Normalize();

        return result;
    }
}
