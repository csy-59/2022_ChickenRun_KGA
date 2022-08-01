using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float Z { get; private set; }
    public float X { get; private set; }

    public bool HasInput { get; private set; }

    // Update is called once per frame
    void Update()
    {
        Z = X = 0f;
        HasInput = false;

        if (Input.GetAxis("Vertical") > 0f)
        {
            Z = 1f;
            HasInput = true;
        }
        else if(Input.GetAxis("Vertical") < 0f)
        {
            Z = -1f;
            HasInput = true;
        }
        else if (Input.GetAxis("Horizontal") > 0f)
        {
            X = 1f;
            HasInput = true;
        }
        else if (Input.GetAxis("Horizontal") < 0f)
        {
            X = -1f;
            HasInput = true;
        }
    }
}
              