using UnityEngine;

namespace Kuraokami
{
    public class OnAirState: BaseState
    {
        protected OnAirState(Player player, PlayerStateMachine stateMachine, InputReader input, PlayerData data, int animationHash) : base(player, stateMachine, input, data, animationHash)
        {
        }
        public override void OnUpdate()
        {
            base.OnUpdate();
            MoveInAir(_input.Direction);
        }

        protected virtual void MoveInAir(Vector2 _direction)
        {
            var velocity = _rigidbody.linearVelocity;
            velocity.x = _direction.x * _data.AirMoveControl;
            _rigidbody.linearVelocity = velocity;
        }
    }
}
