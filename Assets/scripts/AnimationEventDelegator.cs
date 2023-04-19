using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class AnimationEventDelegator : MonoBehaviour
{
    private FPSController _controller;
    void Start()
    {
        Init();
    }

    void Init()
    {
        _controller = GetComponentInParent<FPSController>();
    }

    public void OnEvent(string Type)
    {
        //delegate to parent upon type passed TAKEN STRAIGHT FROM ANIMATION
        if (Type.Equals("shoot"))
            _controller.invoke_sound_event(AudioClipType.SHOOT);

        else if (Type.Equals("reload"))
        {
            _controller.invoke_sound_event(AudioClipType.RELOAD);
            _controller.invokeAmmoAnimDelegator(30);

        }
        else if (Type.Equals("RayCast"))
            _controller.Fire();

    }
    
}
