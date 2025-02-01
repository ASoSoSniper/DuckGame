using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RisingCube : MonoBehaviour
{
    Rigidbody rigidBody;
    BoxCollider boxCollider;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
    }

    public void ToggleRise(bool active)
    {
        rigidBody.isKinematic = active;
        boxCollider.enabled = !active;
    }
}
