using UnityEngine;
using UnityEngine.Events;
using Assets;

public class ShopManager : SingletonBehaviour<ShopManager>
{
    // �� �̵�
    private Rigidbody playerModelsRigid;
    private const float modelSwitchingTime = 4f;
    private bool isModelMoving;

    // �̵� ��ġ ����
    private Vector3 targetPos;
    private static readonly Vector3 rightPosOffset = new Vector3(-5f, 0f, 0f);
    private static readonly Vector3 leftPosOffset = new Vector3(5f, 0f, 0f);

    // ���� �������� �� ����
    private PlayerModelType currentType;
    public PlayerModelType CurrentModelType 
    { 
        get { return currentType; } 
        
        private set 
        { 
            currentType = value;
            OnShowModelChange.Invoke(currentType);
        } 
    }
    public UnityEvent<PlayerModelType> OnShowModelChange = new UnityEvent<PlayerModelType>();

    void Start()
    {
        playerModelsRigid = GetComponent<Rigidbody>();

        CurrentModelType = PlayerModelType.Hannah;
    }

    void FixedUpdate()
    {
        if(isModelMoving)
        {
            Vector3 currentPos = Vector3.Lerp(transform.position, targetPos, modelSwitchingTime * Time.deltaTime);

            if((targetPos - currentPos).sqrMagnitude < 0.0001f)
            {
                transform.position = targetPos;
                isModelMoving = false;
            }
            else
            {
                playerModelsRigid.MovePosition(currentPos);
            }
        }
    }

    public void OnClickLeft()
    {
        ButtonClicked(1);
    }

    public void OnClickRight()
    {
        ButtonClicked(-1);
    }

    /// <summary>
    /// ��ư�� Ŭ�� �� ó��
    /// </summary>
    /// <param name="clickDirection">���� ����: 1 | ������ ����: -1</param>
    private void ButtonClicked(int clickDirection)
    {
        if (isModelMoving || 
            (int)currentType + clickDirection < 0 || 
            (int)currentType + clickDirection >= (int)PlayerModelType.ModelCount)
        {
            return;
        }

        CurrentModelType += clickDirection;

        targetPos = transform.position + ( (clickDirection > 0) ? leftPosOffset : rightPosOffset);
        isModelMoving = true;
    }
}
