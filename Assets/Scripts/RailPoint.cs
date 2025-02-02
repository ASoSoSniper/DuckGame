using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailPoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Soap"))
        {
            CameraRig cameraRig = FindObjectOfType<CameraRig>();
            if (cameraRig)
                cameraRig.AttachToRail(gameObject.transform.GetComponentInParent<CameraRail>());
        }
    }
}

