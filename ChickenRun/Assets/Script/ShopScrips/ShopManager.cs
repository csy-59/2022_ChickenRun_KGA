using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Assets;
using TMPro;

public class ShopManager : SingletonBehaviour<ShopManager>
{
    // 모델 이동 방향
    private enum ButtonDirection
    {
        Right = -1,
        Left = 1
    }

    // 모델 이동
    private Rigidbody playerModelsRigid;
    private const float modelSwitchingSpeed = 6f;
    private bool isModelMoving;

    public GameObject LeftButton;
    public GameObject RightButton;

    // 이동 위치 관련
    private Vector3 targetPos;
    private static readonly Vector3 rightPosOffset = new Vector3(-5f, 0f, 0f);
    private static readonly Vector3 leftPosOffset = new Vector3(5f, 0f, 0f);

    // 지금 보여지는 모델 관련
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


    public TextMeshProUGUI pos;
    void Start()
    {
        playerModelsRigid = GetComponent<Rigidbody>();

        CurrentModelType = PlayerModelType.Hannah;
    }

    void FixedUpdate()
    {
        if(isModelMoving)
        {
            Vector3 currentPos = Vector3.Lerp(transform.position, targetPos, modelSwitchingSpeed * Time.deltaTime);

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

    private void Update()
    {        
        if (SwipeInput.Instance.X > 0)
        {
            OnClickLeft();
        }
        
        else if (SwipeInput.Instance.X < 0)
        {
            OnClickRight();
        }
    }

    public void OnClickLeft()
    {
        ButtonClicked((int)ButtonDirection.Left);
    }

    public void OnClickRight()
    {
        ButtonClicked((int)ButtonDirection.Right);
    }

    /// <summary>
    /// 버튼이 클릭 후 처리
    /// </summary>
    /// <param name="clickDirection">왼쪽 방향: 1 | 오른쪽 방향: -1</param>
    private void ButtonClicked(int clickDirection)
    {
        if (isModelMoving || 
            (int)CurrentModelType + clickDirection < 0 || 
            (int)CurrentModelType + clickDirection >= (int)PlayerModelType.ModelCount)
        {
            return;
        }

        CurrentModelType += clickDirection;
        LeftButton.SetActive((int)CurrentModelType + clickDirection < (int)PlayerModelType.ModelCount);
        RightButton.SetActive((int)CurrentModelType + clickDirection >= 0);

        targetPos = transform.position + ( (clickDirection > 0) ? leftPosOffset : rightPosOffset);
        isModelMoving = true;
    }
}
