using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : Health
{

    [SerializeField] private int plusHealthValue = 1;
    [SerializeField] private int plusManaValue = 1;
    [SerializeField] private float useManaRate = 1f;
    [SerializeField] private float maxMana = 100f;
    [SerializeField] private float waitRecoverMana = 1f;
    [SerializeField] private float waitUseMana = 0f;
    [SerializeField] private float recoverManaRate = 1f;
    float currentWaitRecoverMana = 0f;
    float currentWaitUseMana = 0f;
    float currentMana = 0f;
    float plusMana = 0f;

    private Slider healthSlider;
    private Slider manaSlider;
    private TextMeshProUGUI healthTxt;
    private TextMeshProUGUI manaTxt;
    private PlayerMovement playerMovement;

    private bool canUseMana = true;

    public override void OnNetworkSpawn()
    {
        HealthInit();
        currentMana = maxMana;
        playerMovement = GetComponent<PlayerMovement>();
    }
    private void Update()
    {
        if (playerMovement != null && playerMovement.IsOwner)
        {
            currentWaitRecoverMana += Time.deltaTime;
            if (currentWaitRecoverMana >= waitRecoverMana)
            {
                RecoverMana(Time.deltaTime * recoverManaRate);
            }

            if (!canUseMana)
            {
                currentWaitUseMana += Time.deltaTime;
                if (currentWaitUseMana >= waitUseMana)
                {
                    currentWaitUseMana = 0f;
                    canUseMana = true;
                }
            }
        }
    }
    public override void HealthInit()
    {
        base.HealthInit();
    }

    [ServerRpc(RequireOwnership = false)]
    public override void TakeDamageServerRpc(int damage)
    {
        base.TakeDamageServerRpc(damage);
        ReloadSlider();
        ReloadSliderClientRpc();
    }

    [ClientRpc]
    public void ReloadSliderClientRpc()
    {
        ReloadSlider();
    }

    [ServerRpc(RequireOwnership = false)]
    public override void DestroyObjectServerRpc()
    {
        //base.DestroyObject();
        gameObject.SetActive(false);
    }
    public void AddPlusHealth()
    {
        plusHealth.Value += plusHealthValue;
        ReloadSlider();
    }
    public void AddPlusMana()
    {
        plusMana += plusManaValue;
        ReloadSlider();
    }
    public float GetCurrentMana()
    {
        return currentMana;
    }
    public float GetMaxMana()
    {
        return maxMana + plusMana;
    }
    public void RecoverMana(float v)
    {
        currentMana = Mathf.Min(currentMana + v, GetMaxMana());
        ReloadMana();
    }
    public void InitSlider(Slider healthSlider, Slider manaSlider, TextMeshProUGUI healthTxt, TextMeshProUGUI manaTxt)
    {
        this.healthSlider = healthSlider;
        this.manaSlider = manaSlider;
        this.healthTxt = healthTxt;
        this.manaTxt = manaTxt;

        ReloadSlider();
    }
    private void ReloadSlider()
    {
        if (healthSlider != null)
        {
            healthSlider.maxValue = GetMaxHealth();
            healthSlider.minValue = 0f;
            healthSlider.value = GetCurrentHealth();
            if (healthTxt != null)
            {
                healthTxt.text = GetCurrentHealth() + "/" + GetMaxHealth();
            }
        }

        if (manaSlider != null)
        {
            manaSlider.maxValue = GetMaxMana();
            manaSlider.minValue = 0f;
            manaSlider.value = GetCurrentMana();
            if (manaTxt != null)
            {
                manaTxt.text = (int)GetCurrentMana() + "/" + (int)GetMaxMana();
            }
        }
    }
    private void ReloadMana()
    {
        if (manaSlider != null)
        {
            manaSlider.maxValue = GetMaxMana();
            manaSlider.minValue = 0f;
            manaSlider.value = GetCurrentMana();
            if (manaTxt != null)
            {
                manaTxt.text = (int)GetCurrentMana() + "/" + (int)GetMaxMana();
            }
        }
    }
    public void UseMana()
    {
        if (!canUseMana)
        {
            return;
        }
        currentMana = Mathf.Max(0f, currentMana - Time.deltaTime * useManaRate);
        currentWaitRecoverMana = 0f;
        if (currentMana == 0f)
        {
            canUseMana = false;
        }
        ReloadMana();
    }
    public bool CanUseMana()
    {
        return canUseMana;
    }

    [ServerRpc(RequireOwnership = false)]
    public override void RecoverHealthServerRpc(int v)
    {
        base.RecoverHealthServerRpc(v);
        ReloadSliderClientRpc();
        ReloadSlider();
    }

}
