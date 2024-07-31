using UnityEngine;
using System.Collections;

public class AICarController : MonoBehaviour
{
    // Variables
    public float accelerationMultiplier = 5;
    public float brakeMultiplier = 5;
    public float steeringMultiplier = 2;
    public Transform pathHolder;
    public float raycastRange = 10;
    public float waypointThreshold = 1;
    public bool isPlayer = true;
    public bool isExploded = false;

    private Rigidbody carRigidbody;
    private Vector3 moveVector;
    private int waypointIndex = 0;

    private void Start()
    {
        carRigidbody = GetComponent<Rigidbody>();
        isPlayer = CompareTag("Player");
        SetNextWaypoint();
    }

    private void Update()
    {
        if (CanMove())
        {
            Move();
        }
    }

    private bool CanMove()
    {
        return carRigidbody != null && !isExploded;
    }

    private void Move()
    {
        // Get input values
        float input = Input.GetAxis("Vertical");
        float steer = Input.GetAxis("Horizontal");

        // Acceleration
        if (input > 0)
        {
            carRigidbody.AddForce(transform.forward * accelerationMultiplier * input);
        }

        // Brake
        if (input < 0)
        {
            carRigidbody.AddForce(-transform.forward * brakeMultiplier * -input);
        }

        // Steering
        transform.Rotate(0, steer * steeringMultiplier, 0);

        // Check if we are close to the waypoint
        if (Vector3.Distance(transform.position, pathHolder.GetChild(waypointIndex).position) < waypointThreshold)
        {
            SetNextWaypoint();
        }
    }

    private void SetNextWaypoint()
    {
        waypointIndex++;
        if (waypointIndex >= pathHolder.childCount)
        {
            waypointIndex = 0;
        }
        moveVector = pathHolder.GetChild(waypointIndex).position;
    }

    private void FixedUpdate()
    {
        // Raycast for obstacles
        if (Physics.Raycast(transform.position, transform.forward, raycastRange))
        {
            // Implement obstacle avoidance
            Debug.Log("Obstacle detected");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isPlayer)
        {
            if (collision.transform.root.CompareTag("Untagged") || collision.transform.root.CompareTag("CarAI"))
            {
                return;
            }
        }
        Vector3 velocity = carRigidbody.velocity;
        Debug.Log("Exploded");
        isExploded = true;
        StartCoroutine(SlowDownTimeCO());
    }

    private void Steer()
    {
        float steer = Input.GetAxis("Horizontal");
        transform.Rotate(0, steer * steeringMultiplier, 0);
    }

    private void Accelerate()
    {
        float input = Input.GetAxis("Vertical");
        if (input > 0)
        {
            carRigidbody.AddForce(transform.forward * accelerationMultiplier * input);
        }
    }

    private void Brake()
    {
        float input = Input.GetAxis("Vertical");
        if (input < 0)
        {
            carRigidbody.AddForce(-transform.forward * brakeMultiplier * -input);
        }
    }

    private void CheckObstacle()
    {
        if (Physics.Raycast(transform.position, transform.forward, raycastRange))
        {
            Debug.Log("Obstacle detected");
        }
    }

    private IEnumerator SlowDownTimeCO()
    {
        while (Time.timeScale > 0.1f)
        {
            Time.timeScale -= Time.deltaTime * 2;
            yield return null;
        }
        Time.timeScale = 0.1f;
        yield return new WaitForSeconds(0.5f);
        while (Time.timeScale < 1f)
        {
            Time.timeScale += Time.deltaTime * 2;
            yield return null;
        }
        Time.timeScale = 1f;
    }
}
