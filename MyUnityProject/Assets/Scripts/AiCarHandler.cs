using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiCarHandler : MonoBehaviour
{
    [SerializeField]
    CarHandler carHandler;

    // Collision detection
    RaycastHit[] raycastHits = new RaycastHit[1];
    bool isCarAhead = false;
    LayerMask otherCarsLayerMask;

    // Firing
    WaitForSeconds wait = new WaitForSeconds(0.2f);

    // Unity Message to be called when AI object is destroyed
    private void OnDestroy()
    {
        if (CompareTag("Player"))
        {
            Destroy(this);
            return;
        }
    }

    // Unity Message to be called when the script instance is being loaded
    private void Awake()
    {
        if (CompareTag("Player"))
        {
            Destroy(this);
            return;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(UpdateAccelerateFromEC());
    }

    // Update is called once per frame
    void Update()
    {
        float accelerationInput = 1.0f;
        float steerInput = 0.0f;

        if (isCarAhead)
        {
            accelerationInput = -1.0f;
        }

        float difference = this.transform.position.z - transform.position.z;
        if (Mathf.Abs(difference) > 20)
        {
            steerInput = -1 * Mathf.Sign(difference);
        }

        steerInput = Mathf.Clamp(steerInput, -1.0f, 1.0f);

        carHandler.SetInput(new Vector2(steerInput, accelerationInput));
    }

    IEnumerator UpdateAccelerateFromEC()
    {
        while (true)
        {
            isCarAhead = CheckIfOtherCarsAhead();
            yield return wait;
        }
    }

    // Check if other cars ahead
    bool CheckIfOtherCarsAhead()
    {
        carHandler.GetComponent<Collider>().enabled = false;
        int numberOfHits = Physics.RaycastNonAlloc(transform.position, Vector3.forward, raycastHits, 0.2f, otherCarsLayerMask);
        carHandler.GetComponent<Collider>().enabled = true;

        if (numberOfHits > 0)
        {
            return true;
        }

        return false;
    }

    // Events
    //private void OnDisable()
    //{
    //    carHandler.SetExploded(Random.Range(0, 3));
    //}
}
