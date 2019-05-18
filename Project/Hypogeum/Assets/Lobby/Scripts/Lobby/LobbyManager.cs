/*
Copyright (c) 2018
Unity Technologies ApS (“Unity”, “our” or “we”) provides game-development and related software (the “Software”), development-related services (like Unity Teams (“Developer Services”)), and various Unity communities (like Unity Answers and Unity Connect (“Communities”)), provided through or in connection with our website, accessible at unity3d.com or unity.com (collectively, the “Site”). Except to the extent you and Unity have executed a separate agreement, these terms and conditions exclusively govern your access to and use of the Software, Developer Services, Communities and Site (collectively, the “Services”), and constitute a binding legal agreement between you and Unity (the “Terms”).
If you accept or agree to the Agreement on behalf of a company, organization or other legal entity (a “Legal Entity”), you represent and warrant that you have the authority to bind that Legal Entity to the Agreement and, in such event, “you” and “your” will refer and apply to that company or other legal entity.
You acknowledge and agree that, by accessing, purchasing or using the services, you are indicating that you have read, understand and agree to be bound by the agreement whether or not you have created a unity account, subscribed to the unity newsletter or otherwise registered with the site. If you do not agree to these terms and all applicable additional terms, then you have no right to access or use any of the services.
*/
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.Networking.Types;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Prototype.NetworkLobby
{
    public class LobbyManager : NetworkLobbyManager
    {

        #region Helper functions
        class KickMsg : MessageBase { }

        public delegate void BackButtonDelegate();
        public BackButtonDelegate backDelegate;
        #endregion

        #region Props        
        private static short MsgKicked = MsgType.Highest + 1;
        public static LobbyManager s_Singleton;

        [Header("Unity UI Lobby")]
        [Tooltip("Time in second between all players ready & match start")]
        public float prematchCountdown = 5.0f;

        [Space]
        [Header("UI Reference")]
        public LobbyTopPanel topPanel;

        public Button backButton;
        public Text statusInfo, hostInfo;
        public RectTransform mainMenuPanel, lobbyPanel, currentPanel;
        public LobbyInfoPanel infoPanel;
        public LobbyCountdownPanel countdownPanel;
        public GameObject addPlayerButton;

        //Client numPlayers from NetworkManager is always 0, so we count (throught connect/destroy in LobbyPlayer) the number of players, so that even client know how many player there is.
        [HideInInspector]
        public int _playerNumber = 0;

        //used to disconnect a client properly when exiting the matchmaker
        [HideInInspector]
        public bool _isMatchmaking = false;

        protected bool _disconnectServer = false;
        protected ulong _currentMatchID;
        protected LobbyHook _lobbyHooks;
        #endregion

        void Start()
        {
            s_Singleton = this;
            _lobbyHooks = GetComponent<LobbyHook>();
            currentPanel = mainMenuPanel;

            backButton.gameObject.SetActive(false);
            GetComponent<Canvas>().enabled = true;

            DontDestroyOnLoad(gameObject);

            SetServerInfo("Offline", "None");

            gamePlayerPrefab = GB.LoadAnimalCar(GB.Animal);
        }

        #region UI
        public override void OnLobbyClientSceneChanged(NetworkConnection conn)
        {
            if (SceneManager.GetSceneAt(0).name == lobbyScene)
            {
                if (topPanel.isInGame)
                {
                    ChangeTo(lobbyPanel);

                    if (conn.playerControllers[0].unetView.isServer)
                        backDelegate = StopHostClbk;
                    else
                        backDelegate = StopClientClbk;
                }
                else
                    ChangeTo(mainMenuPanel);

                topPanel.ToggleVisibility(true);
                topPanel.isInGame = false;
            }
            else
            {
                ChangeTo(null);

                Destroy(GameObject.Find("MainMenuUI(Clone)"));

                //backDelegate = StopGameClbk;
                topPanel.isInGame = true;
                topPanel.ToggleVisibility(false);
            }
        }

        public void ChangeTo(RectTransform newPanel)
        {
            if (currentPanel != null)
                currentPanel.gameObject.SetActive(false);

            if (newPanel != null)
                newPanel.gameObject.SetActive(true);

            currentPanel = newPanel;

            if (currentPanel == mainMenuPanel)
            {
                backButton.gameObject.SetActive(false);
                SetServerInfo("Offline", "None");
                _isMatchmaking = false;
            }
            else
                backButton.gameObject.SetActive(true);
        }

        public void DisplayIsConnecting()
        {
            var _this = this;

            infoPanel.Display("Connecting...", "Cancel", () =>
            {
                _this.backDelegate();
            });
        }

        public void SetServerInfo(string status, string host)
        {
            statusInfo.text = status;
            hostInfo.text = host;
        }

        public void GoBackButton()
        {
            backDelegate();
            topPanel.isInGame = false;
        }
        #endregion

        #region Server management
        // ----------------- Server management
        private void IterateOverLobbySlot(System.Action<LobbyPlayer> a)
        {
            foreach (LobbyPlayer p in lobbySlots)
                if (p != null)
                    a(p);
        }

        public void AddLocalPlayer()
        {
            TryToAddPlayer();
        }

        public void RemovePlayer(LobbyPlayer player)
        {
            player.RemovePlayer();
        }

        public void SimpleBackClbk()
        {
            ChangeTo(mainMenuPanel);
        }

        public void StopHostClbk()
        {
            if (_isMatchmaking)
            {
                matchMaker.DestroyMatch((NetworkID)_currentMatchID, 0, OnDestroyMatch);
                _disconnectServer = true;
            }
            else
                StopHost();


            ChangeTo(mainMenuPanel);
        }

        public void StopClientClbk()
        {
            StopClient();

            if (_isMatchmaking)
                StopMatchMaker();

            ChangeTo(mainMenuPanel);
        }

        public void StopServerClbk()
        {
            StopServer();
            ChangeTo(mainMenuPanel);
        }

        public void KickPlayer(NetworkConnection conn)
        {
            conn.Send(MsgKicked, new KickMsg());
        }

        public void KickedMessageHandler(NetworkMessage netMsg)
        {
            infoPanel.Display("Kicked by Server", "Close", null);
            netMsg.conn.Disconnect();
        }
        #endregion

        #region Match
        //===================
        public override void OnStartHost()
        {
            base.OnStartHost();

            ChangeTo(lobbyPanel);
            backDelegate = StopHostClbk;
            SetServerInfo("Hosting", networkAddress);
        }

        public override void OnMatchCreate(bool success, string extendedInfo, MatchInfo matchInfo)
        {
            base.OnMatchCreate(success, extendedInfo, matchInfo);
            _currentMatchID = (ulong)matchInfo.networkId;
        }

        public override void OnDestroyMatch(bool success, string extendedInfo)
        {
            base.OnDestroyMatch(success, extendedInfo);

            if (_disconnectServer)
            {
                StopMatchMaker();
                StopHost();
            }
        }

        //allow to handle the (+) button to add/remove player
        public void OnPlayersNumberModified(int count)
        {
            var localPlayerCount = 0;

            _playerNumber += count;

            foreach (var p in ClientScene.localPlayers)
                localPlayerCount += ((p == null || p.playerControllerId == -1) ? 0 : 1);

            addPlayerButton.SetActive(localPlayerCount < maxPlayersPerConnection && _playerNumber < maxPlayers);
        }
        #endregion

        #region Server callbacks
        // ----------------- Server callbacks ------------------                

        //We want to disable the button JOIN if we don't have enough player. But OnLobbyClientConnect isn't called on hosting player. So we override the lobbyPlayer creation
        public override GameObject OnLobbyServerCreateLobbyPlayer(NetworkConnection conn, short playerControllerId)
        {
            var obj = Instantiate(lobbyPlayerPrefab.gameObject) as GameObject;

            var newPlayer = obj.GetComponent<LobbyPlayer>();
            newPlayer.ToggleJoinButton(numPlayers + 1 >= minPlayers);
            newPlayer.connID = conn.connectionId;

            UpdateRemoveButtonForLobbySlot(numPlayers + 1 >= minPlayers);

            return obj;
        }

        public override void OnLobbyServerPlayerRemoved(NetworkConnection conn, short playerControllerId)
        {
            UpdateRemoveButtonForLobbySlot(numPlayers + 1 >= minPlayers);
        }

        public override void OnLobbyServerDisconnect(NetworkConnection conn)
        {
            UpdateRemoveButtonForLobbySlot(numPlayers >= minPlayers);
        }

        private void UpdateRemoveButtonForLobbySlot(bool Condition)
        {
            IterateOverLobbySlot((p) =>
            {
                p.RpcUpdateRemoveButton();
                p.ToggleJoinButton(Condition);
            });
        }

        public override GameObject OnLobbyServerCreateGamePlayer(NetworkConnection conn, short playerControllerId)
        {
            var car_prefab = GB.LoadAnimalCar(GB.Animal);
            var car_instance = Instantiate(car_prefab, startPositions[conn.connectionId].position, Quaternion.identity);

            return car_instance;
        }

        public override bool OnLobbyServerSceneLoadedForPlayer(GameObject lobbyPlayer, GameObject gamePlayer)
        {
            //This hook allows you to apply state data from the lobby-player to the game-player. Just subclass "LobbyHook" and add it to the lobby object.
            if (_lobbyHooks)
                _lobbyHooks.OnLobbyServerSceneLoadedForPlayer(this, lobbyPlayer, gamePlayer);

            return true;
        }
        #endregion      

        #region Countdown
        // --- Countdown management
        public override void OnLobbyServerPlayersReady()
        {
            var allready = true;

            IterateOverLobbySlot((p) =>
            {
                allready &= p.readyToBegin;
            });

            if (allready)
                StartCoroutine(ServerCountdownCoroutine());
        }

        public IEnumerator ServerCountdownCoroutine()
        {
            var remainingTime = prematchCountdown;
            var floorTime = Mathf.FloorToInt(remainingTime);

            while (remainingTime > 0)
            {
                yield return null;

                remainingTime -= Time.deltaTime;
                var newFloorTime = Mathf.FloorToInt(remainingTime);

                if (newFloorTime != floorTime)
                {
                    //to avoid flooding the network of message, we only send a notice to client when the number of plain seconds change.
                    floorTime = newFloorTime;

                    //there is maxPlayer slots, so some could be == null, need to test it before accessing!
                    UpdateCountdownForLobbySlot(floorTime);
                }
            }

            UpdateCountdownForLobbySlot(0);

            ServerChangeScene(playScene);
        }

        private void UpdateCountdownForLobbySlot(int countdown)
        {
            IterateOverLobbySlot((p) =>
            {
                p.RpcUpdateCountdown(countdown);
            });
        }
        #endregion

        #region Client callbacks    
        // ----------------- Client callbacks ------------------
        public override void OnClientConnect(NetworkConnection conn)
        {
            base.OnClientConnect(conn);

            infoPanel.gameObject.SetActive(false);

            conn.RegisterHandler(MsgKicked, KickedMessageHandler);

            if (!NetworkServer.active)
            {
                //only to do on pure client (not self hosting client)
                ChangeTo(lobbyPanel);
                backDelegate = StopClientClbk;
                SetServerInfo("Client", networkAddress);
            }
        }

        public override void OnClientDisconnect(NetworkConnection conn)
        {
            base.OnClientDisconnect(conn);
            ChangeTo(mainMenuPanel);
        }

        public override void OnClientError(NetworkConnection conn, int errorCode)
        {
            ChangeTo(mainMenuPanel);
            infoPanel.Display("Client error : " + (errorCode == 6 ? "timeout" : errorCode.ToString()), "Close", null);
        }
        #endregion


    }
}