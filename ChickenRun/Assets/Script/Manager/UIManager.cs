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

    private int flowerCount = 0;

    private void Start()
    {
        GameManager.Instance.OnGameOver.RemoveListener(ActiveUI);
        GameManager.Instance.OnGameOver.AddListener(ActiveUI);

        GameManager.Instance.OnSelectShape.RemoveListener(ShowShape);
        GameManager.Instance.OnSelectShape.AddListener(ShowShape);

        GameManager.Instance.OnScoreChange.RemoveListener(ChangeTime);
        GameManager.Instance.OnScoreChange.AddListener(ChangeTime);

        if (!PlayerPrefs.HasKey("BestScore"))
        {
            PlayerPrefs.SetInt("BestScore", 0);
        }
        if (!PlayerPrefs.HasKey("FlowerCount"))
        {
            PlayerPrefs.SetInt("FlowerCount", 0);
        }
        InGameUI.SetActive(true);
        GameOverUI.SetActive(false);
        ShowShape();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {

        }
    }

    public void OnDisable()
    {
        GameManager.Instance.OnGameOver.RemoveListener(ActiveUI);
        GameManager.Instance.OnSelectShape.RemoveListener(ShowShape);
        GameManager.Instance.OnScoreChange.RemoveListener(ChangeTime);
    }

    private void ActiveUI()
    {
        InGameUI.SetActive(false);
        GameOverUI.SetActive(true);
        GameOverUIScoreUI.text = $"Final Score\n{GameManager.Instance.score / 10}.{GameManager.Instance.score % 10}s";
        int bestScore = PlayerPrefs.GetInt("BestScore");
        if (bestScore < GameManager.Instance.score)
        {
            bestScore = GameManager.Instance.score;
            PlayerPrefs.SetInt("BestScore", GameManager.Instance.score);
        }
        GameOverUIBestScoreUI.text = $"Best Score\n{bestScore / 10}.{bestScore % 10}s";
    }

    private void ShowShape()
    {
        switch(GameManager.Instance.Shape)
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

    private void ChangeTime()
    {
        InGameScoreUI.text = $"{GameManager.Instance.score / 10}.{GameManager.Instance.score % 10}s";
    }
}
