using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceManager : MonoBehaviour
{

    public AudioSource src;
    public AudioClip[] voiceOvers;

    public static VoiceManager instance;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayAudio(int index)
    {
        src.Stop();
        src.PlayOneShot(voiceOvers[index]); 
    }
}
