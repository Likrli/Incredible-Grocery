using UnityEngine;
[CreateAssetMenu(fileName = "Product", menuName = "ScriptableObjects/Products")]
public class Products : ScriptableObject
{
    [SerializeField] private int productId;
    [SerializeField] private string productName;
    [SerializeField] private Sprite productSprite;
    public int ProductId => productId;
    public string ProductName => productName;
    public Sprite ProductSprite => productSprite;
}
