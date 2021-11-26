using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    // Start is called before the first frame update

    private float _moveSpeed;
    Transform target, targetbyattack; // �ν� ���� �� Ÿ�� / ������ Ÿ�� 
    float _noise, _range, range, _distance;
    float hp, armor;

    Vector3 originpos, _direction, _walldir;
    Transform _waypoint2, _meleeweapon;

    RaycastHit hit, _hit, _Lhit, _Rhit;
    public int layerMask, EnemyLayer;

    int selecL, selecR;
    Vector3 _waypointL, _waypointR;
    float DilL, DilR;

    bool NearPlayer;
    float roamcd;
    Vector3 randpos;

    float _meleecd;

    [SerializeField]
    private Rigidbody playerRigid;
    Vector3 testVec3;
    public GameObject rp, MeleePrefab;

    public Animator animator;
    
    void MeleeAttack() // �ϴ� �÷��̾� Melee�� ���� �����ϰ� ���. ���ݴ�� Player 
    {
        if (_meleecd <= 0)
        {
            Vector3 MeleePos = transform.position + playerRigid.transform.forward * 0.5f
                + playerRigid.transform.right * 0.5f
                + new Vector3(0f, 0.4f, 0f);
            var Melee = Instantiate(MeleePrefab, MeleePos, Quaternion.identity).GetComponent<MeleeScript>();
            Melee.Setdata(10, 40, gameObject, transform.forward, "Player");
            _meleeweapon = Melee.transform;
            _meleecd = 1f;
        }
    }

    public void Setdata(Vector3 pos) // �������� ���� (��ȸ��)
    {
        originpos = pos;
        randpos = originpos;
    }
    public float GetHP() // hp ���뿡�� hp ��ȸ
    {
        return hp;
    }

    void Goattack() //�÷��̾ ���������� ���� �� ����
    {
        playerRigid.MovePosition(transform.position - (_direction.normalized
            * Time.deltaTime * _moveSpeed));
    }
    void Goattack_BehindEnemy() //�ٸ� ���� �÷��̾ ���� �� ���Ѱ��� �κ�
    {
        playerRigid.MovePosition(transform.position - ((_direction - transform.right * 4).normalized
            * Time.deltaTime * _moveSpeed));
    }
    void Goattack_BehindWall() //���� �÷��̾ ���� �� �����̴� �κ�
    {
        // �ʱⰪ 0, ó���� ��꿡 ���� ���ϱ� ����
        Vector3 _Lasthitpoint = new Vector3(0,0,0);

        // �ݽð�������� ���͸� �����鼭 raycast, ���� ������ �������� Ž�� �ݺ��� ����.
        for (int i = 0; i < 200; i++) 
        {
            if (i < 101)
            {
                _walldir = transform.forward - ((float)i / 30 * transform.right);
            }
            else
            {
                _walldir = -transform.forward - ((float)((200 - i) / 30) * transform.right);
            }

            if (Physics.Raycast(transform.position, _walldir, out _hit, _distance * 2, layerMask))
            {
                // ������ ��ġ�� ���� ������ ��ġ�� ũ�� ���̳��� �ٸ� ���̶�� �Ǵ��ϰ� ���������� Ž�� ����.
                if (_Lasthitpoint.magnitude!= 0 && (_Lasthitpoint - _hit.point).magnitude > 2 )
                {
                    break;
                }
                _waypointL = _hit.point + new Vector3(-_walldir.z, _walldir.y, _walldir.x).normalized * 1.2f;
                selecL = i;
                _Lasthitpoint = _hit.point;
            }
            else
            {
                //Debug.Log("find way left");
                break;
            }
        }

        _Lasthitpoint = new Vector3(0, 0, 0);
        //�ð�������� raycast, ���� ����
        for (int i = 0; i < 201; i++) 
        {
            if (i < 101)
            {
                _walldir = transform.forward + ((float)i / 30 * transform.right);
            }
            else
            {
                _walldir = -transform.forward + ((float)((200 - i) / 30) * transform.right);
            }

            if (Physics.Raycast(transform.position, _walldir, out _hit, _distance * 2, layerMask))
            {
                if (_Lasthitpoint.magnitude != 0 && (_Lasthitpoint - _hit.point).magnitude > 2)
                {
                    break;
                }
                _waypointR = _hit.point + new Vector3(_walldir.z, _walldir.y, -_walldir.x).normalized * 1.2f;
                selecR = i;
                _Lasthitpoint = _hit.point;
            }
            else
            {
                //Debug.Log("find way right");
                break;
            }
        }
        if (DilL + DilR < 2f)
        {
            if (selecL > selecR)
            {
                Instantiate(rp, _waypointR, Quaternion.identity);
                playerRigid.MovePosition(transform.position - ((transform.position - _waypointR).normalized * Time.deltaTime * _moveSpeed));
                DilR += Time.deltaTime;
            }
            else
            {
                Instantiate(rp, _waypointL, Quaternion.identity);
                playerRigid.MovePosition(transform.position - ((transform.position - _waypointL).normalized * Time.deltaTime * _moveSpeed));
                DilL += Time.deltaTime;
            }
        }
        else
        {
            if (DilL > DilR)
            {
                Instantiate(rp, _waypointL - transform.forward, Quaternion.identity);
                playerRigid.MovePosition(transform.position - ((transform.position - _waypointL).normalized * Time.deltaTime * _moveSpeed));
            }
            else
            {
                Instantiate(rp, _waypointR - transform.forward, Quaternion.identity);
                playerRigid.MovePosition(transform.position - ((transform.position - _waypointR).normalized * Time.deltaTime * _moveSpeed));
            }
        }

    }

    void Roaming() //�������� �ֺ� ��ȸ�ϱ�
    {
        if (roamcd <= 0)
        {
            float _x = Random.Range(-5f, 5f);
            float _z = Random.Range(-5f, 5f);
            randpos = originpos + new Vector3(_x, transform.position.y, _z);
            roamcd = 3f;
        }

        if ((transform.position - randpos).magnitude > 0.5f)
        {
            transform.LookAt(randpos);
            playerRigid.MovePosition(transform.position - ((transform.position - randpos).normalized
                                                           * Time.deltaTime * _moveSpeed * 0.5f));
        }
        
    }

    public void Getknockback(Vector3 dirvec, float power) // �˹�
    {
        playerRigid.AddForce(dirvec * power);
        // print("got knockback");
    }

    public void GetDamage(float dam) // ����� >�ֺ� �ٸ� Enemy���� �÷��̾� ������Ʈ ����
    {
        transform.parent.GetComponent<EnemyParentScript>().playdamagedsound(transform.position);
        hp -= dam;
        Collider[] _cols = Physics.OverlapSphere(transform.position, _range);
        if (_cols.Length > 0)
        {
            for (int i = 0; i < _cols.Length; i++)
            {
                if (_cols[i].tag == "Enemy")
                {
                    _cols[i].transform.gameObject.GetComponent<EnemyScript>().GetPlayerobject(targetbyattack);
                }
            }
        }
    }

    public void GetPlayerobject(Transform playertf) // �÷��̾� ������Ʈ Ȯ��(�ޱ�)
    {
        targetbyattack = playertf.transform;
    }

    void Start()
    {
        playerRigid = GetComponent<Rigidbody>();

        _moveSpeed = 10f; // �� �̵��ӵ�

        _noise = 0;
        _meleecd = 1f; // �������� ��Ÿ��
        range = 10f; // �⺻ �������� (�߰��� _range �� �Ѱ��� ���� -5 + noise / 10 �� �ش��ϴ� �����ȿ����� �߰�.)

        originpos = transform.position;

        hp = 100f; // ����� ü�¹ٰ� ü���� �������� ������ �þ
        armor = 100f; // �̻��

        layerMask = 1 << LayerMask.NameToLayer("Environment"); // ���� �ν��ϱ����� ���̾��ũ
        EnemyLayer = 1 << LayerMask.NameToLayer("Enemy"); // �ٸ� ���� �����ϱ����� ���̾��ũ
        targetbyattack = null; // �ڽ� �Ǵ� �ֺ� Enemy�� ���ݹ����� ����Ÿ��ȭ��ų ����
        NearPlayer = false; // �ֺ��� Player�� ���� �� ��ȸ(Roaming)�ϱ����� ����

        animator = transform.GetChild(1).GetComponent<Animator>();
    }

    void Targeted() // �÷��̾ �߰��� �� �Ѿƿ��� �ܰ�
    {
        if (Physics.Raycast(transform.position, transform.forward, out hit, _distance, ~EnemyLayer))
        {
            if (hit.collider.gameObject.tag == "Player")
            {
                if (Physics.Raycast(transform.position, transform.forward, out hit, _distance, EnemyLayer))
                {
                    Goattack_BehindEnemy();
                }
                else
                {
                    if (_distance > 1.5f)
                    {
                        DilL = 0;
                        DilR = 0;
                        Goattack();
                    }
                    if (_distance < 2f)
                    {
                        MeleeAttack();
                    }
                }
            }
            else if (hit.collider.gameObject.tag == "Wall")
            {
                if (_distance < (_range - 5f) / 1.4f)
                {
                    Goattack_BehindWall();
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        NearPlayer = false;
        // �����̳� �ֺ� �����κ��� Ÿ��(�÷��̾�) ���� ���� ���� ��
        if (targetbyattack == null)
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
                        NearPlayer = true;
                        target = cols[i].gameObject.transform;
                        // �÷��̾ ����(�߻���Ų) noise�� ���������� ����.
                        // noise�� Player��ũ��Ʈ ������ ���������� �����ؼ� ���� �ν��ϴٰ� ���� �� ����
                        _noise = cols[i].gameObject.GetComponent<_4._Scripts.Player>().GetNoise();

                        // �⺻ ����(range)�� �׳� ���ؼ� �� ũ�� ������/10 ��ŭ �Ͻ������� �߰ݹ���(_range) ����
                        if (_noise > range) { _range = range + (_noise / 10); }

                        //�÷��̾���� �Ÿ�(_distance)�� �߰ݹ���(_range-5) �̳��� ������ ��Ȳ������ �߰� �Լ��� ����
                        _direction = transform.position - target.position;
                        _distance = _direction.magnitude;
                        if (_distance < _range - 5f)
                        {
                            transform.LookAt(target);
                            Targeted();
                        }
                    }

                }
            }

        }
        //�������� �÷��̾� ���� ���� �� (�ǰ� �Ǵ� �ֺ� �����κ���)
        else
        {
            NearPlayer = true;
            _direction = transform.position - targetbyattack.position;
            _distance = _direction.magnitude;
            transform.LookAt(targetbyattack);
            Targeted();
        }

        // �ֺ��� �ƹ��� ���� Ÿ���õ� �ȵ��� ��, ��ȸ
        if (NearPlayer == false) 
        {
            Roaming();
            if (roamcd > 0)
            {
                roamcd -= Time.deltaTime;
            }
        }

        // �������� ��Ÿ��
        if (_meleecd > 0)
        {
            _meleecd -= Time.deltaTime;
        }
        
        animator.SetFloat(("movingspeed"),playerRigid.velocity.magnitude/Time.deltaTime*100f);

        // ü�� 0�̸� ���
        if (hp <= 0)
        {
            transform.parent.GetComponent<EnemyParentScript>().playdiesound(transform.position);
            Destroy(transform.parent.gameObject,1f);
            Destroy(transform.parent.GetChild(1).gameObject);
            Destroy(gameObject);
            if (_meleeweapon != null)
            {
                Destroy(_meleeweapon.gameObject);
            }
        }
    }
}

