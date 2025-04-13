using UnityEngine;

namespace Kuraokami
{
    public class IdleState : OnGroundState
    {
        public IdleState(Player player, PlayerStateMachine stateMachine, InputReader input, PlayerData data, int animationHash) : base(player, stateMachine, input, data, animationHash)
        {
        }
        public override void OnUpdate()
        {
            base.OnUpdate();
            if (_movementInput.x != 0)
            {
                _stateMachine.ChangeState(_player.Move);
                return;
            }
            _rigidbody.linearVelocity = new Vector3(0, _rigidbody.linearVelocity.y, 0f);
        }

        protected override float AnimationSpeed() => 0;
        protected override bool ReturnToIdleState() => false;
    }
}
