using TMPro;
using Unity.Netcode;
using UnityEngine;

public class CountDownController : NetworkBehaviour
{
    [SerializeField] private NetworkVariable<float> startCountDown = new NetworkVariable<float>(25);
    [SerializeField] private TextMeshProUGUI countDownTxt;
    [SerializeField] private int maxMap = 1;
    bool startGame = false;
    public override void OnNetworkSpawn()
    {
        countDownTxt.text = "Trận đấu sẽ bắt đầu sau " + Mathf.Ceil(startCountDown.Value) + "s";
    }
    private void Update()
    {
        countDownTxt.text = "Trận đấu sẽ bắt đầu sau " + Mathf.Ceil(startCountDown.Value) + "s";
        if (IsServer)
        {
            startCountDown.Value = Mathf.Max(0f, startCountDown.Value - Time.deltaTime);

            if (startCountDown.Value == 0f && !startGame)
            {
                StartGame();
            }
        }
    }
    public void StartGame()
    {
        if (SceneController.instance != null)
        {
            int randomMap = Random.Range(0, maxMap);
            startGame = true;
            SceneController.instance.ChangeSceneSync(randomMap + 2, true);
        }
    }
}
