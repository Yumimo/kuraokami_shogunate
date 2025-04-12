using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Kuraokami
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private InputReader m_inputReader;
        [SerializeField] private PlayerData m_data;
        [SerializeField] private GroundChecker m_groundChecker;
        [SerializeField] private Animator m_animator;

        private Rigidbody _rigidbody;
        private float _jumpVelocity;
        private Vector2 _movementInput;

        public bool IsGrounded => m_groundChecker.IsGrounded;
        public Rigidbody PlayerRigidbody => _rigidbody;

        #region Animation HASH

        private readonly int _locomationHash = Animator.StringToHash("Speed");

        #endregion

        #region STATES
        private PlayerStateMachine _stateMachine;

        public MoveState Move;
        public IdleState Idle;
        public JumpState Jump;
        public FallingState Falling;
        #endregion

        #region Timers
        
        private List<Timer> _timers;
        public CountdownTimer JumpTimer;
        public CountdownTimer JumpCooldownTimer;
        
        #endregion

        
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();

            SetupStates();
            SetupTimers();
        }
        private void OnEnable()
        {
            m_inputReader.Jump += OnJump;
        }

        private void SetupStates()
        {
            _stateMachine = new PlayerStateMachine();
            Idle = new IdleState(this, _stateMachine, m_inputReader, m_data, m_animator, _locomationHash);
            Move = new MoveState(this, _stateMachine, m_inputReader, m_data, m_animator, _locomationHash);
            Jump = new JumpState(this, _stateMachine, m_inputReader, m_data, m_animator, _locomationHash);
            Falling = new FallingState(this, _stateMachine, m_inputReader, m_data, m_animator, _locomationHash);
            
            _stateMachine.Initialize(Idle);
        }

        private void SetupTimers()
        {
            JumpTimer = new CountdownTimer(m_data.m_jumpDuration);
            JumpCooldownTimer = new CountdownTimer(m_data.m_jumpCooldown);
            _timers = new List<Timer>(2) {JumpTimer, JumpCooldownTimer};


            JumpTimer.OnTimerStart += () => _stateMachine.ChangeState(Jump);
            JumpTimer.OnTimerStop += () => JumpCooldownTimer.Start(); 
        }

        private void OnJump(bool arg0)
        {
            switch (arg0)
            {
                case true when !JumpTimer.IsRunning && !JumpCooldownTimer.IsRunning && m_groundChecker.IsGrounded:
                    JumpTimer.Start();
                    break;
                case false when JumpTimer.IsRunning:
                    JumpTimer.Stop();
                    break;
            }
        }

        private void Update()
        {
            _stateMachine?.CurrentState.OnUpdate();
            HandleTimer();
        }
        private void FixedUpdate()
        {
            _stateMachine?.CurrentState.OnFixedUpdate();
        }
        private void HandleTimer()
        {
            foreach (var timer in _timers)
            {
                timer.Tick(Time.deltaTime);
            }
        }
    
        private void OnDisable()
        {
            m_inputReader.Jump -= OnJump;
        }

    }
}