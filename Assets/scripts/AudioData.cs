using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum AudioClipType 
{
    SHOOT,
    RELOAD,
    WALK,
    HIT_SOFT,
    HIT_HARD,
    DEAD,
    BGM       
}

[System.Serializable]
public class AudioData
{
    /// <summary>
    /// dfdf
    /// </summary>

    public AudioClip Clip;// {get; private set; }

    [Range(0,1)]
    public float Volume;// {get; private set; }

    public bool Looping;// { get; private set; }
    public AudioClipType ClipType;// { get; private set; }

}

