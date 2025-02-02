using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillBarrier : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            SpawnPoint.currentSpawn.RespawnPlayer(other.gameObject);
        }
        else if (other.gameObject.CompareTag("Soap"))
        {
            SpawnPoint.currentSpawn.RespawnPlayer(other.gameObject.transform.parent.parent.transform.gameObject);
        }
    }
}
