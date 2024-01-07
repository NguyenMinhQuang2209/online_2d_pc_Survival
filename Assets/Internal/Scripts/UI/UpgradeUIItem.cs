using TMPro;
using UnityEngine;

public class UpgradeUIItem : MonoBehaviour
{
    public TextMeshProUGUI amountTxt;
    public void UpgradeUItemInit(string txt)
    {
        amountTxt.text = txt;
    }
}
