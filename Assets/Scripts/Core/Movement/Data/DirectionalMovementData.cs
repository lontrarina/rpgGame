using System;
using UnityEngine;
using Core.Enums;

namespace Core.Movement.Data
{
    [Serializable]
    public class DirectionalMovementData
    {
        //HorizontalMovement
        [field: SerializeField] public float HorizontalSpeed { get; private set; }
        [field: SerializeField] public Direction Direction { get; private set; }
    }
}
