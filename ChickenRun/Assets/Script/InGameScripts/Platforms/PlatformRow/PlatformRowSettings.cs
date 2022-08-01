using UnityEngine;

public class PlatformRowSettings : MonoBehaviour
{
    public Transform[] CubePosition = new Transform[3];
    public GameObject[] CubePrefabs = new GameObject[3];

    private const int cubePositionCount = 3;

    private void OnEnable()
    {
        SetRow();
    }

    private void SetRow()
    {
        bool[] isFilled = new bool[cubePositionCount];

        foreach (GameObject cube in CubePrefabs)
        {
            cube.SetActive(true);

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
