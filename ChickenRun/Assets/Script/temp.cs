using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class temp : MonoBehaviour
{

    public float Speed = 1.5f;

    private Vector3 moveDiraction;

    private PlayerInput input;
    private Rigidbody rigid;

    private Transform target;

    private bool isMoving = false;

    private void Awake()
    {
        input = GetComponent<PlayerInput>();
        rigid = GetComponent<Rigidbody>();
        moveDiraction = new Vector3(0f, 0f, 0f);
    }

    private void FixedUpdate()
    {
        if (isMoving)
        {
            Vector3 offset = Speed * Time.deltaTime * (target.position - transform.position).normalized;
            rigid.MovePosition(rigid.position + offset);
        }
        else
        {
            float moveX = input.X;
            float moveZ = input.Z;

            moveDiraction = new Vector3(moveX, 0f, moveZ);

            LayerMask targetLayer = LayerMask.NameToLayer("PlayerCubePosition");
            RaycastHit hit;
            if (Physics.Raycast(transform.position, moveDiraction, out hit, 20f))
            {
                Debug.Log(hit.collider.gameObject.name);
                if (hit.collider.gameObject.layer != targetLayer.value)
                {
                    return;
                }

                target = hit.collider.gameObject.transform;
                isMoving = true;
                Invoke("MovingOver", 1.8f * 3 / Speed);
            }
        }
    }

    private void MovingOver()
    {
        isMoving = false;
    }
}
