using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model : MonoBehaviour
{
    public ShopManager manager;
    public float RotateSpeed;

    private bool isBeingShown = false;
    public ModelType MyType = ModelType.Hannah;
    private Vector3 origianlRotate = new Vector3(0f, 180f, 0f);
    private Animator animator;
    private string[] triggerName = { 
        "Idle",
        "Walk",
        "Jump",
        "Fear",
        "Fly",
        "Spin",
        "Death",
        "Bounce",
        "Munch",
        "Roll",
        "Clicked"
    };

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
            transform.Rotate(0f, RotateSpeed * Time.deltaTime, 0f);

            if(Input.GetMouseButtonDown(0))
            {
                animator.SetTrigger(triggerName[Random.Range(0, triggerName.Length)]);
            }
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
            animator.SetTrigger("Idle");
            transform.rotation = Quaternion.Euler(origianlRotate);
        }
    }
}
