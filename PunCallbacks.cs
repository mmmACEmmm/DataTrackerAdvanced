using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace DataLogger
{
    internal class PunCallbacks : MonoBehaviourPunCallbacks
    {
        // List of special cosmetic IDs
        private readonly string[] specialCosmeticIds = { "LBAAK", "LBADE" };

        // Predefined player IDs and names
        private readonly Dictionary<string, string> predefinedPlayerIDs = new Dictionary<string, string>
        {
            {"9DBC90CF7449EF64", "StyledSnail"},
            {"6FE5FF4D5DF68843", "Pine"},
            {"52529F0635BE0CDF", "PapaSmurf"},
            {"10D31D3BDCCE5B1F", "Deezey"},
            {"BAC5807405123060", "britishmonke"},
            {"A6FFC7318E1301AF", "jmancurl"},
            {"3B9FD2EEF24ACB3", "VMT"},
            {"04005517920EBO", "K9?"},
            {"33FFA45DBFD33B01", "will"},
            {"D6971CA01F82A975", "Elliot"},
            {"636D8846E76C9B5A", "Clown"},
            {"65CB0CCF1AED2BF", "Ethyb"},
            {"48437FE432DE48BE", "BBVR"},
            {"61AD990FF3A423B7", "Boda 1"},
            {"AAB44BFD0BA34829", "Boda 2"},
            {"6713DA80D2E9BFB5", "AHauntedArmy"},
            {"B4A3FF01312B55B1", "Pluto"},
            {"E354E818871BD1D8", "developer9998"},
            {"FBE3EE50747CB892", "Lunakitty"},
            {"339E0D392565DC39", "kishark"},
            {"F08CE3118F9E793E", "TurboAlligator"},
            {"D6E20BE9655C798", "TTTPIG 1"},
            {"71AA09D13C0F408D", "TTTPIG 2"},
            {"1D6E20BE9655C798", "TTTPIG 3"},
            {"22A7BCEFFD7A0BBA", "TTTPIG 4"},
            {"C3878B068886F6C3", "ZZEN"},
            {"6F79BE7CB34642AC", "CodyO'Quinn"},
            {"5AA1231973BE8A62", "Apollo"},
            {"7F31BEEC604AE189", "ElectronicWall 1"},
            {"42C809327652ECDD", "ElectronicWall 2"},
            {"ECDE8A2FF8510934", "Antoca"},
            {"80279945E7D3B57D", "Jolyne"},
            {"7E44E8337DF02CC1", "Nunya"},
            {"DE601BC40DB68CE0", "Graic"},
            {"F5B5C64914C13B83", "HatGirl"},
            {"660814E013F31EFA", "HOLLOWZZGT"},
            {"2E408ED946D55D51", "Haunted"},
            {"D345FE394607F946", "Bzzz the 18th"},
            {"498D4C2F23853B37", "POGTROLL"},
            {"BC9764E1EADF8BE0", "Circuit"},
            {"D0CB396539676DD8", "FrogIlla"},
            {"A1A99D33645E4A94", "STEAMVRAVTS / YEAT"},
            {"CA8FDFF42B7A1836", "Brokenstone"},
            {"CBCCBBB6C28A94CF", "PTMstar"},
            {"6DC06EEFFE9DBD39", "Lucio"},
            {"4ACA3C76B334B17F", "Wihz"},
            {"41988726285E534E", "Colussus"},
            {"571776944B6162F1", "CubCub"},
            {"FB5FCEBC4A0E0387", "PepsiDee"},
            {"645222265FB972B", "Chaotic Asriel"},
            {"BC99FA914F506AB8", "Lemming 1"},
            {"3A16560CA65A51DE", "Lemming 2"},
            {"59F3FE769DE93AB9", "Lemming 3"},
            {"EE9FB127CF7DBBD5", "NOTMARK"},
            {"54DCB69545BE0800", "Biffbish"},
            {"A04005517920EB0", "K9"},
            {"3CB4F61C87A5AF24", "Octoburr/Evelyn Standalone"},
            {"4994748F8B361E31", "Octoburr/Evelyn PC"},
            {"5CCCAA8A225A468B", "furina"},
            {"5ACE0508B3B95588", "ACEGT"},
            {"1CF4862F9A7B0D39", "ACEGT"},
            {"1879DA7F4096C99A", "GUY"}
        };

        private void Awake()
        {
            bool flag = !Directory.Exists("DataLogger");
            if (flag)
            {
                Directory.CreateDirectory("DataLogger");
                Directory.CreateDirectory("DataLogger/Rooms");
            }
            UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
        }

        public override void OnJoinedRoom()
        {
            Directory.CreateDirectory("DataLogger/Rooms/" + PhotonNetwork.CurrentRoom.Name);
            base.StartCoroutine(this.SaveDelay());
        }

        private IEnumerator SaveDelay()
        {
            yield return new WaitForSeconds(0.5f);
            this.RefreshData();
            yield break;
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            this.RefreshData();
        }

        private void RefreshData()
        {
            foreach (Player player in PhotonNetwork.PlayerList)
            {
                this.SaveData(player);
                this.SavePlayerID(player);
            }
        }

        private void SaveData(Player player)
        {
            VRRig vrrig = GorillaGameManager.instance.FindPlayerVRRig(player);
            string cosmetics = vrrig.concatStringOfCosmeticsAllowed;

            // Flag special cosmetics
            bool hasSpecialCosmetic = false;
            foreach (string id in specialCosmeticIds)
            {
                if (cosmetics.Contains(id))
                {
                    hasSpecialCosmetic = true;
                    break;
                }
            }

            // Count the number of cosmetics
            int cosmeticCount = cosmetics.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries).Length - 1;

            // Get player color RGB values and convert to 0-9 range
            Color playerColor = vrrig.playerColor;
            int r = (int)Math.Round(playerColor.r * 255 / 28.4);
            int g = (int)Math.Round(playerColor.g * 255 / 28.4);
            int b = (int)Math.Round(playerColor.b * 255 / 28.4);

            // Save data to file
            string filePath = $"DataLogger/Rooms/{PhotonNetwork.CurrentRoom.Name}/{player.NickName}_{player.UserId}.txt";
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine($"Usename: {player.NickName}");
                writer.WriteLine($"Player ID: {player.UserId}");
                writer.WriteLine($"Properties: {Extensions.ToStringFull(player.CustomProperties)}");
                writer.WriteLine($"Cosmetics: {cosmetics}");
                writer.WriteLine(hasSpecialCosmetic ? $"Special Cosmetics: Yes" : $"Special Cosmetics: No");
                writer.WriteLine($"Cosmetic Amount: {cosmeticCount}");
                writer.WriteLine($"Color Code: {r},{g},{b}");
                writer.WriteLine("⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣀⣴⢚⣿⡷⣶⣶⣄⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀");
                writer.WriteLine("⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢠⣴⣿⣾⣿⣿⣿⣿⣿⣿⣿⣿⣤⣶⡀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀");
                writer.WriteLine("⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⣴⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣧⡂⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀");
                writer.WriteLine("⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⣸⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡆⠀⠀⠀⠀⠀⠀⠀⠀⠀");
                writer.WriteLine("⠀⠀⠀⠀⠀⠀⠀⠀⠀⢼⣿⣿⡿⢫⣽⣽⣛⠛⠻⢿⠿⠛⣉⣥⠤⢼⣿⣿⣿⡿⣽⠀⠀⠀⠀⠀⠀⠀⠀");
                writer.WriteLine("⠀⠀⠀⠀⠀⠀⠀⠀⠀⢺⣿⣿⣿⢱⣶⣶⢬⣟⢶⠙⣷⠞⣩⣶⣿⢳⢹⣾⣿⡀⣽⠀⠀⠀⠀⠀⠀⠀⠀");
                writer.WriteLine("⠀⠀⠀⠀⠀⠀⠀⠀⠀⢸⣿⣿⣿⣎⢿⣿⡿⢏⣼⠀⠻⣆⠻⢿⣿⠟⣿⣿⣿⣷⣟⠀⠀⠀⠀⠀⠀⠀⠀");
                writer.WriteLine("⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⣿⣿⣿⣯⡓⠒⠒⠋⠀⠀⢀⠈⠙⠒⢚⣿⣿⣿⣿⣏⣿⠀⠀⠀⠀⠀⠀⠀⠀");
                writer.WriteLine("⠀⠀⠀⠀⠀⠀⠀⠀⠀⢼⣿⣿⣿⣿⣿⣷⡄⠺⢷⠆⠼⠿⠇⣴⣿⣿⣿⣿⣿⣿⡏⠀⠀⠀⠀⠀⠀⠀⠀");
                writer.WriteLine("⠀⠀⠀⠀⠀⠀⠀⠀⠀⣸⣿⣿⣿⣿⣿⣿⡇⠀⠀⠀⠀⠀⢸⣿⣿⣿⣿⣿⣿⢹⠇⠀⠀⠀⠀⠀⠀⠀⠀");
                writer.WriteLine("⠀⠀⠀⠀⠀⠀⠀⣤⣿⣿⣿⣿⣿⣿⣿⣿⠇⠀⠀⠀⠀⠀⠀⢻⣿⣿⢿⣿⣿⣿⣀⠀⠀⠀⠀⠀⠀⠀⠀");
                writer.WriteLine("⠀⠀⠀⠀⠀⢠⣶⣿⣿⣿⣿⣿⣿⣿⣿⣿⠳⣤⣀⣤⣤⣀⣠⢾⣿⣿⣾⣿⣿⣿⣿⣷⣄⣠⠀⠀⠀⠀⠀");
                writer.WriteLine("⠀⠀⠀⢀⣴⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣷⣌⡀⠀⠀⠈⣀⣾⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣷⡀⠀⠀⠀");
                writer.WriteLine("⠀⠀⠀⣾⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣏⣿⣿⣿⣿⣿⣿⣿⣿⣷⠀⠀⠀");
                writer.WriteLine("⠀⣠⣷⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⠏⠉⠉⠉⠙⠛⠛⠛⠛⠛⠛⠛⠛⢿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣧⠀⠀");
                writer.WriteLine("⢀⣾⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⠃⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢻⣿⣿⣿⣿⣿⣿⣿⣿⣿⣧⠀");
                writer.WriteLine("⣾⣿⣿⣿⣿⣿⣿⣿⢿⣿⡏⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢻⣿⣿⣿⣿⣿⣿⣿⣿⣿⡇");
            }
        }

        private void SavePlayerID(Player player)
        {
            string playerID = player.UserId;
            string playerName = player.NickName;

            // Path to the player IDs file
            string playerIDsPath = "DataLogger/player.IDs";

            // Check if the player ID already exists in the file
            bool playerExists = false;
            if (File.Exists(playerIDsPath))
            {
                string[] lines = File.ReadAllLines(playerIDsPath);
                foreach (string line in lines)
                {
                    if (line.Contains(playerID))
                    {
                        playerExists = true;
                        break;
                    }
                }
            }

            // If the player ID doesn't exist, add it to the file
            if (!playerExists)
            {
                string playerEntry = $"{playerID};{playerName}";
                using (StreamWriter writer = new StreamWriter(playerIDsPath, true))
                {
                    writer.WriteLine(playerEntry);
                }

                // Cross-reference with predefined player IDs
                if (predefinedPlayerIDs.ContainsKey(playerID))
                {
                    string predefinedPlayerName = predefinedPlayerIDs[playerID];
                    string newFilePath = $"DataLogger/{predefinedPlayerName}.txt";
                    using (StreamWriter newFileWriter = new StreamWriter(newFilePath))
                    {
                        newFileWriter.WriteLine($"Player ID: {playerID}");
                        newFileWriter.WriteLine($"Player Name: {playerName}");
                        newFileWriter.WriteLine($"Matched with Predefined ID: {predefinedPlayerName}");
                        newFileWriter.WriteLine($"Room: {PhotonNetwork.CurrentRoom.Name}");
                    }
                }
            }
        }
    }
}
