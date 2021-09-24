using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public int NumberOrderedProducts { get { return _numberOrderedProducts; } }
    public Products[] AllProducts { get { return _allProducts; } }
    public List<int> IdOrderedProducts { get { return _idOrderedProducts; } }
    [SerializeField] private int _numberOrderedProducts;
    [SerializeField] private Text _amountCash;

    [SerializeField] private Products[] _allProducts;
    [SerializeField] private List<int> _idOrderedProducts;

    [SerializeField] private GameObject _buyer;
    [SerializeField] private GameObject _bubleWithOrder;
    [SerializeField] private GameObject[] _orderedProducts;

    [SerializeField] private Storage _storage;
    [SerializeField] private SaveData _saveData;
    [SerializeField] private AudioManager _audioManager;

    private void Start()
    {
        _saveData = GetComponent<SaveData>();
        _audioManager = GetComponent<AudioManager>();
        _storage = GetComponent<Storage>();
        _saveData.Recalculation += UpdateTextCash;
        _storage.CashFlow += OnGetCash;
        GenerateBuyer();
        UpdateTextCash(_saveData.Cash);
    }

    private void UpdateTextCash(int cash)
    {
        _amountCash.text = $"$ {cash}";
    }
    public void GenerateBuyer() => Invoke(nameof(SpawnBuyer), 1f);
    private void SpawnBuyer()
    {
        Instantiate(_buyer);
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
        _bubleWithOrder.SetActive(false);
        _audioManager.PlayClip(1, AudioManager.Clip.CloseBuble);
        _storage.ShowStorage();
    }
    private void SelectProducts()
    {
        _audioManager.PlayClip(1, AudioManager.Clip.SpawnBuble); //Play clip spawn cloud
        _bubleWithOrder.SetActive(true);
        for (int i = 0; i < _orderedProducts.Length; i++)
        {
            _orderedProducts[i].SetActive(false); 
        }
        _numberOrderedProducts = Random.Range(1, 4); //generate new amount products for order new buyer
        List<int> idAllProducts = new List<int>();
        List<Sprite> spriteAllProducts = new List<Sprite>();
        foreach (var productId in _allProducts)
        {
            idAllProducts.Add(productId.IdProduct);
            spriteAllProducts.Add(productId.SpriteProduct);
        }
        _idOrderedProducts.Clear();
        for (int i = 0; i < _numberOrderedProducts; i++)
        {
            int selectedItem = Random.Range(0, idAllProducts.Count);
            _idOrderedProducts.Add(idAllProducts[selectedItem]);
            _orderedProducts[i].SetActive(true); //Activate needs amount products
            _orderedProducts[i].GetComponent<Image>().sprite = spriteAllProducts[selectedItem];
            Debug.Log($"Ordered product with id: {idAllProducts[selectedItem]}");
            idAllProducts.RemoveAt(selectedItem);
            spriteAllProducts.RemoveAt(selectedItem);
        }
    }
    private void OnGetCash(bool emotionBuyer, int amountRightProducts)
    {
        if (amountRightProducts > 0)
        {
            _audioManager.PlayClip(1, AudioManager.Clip.Money);
            switch (emotionBuyer)
            {
                case false:
                    _saveData.SaveCash(newCash: amountRightProducts * 10); // Pay for correct products
                    break;
                case true:
                    _saveData.SaveCash(newCash: amountRightProducts * 20); // Pay x2 cash
                    break;
            }
        }
    }

}
