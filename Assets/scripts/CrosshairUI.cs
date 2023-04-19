using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CrosshairUI : MonoBehaviour
{
    [SerializeField] float _bullet_max_distance;
    [SerializeField] LayerMask _react_to_shots;
    private Camera _cam;
    
    void Start()
    {
        _cam = Camera.main;
    }

    
    void Update()
    {
        CheckforEnemies();
    }
    void CheckforEnemies() 
    {
        Ray _ray = _cam.ViewportPointToRay(new Vector3(.5f, .5f, 0));
        RaycastHit hit;
        if (Physics.Raycast(_ray, out hit, _bullet_max_distance, _react_to_shots))
        {
            
                transform.localScale = Vector3.one;
            
           
        }
        else
        {
            transform.localScale = Vector3.one * .5f;
        }

    }

}
