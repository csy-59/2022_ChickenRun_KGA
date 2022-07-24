using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PlayerInput input;


    private void Awake()
    {
        input = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        
    }
}
