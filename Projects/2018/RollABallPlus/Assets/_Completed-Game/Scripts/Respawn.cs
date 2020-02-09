using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour {

    public Vector3 startPosition;
    private Rigidbody myRigidbody;
    private bool respawned;
    public GameObject respawnMarker;

	// Use this for initialization
	void Start () {
        startPosition = transform.position;
        myRigidbody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        float respawnInput = Input.GetAxis("Respawn");
        if (respawnInput > 0 && !respawned)
        {
            respawned = true;
            RespawnThis();
        }
        else if (respawnInput <= 0)
        {
            respawned = false;
        }
        AutoRespawn();
    }

    public void RespawnThis()
    {
        transform.position = startPosition;
        myRigidbody.velocity = new Vector3(0, 0, 0);
        myRigidbody.angularVelocity = new Vector3(0, 0, 0);

        gameObject.GetComponent<PlayerController>().ResetPickups();
    }

    public void AutoRespawn()
    {
        if (transform.position.y < respawnMarker.transform.position.y)
        {
            RespawnThis();
        }
    }
}
