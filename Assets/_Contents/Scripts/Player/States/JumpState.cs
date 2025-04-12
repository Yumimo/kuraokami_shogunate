using UnityEngine;

namespace Kuraokami
{
    public class JumpState: OnAirState
    {
        public JumpState(Player player, PlayerStateMachine stateMachine, InputReader input, PlayerData data, Animator animator, int animationHash) : base(player, stateMachine, input, data, animator, animationHash)
        {
        }
        private float _jumpVelocity;
        private float _launchPoint = 0.9f;
        
        public override void OnEnter()
        {
            base.OnEnter();
            _jumpVelocity = 0;
        }
        public override void OnFixedUpdate()
        {
            OnJump();
            if (!_player.JumpTimer.IsRunning && _rigidbody.linearVelocity.y <= 0f)
            {
                _stateMachine.ChangeState(_player.Falling);
            }
        }

        private void OnJump()
        {
            if (!_player.JumpTimer.IsRunning && _player.IsGrounded)
            {
                _jumpVelocity = 0;
                _player.JumpTimer.Stop();
            }
            if (_player.JumpTimer.IsRunning)
            {
                if (_player.JumpTimer.Progress > _launchPoint)
                {
                    _jumpVelocity = Mathf.Sqrt(2 * _data.m_jumpMaxHeight * Mathf.Abs(Physics.gravity.y));
                }
                else
                {
                    _jumpVelocity += (1 - _player.JumpTimer.Progress) * _data.JumpForce * Time.fixedDeltaTime;
                }
            }
            else
            {
                _jumpVelocity += Physics.gravity.y * _data.JumpGravity * Time.fixedDeltaTime;
            }

            _rigidbody.linearVelocity = new Vector3(_rigidbody.linearVelocity.x, _jumpVelocity, _rigidbody.linearVelocity.z);
        }
    }
}
