using UnityEngine;

namespace Kuraokami
{
    public class FallingState : OnAirState
    {
        public  FallingState(Player player, PlayerStateMachine stateMachine, InputReader input, PlayerData data, Animator animator, int animationHash) : base(player, stateMachine, input, data, animator, animationHash)
        {
        }

        private float _jumpVelocity;
        public override void OnUpdate()
        {
            base.OnUpdate();
            var velocity = _rigidbody.linearVelocity;
            velocity.y += Physics.gravity.y * _data.FallGravity * Time.fixedDeltaTime;
            _rigidbody.linearVelocity = velocity;

            if (_player.IsGrounded)
            {
                _stateMachine.ChangeState(_player.Idle);
            }
        }
    }
}
