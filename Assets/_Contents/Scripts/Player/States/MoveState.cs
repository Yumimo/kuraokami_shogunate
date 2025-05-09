using UnityEngine;

namespace Kuraokami
{
    public class MoveState : OnGroundState
    {
        public MoveState(Player player, PlayerStateMachine stateMachine, InputReader input, PlayerData data, int animationHash) : base(player, stateMachine, input, data, animationHash)
        {
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            OnMove();
        }

        protected virtual void OnMove()
        {
            var moveDir = new Vector3(_movementInput.x, 0f, 0f).normalized;
            var velocity = new Vector3(moveDir.x * _data.m_moveSpeed, _rigidbody.linearVelocity.y, 0f);
            _rigidbody.linearVelocity = velocity;
        }
    }
}
