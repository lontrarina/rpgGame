using UnityEngine;
using Player;


public class InputReader : MonoBehaviour 
{
    [SerializeField] private PlayerEntity _playerEntity;

    private float _horizontalDirection;

    private void Update()
    {
        _horizontalDirection = Input.GetAxisRaw("Horizontal"); 

        if(Input.GetButtonDown("Jump"))
        {
            _playerEntity.Jump();
        }
        if (Input.GetButtonDown("Fire1"))
        {
            _playerEntity.StartAttack();
        }

    }

    private void FixedUpdate()
    {
        _playerEntity.MoveHorizontally(_horizontalDirection);
    }
}