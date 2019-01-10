using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Spaceship : MonoBehaviour {

    [SerializeField]
    private float rcsThrust = 250f;
    [SerializeField]
    private float mainThrust = 1500f;

    private Rigidbody rigidBody;
    private AudioSource[] audioSources;

    private bool isDead = false;
    private bool isWinning = false;

	// Use this for initialization
	private void Start ()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSources = GetComponents<AudioSource>();
	}
	
	// Update is called once per frame
	private void Update ()
    {
        ApplyThrust();
        RotateShip();

        if (isDead && !audioSources[1].isPlaying)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            isDead = false;
        }

        if (isWinning && !audioSources[2].isPlaying)
        {
            if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                SceneManager.LoadScene(1);
            }
            isWinning = false;
        }
	}

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                print("Ok");
                break;
            case "Goal":
                print("Win");

                if (!audioSources[2].isPlaying)
                {
                    audioSources[2].Play();
                    isWinning = true;
                }

                break;
            default:
                print("Dead");

                if (!audioSources[1].isPlaying)
                {
                    audioSources[1].Play();
                    isDead = true;
                }

                break;
        }
    }

    private void ApplyThrust()
    {
        float frameThrust = mainThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(Vector3.up * frameThrust);

            if (!audioSources[0].isPlaying)
            {
                audioSources[0].Play();
            }
        }
        else
        {
            audioSources[0].Stop();
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
