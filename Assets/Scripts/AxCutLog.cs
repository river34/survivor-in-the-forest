using UnityEngine;
using System.Collections;

public class AxCutLog : MonoBehaviour {

    //Initialize audio sources
    private AudioSource _audioSource;
    private AudioClip _chopSound;

    void Start()
    {
        //Loading resources to sound model
        _audioSource = gameObject.AddComponent<AudioSource>();
        _chopSound = Resources.Load("Sounds/Chopping 1") as AudioClip;
    }

    void OnTriggerEnter(Collider other)
    {
        // Debug.Log(other.name);
        if (CompareTag(Tags.AX) && other.CompareTag(Tags.BRANCH_JOINT))
        {
            other.tag = Tags.SMALL_GRABBABLE;
            Rigidbody body = other.GetComponent<Rigidbody>();
            body.isKinematic = false;

            //Play Sound
            _audioSource.clip = _chopSound;
            _audioSource.Play();

            // Debug.Log(Tags.BRANCH_JOINT);
        }
    }
}
