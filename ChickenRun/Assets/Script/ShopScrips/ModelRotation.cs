using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelRotation : MonoBehaviour
{
    public ShopManager manager;
    public float RotateSpeed;

    private bool isBeingShown = false;
    public ModelType MyType = ModelType.Hannah;
    private Vector3 origianlRotate = new Vector3(0f, 180f, 0f);

    private void OnEnable()
    {
        manager.OnShowModelChange.RemoveListener(ShownStart);
        manager.OnShowModelChange.AddListener(ShownStart);
    }

    void Update()
    {
        if(isBeingShown)
        {
            transform.Rotate(0f, RotateSpeed * Time.deltaTime, 0f);
        }
    }

    private void ShownStart(ModelType type)
    {
        if(type == MyType)
        {
            isBeingShown = true;
        }
        else
        {
            isBeingShown = false;
            transform.rotation = Quaternion.Euler(origianlRotate);
        }
    }
}
