using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // 이동 관련
    public float MoveSpeed = 1f;
    private Vector3 moveDirection = new Vector3(0f, 0f, 0f);

    private PlayerInput input;
    private Rigidbody rigid;

    private GameObject target;

    private bool isMoving = false;

    // 회전 관련
    public float RotateSpeed = 100f;
    public Transform CharacterModel;

    private float rotateDirection = 0f;

    // 생명 관련
    private bool isPlayerDead = false;
    private PlayerEncounter encounter;

    private void Awake()
    {
        input = GetComponent<PlayerInput>();
        rigid = GetComponent<Rigidbody>();
        encounter = GetComponent<PlayerEncounter>();
        moveDirection = new Vector3(0f, 0f, 0f);
        target = null;
    }

    private void FixedUpdate()
    {
        if(!isPlayerDead && !encounter.isStanned)
        {
            if (isMoving)
            {
                Vector3 offset = MoveSpeed * Time.deltaTime * (target.transform.position - transform.position).normalized;
                rigid.MovePosition(rigid.position + offset);
            }
            else if(input.Z != 0f || input.X != 0f)
            {
                GetTarget();
            }
        }
    }

    private void Update()
    {
        if(!isPlayerDead)
        {
            if (transform.position.z < GameManager.Instance.RowDisableZPos)
            {
                transform.parent = null;
                gameObject.tag = "PlayerDie";
                //PlayerDead();
            }
            else
            {
                gameObject.tag = "Player";
            }
        }
    }

    private void GetTarget()
    {
        float moveX = input.X;
        float moveZ = input.Z;

        moveDirection = new Vector3(moveX, 0f, moveZ);

        LayerMask targetLayer = LayerMask.GetMask("PlayerCubePosition");
        RaycastHit[] hits = Physics.RaycastAll(transform.position, moveDirection, 20f, targetLayer);

        for(int i = 0; i < hits.Length; ++i)
        {
            RaycastHit other = hits[i];
            if(other.collider.gameObject == target)
            {
                continue;
            }

            target = other.collider.gameObject;
            isMoving = true;
            Invoke("MovingOver", 1.8f * 3 / MoveSpeed);
        }
    }
    private void MovingOver()
    {
        isMoving = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.tag);

        //if (other.tag == "Platform") 
        //{
        //    hasArrived = true;
        //    //transform.position = new Vector3(0f, 0f, 0f);
        //    Invoke("CanMove", inputOffset);
        //}
        //else if(other.tag == "Lava")
        //{
        //    Debug.Log("die");
        //    PlayerDead();
        //}
    }

    private void CanMove()
    {
        //isMoveable = true;
    }

    private void PlayerDead()
    {
        gameObject.tag = "PlayerDie";
        gameObject.layer = 8;
        GameManager.Instance.PlayerDead();
        isPlayerDead = true;
        rigid.velocity = Vector3.zero;
        rigid.AddForce(Vector3.up * 5f, ForceMode.Impulse);
        Invoke("DisableSelf", 1f);
    }

    private void DisableSelf()
    {
        gameObject.SetActive(false);
    }
}
