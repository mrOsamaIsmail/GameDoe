using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUi : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    
    void Start()
    {
        Init();   
    }


    void Init() 
    {
        _slider = GetComponent<Slider>();
        _slider.value = 100;
        GameObject.FindGameObjectWithTag("Player").GetComponent<FPSController>().OnDamage += this.OnHealthChange;
        
    }
    
   

    private void OnHealthChange(float value) 
    {
        _slider.value = value;
        if (value <= 0) 
        {
            UI.Instance.ShowLostMenu();
        }
    }
}
