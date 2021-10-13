using UnityEngine;
using UnityEngine.UI;
public class NewButton : MonoBehaviour
{
    private Button _button;
    private void Awake()
    {
        _button = GetComponent<Button>();
    }
    private void Start()
    {
        SaveData.instance.audioManager.AddListenerButtons(_button);
    }
}
