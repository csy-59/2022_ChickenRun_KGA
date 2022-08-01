using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeRowManager : MonoBehaviour
{
    // Platform Row
    public PlatformRowMovement[] CubeRows;
    private int cubeRowCount;

    // 현재 위치
    private int currentRow = 0;
    private int currentColumn = 1;

    private void Awake()
    {
        cubeRowCount = CubeRows.Length;
    }

    /// <summary>
    /// 플래이어 이동 위치의 대상 큐브의 Transform을 반환
    /// </summary>
    /// <param name="rowPosition">현재 위치에 더해질 row 위치</param>
    /// <param name="columnPosition">현재 위치에 더해질 큐브 번호 위치</param>
    /// <param name="isVailed">외부 인자. 이동 가능한 위치인지 알려줌</param>
    /// <returns>큐브 위치 획득 성공 시 해당 Transform 반환, 실패 시 CubeRowManager의 gameObject의 Transform 반환</returns>
    public Transform GetTargetRowTransform(float rowPosition, float columnPosition, out bool isVailed)
    {
        // 큐브 위치 판별
        int newColumn = currentColumn + (int)columnPosition;
        if(newColumn < 0 || newColumn > 2)
        {
            isVailed = false;
            return gameObject.transform;
        }
        currentColumn = newColumn;

        // Row 위치 판별
        if(rowPosition > 0f) // 앞쪽으로 이동
        {
            int nextRow = (currentRow + 1) % cubeRowCount;
            if (CubeRows[nextRow].PreviousState != PlatformRowMovement.State.Active)
            {
                isVailed = false;
                return gameObject.transform;
            }

            currentRow = nextRow;
        }
        else if(rowPosition < 0f) // 뒤쪽으로 이동
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
        return CubeRows[currentRow].PlayerTargetPosition[currentColumn];
    }
}
