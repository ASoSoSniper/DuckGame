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
    [SerializeField] AudioClip gunshotSound;

    float currFireTime = 0;
    CapsuleCollider capsuleCollider;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        mesh = transform.GetChild(0).gameObject;
        animator = mesh.GetComponent<Animator>();
        audioSource = mesh.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        ManageInput("HorizontalP1", "VerticalP1");
        grounded = GroundCheck();
        capsuleCollider.material = grounded ? groundMat : airMat;
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

        animator.SetBool("IsWalking", !transform.parent && inputDirection != Vector3.zero && grounded);
    }

    void Jump()
    {
        if (grounded || transform.parent)
        {
            DismountSoap();
            animator.SetTrigger("Jump");
            rigidBody.AddForce(Vector3.up * jumpStrength, ForceMode.Impulse);
        }
    }

    void Shoot()
    {
        if (currFireTime > 0) return;

        animator.SetTrigger("Shoot");
        audioSource.PlayOneShot(gunshotSound);

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

    void MountSoap(GameObject soap)
    {
        transform.parent = soap.transform;
        transform.localPosition = Vector3.zero;

        capsuleCollider.enabled = false;
        rigidBody.isKinematic = true;
    }
    public void DismountSoap()
    {
        transform.parent = null;

        capsuleCollider.enabled = true;
        rigidBody.isKinematic = false;
    }

    protected override bool GroundCheck()
    {
        if (!base.GroundCheck()) return false;

        if (groundData.collider.CompareTag("Soap") && rigidBody.velocity.y < 0)
        {
            MountSoap(groundData.collider.gameObject);
        }

        if (!grounded && rigidBody.velocity.y < 0)
        {
            if (!audioSource.isPlaying)
                audioSource.PlayOneShot(groundImpactSound);
        }

        return true;
    }
}
