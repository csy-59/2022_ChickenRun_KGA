using UnityEngine;

public class PlatformSetting : MonoBehaviour
{
    public GameObject Flower;
    private const float flowerGenarateRate = 3f;

    private void OnEnable()
    {
        float random = Random.Range(0f, 100f);

        if(random < flowerGenarateRate)
        {
            Flower.SetActive(true);
        }
    }

    private void OnDisable()
    {
        Flower.SetActive(false);
    }
}
