using UnityEngine;

namespace Kuraokami
{
    public class OnGroundState : BaseState
    {
        protected OnGroundState(Player player, PlayerStateMachine stateMachine, InputReader input, PlayerData data, int animationHash) : base(player, stateMachine, input, data, animationHash)
        {
        }

        protected Vector2 _movementInput;

        public override void OnEnter()
        {
            base.OnEnter();
            _input.Crouch += Crouch;
        }
        public override void OnUpdate()
        {
            base.OnUpdate();
            _movementInput = _player.IsFreezeInput ? Vector2.zero : _input.Direction;
            if (ReturnToIdleState())
            {
                _stateMachine.ChangeState(_player.Idle);
            }
            _animator.SetFloat(_animationHash, AnimationSpeed());
        }

        public override void OnExit()
        {
            base.OnExit();
            _input.Crouch -= Crouch;
        }


        private void Crouch()
        {
            if(_player.IsFreezeInput)return;
            
            _player.IsAnimalForm = !_player.IsAnimalForm;
            var _form = _player.IsAnimalForm ? _player._FOX : string.Empty;
            _player.ToggleAnimalForm(_form);
        }

        protected virtual bool ReturnToIdleState() =>_movementInput == Vector2.zero;
        protected virtual float AnimationSpeed() => _rigidbody.linearVelocity.magnitude;
    }
}
