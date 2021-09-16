using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
public class Storage : MonoBehaviour
{
    private int p_needProduct;
    private int[] p_idNeedProducts;

    public Toggle[] Products;
    public Image[] ImgToggles;

    public GameObject BtnSell;

    public GameObject CollectedOrder;
    public GameObject[] OrderProducts; //Mus for show ordered products 1-3(incl)
    public Image[] ImgProducts;
    public Image[] Checkmark;
    public Sprite[] SpriteProducts;

    public Sprite[] GetOrder;

    private AudioManager p_audioManager;
    private Animator p_animControll;

    public delegate void TakeEmotion(int T);
    public delegate void TakeCash(int T, int C);
    public event TakeEmotion onEmotion;
    public event TakeCash onCash;


    private int p_colrightProduct;

    private int p_currentProduct;
    private bool p_emotion;
    private string p_giveOrder = "GiveOrder";

    private void Start()
    {
        p_animControll = GetComponent<Animator>();
        p_audioManager = FindObjectOfType<AudioManager>();
    }

    public void Show(int amountProductForBuy, int[] indexProduct)
    {
        foreach (var product in Products) //Remove products choices
        {
            product.isOn = false;
        }
        p_animControll.SetTrigger("Show");
        p_needProduct = amountProductForBuy; //Save amount products which need
        p_idNeedProducts = indexProduct; //Save link on Mus index products which need
    }

    public void ChooseProduct(int idChooseProduct) //Player choose product of Storage and set id in this method (OnalueChanged)
    {
        p_audioManager.PlayClip(2,4); //Play clip choose product
        if (Products[idChooseProduct].isOn)
        {
            ImgToggles[idChooseProduct].color = new Color(1, 1, 1, .3f); //Change alfa Image on 30% if product choice
            p_currentProduct++; //Inc var current products 
        }
        else
        {
            ImgToggles[idChooseProduct].color = new Color(1, 1, 1, 1f);
            p_currentProduct--;
        }
        SelectedProducts(); //Call method for check player can be able take btn Sell
    }

    private void SelectedProducts()
    {
        if (p_currentProduct != p_needProduct) 
        {
            for (int i = 0; i < Products.Length; i++)
            {
                BtnSell.GetComponent<Button>().interactable = false;
                BtnSell.GetComponent<Image>().color = new Color(1, 1, 1, .5f);
            }
        }
        else
        {
            for (int i = 0; i < Products.Length; i++)
            {
                BtnSell.GetComponent<Button>().interactable = true;
                BtnSell.GetComponent<Image>().color = new Color(1, 1, 1, 1);
            }
        }
    }

    public void Hide() //This funct call when player take btn Sell 
    {
        for (int i = 0; i < OrderProducts.Length; i++)
        {
            OrderProducts[i].GetComponent<Toggle>().isOn = false; //Remove products Checkmark (correct product true|false) 
            OrderProducts[i].SetActive(false); //Disable all obj if it`s not first buyer
        }
        p_animControll.SetTrigger("Hide");
        StartCoroutine(p_giveOrder);
    }
    private IEnumerator GiveOrder()
    {
        yield return new WaitForSeconds(1f);
        List<int> listPurchase = new List<int>(); //Create List of products for purchase
            for (int i = 0; i < Products.Length; i++)
            {
                if (Products[i].isOn)
                {
                    listPurchase.Add(i); //Save index(id) products which isOn into List 
                }
            }
                foreach (var idProduct in listPurchase)
                {
                    Debug.Log("List Purchase: " + idProduct);
                }

        CollectedOrder.SetActive(true);
        p_audioManager.PlayClip(1, 0);

            for (int i = 0; i < p_needProduct; i++)//Enable purchase window with collected products and set it`s images
            {
                OrderProducts[i].SetActive(true);
                ImgProducts[i].sprite = SpriteProducts[listPurchase[i]];
            }

        yield return new WaitForSeconds(1f); //Start check order until 1 sec
        p_emotion = true; // Var for controll all order (true|false)
        p_colrightProduct = 0; // Var for save info how much products purchase right
            for (int i = 0; i < Checkmark.Length; i++) // Refresh Checkmark of collected products for purchase (if it`s not first buyer)
            {
                Checkmark[i].sprite = GetOrder[1]; 
            }
                for (int i = 0; i < p_needProduct; i++)
                {
                    CheckOrder(i, listPurchase); //Check collect products on correct with products which order buyer
                    yield return new WaitForSeconds(.5f);
                }

        yield return new WaitForSeconds(1f);
        CollectedOrder.SetActive(false);
        p_audioManager.PlayClip(1, 1);
            if (p_emotion)
            {
                onEmotion(1);
                onCash(1, p_colrightProduct);
            }
                else
                {
                    onEmotion(0);
                    onCash(0, p_colrightProduct);
                }
    }

    private void CheckOrder(int index, List<int> newList)
    {
        for (int i = 0; i < p_idNeedProducts.Length; i++)
        {

            if(p_idNeedProducts[i] == newList[index])
            {
                p_colrightProduct++;
                Checkmark[index].sprite = GetOrder[1];
                break;
            }
            else
            {
                Checkmark[index].sprite = GetOrder[0];
            }
        }
        for (int i = 0; i < Checkmark.Length; i++)
        {
            if(Checkmark[i].sprite == GetOrder[0])
            {
                p_emotion = false;
                Debug.Log("Emotion will be negative");
                break;
            }
        }
        OrderProducts[index].GetComponent<Toggle>().isOn = true;
    }

}
