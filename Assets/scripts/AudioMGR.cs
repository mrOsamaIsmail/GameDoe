using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace event_types
{
    public delegate void AudioDelegate(AudioClipType type, AudioSource src);
}

[RequireComponent(typeof(AudioSource))]
public class AudioMGR : MonoBehaviour
{

    private AudioSource BGM;
    
   
    public AudioData[] A_Data;


    private void Awake()
    {
        Init();
    }
    void Init() 
    {
        SubToEvents();
        BGM = GetComponent<AudioSource>();
        _PlayAudio(AudioClipType.BGM, BGM);
    }

    private void SubToEvents()
    {
        // subscribes to audio events
        FPSController player = GameObject.FindWithTag("Player").GetComponent<FPSController>();
        player.OnMakeSound += _PlayAudio;

        var count = 0;
        GameObject[] Enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject item in Enemies)
        {
            EvilBurger burger;
            if (burger = item.GetComponent<EvilBurger>())
            {

                burger.OnMakeSound += _PlayAudio;
                count++;
            }
        }
        
    }

    private void _PlayAudio(AudioClipType cliptype, AudioSource player)
    {
        AudioData clip_d = null;
        for (int iter = 0; iter < A_Data.Length; iter++)
        {
            if (A_Data[iter].ClipType == cliptype)
            {
                clip_d = A_Data[iter];
                break;
            }
        }

        if (clip_d != null)
        {
            player.clip = clip_d.Clip;
            player.loop = clip_d.Looping;
            player.volume = clip_d.Volume;
            player.PlayOneShot(clip_d.Clip);
        }

    }


}

