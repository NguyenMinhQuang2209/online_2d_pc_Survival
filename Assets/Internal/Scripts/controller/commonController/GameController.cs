using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    public enum GameMode
    {
        Lobby,
        SelectWeapon,
        Play,
        Die
    }
    private GameMode currentMode;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        currentMode = GameMode.SelectWeapon;
    }
    public bool CanMove()
    {
        return currentMode == GameMode.SelectWeapon || currentMode == GameMode.Play;
    }
    public bool CanDie()
    {
        return currentMode == GameMode.Play;
    }
    public bool CanShoot()
    {
        return currentMode == GameMode.Play;
    }
    public void ChangeCurrentMode(GameMode newMode)
    {
        currentMode = newMode;
    }
}
