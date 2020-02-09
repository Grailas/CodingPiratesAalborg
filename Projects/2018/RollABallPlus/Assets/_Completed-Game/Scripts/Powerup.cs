using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour {

    public int duration;
    public float remainingDuration;
    public Texture texture;
    protected PlayerController player;
    public bool active;
    public bool hasRun;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void Timer()
    {
        if (active)
        {

            remainingDuration -= Time.deltaTime;

            if (remainingDuration <= 0)
            {
                active = false;
            }
        }
    }

    public void Pickup(PlayerController thisPlayer)
    {
        player = thisPlayer;
        active = true;
        hasRun = false;
        remainingDuration = duration;
    }

    //Create a timer function
    //Reset player ref on time end
}
