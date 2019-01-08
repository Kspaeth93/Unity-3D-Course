using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spaceship : MonoBehaviour {

    [SerializeField]
    private float rcsThrust = 250f;
    [SerializeField]
    private float mainThrust = 1500f;

    private Rigidbody rigidBody;
    private AudioSource audioSource;

	// Use this for initialization
	void Start ()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        ApplyThrust();
        RotateShip();
	}

    private void ApplyThrust()
    {
        float frameThrust = mainThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(Vector3.up * frameThrust);

            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            audioSource.Stop();
        }
    }

    private void RotateShip()
    {
        float frameRotation = rcsThrust * Time.deltaTime;
        rigidBody.freezeRotation = true; // Take manual control of rotation

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(Vector3.forward * frameRotation);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(-Vector3.forward * frameRotation);
        }

        rigidBody.freezeRotation = false; // Resume physics control of rotation
    }

}
