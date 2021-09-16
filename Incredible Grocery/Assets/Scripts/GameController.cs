using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public int AmountProducts;
    public int[] idProducts;

    private string p_delaySpawn = "DelaySpawnBuyer";
    private string p_delayPur = "DelayPurchase";


    public Text AmountCash;
    public Sprite[] SpriteProducts;
    public GameObject[] NeedProducts;
    public GameObject BuyerOrder;
    [SerializeField] private GameObject p_buyer;

    private Storage p_storage;
    private SaveData p_saveData;
    private AudioManager p_audioManager;

    private void Start()
    {
        p_saveData = GetComponent<SaveData>();
        p_saveData.onSetCash += TextCash;
        p_audioManager = FindObjectOfType<AudioManager>();
        p_storage = FindObjectOfType<Storage>();
        p_storage.onCash += GetCash;
        SpawnBuyer();
        TextCash(p_saveData.CASH);
    }

    private void TextCash(int cash)
    {
        AmountCash.text = $"$ {cash}";
    }
    public void SpawnBuyer()
    {
        StartCoroutine(p_delaySpawn);
    }
    private IEnumerator DelaySpawnBuyer()
    {
        yield return new WaitForSeconds(1f); //Spawn buyer until 1 sec
        Instantiate(p_buyer);
    }

    public void ReadyBuyer()
    {
        StartCoroutine(p_delayPur);
    }
    private IEnumerator DelayPurchase()
    {
        Debug.Log("Delay.. 1.5 sec");
        yield return new WaitForSeconds(1.5f);
        OrderProducts(); 
        Debug.Log("Delay.. 5 sec");
        yield return new WaitForSeconds(5f);
        BuyerOrder.SetActive(false);
        p_audioManager.PlayClip(1, 1);
        p_storage.Show(AmountProducts, idProducts);
    }
    private void OrderProducts()
    {
        p_audioManager.PlayClip(1, 0); //Play clip spawn cloud
        BuyerOrder.SetActive(true); //Activate BuyerPanel for order
        for (int i = 0; i < NeedProducts.Length; i++) // Off all need products (if it`s not first buyer)
        {
            NeedProducts[i].SetActive(false); 
        }
        AmountProducts = Random.Range(1, 4); //generate new amount products for new buyer
        idProducts = new int[AmountProducts]; //generate new Mus id`s products size of  AmountProducts
        for (int i = 0; i < idProducts.Length; i++) //Check on the different products
        {
            idProducts[i] = Random.Range(1, SpriteProducts.Length + 1) -1;
            for (int j = 0; j < i; j++)
            {
                if(idProducts[i] == idProducts[j])
                {
                    i--;
                    break;
                }
            }
        }
            for (int i = 0; i < idProducts.Length; i++) 
            {
                Debug.Log("idProducts["+ i +"]: " + idProducts[i]);
            }
        for (int i = 0; i < AmountProducts; i++)
        {
            NeedProducts[i].SetActive(true); //Activate needs amount products
            NeedProducts[i].GetComponent<Image>().sprite = SpriteProducts[idProducts[i]]; // Set sprite for products after its Id
        }
    }
    private void GetCash(int varEmotion, int colProduct)//Emotion 0 - angry(x1 cash) || 1 - good (x2 cash) for all products
    {
        if (colProduct > 0)
        {
            p_audioManager.PlayClip(1, 3);
            switch (varEmotion)
            {
                case 0:
                    p_saveData.SaveCash(colProduct * 10); // Pay for correct products
                    break;
                case 1:
                    p_saveData.SaveCash(colProduct * 20); // Pay x2 cash
                    break;
            }
        }
    }

}
