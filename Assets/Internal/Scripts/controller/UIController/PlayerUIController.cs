using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIController : MonoBehaviour
{
    public static PlayerUIController instance;

    public Slider healthSlider;
    public Slider manaSlider;
    public TextMeshProUGUI healthTxt;
    public TextMeshProUGUI manaTxt;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
}
