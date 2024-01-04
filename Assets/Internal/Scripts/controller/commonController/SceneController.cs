using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController instance;
    public enum SceneName
    {
        Lobby,
        SelectScene
    }
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    public void ChangeScene(SceneName name, bool isSingle)
    {
        if (isSingle)
        {
            SceneManager.LoadScene(name.ToString(), LoadSceneMode.Single);
        }
        else
        {
            SceneManager.LoadScene(name.ToString(), LoadSceneMode.Additive);
        }
    }
    public void ChangeSceneSync(SceneName name, bool isSingle)
    {
        if (isSingle)
        {
            NetworkManager.Singleton.SceneManager.LoadScene(name.ToString(), LoadSceneMode.Single);
        }
        else
        {
            NetworkManager.Singleton.SceneManager.LoadScene(name.ToString(), LoadSceneMode.Additive);
        }
    }

}
