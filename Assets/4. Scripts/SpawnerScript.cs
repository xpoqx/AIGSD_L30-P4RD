using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject EnemyPrefab;
    float SpawnTime;
    float _a, _b;
    int inrange;
    int spawn_range;
    [SerializeField]
    int spawn_amount;
    void Spawn()
    {
        _a = Random.Range(-spawn_range, spawn_range);
        _b = Random.Range(-spawn_range, spawn_range);
        Vector3 Pos = new Vector3(transform.position.x+_a,transform.position.y, transform.position.z+_b);
        var Enemy = Instantiate(EnemyPrefab, Pos, Quaternion.identity).gameObject.transform
            .transform.GetChild(0).GetComponent<EnemyScript>();
        Enemy.Setdata(transform.position);

    }

        
    void Start()
    {
        Spawn();
        SpawnTime = 1;
        spawn_range = 5;
    }

    // Update is called once per frame
    void Update()
    {
        inrange = 0;
        SpawnTime -= Time.deltaTime;
        Collider[] _cols = Physics.OverlapSphere(transform.position, 10f);
        for(int i = 0; i < _cols.Length; i++)
        {
            if (_cols[i].tag == "Enemy")
            {
                inrange++;
            }
        }

        
        if (inrange < spawn_amount)
        {
            if (SpawnTime > 0)
            {
                SpawnTime -= Time.deltaTime;
            }
            else
            {
                Spawn();
                SpawnTime = 5;
            }
            
        }

    }
}
