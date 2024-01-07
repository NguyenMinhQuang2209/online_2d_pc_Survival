using Unity.Netcode;
using UnityEngine;

public class ShowTxtController : NetworkBehaviour
{
    public static ShowTxtController instance;
    [SerializeField] private ShowTxtUI showTxtUI;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    [ServerRpc]
    public void ShowUIServerRpc(Vector3 pos, string txt, Color color)
    {
        ShowTxtUI tempShowTxtUI = Instantiate(showTxtUI, pos + Vector3.up * 0.5f, Quaternion.identity);
        tempShowTxtUI.ShowTxtServerRpc(txt, color);
        if (tempShowTxtUI.TryGetComponent<NetworkObject>(out var networkItem))
        {
            networkItem.Spawn();
        }
    }
}
