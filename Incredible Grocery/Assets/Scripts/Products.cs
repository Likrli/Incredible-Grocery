using UnityEngine;
[CreateAssetMenu(fileName = "Product", menuName = "ScriptableObjects/Products")]
public class Products : ScriptableObject
{
    [SerializeField] private int _idProduct;
    //Amount of products etc..    [SerializeField] private int _amountProduct;
    [SerializeField] private string _nameProduct;
    [SerializeField] private Sprite _spriteProduct;
    public int IdProduct { get { return _idProduct; } private set { } }
    public string NameProduct { get { return _nameProduct; } private set { } }
    public Sprite SpriteProduct { get { return _spriteProduct; } private set { } }
}
