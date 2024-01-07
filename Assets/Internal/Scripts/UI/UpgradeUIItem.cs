using TMPro;
using UnityEngine;

public class UpgradeUIItem : MonoBehaviour
{
    public TextMeshProUGUI amountTxt;
    public DropItemName dropItemName;
    public void UpgradeUItemInit(string txt)
    {
        amountTxt.text = txt;
    }
}
