using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    bool isFire;
    Vector3 direction;
    [SerializeField]
    float speed = 25f;
    float power;

    float damage;
    float size;

    public void Setdata(float pow,float dam,float siz)
    {
        power = pow;
        damage = dam;
        size = siz;
    }

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 2f);
    }

    // Update is called once per frame
    void Update()
    {
        if (isFire)
        {
            transform.Translate(direction * Time.deltaTime * speed);
        }
    }

    public void Fire(Vector3 dir)
    {
        direction = dir;
        isFire = true;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Bullet>() == null && other.tag != "Player" 
            && other.tag != "Weapon")
        {
            Destroy(gameObject);
        }
        if (other.tag == "Enemy")
        {
            other.gameObject.GetComponent<EnemyScript>().Getknockback(direction,power);
            other.gameObject.GetComponent<EnemyScript>().GetDamage(damage);
        }
    }
}
