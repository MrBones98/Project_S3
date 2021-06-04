using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float _speed;

    private Vector2 _direction;
    private Rigidbody2D _rigidbody2D;
    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        PlayerInput();
    }
    private void FixedUpdate()
    {
        _rigidbody2D.velocity = _direction * _speed * Time.deltaTime;
    }

    private void PlayerInput()
    {
        float moveX = 0f;
        float moveY = 0f;
        if (Input.GetKey(KeyCode.W))
        {
            moveY += +1f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveY -= 1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveX += 1f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            moveX -= 1f;
        }
        _direction = new Vector2(moveX, moveY).normalized;
    }
}
