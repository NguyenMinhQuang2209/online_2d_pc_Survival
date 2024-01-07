using System;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class DayNightController : NetworkBehaviour
{
    [SerializeField] private NetworkVariable<FixedString512Bytes> daynightTxtVar = new NetworkVariable<FixedString512Bytes>("");
    [SerializeField] private TextMeshProUGUI dayNightTxt;
    [SerializeField] private float startAtHour = 0f;
    [SerializeField] private float timeRate = 600f;
    DateTime currentTime;
    DateTime startTime;

    public override void OnNetworkSpawn()
    {
        currentTime = DateTime.Now.Date + TimeSpan.FromHours(startAtHour);
        startTime = DateTime.Now.Date;
    }
    private void Update()
    {
        dayNightTxt.text = daynightTxtVar.Value.ToString();
        if (IsServer)
        {
            currentTime = currentTime.AddSeconds(Time.deltaTime * timeRate);
            daynightTxtVar.Value = "Ngày " + ((currentTime - startTime).Days + 1) + "\n" + currentTime.ToString("HH:mm");
        }
    }
}
