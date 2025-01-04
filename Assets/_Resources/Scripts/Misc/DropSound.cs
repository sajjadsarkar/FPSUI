using UnityEngine;

public class DropSound : MonoBehaviour
{
    public AudioClip[] sound;
    public AudioSource aSource;

    void OnCollisionEnter(Collision collision)
    {
        PlayDropSound();
    }

    void PlayDropSound()
    {
        aSource.clip = sound[Random.Range(0, sound.Length)];
        aSource.volume = .7f;
        aSource.Play();
    }
}