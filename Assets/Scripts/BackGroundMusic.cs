using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundMusic : MonoBehaviour
{
    private AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void StopBgMusic()
    {
        audioSource.enabled = false;
        Debug.Log("Stopped the background music");
    }
}
