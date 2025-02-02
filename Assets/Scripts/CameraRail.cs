using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRail : MonoBehaviour
{
    public List<Vector3> railPoints;
    public CameraRig.CameraMovement cameraMoveType = CameraRig.CameraMovement.MoveAlongRail;
    public float cameraDistance = 50f;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            railPoints.Add(transform.GetChild(i).transform.position);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Soap"))
        {
            CameraRig cameraRig = FindObjectOfType<CameraRig>();
            if (cameraRig)
                cameraRig.AttachToRail(this);
        }
    }
}
