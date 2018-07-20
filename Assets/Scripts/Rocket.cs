using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{

    [SerializeField] private float rcsThrust = 100f;
    [SerializeField] private float mainThrust = 20f;

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
        Thrust();
        Rotate();
    }

    private void Thrust()
    {
        Vector3 currentPosition = this.transform.position;

        if (Input.GetKey(KeyCode.Space))
        {
            if (!AudioSource.isPlaying)
                AudioSource.Play();
            rigidBody.AddRelativeForce(Vector3.up * mainThrust); //(new Vector3(currentPosition.x, currentPosition.y + 10f, currentPosition.z)
        }
        else
            AudioSource.Stop();
    }


    private void Rotate()
    {
        rigidBody.freezeRotation = true;
        float rotationThisFrame = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
            this.transform.Rotate(Vector3.forward * rotationThisFrame);

        else if (Input.GetKey(KeyCode.D))
            this.transform.Rotate(-Vector3.forward * rotationThisFrame);

        rigidBody.freezeRotation = false;
    }
}

