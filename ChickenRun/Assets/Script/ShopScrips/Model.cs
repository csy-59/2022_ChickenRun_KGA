using UnityEngine;
using Assets;

public class Model : MonoBehaviour
{
    public ShopManager Manager;

    // 모델 타입
    public PlayerModelType MyType = PlayerModelType.Hannah;

    // 회전 관련
    private static readonly Vector3 origianlRotation = new Vector3(0f, 180f, 0f);
    private const float rotateSpeed = 45f;
    private bool isBeingShown = false;

    // 애니메이션 관련
    private Animator animator;

    private void OnEnable()
    {
        Manager.OnShowModelChange.RemoveListener(ShownStart);
        Manager.OnShowModelChange.AddListener(ShownStart);

        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if(isBeingShown)
        {
            transform.Rotate(0f, -rotateSpeed * Time.deltaTime, 0f);

            if(Input.GetMouseButtonDown(0))
            {
                animator.SetTrigger(AnimationID.Animations[Random.Range(0, AnimationID.AnimationsCount)]);
            }
        }
    }

    private void ShownStart(PlayerModelType type)
    {
        if(type == MyType)
        {
            isBeingShown = true;
        }
        else
        {
            isBeingShown = false;
            transform.rotation = Quaternion.Euler(origianlRotation);
            animator.SetTrigger(AnimationID.Idle);
        }
    }
}
