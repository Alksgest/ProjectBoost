using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    private Rigidbody rigidBody;
    private AudioSource AudioSource;
    // Use this for initialization
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        AudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInput();
    }

    private void ProcessInput()
    {
        Vector3 currentPosition = this.transform.position;

        if (Input.GetKey(KeyCode.Space))
        {
            if (!AudioSource.isPlaying)
                AudioSource.Play();
            rigidBody.AddRelativeForce(new Vector3(currentPosition.x, currentPosition.y + 10f, currentPosition.z));
        }
        else
            AudioSource.Stop();

        if (Input.GetKey(KeyCode.A))
        {
            this.transform.Rotate(Vector3.forward);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            this.transform.Rotate(-Vector3.forward);
        }
    }
}
