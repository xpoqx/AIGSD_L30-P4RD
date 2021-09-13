using UnityEngine;
using UnityEngine.Serialization;

namespace _4._Scripts
{
    public class PlayerCamera : MonoBehaviour
    {
        // Reference
        private Player _player;
        [SerializeField] private Rigidbody cameraRigid;

        [SerializeField] private float smoothSpeed;
        [SerializeField] private Vector3 offset;

        private void Start()
        {
            _player = FindObjectOfType<Player>();
        }

        private void FixedUpdate()
        {
            if (_player == null) return;

            var desiredPosition = _player.transform.position + offset;

            var smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            
            transform.position = smoothedPosition;
        }
    }
}
