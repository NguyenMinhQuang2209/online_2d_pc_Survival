using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class LobbyController : MonoBehaviour
{
    public static LobbyController instance;

    public static string KEY_PLAYER_NAME = "PlayerName";
    public static string KEY_PLAYER_READY = "PlayerReady";
    public static string KEY_START_LOBBY = "StartLobby";


    [Header("Username UI")]
    [SerializeField] private GameObject userNameContainer;
    [SerializeField] private TMP_InputField usernameInput;
    [SerializeField] private Button loginBtn;
    string username = "";

    [Header("Lobby List UI")]
    public GameObject lobbyListContainer;
    public TextMeshProUGUI playerNameTxt;
    public Transform lobbyListContent;
    public TMP_InputField lobbySearchInput;
    public Button lobbySearchBtn;
    public Button reloadBtn;
    public Button createLobbyBtn;
    public LobbyItem lobbyItem;

    [Space(5)]
    [Header("Create lobby UI")]
    public GameObject createLobbyUIContainer;
    public TMP_InputField lobbyNameInput;
    public TextMeshProUGUI minMemeberTxt;
    public TextMeshProUGUI maxMemberTxt;
    public Slider memberSlider;
    public TextMeshProUGUI memberTxt;
    public Button modeBtn;
    public TextMeshProUGUI modeTxt;
    public Button cancelCreateLobbyBtn;
    public Button xCreateLobbyBtn;
    public Button acceptCreateLobbyBtn;
    [Header("Create lobby config")]
    [SerializeField] private int minLobbyMember = 1;
    [SerializeField] private int maxLobbyMember = 16;

    [Space(5)]
    [Header("Inside lobby UI")]
    public GameObject insideLobbyUIContainer;
    public Button outLobbyBtn;
    public TextMeshProUGUI insideLobbyUINameTxt;
    public Transform insideLobbyContent;
    public TextMeshProUGUI insidePlayerNameTxt;
    public InsideLobbyItem insideLobbyItem;
    public Button readyBtn;
    public TextMeshProUGUI readyTxt;

    public event EventHandler<LobbyEventArgs> OnJoinedSystem;
    public event EventHandler<SingleLobbyeventArgs> OnJoinLobbyEvent;
    public event EventHandler OnLeftLobbyEvent;

    private Lobby joinedLobby = null;

    [Space(5)]
    [SerializeField] private float heartBeatLobbyTime = 15f;
    [SerializeField] private float reloadLobbyTime = 1.1f;
    float currentReloadLobbyTime = 0f;
    float currentHeartBeatLobbyTime = 0f;

    public class LobbyEventArgs : EventArgs
    {
        public List<Lobby> lobbies = new();
    }
    public class SingleLobbyeventArgs : EventArgs
    {
        public Lobby lobby = null;
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
    private void Start()
    {
        userNameContainer.SetActive(true);
        lobbyListContainer.SetActive(true);
        createLobbyUIContainer.SetActive(true);
        insideLobbyUIContainer.SetActive(true);

        loginBtn.onClick.AddListener(() =>
        {
            Login();
        });
        createLobbyBtn.onClick.AddListener(() =>
        {
            CreateLobby();
        });
        reloadBtn.onClick.AddListener(() =>
        {
            ReloadLobbyList();
        });
        lobbySearchBtn.onClick.AddListener(() =>
        {
            SearchLobby();
        });

        readyBtn.onClick.AddListener(() =>
        {
            OnInsideLobbyReady();
        });

        // Create new lobby Config

        xCreateLobbyBtn.onClick.AddListener(() =>
        {
            CancelCreateNewLobby();
        });
        cancelCreateLobbyBtn.onClick.AddListener(() =>
        {
            CancelCreateNewLobby();
        });
        acceptCreateLobbyBtn.onClick.AddListener(() =>
        {
            CreateNewLobby();
        });
        modeBtn.onClick.AddListener(() =>
        {
            ChangeCreateLobbyMode();
        });
        memberSlider.minValue = minLobbyMember;
        memberSlider.maxValue = maxLobbyMember;
        memberSlider.value = minLobbyMember;
        memberTxt.text = memberSlider.value.ToString();

        memberSlider.onValueChanged.AddListener((v) =>
        {
            memberTxt.text = v.ToString();
        });

        // end create new lobby
        outLobbyBtn.onClick.AddListener(() =>
        {
            OnOutLobby(GetPlayerId());
        });
        // Inside lobby UI


        lobbyListContainer.SetActive(false);
        createLobbyUIContainer.SetActive(false);
        insideLobbyUIContainer.SetActive(false);

        // event trigger
        OnJoinedSystem += HandleReloadLobbyList;
        OnJoinLobbyEvent += HandleJoinLobbyEvent;
        OnLeftLobbyEvent += HandleLeftLobbyEvent;
    }
    private void Update()
    {
        if (joinedLobby != null)
        {
            if (IsLobbyOwner())
            {
                currentHeartBeatLobbyTime += Time.deltaTime;
                if (currentHeartBeatLobbyTime >= heartBeatLobbyTime)
                {
                    currentHeartBeatLobbyTime = 0f;
                    HeartBeatLobby();
                }
            }

            currentReloadLobbyTime += Time.deltaTime;
            if (currentReloadLobbyTime >= reloadLobbyTime)
            {
                currentReloadLobbyTime = 0f;
                ReloadLobby();
            }
        }
    }

    private void HandleLeftLobbyEvent(object sender, EventArgs e)
    {
        joinedLobby = null;
        GetLobbyList("");
        lobbyListContainer.SetActive(true);
        insideLobbyUIContainer.SetActive(false);
        createLobbyUIContainer.SetActive(false);
    }

    private void HandleJoinLobbyEvent(object sender, SingleLobbyeventArgs e)
    {
        joinedLobby = e.lobby;

        insidePlayerNameTxt.text = username;
        insideLobbyUINameTxt.text = joinedLobby.Name;

        insideLobbyUIContainer.SetActive(true);

        foreach (Transform item in insideLobbyContent)
        {
            Destroy(item.gameObject);
        }

        foreach (Player player in joinedLobby.Players)
        {
            SpawnInsideLobbyItem(player);
        }
        if (IsLobbyOwner())
        {
            readyTxt.text = "Bắt đầu";
        }
        lobbyListContainer.SetActive(false);
        createLobbyUIContainer.SetActive(false);
    }
    private void SpawnInsideLobbyItem(Player player)
    {
        InsideLobbyItem tempInsideLobbyItem = Instantiate(insideLobbyItem, insideLobbyContent);

        bool isReady = player.Data[KEY_PLAYER_READY].Value == "1";
        if (player.Id == GetPlayerId() && !IsLobbyOwner())
        {
            readyTxt.text = isReady ? "Không sẵn sàng" : "Sẵn sàng";
        }
        tempInsideLobbyItem.InsideLobbyItemInit(player.Id, player.Data[KEY_PLAYER_NAME].Value, GetPlayerId() != player.Id && GetPlayerId() == joinedLobby.HostId, isReady);
    }
    public string GetPlayerId()
    {
        return AuthenticationService.Instance.PlayerId;
    }
    public bool IsLobbyOwner()
    {
        if (joinedLobby != null)
        {
            return joinedLobby.HostId == GetPlayerId();
        }
        return false;
    }

    // Handle

    private void HandleReloadLobbyList(object sender, LobbyEventArgs e)
    {
        List<Lobby> lobbyList = e.lobbies;
        foreach (Transform item in lobbyListContent)
        {
            Destroy(item.gameObject);
        }
        for (int i = 0; i < lobbyList.Count; i++)
        {
            Lobby tempLobby = lobbyList[i];
            LobbyItem tempItem = Instantiate(lobbyItem, lobbyListContent.transform);
            Player tempPlayer = null;
            foreach (Player player in tempLobby.Players)
            {
                if (player.Id == GetPlayerId())
                {
                    tempPlayer = player;
                    break;
                }
            }
            string amount = tempLobby.Players.Count + "/" + tempLobby.MaxPlayers;
            tempItem.LobbyInit(tempLobby.Id, tempLobby.Name, tempPlayer?.Profile.ToString(), amount);
        }
    }

    // End handle Action
    private async void Login()
    {
        try
        {
            string usernameTxt = usernameInput.text;
            if (usernameTxt != "")
            {
                username = usernameTxt;
                InitializationOptions options = new();
                options.SetProfile(username);
                await UnityServices.InitializeAsync(options);
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
                userNameContainer.SetActive(false);

                playerNameTxt.text = username;
                GetLobbyList("");
                lobbyListContainer.SetActive(true);
            }
        }
        catch (Exception e)
        {
            LogController.instance.Log(e.Message);
        }
    }
    private void ChangeCreateLobbyMode()
    {
        string currentModeTxt = modeTxt.text;

        string newModeTxt = currentModeTxt == "Private" ? "Public" : "Private";

        modeTxt.text = newModeTxt;
    }
    private async void CreateNewLobby()
    {
        try
        {
            string c_lobbyName = lobbyNameInput.text;
            bool c_isPrivate = modeTxt.text == "Private";
            int members = (int)memberSlider.value;
            if (c_lobbyName != "")
            {
                CreateLobbyOptions options = new()
                {
                    IsPrivate = c_isPrivate,
                };
                Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(c_lobbyName, members, new()
                {
                    Player = GetPlayer()
                });
                joinedLobby = lobby;
                OnJoinLobbyEvent?.Invoke(this, new()
                {
                    lobby = joinedLobby
                });
            }
            else
            {
                LogController.instance.Log("Lobby name can not be empty!");
            }

        }
        catch (Exception e)
        {
            LogController.instance.Log(e.Message);
        }
    }
    public async void JoinLobbyById(string lobbyId)
    {
        try
        {
            Lobby lobby = await LobbyService.Instance.JoinLobbyByIdAsync(lobbyId, new()
            {
                Player = GetPlayer()
            });
            OnJoinLobbyEvent?.Invoke(this, new()
            {
                lobby = lobby
            });
        }
        catch (Exception e)
        {
            LogController.instance.Log(e.Message);
        }
    }
    private void SearchLobby()
    {
        string lobbySearch = lobbySearchInput.text;
        GetLobbyList(lobbySearch);
    }
    private void ReloadLobbyList()
    {
        GetLobbyList("");
    }
    public void CreateLobby()
    {
        createLobbyUIContainer.SetActive(true);
    }
    public async void GetLobbyList(string searchTxt)
    {
        try
        {
            QueryLobbiesOptions options = new()
            {
                Filters = new()
            {
                new QueryFilter(
                    field:QueryFilter.FieldOptions.Name,
                    op:QueryFilter.OpOptions.CONTAINS,
                    value:searchTxt)
            }
            };
            QueryResponse queryReponse = await LobbyService.Instance.QueryLobbiesAsync(options);
            List<Lobby> list = new();
            foreach (Lobby lobby in queryReponse.Results)
            {
                list.Add(lobby);
            }
            OnJoinedSystem?.Invoke(this, new()
            {
                lobbies = list
            });
        }
        catch (Exception e)
        {
            LogController.instance.Log(e.Message);
        }
    }

    // create new lobby
    private void CancelCreateNewLobby()
    {
        createLobbyUIContainer.SetActive(false);
    }

    private async void HeartBeatLobby()
    {
        try
        {
            if (joinedLobby != null)
            {
                await LobbyService.Instance.SendHeartbeatPingAsync(joinedLobby.Id);
            }
        }
        catch (Exception e)
        {
            LogController.instance.Log(e.Message);
        }
    }
    private async void ReloadLobby()
    {
        try
        {
            if (joinedLobby != null)
            {
                Lobby lobby = await LobbyService.Instance.GetLobbyAsync(joinedLobby.Id);
                OnJoinLobbyEvent?.Invoke(this, new()
                {
                    lobby = lobby
                });
            }

            if (!IsPlayerInLobby())
            {
                OnLeftLobbyEvent?.Invoke(this, null);
            }

        }
        catch (Exception e)
        {
            LogController.instance.Log(e.Message);
        }
    }
    private bool IsPlayerInLobby()
    {
        if (joinedLobby != null)
        {
            foreach (var item in joinedLobby.Players)
            {
                if (item.Id == GetPlayerId())
                {
                    return true;
                }
            }
        }
        return false;
    }
    public async void OnOutLobby(string playerId)
    {
        try
        {
            if (joinedLobby != null)
            {
                if (joinedLobby.HostId == playerId)
                {
                    await LobbyService.Instance.DeleteLobbyAsync(joinedLobby.Id);
                }
                else
                {
                    await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, playerId);
                }
                OnLeftLobbyEvent?.Invoke(this, null);
            }
        }
        catch (Exception e)
        {
            LogController.instance.Log(e.Message);
        }
    }
    private async void OnInsideLobbyReady()
    {
        try
        {
            if (joinedLobby != null)
            {
                if (!IsLobbyOwner())
                {
                    Player temp = null;
                    foreach (Player player in joinedLobby.Players)
                    {
                        if (player.Id == GetPlayerId())
                        {
                            temp = player;
                            break;
                        }
                    }

                    string nextReadyTxt = temp.Data[KEY_PLAYER_READY].Value == "0" ? "1" : "0";
                    Lobby lobby = await LobbyService.Instance.UpdatePlayerAsync(joinedLobby.Id, GetPlayerId(), new UpdatePlayerOptions()
                    {
                        Data = new()
                    {
                        {KEY_PLAYER_READY,new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member,nextReadyTxt) }
                    }
                    });
                    OnJoinLobbyEvent?.Invoke(this, new()
                    {
                        lobby = lobby
                    });
                }
                else
                {
                    bool ready = true;
                    foreach (Player player in joinedLobby.Players)
                    {
                        if (player.Id != GetPlayerId())
                        {
                            if (player.Data[KEY_PLAYER_READY].Value != "1")
                            {
                                ready = false;
                                break;
                            }
                        }
                    }

                    if (!ready)
                    {
                        LogController.instance.Log("Member not ready all!");
                    }
                }
            }
        }
        catch (Exception e)
        {
            LogController.instance.Log(e.Message);
        }
    }

    private Player GetPlayer()
    {
        return new Player(GetPlayerId(), null, new Dictionary<string, PlayerDataObject>()
        {
            {KEY_PLAYER_NAME ,new PlayerDataObject(PlayerDataObject.VisibilityOptions.Public,username)},
            {KEY_PLAYER_READY,new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member,"0") }
        });
    }
}
