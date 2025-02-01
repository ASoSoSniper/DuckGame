using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Soap : BaseMovement
{
    CapsuleCollider capsuleCollider;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        mesh = transform.GetChild(0).gameObject;
        capsuleCollider = mesh.GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        ManageInput("HorizontalP2", "VerticalP2");
        grounded = GroundCheck();
    }

    private void FixedUpdate()
    {
        Move();
        //RotateMesh();
    }

    public override Vector3 GetCenter()
    {
        return transform.position + transform.up * verticalLaunchOffset;
    }
}
