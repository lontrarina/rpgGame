using Core.Enums;
using UnityEngine;
using Core.Movement.Data;
using StatsSystem;
using StatsSystem.Enum;

namespace Core.Movement.Controller
{
    public class DirectionalMover
    {
        private readonly Rigidbody2D _rigidbody;
        private readonly Transform _transform;
        private readonly DirectionalMovementData _directionalMovementData;
        private readonly IStatValueGiver _statValueGiver;

        private Vector2 _movement;

        public Direction Direction { get; private set; }
        public bool IsMoving=> _movement.magnitude > 0;


        public DirectionalMover(Rigidbody2D rigidbody, DirectionalMovementData directionalMovementData, IStatValueGiver statValueGiver)
        { 
            _statValueGiver = statValueGiver;
            _rigidbody = rigidbody;
            _transform = rigidbody.transform;
            _directionalMovementData = directionalMovementData;
        }

        public void MoveHorizontally(float direction)
        {
            _movement.x = direction;
            SetDirection(direction);
            Vector2 velocity = _rigidbody.velocity;
            velocity.x = direction * _statValueGiver.GetStatValue(StatType.Speed);
            _rigidbody.velocity = velocity;

        }
        private void SetDirection(float direction)
        {
            if ((Direction == Direction.Right && direction < 0) ||
                (Direction == Direction.Left && direction > 0))
            {
                Flip();
            }
        }

        private void Flip()
        {
            _transform.Rotate(0, 180, 0);
            Direction = Direction == Direction.Right ? Direction.Left : Direction.Right;
           
        }

    }
}
