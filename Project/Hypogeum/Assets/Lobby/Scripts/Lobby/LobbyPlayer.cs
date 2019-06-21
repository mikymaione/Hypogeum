/*
Copyright (c) 2018 Unity Technologies ApS
Author: Unity Technologies ApS
Contributors: Maione Michele
Unity Technologies ApS (“Unity”, “our” or “we”) provides game-development and related software (the “Software”), development-related services (like Unity Teams (“Developer Services”)), and various Unity communities (like Unity Answers and Unity Connect (“Communities”)), provided through or in connection with our website, accessible at unity3d.com or unity.com (collectively, the “Site”). Except to the extent you and Unity have executed a separate agreement, these terms and conditions exclusively govern your access to and use of the Software, Developer Services, Communities and Site (collectively, the “Services”), and constitute a binding legal agreement between you and Unity (the “Terms”).
If you accept or agree to the Agreement on behalf of a company, organization or other legal entity (a “Legal Entity”), you represent and warrant that you have the authority to bind that Legal Entity to the Agreement and, in such event, “you” and “your” will refer and apply to that company or other legal entity.
You acknowledge and agree that, by accessing, purchasing or using the services, you are indicating that you have read, understand and agree to be bound by the agreement whether or not you have created a unity account, subscribed to the unity newsletter or otherwise registered with the site. If you do not agree to these terms and all applicable additional terms, then you have no right to access or use any of the services.
*/
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Prototype.NetworkLobby
{
    //Player entry in the lobby. Handle selecting color/setting name & getting ready for the game
    //Any LobbyHook can then grab it and pass those value to the game player prefab (see the Pong Example in the Samples Scenes)
    public class LobbyPlayer : NetworkLobbyPlayer
    {
        static Color[] Colors = new Color[] { Color.magenta, Color.red, Color.cyan, Color.blue, Color.green, Color.yellow, Color.gray, Color.black };
        //used on server to avoid assigning the same color to two player
        static List<int> _colorInUse = new List<int>();

        public InputField nameInput;
        public Button colorButton, readyButton, waitingPlayerButton, removePlayerButton;
        public GameObject localIcone, remoteIcone;

        [SyncVar]
        public int connID;

        //OnMyName function will be invoked on clients when server change the value of playerName
        //
        [SyncVar(hook = "OnMyName")]
        public string playerName = "";

        [SyncVar(hook = "OnMyColor")]
        public Color playerColor = Color.white;

        [SyncVar(hook = "OnMyAnimal")]
        public GB.EAnimal animal;

        [SyncVar(hook = "OnMyGameType")]
        public GB.EGameType gameType;


        public Color OddRowColor = new Color(250.0f / 255.0f, 250.0f / 255.0f, 250.0f / 255.0f, 1.0f);
        public Color EvenRowColor = new Color(180.0f / 255.0f, 180.0f / 255.0f, 180.0f / 255.0f, 1.0f);

        static Color JoinColor = new Color(255.0f / 255.0f, 0.0f, 101.0f / 255.0f, 1.0f);
        static Color NotReadyColor = new Color(34.0f / 255.0f, 44 / 255.0f, 55.0f / 255.0f, 1.0f);
        static Color ReadyColor = new Color(0.0f, 204.0f / 255.0f, 204.0f / 255.0f, 1.0f);
        static Color TransparentColor = new Color(0, 0, 0, 0);

        //static Color OddRowColor = new Color(250.0f / 255.0f, 250.0f / 255.0f, 250.0f / 255.0f, 1.0f);
        //static Color EvenRowColor = new Color(180.0f / 255.0f, 180.0f / 255.0f, 180.0f / 255.0f, 1.0f);    

        public override void OnClientEnterLobby()
        {
            base.OnClientEnterLobby();

            LobbyManager.s_Singleton?.OnPlayersNumberModified(1);

            LobbyPlayerList._instance.AddPlayer(this);
            LobbyPlayerList._instance.DisplayDirectServerWarning(isServer && LobbyManager.s_Singleton.matchMaker == null);

            if (isLocalPlayer)
                SetupLocalPlayer();
            else
                SetupOtherPlayer();

            //setup the player data on UI. The value are SyncVar so the player will be created with the right value currently on server
            OnMyName(playerName);
            OnMyColor(playerColor);
            OnMyAnimal(animal);
            OnMyGameType(gameType);
        }

        public override void OnStartAuthority()
        {
            base.OnStartAuthority();

            //if we return from a game, color of text can still be the one for "Ready"
            readyButton.transform.GetChild(0).GetComponent<Text>().color = Color.white;

            SetupLocalPlayer();
        }

        void ChangeReadyButtonColor(Color c)
        {
            var b = readyButton.colors;
            b.normalColor = c;
            b.pressedColor = c;
            b.highlightedColor = c;
            b.disabledColor = c;

            readyButton.colors = b;
        }

        void SetupOtherPlayer()
        {
            nameInput.interactable = false;
            removePlayerButton.interactable = NetworkServer.active;

            ChangeReadyButtonColor(NotReadyColor);

            readyButton.transform.GetChild(0).GetComponent<Text>().text = "...";
            readyButton.interactable = false;

            OnClientReady(false);
        }

        void SetupLocalPlayer()
        {
            nameInput.interactable = true;
            remoteIcone.gameObject.SetActive(false);
            localIcone.gameObject.SetActive(true);

            CheckRemoveButton();

            if (playerColor == Color.white)
                CmdColorChange();

            ChangeReadyButtonColor(JoinColor);

            readyButton.transform.GetChild(0).GetComponent<Text>().text = "JOIN";
            readyButton.interactable = true;

            if (playerName.Equals(""))
            {
                var n = System.Environment.UserName;

                if (string.IsNullOrEmpty(n))
                    n = RandomNameGenerator.NameGenerator.GenerateLadieName();

                CmdNameChanged(n);
            }

            CmdSetAnimal(GB.Animal.Value);
            CmdSetGameType(GB.GameType.Value);

            //we switch from simple name display to name input
            colorButton.interactable = true;
            nameInput.interactable = true;

            nameInput.onEndEdit.RemoveAllListeners();
            nameInput.onEndEdit.AddListener(OnNameChanged);

            colorButton.onClick.RemoveAllListeners();
            colorButton.onClick.AddListener(OnColorClicked);

            readyButton.onClick.RemoveAllListeners();
            readyButton.onClick.AddListener(OnReadyClicked);

            //when OnClientEnterLobby is called, the loval PlayerController is not yet created, so we need to redo that here to disable
            //the add button if we reach maxLocalPlayer. We pass 0, as it was already counted on OnClientEnterLobby
            LobbyManager.s_Singleton?.OnPlayersNumberModified(0);
        }

        //This enable/disable the remove button depending on if that is the only local player or not
        public void CheckRemoveButton()
        {
            if (!isLocalPlayer)
                return;

            var localPlayerCount = 0;
            foreach (var p in ClientScene.localPlayers)
                localPlayerCount += (p == null || p.playerControllerId == -1) ? 0 : 1;

            removePlayerButton.interactable = localPlayerCount > 1;
        }

        public override void OnClientReady(bool readyState)
        {
            var textComponent = readyButton.transform.GetChild(0).GetComponent<Text>();

            if (readyState)
            {
                ChangeReadyButtonColor(TransparentColor);

                textComponent.text = "READY";
                textComponent.color = ReadyColor;

                readyButton.interactable = false;
                colorButton.interactable = false;
                nameInput.interactable = false;
            }
            else
            {
                ChangeReadyButtonColor(isLocalPlayer ? JoinColor : NotReadyColor);

                textComponent.text = isLocalPlayer ? "JOIN" : "...";
                textComponent.color = Color.white;

                readyButton.interactable = isLocalPlayer;
                colorButton.interactable = isLocalPlayer;
                nameInput.interactable = isLocalPlayer;
            }
        }

        public void OnPlayerListChanged(int idx)
        {
            GetComponent<Image>().color = (idx % 2 == 0) ? EvenRowColor : OddRowColor;
        }

        ///===== callback from sync var 
		///these are methods called by reflection with string hooks form LobbyPlayer.cs
        public void OnMyAnimal(GB.EAnimal newAnimal)
        {
            animal = newAnimal;
        }

        public void OnMyGameType(GB.EGameType newGameType)
        {
            gameType = newGameType;
        }

        public void OnMyName(string newName)
        {
            playerName = newName;
            nameInput.text = playerName;
        }

        public void OnMyColor(Color newColor)
        {
            playerColor = newColor;
            colorButton.GetComponent<Image>().color = newColor;
        }

        //===== UI Handler

        //Note that those handler use Command function, as we need to change the value on the server not locally
        //so that all client get the new value throught syncvar
        public void OnColorClicked()
        {
            CmdColorChange();
        }

        public void OnReadyClicked()
        {
            SendReadyToBeginMessage();
        }

        public void OnNameChanged(string str)
        {
            CmdNameChanged(str);
        }

        public void OnRemovePlayerClick()
        {
            if (isLocalPlayer)
                RemovePlayer();
            else if (isServer)
                LobbyManager.s_Singleton.KickPlayer(connectionToClient);
        }

        public void ToggleJoinButton(bool enabled)
        {
            readyButton.gameObject.SetActive(enabled);
            waitingPlayerButton.gameObject.SetActive(!enabled);
        }

        [ClientRpc]
        public void RpcUpdateCountdown(int countdown)
        {
            LobbyManager.s_Singleton.countdownPanel.UIText.text = "Match Starting in " + countdown;
            LobbyManager.s_Singleton.countdownPanel.gameObject.SetActive(countdown != 0);
        }

        [ClientRpc]
        public void RpcUpdateRemoveButton()
        {
            CheckRemoveButton();
        }

        //====== Server Command

        [Command]
        public void CmdColorChange()
        {
            var alreadyInUse = false;

            var idx = System.Array.IndexOf(Colors, playerColor);
            var inUseIdx = _colorInUse.IndexOf(idx);

            if (idx < 0)
                idx = 0;

            idx = (idx + 1) % Colors.Length;

            do
            {
                alreadyInUse = false;

                for (var i = 0; i < _colorInUse.Count; ++i)
                    if (_colorInUse[i] == idx)
                    {
                        //that color is already in use
                        alreadyInUse = true;
                        idx = (idx + 1) % Colors.Length;
                    }
            }
            while (alreadyInUse);

            if (inUseIdx >= 0)
                //if we already add an entry in the colorTabs, we change it
                _colorInUse[inUseIdx] = idx;
            else
                //else we add it
                _colorInUse.Add(idx);

            playerColor = Colors[idx];
        }

        [Command]
        public void CmdNameChanged(string name)
        {
            playerName = name;
        }

        [Command]
        public void CmdSetAnimal(GB.EAnimal animal_)
        {
            animal = animal_;
        }

        [Command]
        public void CmdSetGameType(GB.EGameType gameType_)
        {
            gameType = gameType_;
        }

        //Cleanup thing when get destroy (which happen when client kick or disconnect)
        public void OnDestroy()
        {
            LobbyPlayerList._instance.RemovePlayer(this);
            LobbyManager.s_Singleton?.OnPlayersNumberModified(-1);

            var idx = System.Array.IndexOf(Colors, playerColor);

            if (idx < 0)
                return;

            for (var i = 0; i < _colorInUse.Count; ++i)
                if (_colorInUse[i] == idx)
                {
                    //that color is already in use
                    _colorInUse.RemoveAt(i);
                    break;
                }
        }


    }
}