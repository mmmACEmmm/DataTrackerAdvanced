using System.Collections;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using System.IO;

namespace DataLogger
{
    internal class PunCallbacks : MonoBehaviourPunCallbacks
    {
        public Text lobbyInfoText;
        public Text playerInfoText;

        private void Awake()
        {
            bool flag = !Directory.Exists("Rooms");
            if (flag)
            {
                Directory.CreateDirectory("Rooms");
            }
            UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
        }

        public override void OnJoinedRoom()
        {
            Directory.CreateDirectory("Rooms/" + PhotonNetwork.CurrentRoom.Name);
            StartCoroutine(SaveDelay());
        }

        private IEnumerator SaveDelay()
        {
            yield return new WaitForSeconds(0.5f);
            RefreshData();
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            RefreshData();
        }

        private void RefreshData()
        {
            string lobbyInfo = "Room: " + PhotonNetwork.CurrentRoom.Name + "\n";
            lobbyInfo += "Players: " + PhotonNetwork.PlayerList.Length + "\n";
            lobbyInfoText.text = lobbyInfo;

            string playersInfo = "";
            foreach (Player player in PhotonNetwork.PlayerList)
            {
                playersInfo += "Username: " + player.NickName + "\n";
                playersInfo += "Player ID: " + player.UserId + "\n";
                playersInfo += "Properties: " + Extensions.ToStringFull(player.CustomProperties) + "\n";
                playersInfo += "\n";

                SaveData(player); // Save player data to file
            }
            playerInfoText.text = playersInfo;
        }

        private void SaveData(Player player)
        {
            VRRig vrrig = GorillaGameManager.instance.FindPlayerVRRig(player);
            string filePath = Path.Combine("Rooms", PhotonNetwork.CurrentRoom.Name, $"{player.NickName}_{player.UserId}.txt");

            string fileContents = $"Usename: {player.NickName}\r\n";
            fileContents += $"Player ID: {player.UserId}\r\n";
            fileContents += $"Properties: {Extensions.ToStringFull(player.CustomProperties)}\r\n";
            fileContents += $"Cosmetics: {vrrig.concatStringOfCosmeticsAllowed}";

            File.WriteAllText(filePath, fileContents);
        }
    }
}
