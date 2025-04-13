using UnityEngine;

namespace Kuraokami
{
    [CreateAssetMenu(fileName = "AnimalFormData", menuName = "Scriptable Objects/AnimalFormData")]
    public class AnimalFormData : ScriptableObject
    {
        public string Id;
        
        [Header("Stats")]
        public float Speed;
        public float JumpForce;
        
        [Header("Talents")]
        public bool CanWalk;
        public bool CanJump;
        
    }
}
