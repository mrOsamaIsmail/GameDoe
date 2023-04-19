using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioMANAGER : MonoBehaviour, IObserver
{
    [SerializeField] private AudioClip HitGroundClip;
    [SerializeField] private Subject sub;
    private AudioSource a_src;
    private void Awake()
    {
        Init();
    }
    private void Init() 
    {
        a_src= GetComponent<AudioSource>();
        sub = GameObject.FindWithTag("Player").GetComponent<Subject>();
        sub.Subscribe(this);
    }
    private void OnDisable()
    {
        sub.UnSubscribe(this);
    }
    public void OnNotify(Notification context)
    {
        if(context == Notification.GROUND_HIT)
            a_src.PlayOneShot(HitGroundClip);

    }
}
