using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckSounds : MonoBehaviour
{
    [SerializeField] List<AudioClip> walkSounds;
    [SerializeField] AudioClip jumpSound;
    [SerializeField] AudioClip deathSound;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySqueak()
    {
        int rand = Random.Range(0, walkSounds.Count - 1);
        float pitch = Random.Range(1f, 1.2f);
        audioSource.pitch = pitch;
        audioSource.PlayOneShot(walkSounds[rand]);
    }

    public void PlayJumpSound()
    {
        audioSource.PlayOneShot(jumpSound);
    }
}
