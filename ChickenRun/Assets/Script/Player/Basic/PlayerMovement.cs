using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // 이동 관련
    public float MoveSpeed = 1f;
    public float JumpForce = 5f;
    public CubeRowManager rowManager;

    private int position = 1;
    private Vector3 moveDirection = new Vector3(0f, 0f, 0f);

    private PlayerInput input;
    private Rigidbody rigid;

    private Transform target;

    private bool isJumped = false;
    private bool isMoving = false;

    // 회전 관련
    public float RotateSpeed = 100f;
    public Transform CharacterModel;

    private float rotateDirection = 0f;

    // 생명 관련
    private PlayerEncounter encounter;

    public Animator anim;

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
        if(!encounter.isPlayerDead && !encounter.isStanned)
        {
            if (isMoving)
            {
                Vector3 targetPosition = new Vector3(target.position.x, transform.position.y, target.position.z);
                Vector3 newPosition = Vector3.Lerp(transform.position, targetPosition, MoveSpeed * Time.deltaTime);
                
                if ((targetPosition - newPosition).sqrMagnitude < 0.05f)
                {
                    isMoving = false;
                    rigid.useGravity = true;
                    isJumped = false;
                    gameObject.layer = 7;
                    transform.parent = null;
                    transform.position = targetPosition;
                }
                else
                {
                    if(isJumped)
                    {
                        newPosition += new Vector3(0f, JumpForce * Time.deltaTime, 0f);
                    }
                    rigid.MovePosition(newPosition);
                }
            }
            else if(input.Z != 0f || input.X != 0f)
            {
                GetTarget();
            }
        }

    }

    private void Update()
    {
        if(!encounter.isPlayerDead)
        {
            if (transform.position.z < GameManager.Instance.RowDisableZPos + 0.3f)
            {
                gameObject.tag = "PlayerDie";
            }
            else
            {
                gameObject.tag = "Player";
            }
        }
    }

    private void GetTarget()
    {
        bool gotRightTransform;

        target = rowManager.GetTargetRowTransform(input.Z, position + (int) input.X, out gotRightTransform);

        if(gotRightTransform)
        {
            isMoving = true;
            gameObject.layer = 11;
            transform.parent = null;
            position = position + (int)input.X;
            transform.LookAt(target);

            if(target.position.y == transform.position.y)
            {
                rigid.useGravity = false;
            }
            else if(Mathf.Abs(target.position.y - transform.position.y) <= GameManager.Instance.MinMoveableOffset)
            {
                isJumped = true;
            }
        }
    }

    private void Jump()
    {
        //rigid.AddForce(Vector3.up * 10f, ForceMode.Impulse);
    }

    private void MovingOver()
    {
        isMoving = false;
        anim.SetBool("isWalking", false);
    }


    private void CanMove()
    {
        //isMoveable = true;
    }

    private void DisableSelf()
    {
        gameObject.SetActive(false);
    }
}
