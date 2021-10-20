using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField] private List<int> orderedProductsId;
    [SerializeField] private Products[] allProducts;
    [SerializeField] private Text cashAmount;
    [SerializeField] private Transform[] points;
    [SerializeField] private GameObject buyer;
    [SerializeField] private GameObject notification;
    [SerializeField] private GameObject mainCanvas;
    [SerializeField] private Storage storage;
    [SerializeField] private List<Button> allGameButtons;
    public Products[] AllProducts => allProducts;
    public List<int> OrderedProductsId => orderedProductsId;
    public List<Buyer> BuyersQueue => _buyersQueue;
    public Transform[] Points => points;

    public delegate void ChangedQueueHandler();
    public event ChangedQueueHandler ChangedQueue;

    private List<Buyer> _buyersQueue = new List<Buyer>();
    private List<Buyer> _buyersInStore = new List<Buyer>();
    private SaveData _saveData;
    private AudioManager _audioManager;


    private void Start()
    {
        _saveData = SaveData.Instance;
        _audioManager = AudioManager.Instance;
        _saveData.RecalculatedCash += UpdateTextCash;
        _saveData.ReceivedNotification += ShowNotification;
        storage.ReceivedCash += OnGetCash;
        GenerateBuyers();
        UpdateTextCash(_saveData.AllData.cash);
        for (int i = 0; i < allProducts.Length; i++)
        {
            allProducts[i].ProductAmount = _saveData.AllData.productsAmount[i];
        }
        foreach (var button in allGameButtons)
        {
            button.onClick.AddListener(_audioManager.PlaySoundsButton);
        }
    }

    private void UpdateTextCash(int cash)
    {
        cashAmount.text = $"$ {cash}";
    }
    private void ShowNotification(int value)
    {
        _audioManager.PlayClip(value > 0 ? AudioManager.Clip.MoneyEarned : AudioManager.Clip.MoneySpent);
        Notification newNotification = Instantiate(notification.GetComponent<Notification>(), mainCanvas.transform);
        newNotification.SetNotificationText(value);
    }

    public void GenerateBuyers() => InvokeRepeating(nameof(SpawnBuyer), Random.Range(5f, 10f), Random.Range(5f, 10f));
    private void SpawnBuyer()
    {
        if (_buyersInStore.Count < 3)
        {
            _buyersQueue.Add(Instantiate(buyer).GetComponent<Buyer>());
            _buyersInStore.Clear();
            _buyersInStore.AddRange(_buyersQueue);
        }
    }
    public void ChangeBuyersQueue(Buyer servedBuyer)
    {
        _buyersQueue.Remove(servedBuyer);
        ChangedQueue?.Invoke();
    }
    public void SayGoodbyeBuyer(Buyer buyer)
    {
        _buyersInStore.Remove(buyer);
    }
    public void ServeBuyer(List<int> newOrderedProductsId)
    {
        orderedProductsId.Clear();
        orderedProductsId.AddRange(newOrderedProductsId);
        storage.ShowStorage();
    }
    public void RefreshProducts(int productId, bool isAdded)
    {
        if (isAdded)
        {
            _saveData.AllData.productsAmount[productId]++;
        }
        else
        {
            _saveData.AllData.productsAmount[productId]--;
        }
        allProducts[productId].ProductAmount = _saveData.AllData.productsAmount[productId];
    }
    private void OnGetCash(bool wasHappyBuyer, int rightProductsAmount)
    {
        if (rightProductsAmount > 0)
        {
            _audioManager.PlayClip(AudioManager.Clip.Money);
            _saveData.SaveCash(newCash: rightProductsAmount * (wasHappyBuyer? 20 : 10));
        }
    }
}
