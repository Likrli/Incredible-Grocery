using UnityEngine;
using UnityEngine.UI;
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

    public delegate void OnReceivedEmotion(bool emotion);
    public event OnReceivedEmotion ReceivedEmotion;

    public delegate void OnReceivedCash(bool emotion, int numberRightProducts);
    public event OnReceivedCash ReceivedCash;

    public delegate void OnStoppedBuyersTime();
    public event OnStoppedBuyersTime StoppedBuyersTime;

    private const string Show = "Show";
    private const string Hide = "Hide";
    private const string WarehouseShow = "WarehouseShow";
    private const string WarehouseHide = "WarehouseHide";

    private int _selectedProductsNumber;
    private bool _wasHappyBuyer;
    private GameController _gameController;
    private AudioManager _audioManager;
    private Warehouse _warehouse;

    private void Start()
    {
        _gameController = GetComponent<GameController>();
        _audioManager = SaveData.instance.audioManager;
        _warehouse = GetComponent<Warehouse>();
        sell.onClick.AddListener(HideStorage);
        order.onClick.AddListener(ShowWarehouse);
        closeWarehouse.onClick.AddListener(CloseWarehouse);
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
        for (int i = 0; i < productsAmount.Length; i++)
        {
            productsAmount[i].text = $"x{_gameController.AllProducts[i].ProductAmount}";
            products[i].GetComponentInChildren<Image>().color = _gameController.AllProducts[i].ProductAmount != 0 ? Color.white : ChangeAlphaColor(color: products[i].GetComponentInChildren<Image>().color, alpha: .3f);
            products[i].interactable = _gameController.AllProducts[i].ProductAmount > 0 ? true : false; 
        }
        storageAnimator.SetTrigger(Show);
    }
    private void ShowWarehouse()
    {
        _warehouse.RefreshWarehouse();
        storageAnimator.SetTrigger(WarehouseShow);
    }
    private void CloseWarehouse()
    {
        storageAnimator.SetTrigger(WarehouseHide);
        for (int i = 0; i < productsAmount.Length; i++)
        {
            productsAmount[i].text = $"x{_gameController.AllProducts[i].ProductAmount}";
            products[i].GetComponentInChildren<Image>().color = _gameController.AllProducts[i].ProductAmount != 0 ? Color.white : ChangeAlphaColor(color: products[i].GetComponentInChildren<Image>().color, alpha: .3f);
            products[i].interactable = _gameController.AllProducts[i].ProductAmount > 0 ? true : false;
        }
    }

    public void ChooseProduct(Toggle product)
    {
        _audioManager.PlayClip(AudioManager.Clip.SelectProduct);
        _selectedProductsNumber += product.isOn == true ? 1 : -1;
        ActivateBtnSell();
    }

    private void ActivateBtnSell()
    {
        if (_selectedProductsNumber != _gameController.OrderedProductsNumber) 
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

    public void HideStorage()
    {
        for (int i = 0; i < collectedProducts.Length; i++)
        {
            collectedProducts[i].GetComponent<Toggle>().isOn = false;
            collectedProducts[i].SetActive(false);
        }
        storageAnimator.SetTrigger(Hide);
        StoppedBuyersTime?.Invoke();
        StartCoroutine(ShowCollectedProducts());
    }
    public void RefreshStorage()
    {
        sell.interactable = false;
        for (int i = 0; i < collectedProducts.Length; i++)
        {
            collectedProducts[i].GetComponent<Toggle>().isOn = false;
            collectedProducts[i].SetActive(false);
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
        for (int i = 0; i < _gameController.OrderedProductsNumber; i++)
        {
            collectedProducts[i].SetActive(true);
            collectedProductsImgs[i].sprite = _gameController.AllProducts[collectedProductsId[i]].ProductSprite;
            _gameController.RefreshProducts(productId: collectedProductsId[i], isAdded: false);
        }
        yield return new WaitForSeconds(1f);

        _wasHappyBuyer = true;
        rightProductsAmount = 0;

        for (int i = 0; i < checkmarkImgs.Length; i++)
        {
            checkmarkImgs[i].sprite = selectedBadge; 
        }
        for (int i = 0; i < _gameController.OrderedProductsNumber; i++)
        {
            CheckSelectedProducts(numberProduct: i, selectedProductsId: collectedProductsId);
            yield return new WaitForSeconds(.5f);
        }

        yield return new WaitForSeconds(1f);

        collectedOrderBubble.SetActive(false);
        _audioManager.PlayClip(AudioManager.Clip.CloseBubble);
        ReceivedEmotion?.Invoke(_wasHappyBuyer);
        ReceivedCash?.Invoke(_wasHappyBuyer, rightProductsAmount);
    }

    private void CheckSelectedProducts(int numberProduct, List<int> selectedProductsId)
    {
        for (int i = 0; i < _gameController.OrderedProductsNumber; i++)
        {
            if(_gameController.OrderedProductsId[i] == selectedProductsId[numberProduct])
            {
                rightProductsAmount++;
                checkmarkImgs[numberProduct].sprite = selectedBadge;
                break;
            }
            else
            {
                checkmarkImgs[numberProduct].sprite = cancelledBadge;
            }
        }
        for (int i = 0; i < checkmarkImgs.Length; i++)
        {
            if(checkmarkImgs[i].sprite == cancelledBadge)
            {
                _wasHappyBuyer = false;
                Debug.Log("Emotion will be negative :(");
                break;
            }
        }
        collectedProducts[numberProduct].GetComponent<Toggle>().isOn = true;
    }

}
