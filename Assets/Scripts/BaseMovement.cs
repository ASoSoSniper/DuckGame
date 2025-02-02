using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseMovement : MonoBehaviour
{
    protected Vector3 inputDirection = Vector3.zero;
    protected GameObject mesh;
    protected Rigidbody rigidBody;
    protected AudioSource audioSource;
    protected bool grounded = false;
    protected RaycastHit groundData;

    [Header("Movement")]
    [SerializeField] protected float maxSpeed = 20f;
    [SerializeField] protected float acceleration = 5f;
    [SerializeField] protected float stopSpeed = 5f;
    [SerializeField] protected float groundCheckDistance = 5f;
    [SerializeField] protected float rotationSpeed = 20f;
    [SerializeField] protected float verticalLaunchOffset = 1f;

    [Header("Physics")]
    [SerializeField] protected PhysicMaterial groundMat;
    [SerializeField] protected PhysicMaterial airMat;
    [SerializeField] protected float obstacleCheckDistance = 5f;
    [SerializeField] protected LayerMask groundMask;
    [SerializeField] protected AudioClip groundImpactSound;

    protected Vector3 moveDirection = Vector3.zero;

    protected virtual void ManageInput(string horizontal, string vertical)
    {
        float x = Input.GetAxisRaw(horizontal);
        float y = Input.GetAxisRaw(vertical);

        inputDirection = new Vector3(-y, 0, x).normalized;

        moveDirection = rigidBody.velocity;
        moveDirection.y = 0;
    }

    protected virtual void Move()
    {
        if (inputDirection != Vector3.zero)
        {
            float currentSpeed = moveDirection.magnitude;
            float speedDiff = maxSpeed - Mathf.Min(currentSpeed, maxSpeed);

            float speed = speedDiff * acceleration;

            rigidBody.AddForce(speed * (inputDirection + (grounded ? Vector3.zero : ObstacleCheck())));
        }
        else
        {
            Vector3 vel = rigidBody.velocity;

            rigidBody.AddForce(new Vector3(-vel.x, 0, -vel.z) * stopSpeed);
        }
    }

    protected virtual void RotateMesh()
    {
        if (inputDirection == Vector3.zero) return;

        Quaternion floorAlignment = mesh.transform.rotation;

        if (grounded)
        {
            Vector3 floorNormal = groundData.normal;
            floorAlignment = Quaternion.FromToRotation(Vector3.up, floorNormal);
        }

        Vector3 direction = (transform.position + inputDirection * 5f) - transform.position;
        direction.y = 0;
        Quaternion facingRotation = Quaternion.LookRotation(direction);

        Quaternion currRot = mesh.transform.rotation;
        Quaternion targetRot;
        if (grounded) targetRot = floorAlignment * facingRotation;
        else targetRot = facingRotation;

        mesh.transform.rotation = Quaternion.Slerp(currRot, targetRot, rotationSpeed * Time.deltaTime);
    }

    protected virtual bool GroundCheck()
    {
        Ray ray = new Ray(transform.position, Vector3.down);

        bool ground = Physics.Raycast(ray, out groundData, groundCheckDistance);

        return ground;
    }

    public abstract Vector3 GetCenter();

    protected Vector3 ObstacleCheck()
    {
        Vector3 startpoint = GetCenter();

        Ray ray = new Ray(startpoint, inputDirection);
        RaycastHit hit;

        bool obstacle = Physics.Raycast(ray, out hit, obstacleCheckDistance, groundMask);
        Debug.DrawLine(startpoint, startpoint + inputDirection * obstacleCheckDistance);

        if (!obstacle) return Vector3.zero;

        float dot = Vector3.Dot(hit.normal, Vector3.up);

        return dot >= 0 ? hit.normal : Vector3.zero;
    }
}
