using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAudio : MonoBehaviour
{
    public Transform body;
    public GameObject windAudio;
    AudioSource windAudioSource;

    private Vector3 lastLoc;
    void Start()
    {
        lastLoc = body.position;
        windAudioSource = windAudio.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // float speed = (body.position - lastLoc).magnitude / Time.deltaTime;
        float speed = (body.position - lastLoc).magnitude;
        windAudioSource.volume = speed * 0.01f;
    }
}
