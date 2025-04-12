namespace Kuraokami
{
    public class PlayerStateMachine
    {
        public BaseState CurrentState { get; private set; }

        public void Initialize(BaseState state)
        {
            CurrentState = state;
            CurrentState.OnEnter();
        }

        public void ChangeState(BaseState state)
        {
            if(CurrentState == state)return;
            CurrentState.OnExit();
            CurrentState = state;
            CurrentState.OnEnter();
        }
    }
}
