using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GurnishGenerater : MonoBehaviour
{
    public GameObject[] GurnishPrefabs;
    public Transform[] GeneratePosition;

    private float currentCoolTime;
    private int gurnishCount;
    private int positinoCount;

    private void OnEnable()
    {
        StartCoroutine(GenerateGurnish());
        gurnishCount = GurnishPrefabs.Length;
        positinoCount = GeneratePosition.Length;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator GenerateGurnish()
    {
        while (!GameManager.Instance.IsGameOver)
        {
            GurnishCooltimeSet();

            yield return new WaitForSeconds(currentCoolTime);

            int position = Random.Range(0, positinoCount);
            int prefabNumber = Random.Range(0, gurnishCount);

            GameObject gurnish = GurnishPrefabs[prefabNumber];
            if (gurnish.gameObject.activeSelf == true)
                yield break;

            gurnish.transform.position = GeneratePosition[position].position;
            gurnish.SetActive(true);
        }
    }
    private void GurnishCooltimeSet()
    {
        currentCoolTime = Random.Range(GameManager.Instance.MinGurnishCooltime, GameManager.Instance.MaxGurnishCooltime);
    }
}
