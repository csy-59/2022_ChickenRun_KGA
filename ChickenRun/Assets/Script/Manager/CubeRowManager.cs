using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeRowManager : MonoBehaviour
{
    public PlatformRowMovement[] CubeRows;

    //private LinkedList<PlatformRowMovement> rowList;
    //private LinkedListNode<PlatformRowMovement> currentRow;

    private int currentRow = 0;
    private int cubeRowCount;

    private void Awake()
    {
        //rowList = new LinkedList<PlatformRowMovement>(CubeRows);
        //currentRow = rowList.First;
        cubeRowCount = CubeRows.Length;
    }

    public Transform GetTargetRowTransform(float rowPosition, int targetPosition, out bool isVailed)
    {
        if(targetPosition < 0 || targetPosition > 2)
        {
            isVailed = false;
            return gameObject.transform;
        }

        if(rowPosition > 0f)
        {
            int nextRow = (currentRow + 1) % cubeRowCount;
            if (CubeRows[nextRow].PreviousState != PlatformRowMovement.State.Active)
            {
                isVailed = false;
                return gameObject.transform;
            }

            currentRow = nextRow;
        }
        else if(rowPosition < 0f)
        {
            int preRow = (currentRow - 1) % cubeRowCount > 0 ? (currentRow - 1) % cubeRowCount : cubeRowCount - 1;
            if (CubeRows[preRow].PreviousState != PlatformRowMovement.State.Active)
            {
                isVailed = false;
                return gameObject.transform;
            }
            currentRow = preRow;
        }

        isVailed = true;
        return CubeRows[currentRow].PlayerTargetPosition[targetPosition];
    }
}
