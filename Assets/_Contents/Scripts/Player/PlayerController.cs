using UnityEngine;

namespace Kuraokami
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] InputReader m_inputReader;
        [SerializeField] PlayerData m_data;
        [SerializeField] Animator m_animator;

        Rigidbody m_rigidbody;

        private void Awake()
        {
            m_rigidbody = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            HandleMovement();
            Flip();
        }

        private void HandleMovement()
        {
            var moveInput = m_inputReader.Direction.x;
            var moveDir = new Vector3(moveInput, 0f, 0f).normalized;

            var velocity = new Vector3(moveDir.x * m_data.m_moveSpeed, m_rigidbody.linearVelocity.y, 0f);
            m_rigidbody.linearVelocity = velocity;
        }

        private void Flip()
        {
            var moveInput = m_inputReader.Direction.x;
            if (moveInput == 0) return;
            var scale = transform.localScale;
            scale.z = Mathf.Sign(moveInput) * Mathf.Abs(scale.z);
            transform.localScale = scale;
        }
    }
}