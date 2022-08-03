using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Assets;

public class OpeningManager : MonoBehaviour
{
    // UI 관련
    public TextMeshProUGUI StartText;
    private float alphaChangeSpeed = 0.3f;

    private float startAlpha = 1f;
    private float endAlpha = 0f;
    private float elapsedTime = 0f;

    public GameObject HowToPlayPanel;

    // Model 관련
    public GameObject[] PlayerModels;

    private void Awake()
    {
        int selectedPlayer = PlayerPrefsKey.GetIntByKey(PlayerPrefsKey.SelectedPlayerKey);
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
        SceneManager.LoadScene((int) SceneType.Shop);
    }

    public void StartGame()
    {
        SceneManager.LoadScene((int) SceneType.InGame);
    }

    public void OnClickHowToPlay()
    {
        HowToPlayPanel.SetActive(true);
    }

    public void OnClickExit()
    {
        HowToPlayPanel.SetActive(false);
    }
}
