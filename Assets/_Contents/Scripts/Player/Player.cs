using System.Collections.Generic;
using System.Linq;
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

        private Rigidbody _rigidbody;
        private float _jumpVelocity;
        private Vector2 _movementInput;

        public bool IsFreezeInput { get; private set; }
        public bool IsAnimalForm { get; set; }
        public AnimalFormClass CurrentAnimalForm { get; set; }
        public bool IsGrounded => m_groundChecker.IsGrounded;
  
        public Animator CurrentAnimator => IsAnimalForm ? CurrentAnimalForm.FormAnimator : m_animator;
        public Rigidbody PlayerRigidbody => _rigidbody;

        #region Animation HASH

        private readonly int _locomationHash = Animator.StringToHash("Speed");
        private readonly int _jumpHash = Animator.StringToHash("IsJumping");
        private readonly int _fallHash = Animator.StringToHash("IsFalling");

        #endregion

        #region Animal Forms

        public readonly string _FOX = "FOX";

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
        public CountdownTimer CharacterFreezeInputTimer;
        
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
        }

        public void ToggleAnimalForm(string formName = "")
        {
            CharacterFreezeInputTimer.Start();
            if (formName == "")
            {
                CurrentAnimalForm?.EndForm();
                CurrentAnimalForm = null;
                return;
            }

            var _form = m_animalForms.FirstOrDefault(form => form.Id == formName);
            if (_form != null)
            {
                CurrentAnimalForm = _form;
                _form.StartForm(m_humanForm);
            }
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
            CharacterFreezeInputTimer = new CountdownTimer(m_data.m_characterFreezeInputTime);
            _timers = new List<Timer>(3) {JumpTimer, JumpCooldownTimer, CharacterFreezeInputTimer};
            
            JumpTimer.OnTimerStart += () => _stateMachine.ChangeState(Jump);
            JumpTimer.OnTimerStop += () => JumpCooldownTimer.Start(); 
            
            CharacterFreezeInputTimer.OnTimerStart += () => IsFreezeInput = true;
            CharacterFreezeInputTimer.OnTimerStop += () =>
            {
                IsFreezeInput = false;
                _stateMachine?.ChangeState(Idle);
            };
        }

        private void OnJump(bool arg0)
        {
            if(IsFreezeInput)return;
            
            if(IsAnimalForm && !CurrentAnimalForm._data.CanJump)return;
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
        }

    }
}