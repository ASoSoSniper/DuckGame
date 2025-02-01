using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    public static List<Bubble> activeBubbles = new List<Bubble>();

    [Header("Initial Launch")]
    [SerializeField] float launchSpeed = 5f;
    [SerializeField] float launchDuration = 0.5f;

    [Header("Bouncing")]
    [SerializeField] float bounceStrength = 10f;

    [Header("Rising")]
    [SerializeField] float riseSpeed = 10f;
    [SerializeField] float riseDuration = 4f;
    
    float currTime = 0;
    bool rising = false;

    Rigidbody rigidBody;
    RisingCube foundCube;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        currTime -= Time.deltaTime;
        if (currTime <= 0)
        {
            if (rising)
            {
                EndRise();
            }
            else
            {
                rigidBody.velocity = Vector3.zero;
            }
        }
    }

    public void Launch()
    {
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.AddForce(transform.forward * launchSpeed, ForceMode.Impulse);
        currTime = launchDuration;
    }

    public void PopBubble()
    {
        activeBubbles.Remove(this);
        Destroy(gameObject);
    }

    void Bounce(Collision collision)
    {
        BaseMovement movement = collision.gameObject.GetComponent<BaseMovement>();
        if (movement)
        {
            Vector3 direction = (movement.GetCenter() - transform.position).normalized;
            collision.rigidbody.AddForce(direction * bounceStrength, ForceMode.Impulse);
        }
        PopBubble();
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bubble"))
        {
            PopBubble();
            return;
        }

        if (collision.gameObject.CompareTag("RisingCube"))
        {
            BeginRise(collision.gameObject);
        }

        if (!collision.gameObject.CompareTag("Player")) return;

        if (rising || currTime <= 0)
        {
            Bounce(collision);
        }
    }

    void BeginRise(GameObject cube)
    {
        rising = true;
        transform.position = cube.transform.position;
        foundCube = cube.GetComponent<RisingCube>();
        foundCube.ToggleRise(true);

        cube.transform.parent = transform;

        rigidBody.velocity = Vector3.up * riseSpeed;
        currTime = riseDuration;
    }

    void EndRise()
    {
        rising = false;
        foundCube.transform.parent = null;
        foundCube.ToggleRise(false);

        PopBubble();
    }
}
