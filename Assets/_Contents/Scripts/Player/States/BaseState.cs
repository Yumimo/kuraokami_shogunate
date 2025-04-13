
using UnityEngine;

namespace Kuraokami
{
    public class BaseState
    {
        protected readonly Player _player;
        protected readonly PlayerStateMachine _stateMachine;
        protected readonly InputReader _input;
        protected readonly PlayerData _data;
        protected Animator _animator;
        
        protected readonly int _animationHash;
        
        protected Rigidbody _rigidbody;

        protected BaseState(Player player, PlayerStateMachine stateMachine, InputReader input, PlayerData data, int animationHash)
        {
            _player = player;
            _stateMachine = stateMachine;
            _input = input;
            _data = data;
            _animationHash = animationHash;
        }

        public virtual void OnEnter()
        {
            Debug.Log($"Enter State: {this.GetType().Name})");
            _animator = _player.CurrentAnimator;
            _rigidbody = _player.PlayerRigidbody;
        }

        public virtual void OnUpdate()
        {
            Flip();
            if (!_player.IsGrounded && _rigidbody.linearVelocity.y < -0.01f)
            {
                _stateMachine.ChangeState(_player.Falling);
            }
        }

        public virtual void OnFixedUpdate()
        {
            
        }

        public virtual void OnExit()
        {
            
        }
        
        private void Flip()
        {
            var moveInput = _input.Direction.x;
            if (moveInput == 0) return;
            var scale = _player.transform.localScale;
            scale.z = Mathf.Sign(moveInput) * Mathf.Abs(scale.z);
            _player.transform.localScale = scale;
        }
    }
}
