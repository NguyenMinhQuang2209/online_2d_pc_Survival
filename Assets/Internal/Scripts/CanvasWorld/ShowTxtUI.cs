using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class ShowTxtUI : NetworkBehaviour
{
    private NetworkVariable<FixedString32Bytes> txt = new("");
    public TextMeshProUGUI showTxt;
    public float destroyTime = 5f;
    public float floatSpeed = 2f;
    bool startShow = false;
    public override void OnNetworkSpawn()
    {
        showTxt.color = Color.red;
    }

    private void Update()
    {
        showTxt.text = txt.Value.ToString();
        if (startShow && IsServer)
        {
            transform.position = transform.position + floatSpeed * Time.deltaTime * Vector3.up;
        }
    }

    [ServerRpc]
    public void ShowTxtServerRpc(FixedString32Bytes txt)
    {
        this.txt.Value = txt;
        showTxt.text = this.txt.Value.ToString();
        startShow = true;
        Destroy(gameObject, destroyTime);
    }

}
