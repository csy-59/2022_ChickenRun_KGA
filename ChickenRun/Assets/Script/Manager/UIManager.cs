using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : SingletonBehaviour<UIManager>
{
    public Sprite[] ShapeSprites;

    public GameObject InGameUI;
    public TextMeshProUGUI InGameScoreUI;
    public Image ShapeImage;
    
    public GameObject GameOverUI;
    public TextMeshProUGUI GameOverUIScoreUI;
    public TextMeshProUGUI GameOverUIBestScoreUI;

    public int score = 0;

    public void Awake()
    {
        if (!PlayerPrefs.HasKey("BestScore"))
        {
            PlayerPrefs.SetInt("BestScore", 0);
        }
        StartCoroutine(ChangeTime());
    }

    public void ActiveUI(bool isGameOver)
    {
        InGameUI.SetActive(!isGameOver);
        GameOverUI.SetActive(isGameOver);
        if(isGameOver)
        {
            GameOverUIScoreUI.text = $"Final Score\n{score / 10}.{score % 10}s";
            int bestScore = PlayerPrefs.GetInt("BestScore");
            if(bestScore < score)
            {
                bestScore = score;
                PlayerPrefs.SetInt("BestScore", score);
            }
            GameOverUIBestScoreUI.text = $"Best Score\n{bestScore / 10}.{bestScore % 10}s";
        }
    }

    public void ShowShape(GameManager.PlatformShape shape)
    {
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
