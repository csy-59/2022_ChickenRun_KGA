using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class OpeningManager : MonoBehaviour
{
    // UI 관련
    public TextMeshProUGUI startText;
    public float TextAlphaChangeSpeed = 1f;

    private float startAlpha = 1f;
    private float endAlpha = 0f;
    private float elapsedTime = 0f;

    // Model 관련
    public GameObject[] PlayerModel;

    private void Awake()
    {
        if(!PlayerPrefs.HasKey("SelectedPlayer"))
        {
            PlayerPrefs.SetInt("SelectedPlayer", 0);
        }
        int selectedPlayer = PlayerPrefs.GetInt("SelectedPlayer");
        PlayerModel[selectedPlayer].SetActive(true);
    }

    private void Update()
    {
        if (elapsedTime >= TextAlphaChangeSpeed)
        {
            startText.color = new Color(1f, 1f, 1f, endAlpha);
            elapsedTime = 0f;

            float temp = startAlpha;
            startAlpha = endAlpha;
            endAlpha = temp;
        }
        else
        {
            elapsedTime += Time.deltaTime;
            var newAlpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / TextAlphaChangeSpeed);
            startText.color = new Color(1f, 1f, 1f, newAlpha);
        }

        if(Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene(1);
        }
    }
}
