using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootStepSound : MonoBehaviour
{
    private AudioSource audioSource;


    [Header("Footsteps Sources")]
    public AudioClip[] footStepsSound;


    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

    }

    public AudioClip GetRandomFootStep()
    {
        return footStepsSound[UnityEngine.Random.Range(0, footStepsSound.Length)];
    }

    public void Step()
    {
        AudioClip clip = GetRandomFootStep();
        audioSource.PlayOneShot(clip);
    }
}
