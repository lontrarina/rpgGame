using UnityEngine;
using Core.Tools;
using Core.Enums;
using System;
using Player.PlayerAnimation;

namespace Player
{

    [RequireComponent(typeof(Rigidbody2D))]

    public class PlayerEntity : MonoBehaviour
    {
        [SerializeField] private AnimatorController _animator;


        [Header ("HorizontalMovement")]
        [SerializeField] private float _horizontalSpeed;
        [SerializeField] private Direction _direction;     

        [Header("Jump")]
        [SerializeField] private float _jumpForce;

        
        [SerializeField] private DirectionalCameraPair _cameras;

        [Header("GroundedCheck")]
        [SerializeField] private Vector2 _boxSize;
        [SerializeField] private float _maxDistance;
        [SerializeField] private LayerMask _layerMask;

        private Rigidbody2D _rigidbody;

        private bool _isJumping;

        private Vector2 _movement;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }


        private void Update()
        {
            if (_isJumping)
            {
                UpdateJump();
            }
            UpdateAnimations();
        }

        public void MoveHorizontally(float direction)
        {
            _movement.x = direction;
            SetDirection(direction);
            Vector2 velocity=_rigidbody.velocity;
            velocity.x = direction * _horizontalSpeed;
            _rigidbody.velocity = velocity; 

        }

        public void Jump() //added directions and can be problems
        {
            if (_isJumping)
            {
                return;
            }
            _isJumping = true;
            _rigidbody.AddForce(Vector2.up * _jumpForce);
        }
        public bool GroundCheck()
        {
            if (Physics2D.BoxCast(transform.position, _boxSize, 0, -transform.up, _maxDistance, _layerMask))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void StartAttack()
        {
            if (!_animator.PlayAnimation(AnimationType.Attack, true))
            {
                return;
            }
            _animator.ActionRequested += Attack;
            _animator.AnimationEnded += EndAttack;
        }
        private void UpdateAnimations()
        {
            _animator.PlayAnimation(AnimationType.Idle, true);
            _animator.PlayAnimation(AnimationType.Run, _movement.magnitude > 0);
            _animator.PlayAnimation(AnimationType.Jump, _isJumping);
        }

        private void SetDirection(float direction)
        {
            if((_direction==Direction.Right && direction < 0) || 
                (_direction==Direction.Left && direction >0))
            {
                Flip();
            }
        }

        private void Flip()
        {
            transform.Rotate(0, 180, 0);
            _direction= _direction == Direction.Right ? Direction.Left : Direction.Right;
            foreach (var cameraPair in _cameras.DirectionalCameras)
            {
                cameraPair.Value.enabled = cameraPair.Key == _direction;
            }
        }

        private void UpdateJump()
        {
            
            if (GroundCheck())
            {
                ResetJump();
                return;
            }
            
        }

        private void ResetJump()
        {
            _isJumping = false; 
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
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawCube(transform.position - transform.up * _maxDistance, _boxSize);
        }
    }

}