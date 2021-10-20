using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
public class Storage : MonoBehaviour
{
    [SerializeField] private int rightProductsAmount;
    [SerializeField] private Toggle[] products;
    [SerializeField] private Text[] productsAmount;
    [SerializeField] private Image[] collectedProductsImgs;
    [SerializeField] private Image[] checkmarkImgs;
    [SerializeField] private Sprite cancelledBadge;
    [SerializeField] private Sprite selectedBadge;
    [SerializeField] private Button sell;
    [SerializeField] private Button order;
    [SerializeField] private Button closeWarehouse;
    [SerializeField] private Image sellImg;
    [SerializeField] private GameObject collectedOrderBubble;
    [SerializeField] private GameObject[] collectedProducts;
    [SerializeField] private Animator storageAnimator;
    [SerializeField] private GameController gameController;

    public delegate void ReceivedEmotionHandler(bool emotion);
    public event ReceivedEmotionHandler ReceivedEmotion;

    public delegate void ReceivedCashHandler(bool emotion, int numberRightProducts);
    public event ReceivedCashHandler ReceivedCash;

    public delegate void StoppedBuyersTimeHandler();
    public event StoppedBuyersTimeHandler StoppedBuyersTime;

    private const string Show = "Show";
    private const string Hide = "Hide";
    private const string WarehouseShow = "WarehouseShow";
    private const string WarehouseHide = "WarehouseHide";

    private int _selectedProductsNumber;
    private bool _wasHappyBuyer;
    private AudioManager _audioManager;
    private Warehouse _warehouse;

    private void Start()
    {
        _audioManager = AudioManager.Instance;
        _warehouse = GetComponent<Warehouse>();
        sell.onClick.AddListener(OnClickedSellButton);
        order.onClick.AddListener(OnClickedOrderButton);
        closeWarehouse.onClick.AddListener(OnClickedCloseWarehouseButton);
        foreach (var product in products)
        {
            product.onValueChanged.AddListener(isOn => ChooseProduct(product));
        }
    }

    public void ShowStorage()
    {
        storageAnimator.ResetTrigger(Hide);
        storageAnimator.ResetTrigger(WarehouseHide);
        storageAnimator.ResetTrigger(WarehouseShow);
        foreach (var product in products)
        {
            product.isOn = false;
        }
        RefreshStorage();
        storageAnimator.SetTrigger(Show);
    }
    private void OnClickedOrderButton()
    {
        _warehouse.RefreshWarehouse();
        storageAnimator.SetTrigger(WarehouseShow);
    }
    private void OnClickedCloseWarehouseButton()
    {
        storageAnimator.SetTrigger(WarehouseHide);
        RefreshStorage();
    }

    private void ChooseProduct(Toggle product)
    {
        _audioManager.PlayClip(AudioManager.Clip.SelectProduct);
        _selectedProductsNumber += product.isOn == true ? 1 : -1;
        ActivateBtnSell();
    }

    private void ActivateBtnSell()
    {
        if (_selectedProductsNumber != gameController.OrderedProductsId.Count) 
        {
            sell.interactable = false;
            sellImg.color = ChangeAlphaColor(color: sellImg.color, alpha: .5f);
        }
        else
        {
            sell.GetComponent<Button>().interactable = true;
            sellImg.color = Color.white;
        }
    }

    private Color ChangeAlphaColor(Color color, float alpha)
    {
        color.a = alpha;
        return color;
    }

    private void OnClickedSellButton()
    {
        foreach (var collectedProduct in collectedProducts)
        {
            collectedProduct.GetComponent<Toggle>().isOn = false;
            collectedProduct.SetActive(false);
        }
        storageAnimator.SetTrigger(Hide);
        StoppedBuyersTime?.Invoke();
        StartCoroutine(ShowCollectedProducts());
    }
    private void RefreshStorage()
    {
        for (int i = 0; i < productsAmount.Length; i++)
        {
            productsAmount[i].text = $"x{gameController.AllProducts[i].ProductAmount}";
            products[i].GetComponentInChildren<Image>().color = gameController.AllProducts[i].ProductAmount != 0 ? Color.white : ChangeAlphaColor(color: products[i].GetComponentInChildren<Image>().color, alpha: .3f);
            products[i].interactable = gameController.AllProducts[i].ProductAmount > 0;
        }
    }
    public void ResetStorage()
    {
        sell.interactable = false;
        foreach (var collectedProduct in collectedProducts)
        {
            collectedProduct.GetComponent<Toggle>().isOn = false;
            collectedProduct.SetActive(false);
        }
        storageAnimator.ResetTrigger(Show);
        storageAnimator.SetTrigger(Hide);
        storageAnimator.SetTrigger(WarehouseHide);
    }
    private IEnumerator ShowCollectedProducts()
    {
        List<int> collectedProductsId = new List<int>();
        for (int i = 0; i < products.Length; i++)
        {
            if (products[i].isOn)
            {
                collectedProductsId.Add(i);
            }
        }
        collectedOrderBubble.SetActive(true);
        _audioManager.PlayClip(AudioManager.Clip.SpawnBubble);
        for (int i = 0; i < gameController.OrderedProductsId.Count; i++)
        {
            collectedProducts[i].SetActive(true);
            collectedProductsImgs[i].sprite = gameController.AllProducts[collectedProductsId[i]].ProductSprite;
            gameController.RefreshProducts(productId: collectedProductsId[i], isAdded: false);
        }
        yield return new WaitForSeconds(1f);
        _wasHappyBuyer = true;
        rightProductsAmount = 0;
        foreach (var checkmark in checkmarkImgs)
        {
            checkmark.sprite = selectedBadge;
        }
        for (int i = 0; i < gameController.OrderedProductsId.Count; i++)
        {
            CheckSelectedProducts(productNumber: i, selectedProductsId: collectedProductsId);
            yield return new WaitForSeconds(.5f);
        }
        yield return new WaitForSeconds(1f);
        collectedOrderBubble.SetActive(false);
        _audioManager.PlayClip(AudioManager.Clip.CloseBubble);
        ReceivedEmotion?.Invoke(_wasHappyBuyer);
        ReceivedCash?.Invoke(_wasHappyBuyer, rightProductsAmount);
    }

    private void CheckSelectedProducts(int productNumber, List<int> selectedProductsId)
    {
        for (int i = 0; i < gameController.OrderedProductsId.Count; i++)
        {
            if (gameController.OrderedProductsId.Contains(selectedProductsId[productNumber]))
            {
                rightProductsAmount++;
                checkmarkImgs[productNumber].sprite = selectedBadge;
                break;
            }
            else
            {
                checkmarkImgs[productNumber].sprite = cancelledBadge;
            }
        }
        var cancelledCheckmarks = from checkmark in checkmarkImgs where checkmark.sprite == cancelledBadge select checkmark;
        _wasHappyBuyer = cancelledCheckmarks.Count() == 0;
        collectedProducts[productNumber].GetComponent<Toggle>().isOn = true;
    }

}
