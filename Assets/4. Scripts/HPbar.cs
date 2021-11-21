using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPbar : MonoBehaviour
{
    GameObject thisenemy;
    float hp;
    Vector3 enemypos;
    [SerializeField]

    Quaternion enemyrot;
    // Start is called before the first frame update
    void Start()
    {
        thisenemy = gameObject.transform.parent.GetChild(0).gameObject;
        
    }

    // Update is called once per frame
    void Update()
    {
        enemypos = thisenemy.transform.position;
        //enemyrot = thisenemy.transform.rotation;
        hp = thisenemy.GetComponent<EnemyScript>().GetHP();
        transform.localScale = new Vector3 (hp / 100,1,1);
        transform.position = new Vector3(enemypos.x, enemypos.y+1.2f, enemypos.z+0.5f);
    }
}
