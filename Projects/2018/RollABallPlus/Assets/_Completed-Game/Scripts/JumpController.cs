using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpController : MonoBehaviour
{
    public GameObject player;

    private Vector3 offset;
    [SerializeField]
    private GameObject[] surfaces;
    public int startSurfaceNumber = 2;
    [SerializeField]
    private int numberOfSurfaces = 0;

    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - player.transform.position;
        surfaces = new GameObject[startSurfaceNumber];
    }

    void Update()
    {
        MoveToPlayer();
    }

    void MoveToPlayer()
    {
        transform.position = player.transform.position + offset;
    }

    public bool CanPlayerJump()
    {
        return numberOfSurfaces > 0 ? true : false;
    }

    private void ManageSurfaces(GameObject surface)
    {
        if(numberOfSurfaces < surfaces.Length)
        {
            for (int i = 0; i < surfaces.Length; i++)
            {
                if(surfaces[i] == null)
                {
                    surfaces[i] = surface;
                    break;
                }
            }
        }
        else
        {
            Array.Resize(ref surfaces, surfaces.Length + 1);
            surfaces[surfaces.Length - 1] = surface;
        }
        numberOfSurfaces++;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Enter!");

        if(other.gameObject.tag != "Player" && 
            other.gameObject.tag != "Pick Up" && 
            other.gameObject.tag != "Powerup")
        {
            ManageSurfaces(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Exit!");

        if (other.gameObject.tag != "Player" &&
            other.gameObject.tag != "Pick Up" &&
            other.gameObject.tag != "Powerup")
        {
            for(int i = 0; i < surfaces.Length; i++)
            {
                if(surfaces[i] == other.gameObject)
                {
                    surfaces[i] = null;
                    numberOfSurfaces--;
                    break;
                }
            }
        }
    }
}
