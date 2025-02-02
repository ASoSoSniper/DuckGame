using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRail : MonoBehaviour
{
    public List<GameObject> railPoints;

    public CameraRig.CameraMovement cameraMoveType = CameraRig.CameraMovement.MoveAlongRail;
    public float cameraDistance = 50f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Soap"))
        {
            Debug.Log("Hit");
            CameraRig cameraRig = FindObjectOfType<CameraRig>();
            if (cameraRig)
                cameraRig.AttachToRail(this);
        }
    }
}
