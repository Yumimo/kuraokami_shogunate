using UnityEngine;

namespace Kuraokami
{
    public class OnGroundState : BaseState
    {
        protected OnGroundState(Player player, PlayerStateMachine stateMachine, InputReader input, PlayerData data,Animator animator, int animationHash) : base(player, stateMachine, input, data, animator, animationHash)
        {
        }

        protected Vector2 _movementInput;
        public override void OnUpdate()
        {
            base.OnUpdate();
            _movementInput = _input.Direction;
            if (ReturnToIdleState())
            {
                _stateMachine.ChangeState(_player.Idle);
            }
            _animator.SetFloat(_animationHash, AnimationSpeed());
        }

        protected virtual bool ReturnToIdleState() =>_movementInput == Vector2.zero;
        protected virtual float AnimationSpeed() => _rigidbody.linearVelocity.magnitude;
    }
}
