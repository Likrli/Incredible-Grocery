using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField] private int orderedProductsNumber;
    [SerializeField] private List<int> orderedProductsId;
    [SerializeField] private Products[] allProducts;
    [SerializeField] private Text cashAmount;
    [SerializeField] private Transform[] points;
    [SerializeField] private GameObject buyer;
    [SerializeField] private GameObject notification;
    [SerializeField] private GameObject mainCanvas;
    public int OrderedProductsNumber => orderedProductsNumber;
    public Products[] AllProducts => allProducts;
    public List<int> OrderedProductsId => orderedProductsId;
    public List<Buyer> BuyersQueue => _buyersQueue;
    public Transform[] Points => points;
    public delegate void OnRivesedQueue();
    public event OnRivesedQueue RivesedQueue;

    private List<Buyer> _buyersQueue = new List<Buyer>();
    private List<Buyer> _buyersInStore = new List<Buyer>();
    private Storage _storage;
    private SaveData _saveData;
    private AudioManager _audioManager;


    private void Start()
    {
        _saveData = SaveData.instance;
        _audioManager = _saveData.audioManager;
        _storage = GetComponent<Storage>();
        _saveData.RecalculatedCash += UpdateTextCash;
        _saveData.ReceivedNotification += ShowNotification;
        _storage.ReceivedCash += OnGetCash;
        GenerateBuyers();
        UpdateTextCash(_saveData.Cash);
        for (int i = 0; i < allProducts.Length; i++)
        {
            allProducts[i].ProductAmount = _saveData.product.productsAmount[i];
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
        newNotification.SetNotificate(value);
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
        RivesedQueue?.Invoke();
    }
    public void SayGoodbyeBuyer(Buyer buyer)
    {
        _buyersInStore.Remove(buyer);
    }
    public void ServeBuyer(int newOrderedProductsNumber, List<int> newOrderedProductsId)
    {
        orderedProductsId.Clear();
        orderedProductsId.AddRange(newOrderedProductsId);
        orderedProductsNumber = newOrderedProductsNumber;
        _storage.ShowStorage();
    }
    public void RefreshProducts(int productId, bool isAdded)
    {
        _saveData.product.productsAmount[productId] = isAdded ? _saveData.product.productsAmount[productId] + 1 : _saveData.product.productsAmount[productId] - 1;
        allProducts[productId].ProductAmount = _saveData.product.productsAmount[productId];
    }
    private void OnGetCash(bool wasHappyBuyer, int rightProductsAmount)
    {
        if (rightProductsAmount > 0)
        {
            _audioManager.PlayClip(AudioManager.Clip.Money);
            _saveData.SaveCash(newCash: rightProductsAmount * (wasHappyBuyer? 20 : 10));
        }
    }

    private void PlaySoundsButton()
    {
        _audioManager.PlayClip(clip: AudioManager.Clip.ClickButton);
    }
}
