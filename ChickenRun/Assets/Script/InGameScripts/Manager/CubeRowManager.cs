using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeRowManager : MonoBehaviour
{
    // Platform Row
    public PlatformRowMovement[] CubeRows;
    private int cubeRowCount;

    // ���� ��ġ
    private int currentRow = 0;
    private int currentColumn = 1;

    private void Awake()
    {
        cubeRowCount = CubeRows.Length;
    }

    /// <summary>
    /// �÷��̾� �̵� ��ġ�� ��� ť���� Transform�� ��ȯ
    /// </summary>
    /// <param name="rowPosition">���� ��ġ�� ������ row ��ġ</param>
    /// <param name="columnPosition">���� ��ġ�� ������ ť�� ��ȣ ��ġ</param>
    /// <param name="isVailed">�ܺ� ����. �̵� ������ ��ġ���� �˷���</param>
    /// <returns>ť�� ��ġ ȹ�� ���� �� �ش� Transform ��ȯ, ���� �� CubeRowManager�� gameObject�� Transform ��ȯ</returns>
    public Transform GetTargetRowTransform(float rowPosition, float columnPosition, out bool isVailed)
    {
        // ť�� ��ġ �Ǻ�
        int newColumn = currentColumn + (int)columnPosition;
        if(newColumn < 0 || newColumn > 2)
        {
            isVailed = false;
            return gameObject.transform;
        }
        currentColumn = newColumn;

        // Row ��ġ �Ǻ�
        if(rowPosition > 0f) // �������� �̵�
        {
            int nextRow = (currentRow + 1) % cubeRowCount;
            if (CubeRows[nextRow].PreviousState != PlatformRowMovement.State.Active)
            {
                isVailed = false;
                return gameObject.transform;
            }

            currentRow = nextRow;
        }
        else if(rowPosition < 0f) // �������� �̵�
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
