using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{

    // store a public reference to the Player game object, so we can refer to it's Transform
    public GameObject player;
    public Transform myCamera;
    public float zoomSpeed = 1f;
    public float maxRotateSpeed = 5f;
    public float manualCamRotSpeed = 1f;

    // Store a Vector3 offset from the player (a distance to place the camera from the player at all times)
    private Vector3 offset;

    // At the start of the game..
    void Start()
    {
        // Create an offset by subtracting the Camera's position from the player's position
        offset = transform.position - player.transform.position;
    }

    // After the standard 'Update()' loop runs, and just before each frame is rendered..
    void LateUpdate()
    {
        // Set the position of the Camera (the game object this script is attached to)
        // to the player's position, plus the offset amount
        transform.position = player.transform.position + offset;

        RotateCamera();
        ZoomCamera();
    }

    void RotateCamera()
    {
        //The old controller
        //transform.RotateAround(player.transform.position, Vector3.up, Input.GetAxis("Horizontal"));

        float rightMouse = Input.GetAxis("CameraManual");
        float keyboardRotate = Input.GetAxis("CamRotate");

        if (keyboardRotate != 0)
        {
            keyboardRotate *= manualCamRotSpeed;

            transform.RotateAround(player.transform.position, Vector3.up, keyboardRotate);
        }
        else if (rightMouse > 0)
        {
            float mouseX = Input.GetAxis("Mouse X") * manualCamRotSpeed;
            //Debug.Log("debug: " + mouseX);

            transform.RotateAround(player.transform.position, Vector3.up, mouseX);
        }
        else
        {
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");

            Vector3 inputDirection = new Vector3(moveHorizontal, 0, moveVertical).normalized;
            Vector3 moveDirection = player.GetComponent<Rigidbody>().velocity;
            Debug.DrawRay(player.transform.position, moveDirection, Color.blue);

            Vector3 relativeInputDirection = (Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0) * inputDirection);
            Debug.DrawRay(player.transform.position, relativeInputDirection * 2, Color.red);

            Vector3 relativeInputDirectionNoBack = new Vector3(moveHorizontal, 0, moveVertical);
            relativeInputDirectionNoBack.z = Mathf.Clamp(relativeInputDirectionNoBack.z, 0, 1f);
            relativeInputDirectionNoBack = (Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0) * relativeInputDirectionNoBack);
            Debug.DrawRay(player.transform.position, relativeInputDirectionNoBack, Color.yellow);

            if (relativeInputDirectionNoBack.magnitude > 0)
            {

                float angleToTarget = Vector3.Angle(transform.forward, relativeInputDirection);
                float step = relativeInputDirectionNoBack.magnitude * maxRotateSpeed * (angleToTarget / 135);
                //Debug.Log(angleToTarget + " => " + step);

                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(relativeInputDirection), step);
            }
        }
    }

    void ZoomCamera()
    {
        float zoom = Input.GetAxis("CamInOut");
        myCamera.position += myCamera.forward * zoom * zoomSpeed;
    }
}