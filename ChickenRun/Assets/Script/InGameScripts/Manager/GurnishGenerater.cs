using System.Collections;
using UnityEngine;

public class GurnishGenerater : MonoBehaviour
{
    // 가니쉬 프리팹
    public GameObject[] GurnishPrefabs;
    private int gurnishCount;

    // 가니쉬 생성 위치
    public Transform[] GeneratePosition;
    private int positionCount;

    // 쿨타임
    private float currentCoolTime;

    private void OnEnable()
    {
        gurnishCount = GurnishPrefabs.Length;
        positionCount = GeneratePosition.Length;

        StartCoroutine(GenerateGurnish());
    }

    private IEnumerator GenerateGurnish()
    {
        while (!GameManager.Instance.IsGameOver)
        {
            // 대기
            currentCoolTime = Random.Range(GameManager.Instance.MinGurnishCooltime, GameManager.Instance.MaxGurnishCooltime);
            yield return new WaitForSeconds(currentCoolTime);

            // 가니쉬 생성
            int position = Random.Range(0, positionCount);
            int prefabNumber = Random.Range(0, gurnishCount);

            GameObject gurnish = GurnishPrefabs[prefabNumber];
            if (gurnish.gameObject.activeSelf == true)
                yield break;

            gurnish.transform.position = GeneratePosition[position].position;
            gurnish.SetActive(true);
        }
    }
}
