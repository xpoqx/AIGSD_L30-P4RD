using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Serialization;

namespace _4._Scripts
{
    public class Player : MonoBehaviour
    {

        [SerializeField] private Rigidbody playerRigid;
        private float _moveSpeed;
        [SerializeField] private Camera thirdCamera;
        private float _LMcd,_Dashcd;
        float noise;
        float bpower, bdamage, bsize;
     
        

        public GameObject myWeapon;

        public GameObject bulletPrefab;

        public float GetNoise()
        {
            return this.noise;
        }
        void Fire()
        {
            if (Input.GetMouseButton(0) && _LMcd <=0)
            {
                Vector3 firePos = transform.position + playerRigid.transform.forward*0.3f
                    + playerRigid.transform.right *0.38f
                    + new Vector3(0f, 0.7f, 0f);
                var bullet = Instantiate(bulletPrefab, firePos, Quaternion.identity).GetComponent<Bullet>();
                bullet.Fire(playerRigid.transform.forward);
                bullet.Setdata(bpower,bdamage,bsize);
                _LMcd = 0.2f;
                noise = 100f;
            }
        }

        void Dash()
        {
            if (Input.GetKey(KeyCode.Space) && _Dashcd <=0)
            {
                _moveSpeed = UserData.MoveSpeed * 3;
                _Dashcd = 1;
                noise = 20f;
            }
        }

        private void Start()
        {
            _moveSpeed = UserData.MoveSpeed;
            _LMcd = 0.5f;
            _Dashcd = 1f;

            bpower = 100f;
            bdamage = 20f;
            bsize = 1f;
        }

        private void FixedUpdate()
        {
            // move
            
            var moveInput = (new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"))).normalized;
            var moveVelocity = moveInput * _moveSpeed;
            playerRigid.MovePosition(transform.position + moveVelocity * Time.fixedDeltaTime);
            
            // dash
            {
                if (_Dashcd > 0)
                {
                    _Dashcd -= Time.deltaTime;
                    if (_moveSpeed > UserData.MoveSpeed)
                    {
                        _moveSpeed -= Time.deltaTime*50f;
                    }
                }
                if (_Dashcd <0.65)
                {
                    _moveSpeed = UserData.MoveSpeed;
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
            // Fire 
            {
                if (_LMcd > 0)
                {
                    _LMcd -= Time.deltaTime;
                }

                Fire();
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
            
        }
    }
}
