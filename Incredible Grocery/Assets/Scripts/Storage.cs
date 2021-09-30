using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
public class Storage : MonoBehaviour
{
    [SerializeField] private int rightProductsAmount;
    [SerializeField] private Toggle[] products;
    [SerializeField] private Image[] toggleImgs;
    [SerializeField] private Image[] collectedProductsImgs;
    [SerializeField] private Image[] checkmarkImgs;
    [SerializeField] private Sprite cancelledBadge;
    [SerializeField] private Sprite selectedBadge;
    [SerializeField] private Button sell;
    [SerializeField] private Image sellImg;
    [SerializeField] private GameObject collectedOrderBubble;
    [SerializeField] private GameObject[] collectedProducts;
    [SerializeField] private Animator storageAnimator;
    private const string Show = "Show";
    private const string Hide = "Hide";
    private int _selectedProductsNumber;
    private bool _wasHappyBuyer;
    private GameController _gameController;
    private AudioManager _audioManager;

    public delegate void OnReceivedEmotion(bool emotion);
    public event OnReceivedEmotion ReceivedEmotion;

    public delegate void OnReceivedCash(bool emotion, int numberRightProducts);
    public event OnReceivedCash ReceivedCash;

    private void Start()
    {
        _gameController = GetComponent<GameController>();
        _audioManager = GetComponent<AudioManager>();
    }

    public void ShowStorage()
    {
        foreach (var product in products)
        {
            product.isOn = false;
        }
        storageAnimator.SetTrigger(Show);
    }

    public void ChooseProduct(int idChooseProduct)
    {
        _audioManager.PlayClip(AudioManager.Clip.SelectProduct); 
        if (products[idChooseProduct].isOn)
        {
            toggleImgs[idChooseProduct].color = ChangeAlphaColor(color: toggleImgs[idChooseProduct].color, alpha: .3f);
            _selectedProductsNumber++;
        }
        else
        {
            toggleImgs[idChooseProduct].color = Color.white;
            _selectedProductsNumber--;
        }
        ActivateBtnSell();
    }

    private void ActivateBtnSell()
    {
        if (_selectedProductsNumber != _gameController.OrderedProductsNymber) 
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
        StartCoroutine(ShowCollectedProducts());
    }
    private IEnumerator ShowCollectedProducts()
    {
        yield return new WaitForSeconds(1f);
        List<int> collectedProductsId = new List<int>();
        for (int i = 0; i < products.Length; i++)
        {
            if (products[i].isOn)
            {
                collectedProductsId.Add(i);
            }
        }
        collectedOrderBubble.SetActive(true);
        _audioManager.PlayClip(AudioManager.Clip.SpawnBuble);
        for (int i = 0; i < _gameController.OrderedProductsNymber; i++)
        {
            collectedProducts[i].SetActive(true);
            collectedProductsImgs[i].sprite = _gameController.AllProducts[collectedProductsId[i]].ProductSprite;
        }
        yield return new WaitForSeconds(1f);

        _wasHappyBuyer = true;
        rightProductsAmount = 0;

        for (int i = 0; i < checkmarkImgs.Length; i++)
        {
            checkmarkImgs[i].sprite = selectedBadge; 
        }
        for (int i = 0; i < _gameController.OrderedProductsNymber; i++)
        {
            CheckSelectedProducts(numberProduct: i, selectedProductsId: collectedProductsId);
            yield return new WaitForSeconds(.5f);
        }

        yield return new WaitForSeconds(1f);

        collectedOrderBubble.SetActive(false);
        _audioManager.PlayClip(AudioManager.Clip.CloseBuble);
        ReceivedEmotion?.Invoke(_wasHappyBuyer);
        ReceivedCash?.Invoke(_wasHappyBuyer, rightProductsAmount);
    }

    private void CheckSelectedProducts(int numberProduct, List<int> selectedProductsId)
    {
        for (int i = 0; i < _gameController.OrderedProductsNymber; i++)
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
