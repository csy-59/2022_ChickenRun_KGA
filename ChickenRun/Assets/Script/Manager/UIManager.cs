using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject InGameUI;
    public GameObject GameOverUI;

    private void OnEnable()
    {
        GameManager.Instance.OnActiveUI.RemoveListener(ActiveUI);
        GameManager.Instance.OnActiveUI.AddListener(ActiveUI);
    }

    private void ActiveUI(bool isGameOver)
    {
        InGameUI.SetActive(!isGameOver);
        GameOverUI.SetActive(isGameOver);
    }

    private void OnDisable()
    {
        GameManager.Instance.OnActiveUI.RemoveListener(ActiveUI);
    }
}
