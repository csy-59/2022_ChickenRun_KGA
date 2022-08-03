using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeInput : SingletonBehaviour<SwipeInput>
{
    private Vector2 touchStartPos;
    private Vector2 touchMovedPos;
    private bool isMoving;
    private static readonly float sensitivity = 50f;

    public float Z { get; private set; }
    public float X { get; private set; }
    public bool HasInput { get; private set; }

    void Update()
    {
        Z = X = 0f;
        HasInput = false;

        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Debug.Log("touch");
        
            if (!isMoving)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    isMoving = true;
                    touchStartPos = touch.position;
                }
            }
            else
            {
                if (touch.phase == TouchPhase.Moved)
                {
                    touchMovedPos = touch.position;
                    Vector3 touchOffset = touchMovedPos - touchStartPos;
                    if (Mathf.Abs(touchOffset.x) > sensitivity || Mathf.Abs(touchOffset.y) > sensitivity)
                    {
                        isMoving = false;
        
                        if(Mathf.Abs(touchOffset.y) > Mathf.Abs(touchOffset.x))
                        {
                            Z = (touchOffset.normalized.y < 0 ? -1 : 1);
                            HasInput = true;
                        }
                        else if (Mathf.Abs(touchOffset.y) < Mathf.Abs(touchOffset.x))
                        {
                            X = (touchOffset.normalized.x < 0 ? -1 : 1);
                            HasInput = true;
                        }
                    }
                }
                else if(touch.phase == TouchPhase.Ended)
                {
                    // ÅÍÄ¡ÇÔ
                    isMoving = false;
                }
            }
        }
    }
}
