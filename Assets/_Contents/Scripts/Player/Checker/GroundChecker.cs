using UnityEngine;

namespace Kuraokami
{
    public class GroundChecker : MonoBehaviour
    {
        [SerializeField] private float m_groundCheckDistance;
        [SerializeField] private float m_radius;
        [SerializeField] private LayerMask m_groundLayer;

        [SerializeField]public bool IsGrounded { get; private set; }
        private void Update()
        {
           IsGrounded = Physics.CheckSphere(transform.position, m_radius, m_groundLayer);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = IsGrounded? Color.green : Color.red;
            Gizmos.DrawWireSphere(transform.position, m_radius);
            // var endPoint = transform.position + Vector3.down * m_groundCheckDistance;
            // Gizmos.DrawLine(transform.position, endPoint);

        }
    }
}
