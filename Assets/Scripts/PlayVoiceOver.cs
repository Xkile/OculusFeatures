using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayVoiceOver : MonoBehaviour
{

    public int setvVoiceIndex;
 
    private void OnEnable()
    {
        VoiceManager voiceplayer = GameObject.FindObjectOfType<VoiceManager>() as VoiceManager;

        voiceplayer.PlayAudio(setvVoiceIndex);
    }

}
