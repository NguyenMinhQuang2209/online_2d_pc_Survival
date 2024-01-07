using Cinemachine;
using System;
using Unity.Netcode;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    private Rigidbody2D rb;
    [Header("Config")]
    private Transform playerAvatar;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float runSpeed = 2f;
    [SerializeField] private float plusSpeed = 0.2f;

    float currentPlusSpeed = 0f;

    [Space(5)]
    [Header("Weapon config")]
    [SerializeField] private Transform weaponContainer;
    [SerializeField] private GameObject defaultWeapon;

    Vector2 movement;
    Animator animator;

    private PlayerHealth playerHealth;


    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            virtualCamera.Priority = 1;
            virtualCamera.enabled = true;

            rb = GetComponent<Rigidbody2D>();
            foreach (Transform child in transform)
            {
                if (child.gameObject.TryGetComponent<Animator>(out animator))
                {
                    playerAvatar = child.transform;
                    break;
                }
            }
            playerHealth = GetComponent<PlayerHealth>();

            SceneController.instance.ChangeSceneEvent += HandleChangeScene;
        }
        else
        {
            virtualCamera.Priority = 0;
            virtualCamera.enabled = false;
        }
    }

    private void HandleUpgradeEvent(object sender, DropItemName e)
    {
        if (IsOwner)
        {
            switch (e)
            {
                case DropItemName.Health:
                    playerHealth.AddPlusHealth();
                    break;
                case DropItemName.Mana:
                    playerHealth.AddPlusMana();
                    break;
                case DropItemName.Speed:
                    currentPlusSpeed = UpgradeController.instance.GetPlusSpeed() * plusSpeed;
                    break;
            }
        }
    }

    private void HandleChangeScene(object sender, EventArgs e)
    {
        if (IsOwner)
        {
            if (CameraBoundController.instance != null)
            {
                if (virtualCamera.TryGetComponent<CinemachineConfiner2D>(out var compound))
                {
                    compound.m_BoundingShape2D = CameraBoundController.instance.cameraCompound;
                }
            }

            if (PlayerUIController.instance != null)
            {
                playerHealth.InitSlider(PlayerUIController.instance.healthSlider, PlayerUIController.instance.manaSlider,
                   PlayerUIController.instance.healthTxt, PlayerUIController.instance.manaTxt);
            }

            if (UpgradeController.instance != null)
            {
                UpgradeController.instance.UpgrdateEvent += HandleUpgradeEvent;
            }
        }
    }

    private void Update()
    {
        if (!IsOwner) return;


        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        MouseRotation();
    }
    private void FixedUpdate()
    {
        if (!IsOwner) return;

        Movement();
    }
    private void Movement()
    {
        if (GameController.instance != null && !GameController.instance.CanMove())
        {
            return;
        }

        if (animator != null)
        {
            animator.SetFloat("Speed", movement.sqrMagnitude >= 0.1f ? 1f : 0f);
        }
        if (movement.sqrMagnitude >= 0.1f)
        {
            if (playerAvatar != null)
            {
                playerAvatar.rotation = Quaternion.Euler(new(0f, movement.x < 0 ? 180f : 0f, 0f));
            }
            float speed = moveSpeed;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                if (playerHealth.CanUseMana())
                {
                    speed = runSpeed + currentPlusSpeed;
                    playerHealth.UseMana();
                }
            }
            rb.MovePosition(rb.position + (speed + currentPlusSpeed) * Time.fixedDeltaTime * movement.normalized);
        }
    }
    public void EquipmentWeapon(Weapon weapon)
    {
        foreach (Transform child in weaponContainer)
        {
            if (child.TryGetComponent<NetworkObject>(out var networkObject))
            {
                networkObject.Despawn();
            }
            Destroy(child.gameObject);
        }

        if (weapon != null)
        {
            Destroy(defaultWeapon);
            Weapon weaponTemp = Instantiate(weapon, weaponContainer.transform);
            weaponTemp.transform.localPosition = Vector3.zero;
        }
    }
    private void MouseRotation()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mouseDir = mousePos - (Vector2)weaponContainer.position;
        float angle = Mathf.Atan2(mouseDir.y, mouseDir.x) * Mathf.Rad2Deg;
        weaponContainer.rotation = Quaternion.Euler(new(0f, 0f, angle));
    }

}
