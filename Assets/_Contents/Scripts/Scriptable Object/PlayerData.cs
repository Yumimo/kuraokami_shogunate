using UnityEngine;

namespace Kuraokami
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/PlayerData")]
    public class PlayerData : ScriptableObject
    {
        [Header("Movement")]
        public float m_moveSpeed;
        public float m_onAirMoveSpeed;
        
        [Header("Jump")]
        public float m_jumpForce;
        public float m_jumpDuration;
        public float m_jumpMaxHeight;
        public float m_jumpCooldown;
        private float m_jumpForceMultiplier = 1.2f;

        [Header("Gravity")]
        public float m_gravity = 9.81f;
        public float m_jumpGravityMultiplier = 1.0f;
        public float m_fallGravityMultiplier = 2.0f;

        public float AirMoveControl => m_moveSpeed * m_onAirMoveSpeed;
        
        public float JumpForce => m_jumpForce * m_jumpForceMultiplier;
        public float JumpGravity => m_gravity * m_jumpGravityMultiplier;
        public float FallGravity => m_gravity * m_fallGravityMultiplier;
    }
}
