using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField] private int orderedProductsNumber;
    [SerializeField] private Text cashAmount;

    [SerializeField] private Products[] allProducts;
    [SerializeField] private List<int> orderedProductsId;

    [SerializeField] private GameObject buyer;
    [SerializeField] private GameObject orderBubble;
    [SerializeField] private GameObject[] orderedProducts;
    [SerializeField] private Button[] allButtons;

    public int OrderedProductsNumber => orderedProductsNumber;
    public Products[] AllProducts =>  allProducts;
    public List<int> OrderedProductsId => orderedProductsId;

    private Storage _storage;
    private SaveData _saveData;
    private AudioManager _audioManager;


    private void Start()
    {
        _saveData = GetComponent<SaveData>();
        _audioManager = GetComponent<AudioManager>();
        _storage = GetComponent<Storage>();
        _saveData.RecalculatedCash += UpdateTextCash;
        _storage.ReceivedCash += OnGetCash;
        GenerateBuyer();
        UpdateTextCash(_saveData.Cash);
        foreach (var button in allButtons)
        {
            button.onClick.AddListener(PlaySoundsButton);
        }
    }

    private void UpdateTextCash(int cash)
    {
        cashAmount.text = $"$ {cash}";
    }
    public void GenerateBuyer() => Invoke(nameof(SpawnBuyer), 1f);
    private void SpawnBuyer()
    {
        Instantiate(buyer);
    }
    public void ServeBuyer()
    {
        StartCoroutine(OrderGeneration());
    }
    private IEnumerator OrderGeneration()
    {
        Debug.Log("Buyer selects a product..");
        yield return new WaitForSeconds(1.5f);
        SelectProducts();
        yield return new WaitForSeconds(5f);
        orderBubble.SetActive(false);
        _audioManager.PlayClip(AudioManager.Clip.CloseBuble);
        _storage.ShowStorage();
    }
    private void SelectProducts()
    {
        _audioManager.PlayClip(AudioManager.Clip.SpawnBuble); 
        orderBubble.SetActive(true);
        for (int i = 0; i < orderedProducts.Length; i++)
        {
            orderedProducts[i].SetActive(false); 
        }
        orderedProductsNumber = Random.Range(1, 4);
        List<int> allProductsId = new List<int>();
        List<Sprite> allProductsSprite = new List<Sprite>();
        foreach (var product in allProducts)
        {
            allProductsId.Add(product.ProductId);
            allProductsSprite.Add(product.ProductSprite);
        }
        orderedProductsId.Clear();
        for (int i = 0; i < orderedProductsNumber; i++)
        {
            int selectedProduct = Random.Range(0, allProductsId.Count);
            orderedProductsId.Add(allProductsId[selectedProduct]);
            orderedProducts[i].SetActive(true); 
            orderedProducts[i].GetComponent<Image>().sprite = allProductsSprite[selectedProduct];
            Debug.Log($"Ordered product with id: {allProductsId[selectedProduct]}");
            allProductsId.RemoveAt(selectedProduct);
            allProductsSprite.RemoveAt(selectedProduct);
        }
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
