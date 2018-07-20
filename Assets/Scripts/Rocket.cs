using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{

    public float rcsThrust = 100f;
    public float mainThrust = 20f;

    public AudioClip mainEngine;
    public AudioClip deathClip;
    public AudioClip winClip;

    private Rigidbody rigidBody;
    private AudioSource AudioSource;
    private State state = Rocket.State.Alive;

    enum State
    {
        Alive = 0,
        Dying,
        Transcending
    }
 
    // Use this for initialization
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        AudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.Alive)
        {
            RespondToThrustInput();
            RespondToRotateInput();
        }
    }

    private void RespondToThrustInput()
    {
        Vector3 currentPosition = this.transform.position;

        if (Input.GetKey(KeyCode.Space))
            ApplyThrust();
        else
            AudioSource.Stop();
    }

    private void ApplyThrust()
    {
        if (!AudioSource.isPlaying)
            AudioSource.PlayOneShot(mainEngine);
        rigidBody.AddRelativeForce(Vector3.up * mainThrust);
    }

    private void RespondToRotateInput()
    {
        rigidBody.freezeRotation = true;
        float rotationThisFrame = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
            this.transform.Rotate(Vector3.forward * rotationThisFrame);

        else if (Input.GetKey(KeyCode.D))
            this.transform.Rotate(-Vector3.forward * rotationThisFrame);

        rigidBody.freezeRotation = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive)
            return;

        switch(collision.gameObject.tag)
        {
            case "Friendly":
                break;

            case "Finish":
                AudioSource.Stop();
                AudioSource.PlayOneShot(winClip);
                state = State.Transcending;
                Invoke("LoadNextScene", 2f);
                break;

            default:
                AudioSource.Stop();
                AudioSource.PlayOneShot(deathClip);
                state = State.Dying;
                Invoke("LoadCurrentScene", 2f);
                break;
        }
    }

    private void LoadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void LoadNextScene()
    {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}

