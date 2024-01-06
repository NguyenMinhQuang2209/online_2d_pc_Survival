using TMPro;
using Unity.Netcode;
using UnityEngine;

public class ShowTxtUI : NetworkBehaviour
{
    public TextMeshProUGUI showTxt;
    public float destroyTime = 5f;
    public float floatSpeed = 2f;
    bool startShow = false;
    private void Update()
    {
        if (startShow)
        {
            transform.position = transform.position + floatSpeed * Time.deltaTime * Vector3.up;
        }
    }
    public void ShowTxt(string txt, Color color)
    {
        showTxt.text = txt;
        showTxt.color = color;
        startShow = true;
        Destroy(gameObject, destroyTime);
    }
}
