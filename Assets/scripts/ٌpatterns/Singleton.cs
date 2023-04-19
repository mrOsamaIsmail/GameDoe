using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] bool Keep_ACrossScenes;
    public static T Instance;
    private void Awake() 
    {
        RegisterSingleton();
    }
    private void RegisterSingleton()
    {


        if (Instance == null)
            Instance = this as T;
        else
            Destroy(gameObject);

        if (Keep_ACrossScenes) 
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
