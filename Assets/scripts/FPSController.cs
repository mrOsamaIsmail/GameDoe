using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using event_types;
namespace event_types
{
    public delegate void ShootDelegate(ShotObjectArgs arg);
    public delegate void HealthDelegate(float new_val);
    public delegate void AmmoDelegate(int bullet_count);
}
public class ShotObjectArgs
{
    //more to be added if needed
    //GameObject _Targeted;
    
    public RaycastHit _RayCastHit { get; private set; }
    public ShotObjectArgs(RaycastHit _hit)
    {
        this._RayCastHit = _hit;
        
    }
}
public class FPSController : Singleton<FPSController>
{

    

    public event HealthDelegate OnDamage;
    public event AudioDelegate OnMakeSound;
    public event ShootDelegate OnShoot;
    public event AmmoDelegate OnAmmoChange;
    

    [SerializeField] private float _walk_speed;
    [SerializeField] private float _run_speed;
    [SerializeField] private float _mouse_sensitivity;
    [SerializeField] private float _jump_height;
    [SerializeField] private float ground_check_distance;
    [SerializeField] private bool _grounded;

    
    public float Health { get; private set; }

    [SerializeField] private bool _lock_mouse;

    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private LayerMask _canJumpOn;
    

    [Header("Shooting Parameters")]
    [SerializeField] private float _bullet_max_distance;
    [SerializeField] private LayerMask _react_to_shots;
    


    private Rigidbody _rb;
    private Animator _rig_anim;
    private Camera _cam;
    private ParticleSystem _ps;

    
    private Vector2 _input_vector;
    private Vector2 _look_vector;
    private float current_speed;
    private float current_cam_pitch;
    private float current_cam_yaw;
    private int current_ammo;
    //is set true for just a .001 sec when the player hits jump
    //just for the sake of the jump launch being in the fixedupdate
    [SerializeField]private bool jump_trigger;
    
    void Start()
    {
        Init();
    }

    

    void FixedUpdate()
    {

        CalculateGravity();
        if (Health > 0 && _grounded)
        {
            Move();
            if (jump_trigger)
                Jump();

        }
    }

    
    void Init() 
    {
        _rb = GetComponent<Rigidbody>() ;
        _rig_anim = GetComponentInChildren<Animator>();
        _cam = GetComponentInChildren<Camera>();
        _audioSource= GetComponent<AudioSource>();
        _ps = GetComponentInChildren<ParticleSystem>();
        

        OnAmmoChange += this.OnAmmoChanged;

        if(_lock_mouse)
            Cursor.lockState = CursorLockMode.Locked;


        current_speed = _walk_speed;
        Health = 100;
        current_ammo = 30;
        OnDamage += this.OnHealthChange;
        jump_trigger = false;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Enemy"))
        {
            OnDamage?.Invoke(Health - 10);
            //_rb.AddForce((transform.position - collision.transform.position) * 3, ForceMode.Impulse);
        }
    }
    void CalculateGravity()
    {
        if (Physics.Raycast(transform.position, Vector3.down, ground_check_distance,_canJumpOn))
        {
            
            _grounded = true;
        }
        else 
        {
            
            _grounded = false;
        }
    }
    private void Move() 
    {
        
            _input_vector.Normalize();
            float x = _input_vector.x;
            float z = _input_vector.y;

            x = Input.GetAxis("Horizontal");
            z = Input.GetAxis("Vertical");
            Vector3 movement = (transform.right*x )+ (transform.forward*z);
        movement *= current_speed;

        
            
            _rb.velocity = movement ;
        
    }
    private void LookAround()
    {
        if (Health > 0)
        {
            _look_vector.Normalize();

            current_cam_pitch += -_look_vector.y * _mouse_sensitivity * Time.deltaTime;
            current_cam_pitch = Mathf.Clamp(current_cam_pitch, -80, 80);

            current_cam_yaw += _look_vector.x * _mouse_sensitivity * Time.deltaTime;



            Quaternion cam_quat = Quaternion.Euler(current_cam_pitch, 0, 0);
            Quaternion quat = Quaternion.Euler(0, current_cam_yaw, 0);

            _cam.transform.localRotation = cam_quat;
            transform.localRotation = quat;


           
        }
    }
    
    
    internal void Fire() 
    {
        if (Health > 0)
        {
            if (current_ammo <= 0)
            {
                _rig_anim.SetTrigger("Reload");
                OnAmmoChange?.Invoke(30);
                return;
            }
            _ps.Play();
            Ray _ray = _cam.ViewportPointToRay(new Vector3(.5f, .5f, 0));
            RaycastHit hit;

            if (Physics.Raycast(_ray, out hit, _bullet_max_distance, _react_to_shots))
            {
                //Debug.Log(hit.transform.name);


                OnShoot?.Invoke(new ShotObjectArgs(hit));
            }
            OnAmmoChange?.Invoke(current_ammo-1);
        }
        
    }
    private void StopFire() 
    {
        _rig_anim.SetBool("Shoot", false);
    }
    private void Jump() 
    {
        if (_grounded)
        {
            _rb.AddForce(Vector3.up * _jump_height, ForceMode.Impulse);
            
        }
        jump_trigger = false;
    }


    #region Event and stuff
    internal void invokeAmmoAnimDelegator(int count) 
    {
        OnAmmoChange?.Invoke(count);  
    }
    private void OnHealthChange(float new_v)
    {
        this.Health = new_v;
    }
    
    private void OnAmmoChanged(int count) 
    {
        this.current_ammo = count;
    }
    #endregion
    #region sound_Emmiting
    internal void invoke_sound_event(AudioClipType _type)
    {
        OnMakeSound?.Invoke(_type, _audioSource);
    }
    
    #endregion


    #region Input Receivers
    public void OnMove(InputAction.CallbackContext context) 
    {
        if (context.canceled)
        {
            _input_vector = Vector2.zero;
           
        }
        else
        {
            _input_vector = context.ReadValue<Vector2>();
            
        }
    }
    public void OnLook(InputAction.CallbackContext context)
    {
        _look_vector = context.ReadValue<Vector2>();
        LookAround();
    }
    public void OnFire(InputAction.CallbackContext context) 
    {
        if (Health > 0)
        {
            if (context.started)
                //actual shooting call happens in the animator as an animation event
                _rig_anim.SetBool("Shoot", true);

            else if (context.canceled)
                StopFire();
        }
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if (Health > 0)
        { 
            if (context.started)
            {
                jump_trigger = true;
               
            } 
        }  
    }
    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            current_speed = _run_speed;
            
        }
        else if (context.canceled)
        {
            
            current_speed = _walk_speed;
        }
    }
    public void OnReload(InputAction.CallbackContext context)
    {
        if (Health > 0)
        {
            if (context.performed)
                _rig_anim.SetTrigger("Reload");
        }
    }
    #endregion


}
