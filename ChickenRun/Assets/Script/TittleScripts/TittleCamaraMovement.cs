using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TittleCamaraMovement : MonoBehaviour
{
    public float RotateSpeed = 10f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        transform.Rotate(0f, RotateSpeed * Time.deltaTime, 0f);
    }
}
