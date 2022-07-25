using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // �̵� ����
    public float MoveSpeed = 1f;
    public CubeRowManager rowManager;

    private int position = 1;
    private Vector3 moveDirection = new Vector3(0f, 0f, 0f);

    private PlayerInput input;
    private Rigidbody rigid;

    private Transform targetPosition;

    private bool isMoving = false;

    // ȸ�� ����
    public float RotateSpeed = 100f;
    public Transform CharacterModel;

    private float rotateDirection = 0f;

    // ���� ����
    private bool isPlayerDead = false;
    private PlayerEncounter encounter;

    public Animator anim;

    private void Awake()
    {
        input = GetComponent<PlayerInput>();
        rigid = GetComponent<Rigidbody>();
        encounter = GetComponent<PlayerEncounter>();
        moveDirection = new Vector3(0f, 0f, 0f);
        targetPosition = null;
        isPlayerDead = false;
    }

    private void FixedUpdate()
    {
        if(!isPlayerDead && !encounter.isStanned)
        {
            if (isMoving)
            {
                Vector3 newPosition = Vector3.Lerp(transform.position, targetPosition.position, MoveSpeed * Time.deltaTime);
                
                if ((targetPosition.position - newPosition).sqrMagnitude < 0.05f)
                {
                    isMoving = false;
                    transform.position = targetPosition.position;
                }
                else
                {
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
        if(!isPlayerDead)
        {
            if (transform.position.z < GameManager.Instance.RowDisableZPos + 0.3f)
            {
                transform.parent = null;
                gameObject.tag = "PlayerDie";
                if(transform.position.y < 0.9f)
                PlayerDead();
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

        targetPosition = rowManager.GetTargetRowTransform(input.Z, position + (int) input.X, out gotRightTransform);

        if(gotRightTransform)
        {
            isMoving = true;
            position = position + (int)input.X;
        }
    }
    private void MovingOver()
    {
        isMoving = false;
        anim.SetBool("isWalking", false);
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
        anim.SetTrigger("Die");
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
