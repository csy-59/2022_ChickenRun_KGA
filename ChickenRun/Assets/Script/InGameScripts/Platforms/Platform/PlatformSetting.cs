using UnityEngine;

public class PlatformSetting : MonoBehaviour
{
    public GameObject Flower;

    private void OnEnable()
    {
        float random = Random.Range(0f, 100f);

        if(random < GameManager.Instance.FlowerGenerateRate)
        {
            Flower.SetActive(true);
        }
    }

    private void OnDisable()
    {
        Flower.SetActive(false);
    }
}
