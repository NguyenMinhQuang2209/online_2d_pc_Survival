using UnityEngine;

public class ShowTxtController : MonoBehaviour
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
    public void ShowUI(Vector3 pos, string txt, Color color)
    {
        ShowTxtUI tempShowTxtUI = Instantiate(showTxtUI, pos + Vector3.up * 0.5f, Quaternion.identity);
        tempShowTxtUI.ShowTxt(txt, color);
    }
    public void ShowUI(Vector3 pos, string txt)
    {
        ShowTxtUI tempShowTxtUI = Instantiate(showTxtUI, pos + Vector3.up * 0.5f, Quaternion.identity);
        tempShowTxtUI.ShowTxt(txt, Color.red);
    }
}
