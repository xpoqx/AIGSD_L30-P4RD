using UnityEngine;
using UnityEngine.SceneManagement;

namespace _4._Scripts
{
    public class Player : MonoBehaviour
    {
        private float _moveSpeed;
        
        private float _LMcd, _Dashcd, _Flashcd, _RMcd, _knockbackcd;
        float bpower, bdamage, bsize, brpm, bnoise, noise, hp;
        

        GameObject w_stat;

        private GameObject Flash;
        private bool Flashis;
        //public float[] _flashint;

        [SerializeField] 
        private Camera thirdCamera;
        [SerializeField]
        private Rigidbody playerRigid;
        public GameObject myWeapon, bulletPrefab, MeleePrefab;

        public AudioClip shootsoundtouse,gunshootsound1,gunshootsound2,gunshootsound3,gunshootsound4
            ,reloadsoundtouse,gunreloadsound1,gunreloadsound2,gunreloadsound3,gunreloadsound4;
        public AudioSource audiosource;

        public int selectedweapon;
        
        public void Getknockback(Vector3 dirvec, float power)
        {
            if (_knockbackcd <= 0)
            {
                playerRigid.AddForce(dirvec * power);
                _knockbackcd = 0.5f;
            }
        }

        public void GetDamage(float dam) 
        {
            hp -= dam;
            Debug.Log("Player Got Damage!! "+ hp + "hp remaining");
        }

        void SwitchFlash() // 플래시라이트 켜고끄기
        {
            if (_Flashcd <= 0)
            {
                if (Input.GetKey(KeyCode.F))
                {
                    if (Flashis)
                    {
                        Flash.GetComponent<Light>().intensity = 0;
                        Flashis = false;
                    }
                    else
                    {
                        Flash.GetComponent<Light>().intensity = 40;
                        Flashis = true;
                    }
                    _Flashcd = 0.5f;
                    noise = 15f;
                }
            }
            else
            {
                _Flashcd -= Time.deltaTime;
            }
        } 

        public float GetNoise() // noise 리턴 함수
        {
            return this.noise;
        }


        private void Fire() // 불릿 발사 함수
        {
            if (Input.GetMouseButton(0) && _LMcd <= 0)
            {
                audiosource.PlayOneShot(shootsoundtouse,0.1f);
                var firePos = transform.position + playerRigid.transform.forward * 0.7f
                                                 + playerRigid.transform.right * 0.38f
                                                 + new Vector3(0f, 0.7f, 0f);
                var bullet = Instantiate(bulletPrefab, firePos, Quaternion.identity).GetComponent<Bullet>();
                bullet.Fire(playerRigid.transform.forward);

                // 무기의 넉백계수,데미지,사이즈(현재 미사용), 연사력, 플레이어오브젝트(적의 고정타게팅 용도) 전달
                bullet.Setdata(bpower, bdamage, bsize, brpm, gameObject); 

                _LMcd = brpm;
                noise = bnoise; // 무기 noise 만큼 증가해서 주변 적 추격범위가 늘어남 >> EnemyScript의 Update > 직접 전달 없을 때
            }
        }

        void Melee()
        {
            if (Input.GetMouseButton(1) && _RMcd <=0)
            {
                Vector3 MeleePos = transform.position + playerRigid.transform.forward * 0.5f
                    + playerRigid.transform.right * 0.5f
                    + new Vector3(0f, 0.4f, 0f);
                var Melee = Instantiate(MeleePrefab, MeleePos, Quaternion.identity).GetComponent<MeleeScript>();
                Melee.Setdata(150, 50, gameObject, transform.forward,"Enemy");
                _RMcd = 0.5f;
            }
        }

        void Dash()
        {
            if (Input.GetKey(KeyCode.Space) && _Dashcd <= 0)
            {
                _moveSpeed = UserData.MoveSpeed * 4;
                _Dashcd = 1;
                noise = 20f;
            }
        }

        private void Start()
        {
            _moveSpeed = UserData.MoveSpeed;
            _LMcd = 0.2f;
            _Dashcd = 0.2f;

            // Default values to test when start at this Scene first
            bpower = 100f;
            bdamage = 120f;
            bsize = 1f;
            brpm = 0.1f;
            bnoise = 100f;

            Flash =transform.GetChild(0).GetChild(0).gameObject;
            Flashis = false;
            //_flashint = new float[] { 0, 0 };
            _Flashcd = 1f;

            hp = 100;

            w_stat = GameObject.Find("Weapon_stat"); // 무기 선택씬에서 파괴되지않은 오브젝트로 무기스탯 받아옴
            var weaponstat = w_stat.GetComponent<W_statscript>();
            bdamage = weaponstat.w_damage;
            bpower = weaponstat.w_power;
            brpm = weaponstat.w_rpm;
            bnoise = weaponstat.w_noise;
            selectedweapon = weaponstat.selected;
            shootsoundtouse = gunshootsound1;
            if (selectedweapon == 0)
            {
                shootsoundtouse = gunshootsound1; 
                reloadsoundtouse = gunreloadsound1;
            }
            else if (selectedweapon == 1)
            {
                shootsoundtouse = gunshootsound2;
                reloadsoundtouse = gunreloadsound2;
            }
            else if (selectedweapon == 2)
            {
                shootsoundtouse = gunshootsound3;
                reloadsoundtouse = gunreloadsound3;
            }
            else
            {
                shootsoundtouse = gunshootsound4;
                reloadsoundtouse = gunreloadsound4;
            }
            

            audiosource = gameObject.GetComponent<AudioSource>();
            //gameObject.transform.GetChild(1).gameObject.
        }

        private void FixedUpdate()
        {
            // move

            var moveInput = (new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"))).normalized;
            var moveVelocity = moveInput * _moveSpeed;
            playerRigid.MovePosition(transform.position + moveVelocity * Time.fixedDeltaTime);

            // dash : 일시적으로 속도 4배 증가후 빠르게 감소. 0.35초 후(쿨타임이 0.65남을때) 원래속도로 고정
            
            { 
                if (_Dashcd > 0)
                {
                    _Dashcd -= Time.deltaTime;
                    if (_moveSpeed > UserData.MoveSpeed)
                    {
                        _moveSpeed -= Time.deltaTime * 50f;
                    }
                }
                if (_Dashcd < 0.65) 
                {
                    _moveSpeed = UserData.MoveSpeed * 0.7f; // AI 테스트용으로 임시로 4배속 걸어놓음
                }
                Dash();

            }
            // look
            {
                var ray = thirdCamera.ScreenPointToRay(Input.mousePosition);
                var groundPlane = new Plane(Vector3.up, Vector3.up);
                if (groundPlane.Raycast(ray, out var rayDistance))
                {
                    var point = ray.GetPoint(rayDistance);
                    Debug.DrawLine(ray.origin, point, Color.red);
                    var heightCorrectPoint = new Vector3(point.x, transform.position.y, point.z);
                    transform.LookAt(heightCorrectPoint);
                }
            }
            // Attack (Fire & Melee)
            {
                if (_LMcd > 0)
                {
                    _LMcd -= Time.deltaTime;
                }
                if (_RMcd > 0)
                {
                    _RMcd -= Time.deltaTime;
                }
                Fire();
                Melee();
            }
            // Weapon Change Input
            {
                var wheelInput = Input.GetAxis("Mouse ScrollWheel");

            }
            //Noise
            {
                if (noise > 0)
                {
                    noise -= Time.deltaTime * 20f;
                }
            }
            //Flashlight
            {
                SwitchFlash();
            }
            //넉백면역 쿨타임
            if (_knockbackcd > 0)
            {
                _knockbackcd -= Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.Escape))
            {
                SceneManager.LoadScene((1));
            }
            

        }
    }
}
