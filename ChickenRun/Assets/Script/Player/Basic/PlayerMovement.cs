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

    // 기타
    public Animator anim;
    private AudioSource audioSource;

    private void Awake()
    {
        input = GetComponent<PlayerInput>();
        rigid = GetComponent<Rigidbody>();
        encounter = GetComponent<PlayerEncounter>();
        audioSource = GetComponent<AudioSource>();
        moveDirection = new Vector3(0f, 0f, 0f);
        target = null;
    }

    private void FixedUpdate()
    {
        if(!encounter.isPlayerDead && !encounter.isStanned)
        {
            if (isMoving)
            {
                Vector3 targetPosition = target.position;
                Vector3 newPosition = Vector3.Lerp(transform.position, targetPosition, MoveSpeed);
                
                if ((targetPosition - newPosition).sqrMagnitude < 0.008f)
                {
                    isMoving = false;
                    isJumped = false;
                    gameObject.tag = "Player";
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

        target = rowManager.GetTargetRowTransform(input.Z, input.X, out gotRightTransform);

        if(gotRightTransform)
        {
            isMoving = true;
            gameObject.tag = "PlayerMove";
            transform.parent = null;
            transform.LookAt(target);
            anim.SetTrigger("Move");
            audioSource.Play();
        }
    }
}
