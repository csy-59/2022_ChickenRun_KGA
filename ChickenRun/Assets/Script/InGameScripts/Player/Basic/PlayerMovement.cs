using UnityEngine;
using Assets;

public class PlayerMovement : MonoBehaviour
{
    // 이동 관련
    private bool isOnPlatform = false;
    private bool isMoving = false;
    
    public float MoveSpeed = 1f;

    private PlayerInput input;
    private Rigidbody rigid;

    private CubeRowManager rowManager;
    private Transform targetPos;


    // 생명 관련
    private PlayerEncounter encounter;

    // 효과 관련
    public Animator Anim;
    private AudioSource audioSource;

    private void Awake()
    {
        input = GetComponent<PlayerInput>();
        rigid = GetComponent<Rigidbody>();

        encounter = GetComponent<PlayerEncounter>();
        
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        rowManager = FindObjectOfType<CubeRowManager>().GetComponent<CubeRowManager>();
    }
    private void FixedUpdate()
    {
        if(!encounter.IsPlayerDead && !encounter.IsStanned && isOnPlatform)
        {
            if (isMoving)
            {
                Vector3 targetPosition = targetPos.position;
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
            else if(input.HasInput || SwipeInput.Instance.HasInput)
            {
                GetTarget();
            }
        }

    }

    private void Update()
    {
        if(!encounter.IsPlayerDead)
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
        bool gotRightTransform = false;

        if(input.Z != 0 || input.X != 0)
            targetPos = rowManager.GetTargetRowTransform(input.Z, input.X, out gotRightTransform);
        else if(SwipeInput.Instance.X != 0 || SwipeInput.Instance.Z != 0)
            targetPos = rowManager.GetTargetRowTransform(SwipeInput.Instance.Z, SwipeInput.Instance.X, out gotRightTransform);


        if(gotRightTransform)
        {
            isMoving = true;

            gameObject.tag = "PlayerMove";
            
            transform.parent = null;
            transform.LookAt(targetPos);
            
            Anim.SetTrigger(AnimationID.Move);
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
