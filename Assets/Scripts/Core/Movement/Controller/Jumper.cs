﻿using UnityEngine;
using Core.Movement.Data;

namespace Core.Movement.Controller
{
    public class Jumper
    {
        private readonly JumpData _jumpData;
        private readonly Rigidbody2D _rigidbody;

        private readonly Transform _transform;

        public bool IsJumping { get; private set; }

        public Jumper(Rigidbody2D rigidbody, JumpData jumpData)
        {
            _rigidbody = rigidbody;
            _jumpData = jumpData;
            _transform =_rigidbody.transform;
        }
        public void Jump() 
        {
            if (IsJumping)
            {
                return;
            }
            IsJumping = true;
            _rigidbody.AddForce(Vector2.up * _jumpData.JumpForce);
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
