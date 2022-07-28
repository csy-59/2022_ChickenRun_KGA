using UnityEngine;
using Assets;

public class Model : MonoBehaviour
{
    public PlayerModelType MyType = PlayerModelType.Hannah;

    public ShopManager manager;

    // ȸ�� ����
    private Vector3 origianlRotation = new Vector3(0f, 180f, 0f);
    private float rotateSpeed = 45f;
    private bool isBeingShown = false;

    // �ִϸ��̼� ����
    private Animator animator;

    private void OnEnable()
    {
        manager.OnShowModelChange.RemoveListener(ShownStart);
        manager.OnShowModelChange.AddListener(ShownStart);

        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if(isBeingShown)
        {
            transform.Rotate(0f, -rotateSpeed * Time.deltaTime, 0f);

            if(Input.GetMouseButtonDown(0))
            {
                animator.SetTrigger(AnimationID.Animations[Random.Range(0, AnimationID.Animations.Length)]);
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
