using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject EnemyPrefab;
    public float SpawnTime,_a,_b,_Enemyrange;
    int inrange;
    int spawn_range;

    public bool NearSpawn, isCrystal, infSpawn;
    
    private int spawn_amount;

    public GameObject[] EnemiesFromThis;

    public GameObject CrystalPrefab, Crystal;
    
    public void SetAmount(int amt)
    {
        spawn_amount = amt;
        //print("Set amount!");
    }
    
    void Spawn( Vector3 Posit)
    {
        var Enemy = Instantiate(EnemyPrefab, Posit, Quaternion.identity).gameObject.transform
            .transform.GetChild(0).GetComponent<EnemyScript>();
        Enemy.Setdata(transform.position,_Enemyrange);
        //print("set enemyrange to" + _Enemyrange);
        EnemiesFromThis[inrange]=Enemy.gameObject;
        if (!infSpawn)
        {
            inrange++;
        }
        

    }

    void CheckEnemies()
    {
        if (infSpawn)
        {
            
        }
        else
        { 
            bool isEmpty = true;
            for (int b = 0; b < EnemiesFromThis.Length; b++)
            {
                if (EnemiesFromThis[b] != null)
                {
                    isEmpty = false;
                }
            }

            if (isEmpty)
            {
                if (isCrystal)
                {
                    Crystal=Instantiate(CrystalPrefab, transform.position-new Vector3(0,0.9f,0), Quaternion.identity);
                }
                Destroy(gameObject);
            }
        }
        
    }

    public void DelayedDestroy(float sec)
    {
        Destroy(gameObject,sec);
    }
        
    void Start()
    {
        _Enemyrange = 10f;
        isCrystal = false;
        infSpawn = true;
        spawn_amount = 0;
        EnemiesFromThis = new GameObject[5];
        inrange = 0;
        Spawn(transform.position);
        SpawnTime = 2;
        spawn_range = 2;
        NearSpawn = false;
        

    }

    // Update is called once per frame
    void Update()
    {
        NearSpawn = false;
        if(infSpawn)
        {
            inrange = 0;
            SpawnTime -= Time.deltaTime;
            Collider[] _cols = Physics.OverlapSphere(transform.position, 10f);
            for (int i = 0; i < _cols.Length; i++)
            {
                if (_cols[i].tag == "Enemy")
                {
                    inrange++;
                }
            }
        }

        if (inrange > 0)
        {
            CheckEnemies();
        }
        
        
        if (inrange < spawn_amount)
        {
            if (SpawnTime > 0)
            {
                SpawnTime -= Time.deltaTime;
            }
            else
            {
                _a = Random.Range(-spawn_range, spawn_range);
                _b = Random.Range(-spawn_range, spawn_range);
                Vector3 Pos = new Vector3(transform.position.x+_a,transform.position.y, transform.position.z+_b);
                Collider[] cols = Physics.OverlapSphere(Pos, 0.5f);
                for (int i = 0; i < cols.Length; i++)
                {
                    if (cols[i].tag == "Spawner")
                    {
                        NearSpawn = true;
                    }
                }
                if (!NearSpawn)
                {
                    Spawn(Pos);
                    SpawnTime = 3;
                }
            }
        }
    }
}
