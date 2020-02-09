﻿using UnityEngine;

// Include the namespace required to use Unity UI
using UnityEngine.UI;

using System.Collections;

public class PlayerController : MonoBehaviour {
	
	// Create public variables for player speed, and for the Text UI game objects
	public float speed;
    public float jumpForce;
	public Text countText;
	public Text winText;
    public Transform myCameraController;

	// Create private references to the rigidbody component on the player, and the count of pick up objects picked up so far
	private Rigidbody rb;
	private int count;
    public int toCount = 12;
    public GameObject pickupContainer;
    public GameObject[] pickups;

    // Jump Controller
    public JumpController jumpControl;

	// At the start of the game..
	void Start ()
	{
		// Assign the Rigidbody component to our private rb variable
		rb = GetComponent<Rigidbody>();

		// Set the count to zero 
		count = 0;

		// Run the SetCountText function to update the UI (see below)
		SetCountText ();

		// Set the text property of our Win Text UI to an empty string, making the 'You Win' (game over message) blank
		winText.text = "";

        CountPickups();
	}

	// Each physics step..
	void FixedUpdate ()
	{
		// Set some local float variables equal to the value of our Horizontal and Vertical Inputs
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");
        bool jump = Input.GetButtonDown("Jump");

		// Create a Vector3 variable, and assign X and Z to feature our horizontal and vertical float variables above
		Vector3 controlDirection = new Vector3 (moveHorizontal, 0.0f, moveVertical);
        Vector3 movement = myCameraController.TransformDirection(controlDirection);


        // Add a physical force to our Player rigidbody using our 'movement' Vector3 above, 
        // multiplying it by 'speed' - our public player speed that appears in the inspector
        rb.AddForce (movement * speed);

        // Add force upwards if jump is pressed
        if(jump && jumpControl.CanPlayerJump())
        {
            rb.AddForce(Vector3.up * jumpForce);
        }
	}

	// When this game object intersects a collider with 'is trigger' checked, 
	// store a reference to that collider in a variable named 'other'..
	void OnTriggerEnter(Collider other) 
	{
		// ..and if the game object we intersect has the tag 'Pick Up' assigned to it..
		if (other.gameObject.CompareTag ("Pick Up"))
		{
			// Make the other game object (the pick up) inactive, to make it disappear
			other.gameObject.SetActive (false);

			// Add one to the score variable 'count'
			count = count + 1;

			// Run the 'SetCountText()' function (see below)
			SetCountText ();
		}
	}

	// Create a standalone function that can update the 'countText' UI and check if the required amount to win has been achieved
	void SetCountText()
	{
		// Update the text field of our 'countText' variable
		countText.text = "Count: " + count.ToString ();

		// Check if our 'count' is equal to or exceeded 12
		if (count >= toCount) 
		{
			// Set the text value of our 'winText'
			winText.text = "You Win!";
		}
	}

    void CountPickups()
    {
        pickups = GameObject.FindGameObjectsWithTag("Pick Up");
        toCount = pickups.Length;
    }

    public void ResetPickups()
    {
        for(int i = 0; i < toCount; i++)
        {
            pickups[i].SetActive(true);
        }
        count = 0;

        //Reset UI
        SetCountText();
    }
}