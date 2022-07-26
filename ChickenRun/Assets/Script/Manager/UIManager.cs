using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public Sprite[] ShapeSprites;

    public GameObject InGameUI;
    public TextMeshProUGUI InGameScoreUI;
    public Image ShapeImage;
    
    public GameObject GameOverUI;
    public TextMeshProUGUI GameOverUIScoreUI;
    public TextMeshProUGUI GameOverUIBestScoreUI;

    public int score = 0;

    private void OnEnable()
    {
        GameManager.Instance.OnGameOver.RemoveListener(ActiveUI);
        GameManager.Instance.OnGameOver.AddListener(ActiveUI);

        GameManager.Instance.OnSelectShape.RemoveListener(ShowShape);
        GameManager.Instance.OnSelectShape.AddListener(ShowShape);
        Debug.Log("added");

        if (!PlayerPrefs.HasKey("BestScore"))
        {
            PlayerPrefs.SetInt("BestScore", 0);
        }
        StartCoroutine(ChangeTime());
        InGameUI.SetActive(true);
        GameOverUI.SetActive(false);
    }

    public void OnDisable()
    {
        GameManager.Instance.OnGameOver.RemoveListener(ActiveUI);
        GameManager.Instance.OnSelectShape.RemoveListener(ShowShape);
    }

    private void ActiveUI()
    {
        InGameUI.SetActive(false);
        GameOverUI.SetActive(true);
        GameOverUIScoreUI.text = $"Final Score\n{score / 10}.{score % 10}s";
        int bestScore = PlayerPrefs.GetInt("BestScore");
        if (bestScore < score)
        {
            bestScore = score;
            PlayerPrefs.SetInt("BestScore", score);
        }
        GameOverUIBestScoreUI.text = $"Best Score\n{bestScore / 10}.{bestScore % 10}s";
    }

    private void ShowShape(GameManager.PlatformShape shape)
    {
        Debug.Log($"UI: {shape}");
        switch(shape)
        {
            case GameManager.PlatformShape.CIRCLE:
                ShapeImage.sprite = ShapeSprites[0];
                break;
            case GameManager.PlatformShape.TRIANGLE:
                ShapeImage.sprite = ShapeSprites[1];
                break;
            case GameManager.PlatformShape.SQUARE:
                ShapeImage.sprite = ShapeSprites[2];
                break;
        }

        Debug.Log(ShapeImage.sprite);
    }

    private IEnumerator ChangeTime()
    {
        while(!GameManager.Instance.IsGameOver)
        {
            yield return new WaitForSeconds(0.1f);
            ++score;
            InGameScoreUI.text = $"{score/10}.{score%10}s";
        }
    }
}
