using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PU_Size : Powerup
{
    public float scale = 2f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            if (!active && !hasRun)
            {
                ResetSize();
                hasRun = true;
            }
            else if (active)
            {
                Timer();
            }
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        Debug.Log("Collision!");
        if (collider.gameObject.tag == "Player")
        {
            PlayerController thisPlayer = collider.gameObject.GetComponent<PlayerController>();
            Pickup(thisPlayer);
            ChangeSize();
        }
    }

    private void ChangeSize()
    {
        player.gameObject.transform.localScale = new Vector3(scale, scale, scale);
    }

    private void ResetSize()
    {
        player.gameObject.transform.localScale = new Vector3(1, 1, 1);
        player = null;
    }
}
