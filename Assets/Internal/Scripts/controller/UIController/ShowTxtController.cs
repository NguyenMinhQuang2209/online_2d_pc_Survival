using Unity.Netcode;
using UnityEngine;

public class ShowTxtController : NetworkBehaviour
{
    public static ShowTxtController instance;
    [SerializeField] private ShowTxtUI showTxtUI;
    public override void OnNetworkSpawn()
    {
        enabled = IsServer;
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    public void ShowUI(Vector3 pos, string txt)
    {
        ShowTxtUI tempShowTxtUI = Instantiate(showTxtUI, pos + Vector3.up * 0.5f, Quaternion.identity);
        if (tempShowTxtUI.TryGetComponent<NetworkObject>(out var networkItem))
        {
            networkItem.Spawn();
        }
        tempShowTxtUI.ShowTxtServerRpc(new(txt));
    }
}
