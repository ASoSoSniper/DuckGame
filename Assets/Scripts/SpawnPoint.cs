using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] Transform p1Spawn;
    [SerializeField] Transform p2Spawn;
    [SerializeField] bool levelStartSpawn;

    public static SpawnPoint currentSpawn;

    public void RespawnPlayer(GameObject player)
    {
        bool isP1 = player.GetComponent<Duck>();

        Transform spawnPoint = isP1 ? p1Spawn : p2Spawn;
        player.gameObject.transform.position = spawnPoint.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            currentSpawn = this;
        }
    }
}
