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

    [ServerRpc(RequireOwnership = false)]
    public void ShowUIServerRpc(float[] pos, string txt)
    {
        ShowTxtUI tempShowTxtUI = Instantiate(showTxtUI, new Vector3(pos[0], pos[1], pos[2]) + Vector3.up * 0.5f, Quaternion.identity);
        if (tempShowTxtUI.TryGetComponent<NetworkObject>(out var networkItem))
        {
            networkItem.Spawn();
        }
        tempShowTxtUI.ShowTxtServerRpc(new(txt));
    }
}
