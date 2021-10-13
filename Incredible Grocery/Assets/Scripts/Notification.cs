using UnityEngine;
using UnityEngine.UI;

public class Notification : MonoBehaviour
{
    [SerializeField] private Text notificate;
    public void SetNotificate(int value)
    {
        notificate.text = value > 0 ? $"+${value}" : $"-${Mathf.Abs(value)}";
    }
    private void DestroyNotification()
    {
        Destroy(gameObject);
    }
}
