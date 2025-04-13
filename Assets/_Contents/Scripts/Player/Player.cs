using System.Collections.Generic;
using UnityEngine;

namespace Kuraokami
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private InputReader m_inputReader;
        [SerializeField] private PlayerData m_data;
        [SerializeField] private GameObject m_humanForm;
        [SerializeField] private GroundChecker m_groundChecker;
        [SerializeField] private Animator m_animator;
        
        
        [SerializeField] private AnimalFormClass[] m_animalForms;

        private AnimalFormClass _currentFormClass;
        private Rigidbody _rigidbody;
        private float _jumpVelocity;
        private Vector2 _movementInput;
        private bool _isAnimalForm;

        public bool IsGrounded => m_groundChecker.IsGrounded;
        public AnimalFormClass CurrentFormClass => _currentFormClass;
        public Animator CurrentAnimator => _isAnimalForm ? _currentFormClass.FormAnimator : m_animator;
        public Rigidbody PlayerRigidbody => _rigidbody;

        #region Animation HASH

        private readonly int _locomationHash = Animator.StringToHash("Speed");
        private readonly int _jumpHash = Animator.StringToHash("IsJumping");
        private readonly int _fallHash = Animator.StringToHash("IsFalling");

        #endregion

        #region Animal Forms

        private const string _FOX = "FOX";

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
        
        [Header("Debugger")]
        [SerializeField] private string _currentStateName;

        
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();

            SetupStates();
            SetupTimers();
        }
        private void OnEnable()
        {
            m_inputReader.Jump += OnJump;
            m_inputReader.Crouch += OnCrouch;
        }

        private void SetupStates()
        {
            _stateMachine = new PlayerStateMachine();
            Idle = new IdleState(this, _stateMachine, m_inputReader, m_data, _locomationHash);
            Move = new MoveState(this, _stateMachine, m_inputReader, m_data, _locomationHash);
            Jump = new JumpState(this, _stateMachine, m_inputReader, m_data, _jumpHash);
            Falling = new FallingState(this, _stateMachine, m_inputReader, m_data, _fallHash);
            
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
        
        private void OnCrouch()
        {
            Debug.Log("Crouch");
            _isAnimalForm = !_isAnimalForm;
            if (_isAnimalForm)
            {
                foreach (var forms in m_animalForms)
                {
                    if (forms.Id != _FOX) continue;
                    _currentFormClass = forms;
                    forms.OnTransform(m_humanForm);
                }
            }
            else
            {
                _currentFormClass?.ReturnToOriginalForm();
                _currentFormClass = null;
            }
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
            _currentStateName = _stateMachine.CurrentState.GetType().Name;
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
            m_inputReader.Crouch -= OnCrouch;
        }

    }
}