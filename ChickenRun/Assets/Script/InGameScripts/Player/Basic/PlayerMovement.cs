using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // 이동 관련
    public float MoveSpeed = 1f;
    private CubeRowManager rowManager;

    private PlayerInput input;
    private Rigidbody rigid;

    private Transform target;

    private bool isOnPlatform = false;
    private bool isMoving = false;

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
        target = null;
    }

    private void Start()
    {
        rowManager = FindObjectOfType<CubeRowManager>().GetComponent<CubeRowManager>();
    }
    private void FixedUpdate()
    {
        if(!encounter.isPlayerDead && !encounter.isStanned && isOnPlatform)
        {
            if (isMoving)
            {
                Vector3 targetPosition = target.position;
                Vector3 newPosition = Vector3.Slerp(transform.position, targetPosition, MoveSpeed);
                
                if ((targetPosition - newPosition).sqrMagnitude < 0.008f)
                {
                    isMoving = false;
                    gameObject.tag = "Player";
                    transform.position = targetPosition;
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
        if(!encounter.isPlayerDead)
        {
            if (transform.position.z < GameManager.RowDisableZPos + 0.3f)
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

    private void OnTriggerEnter(Collider other)
    {
        if(!isOnPlatform && other.tag == "Platform")
        {
            isOnPlatform = true;
        }
    }
}
