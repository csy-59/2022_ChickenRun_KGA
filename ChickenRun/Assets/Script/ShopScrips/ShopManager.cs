using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum ModelType
{
    Hannah,
    Pips,
    Diva,
    ModelCount
}

public class ShopManager : SingletonBehaviour<ShopManager>
{
    private ModelType currentType = ModelType.Hannah;

    public GameObject PlayerModels;
    private Rigidbody playerModelsRigid;
    public float ModelTransSpeed = 5f;

    private bool isModelMoving;
    private Vector3 targetPos;

    private Vector3 rightPosOffset = new Vector3(-5f, 0f, 0f);
    private Vector3 leftPosOffset = new Vector3(5f, 0f, 0f);

    public ModelType ShownModel 
    { 
        get 
        { 
            return currentType; 
        } 
        
        private set 
        { 
            currentType = value;
            OnShowModelChange.Invoke(currentType);
        } 
    }

    public UnityEvent<ModelType> OnShowModelChange = new UnityEvent<ModelType>();

    // Start is called before the first frame update
    void Start()
    {
        ShownModel = ModelType.Hannah;
        playerModelsRigid = PlayerModels.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(isModelMoving)
        {
            Vector3 currentPos = Vector3.Lerp(PlayerModels.transform.position, targetPos, ModelTransSpeed * Time.deltaTime);

            if((targetPos - currentPos).sqrMagnitude < 0.0001f)
            {
                PlayerModels.transform.position = targetPos;
                isModelMoving = false;
            }
            else
            {
                playerModelsRigid.MovePosition(currentPos);
            }
        }
    }

    public void OnClickModel()
    {

    }

    public void OnClickRight()
    {
        if (isModelMoving || (int)currentType - 1 < 0)
        {
            return;
        }

        targetPos = PlayerModels.transform.position + rightPosOffset;
        --ShownModel;
        isModelMoving = true;
    }

    public void OnClickLeft()
    {
        if (isModelMoving || (int)currentType + 1 == (int)ModelType.ModelCount)
        {
            return;
        }

        targetPos = PlayerModels.transform.position + leftPosOffset;
        ++ShownModel;
        isModelMoving = true;
    }

    public void ReturnToMain()
    {

    }

    public void OnClickBuy()
    {

    }

}
