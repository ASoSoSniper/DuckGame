using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Soap : BaseMovement
{
    [SerializeField] float ramSpeed = 30f;
    [SerializeField] float ramKnockback = 20f;

    float currSpeed = 0;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        mesh = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        ManageInput("HorizontalP2", "VerticalP2");
        grounded = GroundCheck();

        //Debug.Log(rigidBody.velocity.magnitude);
    }

    private void FixedUpdate()
    {
        Move();
        //RotateMesh();
    }

    private void LateUpdate()
    {
        currSpeed = rigidBody.velocity.magnitude;
    }

    public override Vector3 GetCenter()
    {
        return transform.position + transform.up * verticalLaunchOffset;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Obstacle")) return;

        Debug.Log(currSpeed);
        if (currSpeed < ramSpeed) return;

        Obstacle obstacle = collision.gameObject.GetComponent<Obstacle>();
        if (obstacle)
            obstacle.DestroyObstacle();

        rigidBody.AddForce((-moveDirection + Vector3.up) * ramKnockback, ForceMode.Impulse);
    }
}
