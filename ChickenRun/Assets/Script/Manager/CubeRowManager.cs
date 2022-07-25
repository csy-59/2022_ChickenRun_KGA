using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeRowManager : MonoBehaviour
{
    public PlatformRowMovement[] CubeRows;

    private LinkedList<PlatformRowMovement> rowList;
    private LinkedListNode<PlatformRowMovement> currentRow;

    private void Awake()
    {
        rowList = new LinkedList<PlatformRowMovement>(CubeRows);
        currentRow = rowList.First;
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
            if (currentRow.Next.Value.PreviousState != PlatformRowMovement.State.Active)
            {
                isVailed = false;
                return gameObject.transform;
            }

            currentRow = currentRow.Next;
        }
        else if(rowPosition < 0f)
        {
            if (currentRow.Previous.Value.PreviousState != PlatformRowMovement.State.Active)
            {
                isVailed = false;
                return gameObject.transform;
            }
            currentRow = currentRow.Previous;
        }

        isVailed = true;
        return currentRow.Value.PlayerTargetPosition[targetPosition];
    }
}
