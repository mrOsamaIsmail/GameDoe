using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
public class BurgerPooler : MonoBehaviour
{
    [SerializeField] private List<Vector3> _spawn_positions; //= new List<Vector3>();
    [SerializeField] private GameObject BurgerPrefab;
    [SerializeField] private int MaxBurgerAtOnce;
    [SerializeField] private float InitialSpawnInterval;

    private IObjectPool<GameObject> BurgerPool;

    private void Awake()
    {
        Init();
    }
    private void Init()
    {
        //_spawn_positions = new List<Vector3>();
        BurgerPool = new ObjectPool<GameObject>
            (
               this._creator,
                this._getter,
                this._release,
                this._destroy,
                true,
                MaxBurgerAtOnce,20
            );
        EvilBurger.OnDeath += ABurgerDied;


        StartCoroutine("InitialSpawn", InitialSpawnInterval);    

    }
    private void Update()
    {
       
    }
    void ABurgerDied(EvilBurger evilburger)
    {
        
        //pool dead
        BurgerPool.Release(evilburger.gameObject);
        //and get another
        BurgerPool.Get(); 
    }
    Vector3 GetSpawnPos() 
    {
        int random_int = Random.Range(0, _spawn_positions.Count);
        Vector3 pos = _spawn_positions[random_int];
        if ((int)Time.time % 2 == 0)
        {
            pos.z += .01f;
        }
        else
        {
            pos.x += .01f;
        }
        return _spawn_positions[random_int];
    }

    private IEnumerator InitialSpawn(int interval) 
    {
        for (int i = 0; i < MaxBurgerAtOnce; i++) 
        {
            BurgerPool.Get();
            yield return new WaitForSeconds(interval);
        
        }
    }
    GameObject _creator() 
    {
        //instantiation
        Vector3 v = GetSpawnPos();
        GameObject go = Instantiate(BurgerPrefab,
                                       Vector3.zero,
                                       Quaternion.identity);
        go.transform.position = v;
        go.transform.parent = this.transform;
        return go;
    }
    void _getter(GameObject evilburger) 
    {
        //out of pool
        evilburger.SetActive(true);
        //evilburger.transform.parent = this.transform;
        evilburger.transform.position = GetSpawnPos();
    }
    void _release(GameObject evilburger) 
    {
        //back to pool
        evilburger.gameObject.SetActive(false);
    }
    void _destroy(GameObject evilburger) 
    {
        Destroy(evilburger);
    }
}
