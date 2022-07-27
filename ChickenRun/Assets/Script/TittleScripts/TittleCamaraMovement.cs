using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TittleCamaraMovement : MonoBehaviour
{
    public float RotateSpeed = 10f;

    private bool inTittle = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        if(inTittle)
        {
            transform.Rotate(0f, RotateSpeed * Time.deltaTime, 0f);
        }
    }
}
