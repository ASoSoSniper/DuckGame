using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseMovement : MonoBehaviour
{
    protected Vector3 inputDirection = Vector3.zero;
    protected GameObject mesh;
    protected Rigidbody rigidBody;
    protected bool grounded = false;

    [Header("Movement")]
    [SerializeField] protected float maxSpeed = 20f;
    [SerializeField] protected float acceleration = 5f;
    [SerializeField] protected float stopSpeed = 5f;
    [SerializeField] protected float groundCheckDistance = 5f;
    [SerializeField] protected float rotationSpeed = 20f;
    [SerializeField] protected float verticalLaunchOffset = 1f;

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

            rigidBody.AddForce(speed * inputDirection);
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

        Vector3 direction = (transform.position + inputDirection * 5f) - transform.position;
        direction.y = 0;

        Quaternion currRot = mesh.transform.rotation;
        Quaternion targetRot = Quaternion.LookRotation(direction);

        mesh.transform.rotation = Quaternion.Slerp(currRot, targetRot, rotationSpeed * Time.deltaTime);
    }

    protected bool GroundCheck()
    {
        if (Physics.Raycast(transform.position, Vector3.down, groundCheckDistance))
        {
            return true;
        }

        return false;
    }

    public abstract Vector3 GetCenter();
}
