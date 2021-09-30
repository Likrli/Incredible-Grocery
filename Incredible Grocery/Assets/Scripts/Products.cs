using UnityEngine;
[CreateAssetMenu(fileName = "Product", menuName = "ScriptableObjects/Products")]
public class Products : ScriptableObject
{
    [SerializeField] private int productId;
    //Amount of products etc..    [SerializeField] private int _amountProduct;
    [SerializeField] private string productName;
    [SerializeField] private Sprite productSprite;
    public int ProductId => productId;
    public string ProductName => productName;
    public Sprite ProductSprite => productSprite;
}
