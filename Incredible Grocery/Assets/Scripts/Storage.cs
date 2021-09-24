using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
public class Storage : MonoBehaviour
{
    [SerializeField] private int _numberRightProducts;
    [SerializeField] private Toggle[] _products;
    [SerializeField] private Image[] ImgToggles;
    [SerializeField] private Image[] ImgCollectedProducts;
    [SerializeField] private Image[] Checkmark;
    [SerializeField] private Sprite[] _badgeCheckmark; // 0 - canceled, 1 - selected
    [SerializeField] private GameObject _buttonSell;
    [SerializeField] private GameObject _bubleCollectedOrder;
    [SerializeField] private GameObject[] _collectedProducts; //Mus for show ordered products 1-3(incl)
    [SerializeField] private Animator _animController;
    private GameController _gameController;
    private AudioManager _audioManager;
    private int _numberCollectedProducts;
    private bool _emotionBuyer;
    public delegate void GetEmotions(bool emotion);
    public event GetEmotions Emotion;

    public delegate void GetCash(bool emotion, int numberRightProducts);
    public event GetCash CashFlow;

    private void Start()
    {
        _gameController = GetComponent<GameController>();
        _audioManager = GetComponent<AudioManager>();
    }

    public void ShowStorage()
    {
        foreach (var product in _products) //Remove products choices
        {
            product.isOn = false;
        }
        _animController.SetTrigger("Show");
    }

    public void ChooseProduct(int idChooseProduct) //Player choose product of Storage and set id in this method (OnalueChanged)
    {
        _audioManager.PlayClip(2, AudioManager.Clip.SelectProduct); //Play clip choose product
        if (_products[idChooseProduct].isOn)
        {
            ImgToggles[idChooseProduct].color = ChangeAlphaColor(alpha: .3f); //Change alfa Image on 30% if product choice
            _numberCollectedProducts++; //Inc var current products 
        }
        else
        {
            ImgToggles[idChooseProduct].color = Color.white;
            _numberCollectedProducts--;
        }
        ActivateBtnSell(); //Call method for check player can be able take btn Sell
    }

    private void ActivateBtnSell()
    {
        if (_numberCollectedProducts != _gameController.NumberOrderedProducts) 
        {
            _buttonSell.GetComponent<Button>().interactable = false;
            _buttonSell.GetComponent<Image>().color = ChangeAlphaColor(alpha: .5f);
        }
        else
        {
            _buttonSell.GetComponent<Button>().interactable = true;
            _buttonSell.GetComponent<Image>().color = Color.white;
        }
    }

    private Color ChangeAlphaColor(float alpha)
    {
        Color color;
        color = Color.white;
        color.a = alpha;
        return color;
    }

    public void HideStorage() //This funct call when player take btn Sell 
    {
        for (int i = 0; i < _collectedProducts.Length; i++)
        {
            _collectedProducts[i].GetComponent<Toggle>().isOn = false; //Remove products Checkmark (correct product true|false) 
            _collectedProducts[i].SetActive(false); //Disable all obj if it`s not first buyer
        }
        _animController.SetTrigger("Hide");
        StartCoroutine(ShowCollectedProducts());
    }
    private IEnumerator ShowCollectedProducts()
    {
        yield return new WaitForSeconds(1f);
        List<int> collectedProductsId = new List<int>();
        for (int i = 0; i < _products.Length; i++)
        {
            if (_products[i].isOn)
            {
                collectedProductsId.Add(i); //Save (id) collected products which isOn into List 
            }
        }
        _bubleCollectedOrder.SetActive(true);
        _audioManager.PlayClip(1, AudioManager.Clip.SpawnBuble);
        for (int i = 0; i < _gameController.NumberOrderedProducts; i++)
        {
            _collectedProducts[i].SetActive(true);
            ImgCollectedProducts[i].sprite = _gameController.AllProducts[collectedProductsId[i]].SpriteProduct; //Important: Id product need = index product in Products
        }
        yield return new WaitForSeconds(1f); //Start check collected products until 1 sec

        _emotionBuyer = true;
        _numberRightProducts = 0; // Var for save info how much products collected right

        for (int i = 0; i < Checkmark.Length; i++) // Refresh Checkmark of collected products for purchase (if it`s not first buyer)
        {
            Checkmark[i].sprite = _badgeCheckmark[1]; 
        }
        for (int i = 0; i < _gameController.NumberOrderedProducts; i++)
        {
            CheckSelectedProducts(numberProduct: i, selectedProductsId: collectedProductsId); //Check collect products on correct with products which order buyer
            yield return new WaitForSeconds(.5f);
        }

        yield return new WaitForSeconds(1f);

        _bubleCollectedOrder.SetActive(false);
        _audioManager.PlayClip(1, AudioManager.Clip.CloseBuble);
        Emotion?.Invoke(_emotionBuyer);
        CashFlow?.Invoke(_emotionBuyer, _numberRightProducts);
    }

    private void CheckSelectedProducts(int numberProduct, List<int> selectedProductsId)
    {
        for (int i = 0; i < _gameController.NumberOrderedProducts; i++)
        {
            if(_gameController.IdOrderedProducts[i] == selectedProductsId[numberProduct])
            {
                _numberRightProducts++;
                Checkmark[numberProduct].sprite = _badgeCheckmark[1];
                break;
            }
            else
            {
                Checkmark[numberProduct].sprite = _badgeCheckmark[0];
            }
        }
        for (int i = 0; i < Checkmark.Length; i++)
        {
            if(Checkmark[i].sprite == _badgeCheckmark[0])
            {
                _emotionBuyer = false;
                Debug.Log("Emotion will be negative :(");
                break;
            }
        }
        _collectedProducts[numberProduct].GetComponent<Toggle>().isOn = true;
    }

}
