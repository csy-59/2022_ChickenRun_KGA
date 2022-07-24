using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformRowSettings : MonoBehaviour
{
    public Transform[] CubePosition = new Transform[3];
    public GameObject[] CubePrefabs = new GameObject[3];
    public Transform[] PlayerTargetPosition = new Transform[3];

    private const int cubePositionCount = 3;

    private void OnEnable()
    {
        SetRow();
    }

    private void SetRow()
    {
        bool[] isFilled = new bool[3];
        foreach (GameObject cube in CubePrefabs)
        {
            int randomPosition;
            do
            {
                randomPosition = Random.Range(0, cubePositionCount);
            } while (isFilled[randomPosition]);

            isFilled[randomPosition] = true;
            cube.transform.position = CubePosition[randomPosition].position;
        }
    }
}
