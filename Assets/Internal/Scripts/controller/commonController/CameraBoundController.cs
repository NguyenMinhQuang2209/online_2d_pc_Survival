using UnityEngine;

public class CameraBoundController : MonoBehaviour
{
    public static CameraBoundController instance;
    public Collider2D cameraCompound;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
}
