using UnityEngine;
[CreateAssetMenu(fileName = "Product", menuName = "ScriptableObjects/Products")]
public class Products : ScriptableObject
{
    [SerializeField] private int productId;
    [SerializeField] private int productAmount;
    [SerializeField] private string productName;
    [SerializeField] private Sprite productSprite;

    public int ProductId => productId;
    public string ProductName => productName;
    public Sprite ProductSprite => productSprite;
    public int ProductAmount { get { return productAmount; } set { productAmount = value; } }
}
