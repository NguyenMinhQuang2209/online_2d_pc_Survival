using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class ShowTxtUI : NetworkBehaviour
{
    private NetworkVariable<FixedString32Bytes> txt = new();
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
    [ServerRpc]
    public void ShowTxtServerRpc(string txt, Color color)
    {
        this.txt.Value = new FixedString32Bytes(txt);
        showTxt.text = this.txt.Value.ToString();
        showTxt.color = color;
        startShow = true;
        Destroy(gameObject, destroyTime);
    }
    /*[ClientRpc]
    public void ShowTxtClientRpc(string txt, Color color)
    {
        this.txt.Value = new FixedString32Bytes(txt);
        showTxt.text = this.txt.Value.ToString();
        showTxt.color = color;
        startShow = true;
    }*/
}
