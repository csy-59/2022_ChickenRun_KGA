using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State
{
    public enum StateType
    {
        IDEL,
        SINK,
        NULLSTATE
    };

    public virtual void Execute(GameObject other)
    {
        Debug.Log($"ERROR!!! WRONG EXECUDE [{other.name}]");
    }
}
