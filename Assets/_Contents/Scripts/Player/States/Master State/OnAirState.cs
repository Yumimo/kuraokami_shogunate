using UnityEngine;

namespace Kuraokami
{
    public class OnAirState: BaseState
    {
        protected OnAirState(Player player, PlayerStateMachine stateMachine, InputReader input, PlayerData data, Animator animator, int animationHash) : base(player, stateMachine, input, data, animator, animationHash)
        {
        }
        public override void OnUpdate()
        {
            base.OnUpdate();
            MoveInAir(_input.Direction);
        }

        protected virtual void MoveInAir(Vector2 _direction)
        {
            var moveDir = new Vector3(_direction.x, 0f, 0f).normalized;
            var velocity = new Vector3(moveDir.x * _data.m_moveSpeed, _rigidbody.linearVelocity.y, 0f);
            _rigidbody.linearVelocity = velocity;
        }
    }
}
