using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController instance;
    public enum SceneName
    {
        Lobby,
        SelectScene,
        Play
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
        switch (name)
        {
            case SceneName.Lobby:
                GameController.instance.ChangeCurrentMode(GameController.GameMode.Lobby);
                break;
            case SceneName.SelectScene:
                GameController.instance.ChangeCurrentMode(GameController.GameMode.SelectWeapon);
                break;
            case SceneName.Play:
                GameController.instance.ChangeCurrentMode(GameController.GameMode.Play);
                break;
        }

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
        switch (name)
        {
            case SceneName.Lobby:
                GameController.instance.ChangeCurrentMode(GameController.GameMode.Lobby);
                break;
            case SceneName.SelectScene:
                GameController.instance.ChangeCurrentMode(GameController.GameMode.SelectWeapon);
                break;
            case SceneName.Play:
                GameController.instance.ChangeCurrentMode(GameController.GameMode.Play);
                break;
        }

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
