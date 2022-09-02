using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootStepSound : MonoBehaviour
{
    public AudioClip clip;
    public AudioSource audioFoot;

    // Update is called once per frame
    private void FootStep()
    {
        //AudioClip clip = GetRandomClip();
        //GetComponent<AudioSource>().PlayOneShot(GetComponent<AudioSource>().clip);
        audioFoot.PlayOneShot(clip);
    }

    //private AudioClip GetRandomClip()
    //{
    //    return clips[UnityEngine.Random.Range(0, clips.Length)];
    //}
}
