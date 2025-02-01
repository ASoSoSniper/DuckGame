using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Duck : BaseMovement
{
    [Header("Movement")]
    [SerializeField] float jumpStrength = 50f;

    [Header("Shooting")]
    [SerializeField] GameObject bubblePrefab;
    [SerializeField] Transform muzzleTransform;
    [SerializeField] float fireRate = 0.5f;
    [SerializeField] int maxBubbles = 2;

    float currFireTime = 0;
    CapsuleCollider capsuleCollider;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        mesh = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        ManageInput("HorizontalP1", "VerticalP1");
        grounded = GroundCheck();
        ShootTimer();
    }

    private void FixedUpdate()
    {
        Move();
        RotateMesh();
    }

    protected override void ManageInput(string horizontal, string vertical)
    {
        base.ManageInput(horizontal, vertical);

        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }
        if (Input.GetButtonDown("Shoot"))
        {
            Shoot();
        }
    }

    void Jump()
    {
        if (!grounded) return;

        rigidBody.AddForce(Vector3.up * jumpStrength, ForceMode.Impulse);
    }

    void Shoot()
    {
        if (currFireTime > 0) return;

        GameObject bubble = Instantiate(bubblePrefab);
        bubble.transform.position = muzzleTransform.position;
        bubble.transform.rotation = muzzleTransform.rotation;

        Bubble bubbleComp = bubble.GetComponent<Bubble>();
        bubbleComp.Launch();

        Bubble.activeBubbles.Add(bubbleComp);

        if (Bubble.activeBubbles.Count > maxBubbles)
        {
            Bubble.activeBubbles[0].PopBubble();
        }

        currFireTime = fireRate;
    }

    void ShootTimer()
    {
        if (currFireTime > 0)
        {
            currFireTime -= Time.deltaTime;
        }
    }

    public override Vector3 GetCenter()
    {
        return transform.position + transform.up * (capsuleCollider.height + verticalLaunchOffset);
    }
}
