using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    public float rcsThrust = 100f;
    public float mainThrust = 20f;
    public float levelLoadDelay = 3f;

    public AudioClip mainEngine;
    public AudioClip deathClip;
    public AudioClip winClip;

    public ParticleSystem mainEngineParticles;
    public ParticleSystem deathParticles;
    public ParticleSystem winPartilces;

    private Rigidbody rigidBody;
    private AudioSource AudioSource;
    private State state = Rocket.State.Alive;

    private static bool DebugModActive = false;

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
        ChangeDebugMod();

        if (DebugModActive)
            ChangeScene();

        if (Input.GetKeyDown(KeyCode.H))
            ShowAllCommandKeys();

        if (state == State.Alive)
        {
            RespondToThrustInput();
            RespondToRotateInput();
        }
    }

    private void ChangeScene()
    {
        if (Input.GetKeyDown(KeyCode.N))
            LoadNextScene();
        if (Input.GetKeyDown(KeyCode.P))
            LoadPreviousScene();
    }
    private void ShowAllCommandKeys()
    {
        Debug.Log("Press \"C\" to actovate\\deactivate Debug Mod");
        Debug.Log("Press \"N\" to load next scene");
        Debug.Log("Press \"P\" to load previous scene");
    }

    private void ChangeDebugMod()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            DebugModActive = !DebugModActive;
            if (DebugModActive)
                Debug.Log("Debug mod activated.");
            else
                Debug.Log("Debug mod deactivated.");
        }
    }

    private void RespondToThrustInput()
    {
        Vector3 currentPosition = this.transform.position;

        if (Input.GetKey(KeyCode.Space))
            ApplyThrust();
        else
        {
            AudioSource.Stop();
            mainEngineParticles.Stop();
        }
    }

    private void ApplyThrust()
    {
        if (!AudioSource.isPlaying)
            AudioSource.PlayOneShot(mainEngine);
        rigidBody.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);

        mainEngineParticles.Play();
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
            if (state != State.Alive || DebugModActive)
                return;

            switch (collision.gameObject.tag)
            {
                case "Friendly":
                    break;

                case "Finish":
                    StartSuccessSequence();
                    break;

                default:
                    StartDeathSequence();
                    break;          
        }
    }

    private void StartSuccessSequence()
    {
        AudioSource.Stop();
        AudioSource.PlayOneShot(winClip);
        state = State.Transcending;
        mainEngineParticles.Stop();
        winPartilces.Play();
        Invoke("LoadNextScene", levelLoadDelay);
    }

    private void StartDeathSequence()
    {
        AudioSource.Stop();
        AudioSource.PlayOneShot(deathClip);
        state = State.Dying;
        mainEngineParticles.Stop();
        deathParticles.Play();
        Invoke("LoadCurrentScene", levelLoadDelay);
    }

    private void LoadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void LoadNextScene()
    {
        if (SceneManager.GetActiveScene().buildIndex + 1 != SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    private void LoadPreviousScene()
    {
        if (SceneManager.GetActiveScene().buildIndex -1 != -1)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}

