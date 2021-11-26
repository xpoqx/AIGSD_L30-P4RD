using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    // Start is called before the first frame update

    private float _moveSpeed;
    Transform target, targetbyattack; // 인식 범위 안 타겟 / 고정용 타겟 
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
    
    void MeleeAttack() // 일단 플레이어 Melee랑 거의 동일하게 사용. 공격대상만 Player 
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

    public void Setdata(Vector3 pos) // 스폰지역 저장 (배회용)
    {
        originpos = pos;
        randpos = originpos;
    }
    public float GetHP() // hp 막대에서 hp 조회
    {
        return hp;
    }

    void Goattack() //플레이어가 직접적으로 보일 때 직진
    {
        playerRigid.MovePosition(transform.position - (_direction.normalized
            * Time.deltaTime * _moveSpeed));
    }
    void Goattack_BehindEnemy() //다른 적이 플레이어를 가릴 때 비켜가는 부분
    {
        playerRigid.MovePosition(transform.position - ((_direction - transform.right * 4).normalized
            * Time.deltaTime * _moveSpeed));
    }
    void Goattack_BehindWall() //벽이 플레이어를 가릴 때 움직이는 부분
    {
        // 초기값 0, 처음엔 계산에 관여 안하기 위해
        Vector3 _Lasthitpoint = new Vector3(0,0,0);

        // 반시계방향으로 벡터를 돌리면서 raycast, 벽이 끝나는 지점에서 탐색 반복문 종료.
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
                // 마지막 위치와 현재 레이저 위치가 크게 차이나면 다른 벽이라고 판단하고 마찬가지로 탐색 종료.
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
        //시계방향으로 raycast, 위와 동일
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

    void Roaming() //스폰지역 주변 배회하기
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

    public void Getknockback(Vector3 dirvec, float power) // 넉백
    {
        playerRigid.AddForce(dirvec * power);
        // print("got knockback");
    }

    public void GetDamage(float dam) // 대미지 >주변 다른 Enemy에게 플레이어 오브젝트 전달
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

    public void GetPlayerobject(Transform playertf) // 플레이어 오브젝트 확보(받기)
    {
        targetbyattack = playertf.transform;
    }

    void Start()
    {
        playerRigid = GetComponent<Rigidbody>();

        _moveSpeed = 10f; // 적 이동속도

        _noise = 0;
        _meleecd = 1f; // 근접공격 쿨타임
        range = 10f; // 기본 감지범위 (추격은 _range 로 넘겨준 다음 -5 + noise / 10 에 해당하는 범위안에서만 추격.)

        originpos = transform.position;

        hp = 100f; // 현재는 체력바가 체력이 많을수록 무한히 늘어남
        armor = 100f; // 미사용

        layerMask = 1 << LayerMask.NameToLayer("Environment"); // 벽만 인식하기위한 레이어마스크
        EnemyLayer = 1 << LayerMask.NameToLayer("Enemy"); // 다른 적만 무시하기위한 레이어마스크
        targetbyattack = null; // 자신 또는 주변 Enemy가 공격받으면 고정타겟화시킬 공간
        NearPlayer = false; // 주변에 Player가 없을 때 배회(Roaming)하기위한 변수

        animator = transform.GetChild(1).GetComponent<Animator>();
    }

    void Targeted() // 플레이어를 발견한 뒤 쫓아오는 단계
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
        // 공격이나 주변 적으로부터 타겟(플레이어) 직접 전달 없을 때
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
                        // 플레이어가 가진(발생시킨) noise를 지속적으로 감지.
                        // noise는 Player스크립트 내에서 지속적으로 감소해서 적도 인식하다가 멈출 수 있음
                        _noise = cols[i].gameObject.GetComponent<_4._Scripts.Player>().GetNoise();

                        // 기본 범위(range)와 그냥 비교해서 더 크면 노이즈/10 만큼 일시적으로 추격범위(_range) 증가
                        if (_noise > range) { _range = range + (_noise / 10); }

                        //플레이어와의 거리(_distance)가 추격범위(_range-5) 이내로 들어오면 상황에따라 추격 함수들 실행
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
        //직접적인 플레이어 전달 있을 때 (피격 또는 주변 적으로부터)
        else
        {
            NearPlayer = true;
            _direction = transform.position - targetbyattack.position;
            _distance = _direction.magnitude;
            transform.LookAt(targetbyattack);
            Targeted();
        }

        // 주변에 아무도 없고 타게팅도 안됐을 때, 배회
        if (NearPlayer == false) 
        {
            Roaming();
            if (roamcd > 0)
            {
                roamcd -= Time.deltaTime;
            }
        }

        // 근접공격 쿨타임
        if (_meleecd > 0)
        {
            _meleecd -= Time.deltaTime;
        }
        
        animator.SetFloat(("movingspeed"),playerRigid.velocity.magnitude/Time.deltaTime*100f);

        // 체력 0이면 사망
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

