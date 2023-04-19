using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using event_types;
using System;
using System.Drawing;

namespace event_types
{
    public delegate void BurgerDelegate(EvilBurger burger);
}
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
public class EvilBurger : MonoBehaviour
{
    public event AudioDelegate OnMakeSound;
    public static event BurgerDelegate OnDeath;
    //public static event BurgerDelegate OnCreation;

    [SerializeField] private float _follow_speed;
    [SerializeField] private float _oscilating_freq;
    [SerializeField] private float _oscilating_range;
    [SerializeField] private float _hit_damage=10;
    [SerializeField] private float _knockback_power=10;

    

    private AudioSource _audio_src;

    private SpriteRenderer _mipmapIcon;
    private bool _Alive = true;
    private float _health = 100;
    private GameObject _Player;
    private Rigidbody _rb;
    private ParticleSystem _particleSystem;
    
    
    void FixedUpdate()
    {
        if (_Alive)
         FollowPlayer();
    }
    private void Awake()
    {
        InitVars();
    }
    private void OnEnable()
    {
        Init();
    }
    void FollowPlayer() 
    {
        
        Vector3 dir = _Player.transform.position - transform.position;
        dir.y = _oscilating_range * Mathf.Cos(Time.time * _oscilating_freq);
        dir *= _follow_speed * Time.deltaTime;
        
        _rb.velocity = dir;
    }

    void InitVars() 
    {
        
        _Player = GameObject.FindWithTag("Player");
        _rb = GetComponent<Rigidbody>();
        
        _mipmapIcon = GetComponentInChildren<SpriteRenderer>();
        _audio_src = GetComponent<AudioSource>();
        _particleSystem = GetComponentInChildren<ParticleSystem>();

    }
    
    void Init() 
    {
        _health = 100;
        _Alive = true;
        _mipmapIcon.gameObject.SetActive(true);
        _rb.useGravity = false;
        _Player.GetComponent<FPSController>().OnShoot += this.OnShot;
    }

    void Die() 
    {
        OnMakeSound?.Invoke(AudioClipType.HIT_HARD, _audio_src);
        _Alive = false;
        _mipmapIcon.gameObject.SetActive(false);
        _rb.useGravity= true;
        _Player.GetComponent<FPSController>().OnShoot -= this.OnShot;
        _particleSystem.Play();
        StartCoroutine("destroyAfter", 1);
        //
    }
    IEnumerator destroyAfter(int sec) 
    {
        yield return new WaitForSeconds(sec);
        OnDeath?.Invoke(this);
        
    }

    private void OnDisable()
    {
       
    }
    public void OnShot(ShotObjectArgs arg) 
    {
        if (arg._RayCastHit.transform.gameObject == gameObject) 
        {
            _particleSystem.Play();

            OnMakeSound?.Invoke(AudioClipType.HIT_SOFT, _audio_src);
            
            _health -= _hit_damage;
            //Debug.Log(_health);
            if (_health <= 0) 
            {
                Die();
            }
        }
    }
    
}
