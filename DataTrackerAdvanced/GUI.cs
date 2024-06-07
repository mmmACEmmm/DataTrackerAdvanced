using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using BepInEx;

namespace AcesDataTrackerAdvanced
{
    [BepInPlugin("AcesDataTrackerAdvanced", "Aces Data Tracker Advanced", "1.0.0")]
    public class AcesDataTrackerAdvanced : MonoBehaviourPunCallbacks
    {
        private bool showGUI = false;
        private Dictionary<int, PlayerInfo> playerInfoDict = new Dictionary<int, PlayerInfo>();

        private class PlayerInfo
        {
            public string playerName;
            public string playerId;
            public string playerProperties;
            public string playerCosmetics;

            public PlayerInfo(Player player)
            {
                playerName = player.NickName;
                playerId = player.UserId;
                playerProperties = Extensions.ToStringFull(player.CustomProperties);
                // For demonstration, assuming some cosmetic info
                playerCosmetics = "Cosmetics Info";
            }
        }

        private void Update()
        {
            // Open GUI
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.C))
            {
                showGUI = true;
            }

            // Close GUI
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.P))
            {
                showGUI = false;
            }
        }

        private void OnGUI()
        {
            if (showGUI)
            {
                GUI.Box(new Rect(40, 40, 460, 360), "Aces Data Tracker Advanced [CTRL + P = Close]");

                int posY = 70;
                foreach (var playerInfo in playerInfoDict.Values)
                {
                    GUI.Label(new Rect(50, posY, 200, 20), "Name: " + playerInfo.playerName);
                    GUI.Label(new Rect(250, posY, 200, 20), "ID: " + playerInfo.playerId);
                    GUI.Label(new Rect(50, posY + 20, 200, 20), "Properties: " + playerInfo.playerProperties);
                    GUI.Label(new Rect(250, posY + 20, 200, 20), "Cosmetics: " + playerInfo.playerCosmetics);
                    posY += 50;
                }
            }
        }

        public override void OnJoinedRoom()
        {
            UpdatePlayerInfo();
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            UpdatePlayerInfo();
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            UpdatePlayerInfo();
        }

        private void UpdatePlayerInfo()
        {
            playerInfoDict.Clear();

            foreach (Player player in PhotonNetwork.PlayerList)
            {
                playerInfoDict[player.ActorNumber] = new PlayerInfo(player);
            }
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            playerInfoDict.Clear();
        }
    }
}
