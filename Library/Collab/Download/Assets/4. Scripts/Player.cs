using UnityEngine;
using UnityEngine.Animations;

namespace _4._Scripts
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private Rigidbody playerRigid;
        private float _moveSpeed;
        private Camera _camera;
    
        private void Start()
        {
            _moveSpeed = UserData.MoveSpeed;
            _camera = Camera.main;
        }

        private void FixedUpdate()
        {
            // move
            var moveInput = (new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"))).normalized;
            var moveVelocity = moveInput * _moveSpeed;
            playerRigid.MovePosition(transform.position + moveVelocity * Time.fixedDeltaTime);
            
            // look
            var ray = _camera.ScreenPointToRay(Input.mousePosition);
            var groundPlane = new Plane(Vector3.up, Vector3.up);
            if (groundPlane.Raycast(ray, out var rayDistance))
            {
                var point = ray.GetPoint(rayDistance);
                Debug.DrawLine(ray.origin, point, Color.red);
                this.LookAt(point);
            }
        }

        private void LookAt(Vector3 lookPoint)
        {
            var heightCorrectPoint = new Vector3(lookPoint.x, transform.position.y, lookPoint.z);
            transform.LookAt(heightCorrectPoint);
        }
    }
}
