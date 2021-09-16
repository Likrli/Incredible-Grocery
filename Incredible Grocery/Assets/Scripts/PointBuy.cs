using UnityEngine;

public class PointBuy : MonoBehaviour
{
    private GameController p_gameControl;

    private void Start()
    {
        p_gameControl = FindObjectOfType<GameController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Buyer"))
        {
            p_gameControl.ReadyBuyer();
            Debug.Log("New Buyer");
        }
    }
}
