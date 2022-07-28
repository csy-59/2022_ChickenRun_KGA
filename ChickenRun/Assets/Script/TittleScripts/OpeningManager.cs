using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Assets;

public class OpeningManager : MonoBehaviour
{
    // UI ����
    public TextMeshProUGUI StartText;
    private float alphaChangeSpeed = 0.3f;

    private float startAlpha = 1f;
    private float endAlpha = 0f;
    private float elapsedTime = 0f;

    // Model ����
    public GameObject[] PlayerModels;

    private void Awake()
    {
        if(!PlayerPrefs.HasKey("SelectedPlayer"))
        {
            PlayerPrefs.SetInt("SelectedPlayer", (int)PlayerModelType.Hannah);
        }
        int selectedPlayer = PlayerPrefs.GetInt("SelectedPlayer");
        PlayerModels[selectedPlayer].SetActive(true);
    }

    private void Update()
    {
        if (elapsedTime >= alphaChangeSpeed)
        {
            elapsedTime = 0f;

            StartText.color = new Color(1f, 1f, 1f, endAlpha);

            float temp = startAlpha;
            startAlpha = endAlpha;
            endAlpha = temp;
        }
        else
        {
            elapsedTime += Time.deltaTime;

            var newAlpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / alphaChangeSpeed);
            StartText.color = new Color(1f, 1f, 1f, newAlpha);
        }

    }

    public void MoveToStore()
    {
        SceneManager.LoadScene(2);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
}
