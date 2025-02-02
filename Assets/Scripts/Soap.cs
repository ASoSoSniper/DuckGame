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
    [SerializeField] AudioClip moveSound;
    [SerializeField] float moveSoundPlayRate = 0.5f;
    float soundPlayTime = 0f;

    MeshCollider meshCollider;

    float currSpeed = 0;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        mesh = transform.GetChild(0).gameObject;
        meshCollider = mesh.transform.GetChild(0).GetComponent<MeshCollider>();
        audioSource = meshCollider.gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        ManageInput("HorizontalP2", "VerticalP2");
        grounded = GroundCheck();
        meshCollider.material = grounded ? groundMat : airMat;

        if (moveDirection != Vector3.zero)
        {
            soundPlayTime -= Time.deltaTime;
            if (soundPlayTime <= 0f)
            {
                soundPlayTime = moveSoundPlayRate;
                audioSource.pitch = Random.Range(1f, 1.2f);
                audioSource.PlayOneShot(moveSound);
            }
        }
        else soundPlayTime = 0f;
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

        if (ground && !grounded && rigidBody.velocity.y < 0)
        {
            if (!audioSource.isPlaying)
                audioSource.PlayOneShot(groundImpactSound);
        }

        return ground;
    }
}
