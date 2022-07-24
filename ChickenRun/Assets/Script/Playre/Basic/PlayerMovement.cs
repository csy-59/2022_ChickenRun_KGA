using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float MoveSpeed;

    private PlayerInput input;
    private Rigidbody rigid;
    private struct Position
    {
        public int row;
        public int pos;
    }

    private Position position = new Position { row = 2, pos = 1 };

    private bool isMoving = false;

    private void Awake()
    {
        input = GetComponent<PlayerInput>();
        rigid = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        float moveDirection = input.Up ? 1f : -1f;
        Vector3 offset = MoveSpeed * moveDirection * Time.fixedDeltaTime * transform.forward;


    }

    private void Update()
    {
        
    }
}
