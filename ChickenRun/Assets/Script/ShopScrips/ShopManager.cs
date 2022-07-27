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

    public float CamaraMoveSpeed = 1f;
    private float elapsedTime = 0f;
    private bool isCameraMoving = false;
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
    }

    // Update is called once per frame
    void Update()
    {
        if(isCameraMoving)
        {
            Vector3 currentPos = Vector3.Lerp(Camera.main.transform.position, targetPos, elapsedTime / CamaraMoveSpeed);
            Camera.main.transform.Translate(currentPos);

            if((targetPos - currentPos).sqrMagnitude < 0.01f)
            {
                Camera.main.transform.position = targetPos;
                isCameraMoving = false;
            }
        }
    }

    public void OnClickModel()
    {

    }

    public void OnClickRight()
    {
        if (isCameraMoving || (int)currentType - 1 < 0)
        {
            return;
        }

        targetPos = Camera.main.transform.position + rightPosOffset;
        isCameraMoving = true;
        --ShownModel;
    }

    public void OnClickLeft()
    {
        if (isCameraMoving || (int)currentType + 1 == (int)ModelType.ModelCount)
        {
            return;
        }

        targetPos = Camera.main.transform.position + leftPosOffset;
        isCameraMoving = true;
        ++ShownModel;
    }

    public void ReturnToMain()
    {

    }

    public void OnClickBuy()
    {

    }

}
