using UnityEngine;

namespace Kuraokami
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/PlayerData")]
    public class PlayerData : ScriptableObject
    {
        public float m_moveSpeed;
        public float m_smoothTime;
        public float m_jumpForce;
        public float m_gravity;
        public float m_jumpTime;
    }
}
