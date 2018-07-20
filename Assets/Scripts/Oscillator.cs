using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{

    public Vector3 movementVector = new Vector3(0f, 10f, 0f);
    public float period = 3f;

    [Range(0, 1)] public float movementFactor;

    private Vector3 startingPosition;

    void Start()
    {
        startingPosition = this.transform.position;
    }

    void Update()
    {
        if (period > 0f)
        {
            float cycles = Time.time / period;

            const float tau = Mathf.PI * 2;
            float rawSinWave = Mathf.Sin(cycles * tau);

            movementFactor = rawSinWave / 2f + 0.5f;

            Vector3 offset = movementFactor * movementVector;
            transform.position = startingPosition + offset;
        }
    }
}
