using UnityEngine;

namespace LivingBeings.Player.CharacterMovement.MovementStateMachine.States
{
    public class RunningState : BaseState
    {
        public RunningState (PlayerController characterMovement, StateMachine stateMachine) : base (characterMovement, stateMachine)
        {
            StateName = "Running";
        }


        public override void Enter()
        {
            base.Enter();
            
            // EventSystem.TriggerEvent("OnRun");
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

            bool isStartFalling = _characterController.RigidBody.velocity.y < -1f;
            bool isStopRunning = (_characterController.RigidBody.velocity.x < 1f) &&
                                 (_characterController.RigidBody.velocity.x > -1);
            bool isUpButtonPressed = _characterController.IsJumpButtonPressed;

            if (isStartFalling)
            {
                _stateMachine.TransitionToState(_characterController.Falling);
            }

            if (isStopRunning)
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
    }
}
