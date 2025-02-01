using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseMovement : MonoBehaviour
{
    Vector3 inputDirection = Vector3.zero;
    protected GameObject mesh;
    protected Rigidbody rigidBody;
    protected bool grounded = false;

    [Header("Movement")]
    [SerializeField] float maxSpeed = 20f;
    [SerializeField] float acceleration = 5f;
    [SerializeField] float stopSpeed = 5f;
    [SerializeField] float groundCheckDistance = 5f;
    [SerializeField] float rotationSpeed = 20f;
    [SerializeField] protected float verticalLaunchOffset = 1f;

    Vector3 moveDirection = Vector3.zero;

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

        Quaternion currRot = mesh.transform.rotation;
        Quaternion targetRot = Quaternion.LookRotation(inputDirection);

        mesh.transform.rotation = Quaternion.Lerp(currRot, targetRot, rotationSpeed * Time.fixedDeltaTime);
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
