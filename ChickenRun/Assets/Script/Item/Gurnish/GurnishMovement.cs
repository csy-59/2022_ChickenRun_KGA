using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GurnishMovement : MonoBehaviour
{
    public Transform ModelTransform;

    private bool isOnGround = false;
    private Rigidbody rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if(isOnGround)
        {
            Move();
            Rotate();
        }
    }

    private void Move()
    {
        Vector3 offset = GameManager.Instance.GurnishMoveSpeed * Time.fixedDeltaTime * transform.forward;
        rigid.MovePosition(rigid.position + offset);
    }

    private void Rotate()
    {
        ModelTransform.Rotate(GameManager.Instance.GurnishRotateSpeed * Time.deltaTime, 0f, 0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!isOnGround && other.tag == "Platform")
        {
            isOnGround = true;
        }
        else if(other.tag == "Lava")
        {
            Invoke("DisableSelf", 0.5f);
        }
    }

    private void DisableSelf()
    {
        gameObject.SetActive(false);
    }
}
