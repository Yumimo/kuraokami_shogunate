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
           IsGrounded = Physics.SphereCast(transform.position, m_groundCheckDistance, Vector3.down, out _, m_groundCheckDistance,m_groundLayer);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, m_groundCheckDistance);
            var endPoint = transform.position + Vector3.down * m_groundCheckDistance;
            Gizmos.DrawLine(transform.position, endPoint);
        }
    }
}
