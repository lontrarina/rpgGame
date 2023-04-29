using UnityEngine;
using Core.Movement.Data;
using StatsSystem;
using StatsSystem.Enum;

namespace Core.Movement.Controller
{
    public class Jumper
    {
        private readonly Rigidbody2D _rigidbody;
        private readonly Transform _transform;
        private readonly JumpData _jumpData;
        private readonly IStatValueGiver _statValueGiver;

        public bool IsJumping { get; private set; }

        public Jumper(Rigidbody2D rigidbody, JumpData jumpData, IStatValueGiver statValueGiver)
        {
            _statValueGiver = statValueGiver;
            _rigidbody = rigidbody;
            _transform = rigidbody.transform;
            _jumpData = jumpData;
        }
        public void Jump() 
        {
            if (IsJumping)
            {
                return;
            }
            IsJumping = true;
            _rigidbody.AddForce(Vector2.up * _statValueGiver.GetStatValue(StatType.JumpForce));
        }

        public void UpdateJump()
        {

            if (GroundCheck())
            {
                ResetJump();
                return;
            }
        }

        private void ResetJump()
        {
            IsJumping = false;
        }

        private bool GroundCheck()
        {
            if (Physics2D.BoxCast(_transform.position, _jumpData.BoxSize, 0, -_transform.up, _jumpData.MaxDistance, _jumpData.LayerMask))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
