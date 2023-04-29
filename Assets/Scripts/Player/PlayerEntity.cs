using UnityEngine;
using System;
using Core.Tools;
using Core.Animation;
using Core.Movement.Data;
using Core.Movement.Controller;
using Core.Services.Updater;
using StatsSystem;

namespace Player
{

    [RequireComponent(typeof(Rigidbody2D))]

    public class PlayerEntity : MonoBehaviour, IDisposable
    {
        [SerializeField] private AnimatorController _animator;
        [SerializeField] private DirectionalMovementData _directionalMovementData;
        [SerializeField] private JumpData _jumpData;
        [SerializeField] private DirectionalCameraPair _cameras;

        private Rigidbody2D _rigidbody;
        private DirectionalMover _directionalMover;
        private Jumper _jumper;

        
        public void Initialize(IStatValueGiver statValueGiver)
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            ProjectUpdater.Instance.UpdateCalled += OnUpdate;
            _directionalMover = new DirectionalMover(_rigidbody, _directionalMovementData, statValueGiver);

            _jumper = new Jumper(_rigidbody, _jumpData, statValueGiver);
        }

        private void OnUpdate()
        {
            if (_jumper.IsJumping)
            {
                _jumper.UpdateJump();
            }
            UpdateAnimations();
            UpdateCameras();
        }
        public void Dispose() => ProjectUpdater.Instance.UpdateCalled -= OnUpdate;

        public void MoveHorizontally(float direction)=>_directionalMover.MoveHorizontally(direction);

        public void Jump() =>_jumper.Jump();

        public void StartAttack()
        {
            if (!_animator.PlayAnimation(AnimationType.Attack, true))
            {
                return;
            }
            _animator.ActionRequested += Attack;
            _animator.AnimationEnded += EndAttack;
        }
        private void UpdateCameras()
        {
            foreach (var cameraPair in _cameras.DirectionalCameras)
            {
                cameraPair.Value.enabled = cameraPair.Key == _directionalMover.Direction;
            }
        }
        private void UpdateAnimations()
        {
            _animator.PlayAnimation(AnimationType.Idle, true);
            _animator.PlayAnimation(AnimationType.Run, _directionalMover.IsMoving);
            _animator.PlayAnimation(AnimationType.Jump, _jumper.IsJumping);
        }

        private void Attack()
        {
            Debug.Log("Attack");
        }

        private void EndAttack()
        {
            _animator.ActionRequested -= Attack;
            _animator.AnimationEnded -= EndAttack;
            _animator.PlayAnimation(AnimationType.Attack, false);
        }
    }
}