using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Rigidbody playerRigid;
    private float _moveSpeed;
    Transform target;
    GameObject thing;
    float _noise;
    float _attacktime;
    float _range, range;
    float _distance;

    float hp;
    float armor;

    Vector3 originpos;

    RaycastHit hit;

    public float GetHP()
    {
        return hp;
    }
    void chase()
    {
        if (_range > range)
        {
            _range -= Time.deltaTime;
        }
        _range = range;
        Collider[] cols = Physics.OverlapSphere(transform.position, _range);
        if (cols.Length > 0)
        {
            for (int i = 0; i < cols.Length; i++)
            {
                if (cols[i].tag == "Player")
                {
                    //Debug.Log("Found Player");
                    thing = cols[i].gameObject;

                    target = thing.transform;
                    _noise = thing.GetComponent<_4._Scripts.Player>().GetNoise();
                    if (_noise > range) { _range = range + (_noise / 10); }
                    // Move by Noise

                    //if (_noise > 10)
                    //{
                    //    transform.LookAt(target);
                    //    playerRigid.MovePosition(transform.position - ((transform.position - target.position).normalized
                    //        * Time.deltaTime * _moveSpeed));
                    //}

                    // Move by Range
                    _distance = (transform.position - target.position).magnitude;
                    if (_distance < _range - 5f)
                    {
                        transform.LookAt(target);
                        if (Physics.Raycast(transform.position + transform.forward, transform.forward, out hit, 1000))
                        {
                            Debug.DrawLine(transform.position, transform.forward * hit.distance, Color.red);
                            Debug.Log(hit.collider.gameObject.tag);
                        }
                        if (_distance > 2f && hit.collider.gameObject.tag == "Player")
                        {
                            playerRigid.MovePosition(transform.position - ((transform.position - target.position).normalized
                                * Time.deltaTime * _moveSpeed));
                        }
                    }
                }
            }
        }
        else
        {

        }
    }
    public void Getknockback(Vector3 dirvec,float power)
    {
        // transform.Translate(new Vector3(0, 0, 0));
        playerRigid.AddForce(dirvec * power);
        print("got knockback");
    }

    public void GetDamage(float dam)
    {
        hp -= dam;
    }

    void Start()
    {
        playerRigid = GetComponent<Rigidbody>();
        _moveSpeed = 20f;

        _noise = 0;
        _attacktime = 0;
        range = 10f;

        originpos = transform.position;

        hp = 100f;
        armor = 100f;
    }

    // Update is called once per frame
    void Update()
    {
        if (hp > 20) { chase(); }
        else
        {

        }
        if (hp <= 0)
        {
            Destroy(transform.parent.gameObject);
        }
    }
}

