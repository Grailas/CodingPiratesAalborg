using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPadZone : MonoBehaviour
{
    public bool fixedJump = false;
    public int jumpVelocity = 10;
    public uint maxJumpVelocity = 0;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Rigidbody rigidbody = other.GetComponent<Rigidbody>();
            Debug.DrawRay(transform.position, rigidbody.velocity, Color.blue, 2f);
            Debug.DrawLine(transform.position, transform.position * jumpVelocity, Color.magenta, 2f);
            if (fixedJump)
            {
                rigidbody.velocity = (transform.up * jumpVelocity);
            }
            else
            {
                rigidbody.velocity += (transform.up * jumpVelocity);
            }

            if (maxJumpVelocity > 0)
            {
                float jumpDirectionalVelocity = Vector3.Dot(rigidbody.velocity, transform.up);

                if (jumpDirectionalVelocity > maxJumpVelocity)
                {
                    Debug.DrawRay(transform.position, rigidbody.velocity, Color.red, 2f);
                    rigidbody.velocity = rigidbody.velocity.normalized * maxJumpVelocity;

                }
            }

            Debug.DrawRay(transform.position, rigidbody.velocity, Color.green, 2f);
        }
    }
}
