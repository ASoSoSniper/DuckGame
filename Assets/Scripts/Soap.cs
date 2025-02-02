using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Soap : BaseMovement
{
    [Header("Ramming")]
    [SerializeField] float ramSpeed = 30f;
    [SerializeField] float ramKnockback = 20f;

    [Header("Groundcheck")]
    [SerializeField] float groundCheckOffset = 5f;

    MeshCollider meshCollider;

    float currSpeed = 0;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        mesh = transform.GetChild(0).gameObject;
        meshCollider = mesh.transform.GetChild(0).GetComponent<MeshCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        ManageInput("HorizontalP2", "VerticalP2");
        grounded = GroundCheck();
        meshCollider.material = grounded ? groundMat : airMat;
    }

    private void FixedUpdate()
    {
        Move();
        RotateMesh();
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

        if (currSpeed < ramSpeed) return;

        Obstacle obstacle = collision.gameObject.GetComponent<Obstacle>();
        if (obstacle)
            obstacle.DestroyObstacle();

        rigidBody.AddForce((-moveDirection + Vector3.up) * ramKnockback, ForceMode.Impulse);
    }

    protected override bool GroundCheck()
    {
        bool ground = false;

        for (int i = 1; i > -2; i--)
        {
            Vector3 startPoint = mesh.transform.position +
            (mesh.transform.forward * i * groundCheckOffset) + (mesh.transform.up * 2);

            Ray ray = new Ray(startPoint, Vector3.down);

            ground = Physics.Raycast(ray, out groundData, groundCheckDistance, groundMask);

            if (ground) break;
        }

        return ground;
    }
}
