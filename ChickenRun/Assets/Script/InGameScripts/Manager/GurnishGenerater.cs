using System.Collections;
using UnityEngine;

public class GurnishGenerater : MonoBehaviour
{
    // ���Ͻ� ������
    public GameObject[] GurnishPrefabs;
    private int gurnishCount;

    // ���Ͻ� ���� ��ġ
    public Transform[] GeneratePosition;
    private int positionCount;

    // ��Ÿ��
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
            // ���
            currentCoolTime = Random.Range(GameManager.Instance.MinGurnishCooltime, GameManager.Instance.MaxGurnishCooltime);
            yield return new WaitForSeconds(currentCoolTime);

            // ���Ͻ� ����
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
