using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AmmoUi : MonoBehaviour
{
    [SerializeField] private TMP_Text _tmp;
    //[SerializeField] private GameObject lostWindow;
    void Start()
    {
        Init();
    }
    void Init() 
    {
        _tmp = GetComponentInChildren<TMP_Text>();
        _tmp.text = "30";
        GameObject.FindGameObjectWithTag("Player").GetComponent<FPSController>().OnAmmoChange += this.AmmoChange;
    }

    
    private void AmmoChange(int new_ammo) 
    {
        _tmp.text = new_ammo.ToString();
    }
}
