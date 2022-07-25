using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public bool Up { get; private set; }
    public bool Down { get; private set; }
    public bool Left { get; private set; }
    public bool Right { get; private set; }

    // Update is called once per frame
    void Update()
    {
        Up = Down = Left = Right = false;

        if (Input.GetAxis("Vertical") > 0)
            Up = true;
        else
            Down = true;

        if (Input.GetAxis("Horizontal") > 0)
            Right = true;
        else
            Left = true;
    }
}
              