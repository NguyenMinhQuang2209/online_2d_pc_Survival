using Cinemachine;
using System;
using Unity.Multiplayer.Samples.Utilities.ClientAuthority;
using Unity.Netcode;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    private Rigidbody2D rb;
    [Header("Config")]
    private Transform playerAvatar;
    private PlayerHealth playerHealth;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float runSpeed = 2f;

    [Space(5)]
    [Header("Weapon config")]
    [SerializeField] private Transform weaponContainer;

    Vector2 movement;
    Animator animator;


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
            if (TryGetComponent<PlayerHealth>(out playerHealth))
            {
                playerHealth.HealthInit();
            }

            SceneController.instance.ChangeSceneEvent += HandleChangeScene;
        }
        else
        {
            virtualCamera.Priority = 0;
            virtualCamera.enabled = false;
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
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                speed = runSpeed;
            }
            rb.MovePosition(rb.position + speed * Time.fixedDeltaTime * movement.normalized);
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
