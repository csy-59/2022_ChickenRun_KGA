using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using TMPro;

public class ShopUIManager : MonoBehaviour
{
    public ShopManager manager;

    public GameObject BuyPanel;
    public TextMeshProUGUI ModelNameText;
    public GameObject BuyButton;
    public GameObject SelectButton;
    public GameObject LetsGoButton;

    // Start is called before the first frame update
    void Start()
    {
        manager.OnMoveButtonClicked.RemoveListener(BuyPanelActive);
        manager.OnMoveButtonClicked.AddListener(BuyPanelActive);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void BuyPanelActive(bool isBuyable)
    {
        if(isBuyable)
        {
            ModelNameText.text = manager.ShownModel.ToString();
            BuyPanel.SetActive(true);
        }
        else
        {
            BuyPanel.SetActive(false);
        }
    }

    public void ReturnToMain()
    {
        SceneManager.LoadScene(0);
    }
}
