using UnityEngine;
using UnityEngine.UI;
public class Warehouse : MonoBehaviour
{
    [SerializeField] private Toggle[] products;
    [SerializeField] private Button orderButton;
    [SerializeField] private Text priceText;
    [SerializeField] private GameController gameController;

    private int _orderedProductNumber;
    private int _priceProduct = 5;
    private int _orderPrice;
    private AudioManager _audioManager;
    private SaveData _saveData;
   
    private void Start()
    {
        _saveData = SaveData.Instance;
        _audioManager = AudioManager.Instance;
        orderButton.onClick.AddListener(OnClickedOrderButton);
        foreach (var product in products)
        {
            product.onValueChanged.AddListener(isOn => ChooseProduct(product));
        }
    }
    public void RefreshWarehouse()
    {
        foreach (var product in products)
        {
            product.isOn = false;
        }
        _orderedProductNumber = 0;
        _orderPrice = 0;
        priceText.text = $"${_orderPrice}";
        priceText.color = Color.green;
        orderButton.interactable = false;
    }

    private void ChooseProduct(Toggle product)
    {
        _audioManager.PlayClip(AudioManager.Clip.SelectProduct);
        _orderedProductNumber+= product.isOn == true ? 1 : -1;
        ControlOrderButton();
    }
    private void ControlOrderButton()
    {
        if (!(_orderedProductNumber >= 0))
        {
            return;
        }
        _orderPrice = _orderedProductNumber * _priceProduct;
        priceText.text = $"${_orderPrice}";
        priceText.color = _saveData.AllData.cash >= _orderPrice ? Color.green : Color.red;
        orderButton.interactable = _orderedProductNumber > 0 && _saveData.AllData.cash >= _orderPrice;
    }
    private void OnClickedOrderButton()
    {
        for (int i = 0; i < products.Length; i++)
        {
            if (products[i].isOn)
            {
                gameController.RefreshProducts(productId: i, isAdded: true);
            }
        }
        _saveData.SaveCash(-_orderPrice);
        RefreshWarehouse();
    }
}
