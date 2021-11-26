using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeScript : MonoBehaviour
{
    Vector3 direction;
    float power;
    Transform _playertf;
    float damage;
    float MeleeRange;
    string target_name;

    // nameoftarget으로 피격자가 플레이어, 에너미 둘중 누구인지 판단함(팀킬방지)
    public void Setdata(float pow, float dam, GameObject playerOBJECT, Vector3 dir, string nameoftarget)
    {
        power = 300f;
        //damage = dam;
        _playertf = playerOBJECT.transform;
        direction = dir;
        target_name = nameoftarget;
    }
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 0.5f);
        damage = 20f;
        MeleeRange = 0.2f;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = _playertf.position + _playertf.forward * MeleeRange
                    - _playertf.right * 0.4f + new Vector3(0f, 0.4f, 0f);
        transform.rotation = _playertf.rotation ;
        if (MeleeRange < 1f)
        {
            MeleeRange += Time.deltaTime * 5f;
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == target_name ) 
        {
            if (target_name == "Enemy")
            {
                var Escript = other.gameObject.GetComponent<EnemyScript>();
                Escript.Getknockback(direction, power);
                Escript.GetPlayerobject(_playertf);
                Escript.GetDamage(damage);
            }
            else if ( target_name== "Player")
            {
                var Pscript = other.gameObject.GetComponent<_4._Scripts.Player>();
                Pscript.Getknockback(direction, power);
                Pscript.GetDamage(damage);

            }
        }
    }
}
