using UnityEngine;

namespace LivingBeings.Player.CharacterMovement.MovementStateMachine.States
{
    public class LandingState : BaseState
    {
        public LandingState (PlayerController characterMovement, StateMachine stateMachine) : base (characterMovement, stateMachine)
        {
            StateName = "Landing";
        }


        public override void Enter()
        {
            base.Enter();
            
            _characterController.LandingPosition = _characterController.Transform.position;
            _characterController.LastFallHeight = GetLastFallHeight();

            // EventSystem.TriggerEvent("OnLand");

            if (_characterController.PressButtonTimer > 0)
            {
                _stateMachine.TransitionToState(_characterController.Jumping);
            }
        }


        public override void OnTriggerEnter2D(Collider2D other)
        {
            base.OnTriggerEnter2D(other);
        }


        public override void OnTriggerExit2D(Collider2D other)
        {
            base.OnTriggerExit2D(other);
        }


        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            bool isRunning = (_characterController.RigidBody.velocity.x < -1f) || (_characterController.RigidBody.velocity.x > 1f);
            bool isUpButtonPressed = _characterController.IsJumpButtonPressed;

            if (isRunning)
            {
                _stateMachine.TransitionToState(_characterController.Running);
            }
            else 
            {
                _stateMachine.TransitionToState(_characterController.Idle);   
            }

            if (isUpButtonPressed)
            {
                _stateMachine.TransitionToState(_characterController.Jumping);
            }
        }


        public override void Exit()
        {
            _characterController.AfterGroundTouchTimer = _characterController.AfterGroundTouchJumpTime;
        }
        
        
        private float GetLastFallHeight()
        {
            return _characterController.StartFallingPosition.y - _characterController.LandingPosition.y;
        }
    }
}
