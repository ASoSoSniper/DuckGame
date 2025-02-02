using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] GameObject destroySound;
    public void DestroyObstacle()
    {
        GameObject sound = Instantiate(destroySound);
        sound.transform.position = transform.position;
        Destroy(gameObject);
    }
}
