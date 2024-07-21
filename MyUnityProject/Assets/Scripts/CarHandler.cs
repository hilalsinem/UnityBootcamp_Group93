using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarHandler : MonoBehaviour
{
    [SerializeField]
    Rigidbody rb;
    [SerializeField]
    Transform gameModel;

    float maxSteerVelocity = 450;
    float maxForwardVelocity = 50; // Changed to 150

    // Multipliers
    float accelerationMultiplier = 700;
    float brakesMultiplier = 1500;
    float steeringMultiplier = 5000;

    // Input
    Vector2 input = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        gameModel.transform.rotation = Quaternion.Euler(0, rb.velocity.x * 5, 0);
    }

    private void FixedUpdate()
    {
        // Apply Acceleration
        if (input.y > 0)
            Accelerate();
        else
            rb.drag = 0.2f;

        // Apply Brakes
        if (input.y < 0)
            Brake();

        Steer();
    }

    void Accelerate()
    {
        rb.drag = 0;
        if (rb.velocity.z >= maxForwardVelocity)
            return;
        rb.AddForce(rb.transform.forward * accelerationMultiplier * input.y);
    }

    void Brake()
    {
        // Don't brake unless we are going forward
        if (rb.velocity.z <= 0)
            return;

        rb.AddForce(rb.transform.forward * brakesMultiplier * input.y);
    }

    void Steer()
    {
        if (Mathf.Abs(input.x) > 0)
        {
            // Move the car sideways
            float speedBaseSteerLimit = rb.velocity.z / 5.0f;
            speedBaseSteerLimit = Mathf.Clamp01(speedBaseSteerLimit);

            rb.AddForce(rb.transform.right * steeringMultiplier * input.x * speedBaseSteerLimit);

            // Normalize the X Velocity
            float normalizedX = rb.velocity.x / maxSteerVelocity;

            // Ensure that we don't allow it to get bigger than 1 in magnitude.
            normalizedX = Mathf.Clamp(normalizedX, -1.0f, 1.0f);

            // Make sure we stay within the turn speed limit
            rb.velocity = new Vector3(normalizedX * maxSteerVelocity, 0, rb.velocity.z);
        }
        else
        {
            // Auto center car
            rb.velocity = Vector3.Lerp(rb.velocity, new Vector3(0, 0, rb.velocity.z), Time.fixedDeltaTime * 3);
        }
    }

    public void SetInput(Vector2 inputVector)
    {
        inputVector.Normalize();
        input = inputVector;
    }
}
