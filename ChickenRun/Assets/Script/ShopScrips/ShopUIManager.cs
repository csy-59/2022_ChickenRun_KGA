using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Assets;

public class ShopUIManager : MonoBehaviour
{
    // 상점 매니저
    public ShopManager manager;

    // 기본 환경
    public TextMeshProUGUI FlowerCountText;

    // 구매 환경
    public GameObject BuyPanel;
    public TextMeshProUGUI ModelNameText;
    public GameObject BuyButton;
    public GameObject SelectButton;
    public GameObject LetsRunButton;

    // 구매 관련
    private int flowerCount = 0;

    private void OnEnable()
    {
        manager.OnShowModelChange.RemoveListener(EditBuyPanel);
        manager.OnShowModelChange.AddListener(EditBuyPanel);

        ShopSet();

        SetFlowerCount(PlayerPrefsKey.GetIntByKey(PlayerPrefsKey.FlowerCountKey));
    }

    private void SetFlowerCount(int newFlowerCount)
    {
        flowerCount = newFlowerCount;
        PlayerPrefs.SetInt(PlayerPrefsKey.FlowerCountKey, flowerCount);
        FlowerCountText.text = flowerCount.ToString();
    }

    private void ShopSet()
    {
        PlayerPrefsKey.SetModelInfo();
    }

    public void EditBuyPanel(PlayerModelType type)
    {
        PlayerModel model = PlayerPrefsKey.GetModel(type);

        ModelNameText.text = model.Name;

        if(model.IsBought)
        {
            LetsRunButton.SetActive(model.IsSelected);
            SelectButton.SetActive(!model.IsSelected);

            BuyButton.SetActive(false);
        }
        else
        {
            BuyButton.GetComponentInChildren<TextMeshProUGUI>().text = model.Price.ToString();
            BuyButton.GetComponent<Button>().interactable = (flowerCount < model.Price);

            LetsRunButton.SetActive(false);
            SelectButton.SetActive(false);

            BuyButton.SetActive(true);
        }
    }

    public void OnClickBuyButton()
    {
        BuyButton.SetActive(false);
        SelectButton.SetActive(true);

        PlayerModel model = PlayerPrefsKey.GetModel(manager.CurrentModelType);
        SetFlowerCount(flowerCount - model.Price);

        model.IsBought = true;
    }

    public void OnClickSelecButton()
    {
        SelectButton.SetActive(false);
        LetsRunButton.SetActive(true);

        PlayerModel model = PlayerPrefsKey.GetModel(manager.CurrentModelType);
        model.IsSelected = true;
    }

    public void ReturnToMain()
    {
        SceneManager.LoadScene((int) SceneType.Main);
    }

    public void OnClickLetsRunButton()
    {
        SceneManager.LoadScene((int) SceneType.InGame);
    }
}
