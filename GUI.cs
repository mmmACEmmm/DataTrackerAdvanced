using UnityEngine;
using Photon.Pun;
using BepInEx;
using System.Collections.Generic;
using System.Linq;

namespace GhostsGUI
{
    [BepInPlugin("AceDataLoggerGui", "ACE’S DATA LOGGER GUI", "1.0.0")]
    public class GhostGUI : BaseUnityPlugin
    {
        private bool showGUI = false;
        private bool showDataLogger = false;
        private string currentTab = "";
        private Dictionary<string, string> playerDetails = new Dictionary<string, string>();
        private string selectedPlayerID = "";

        // List of special player IDs
        private readonly Dictionary<string, string> specialPlayerIDs = new Dictionary<string, string>
        {
            { "9DBC90CF7449EF64", "StyledSnail" },
            { "6FE5FF4D5DF68843", "Pine" },
            { "52529F0635BE0CDF", "PapaSmurf" },
            { "10D31D3BDCCE5B1F", "Deezey" },
            { "BAC5807405123060", "britishmonke" },
            { "A6FFC7318E1301AF", "jmancurl" },
            { "3B9FD2EEF24ACB3", "VMT" },
            { "04005517920EBO", "K9?" },
            { "33FFA45DBFD33B01", "will" },
            { "D6971CA01F82A975", "Elliot" },
            { "636D8846E76C9B5A", "Clown" },
            { "65CB0CCF1AED2BF", "Ethyb" },
            { "48437FE432DE48BE", "BBVR" },
            { "61AD990FF3A423B7", "Boda 1" },
            { "AAB44BFD0BA34829", "Boda 2" },
            { "6713DA80D2E9BFB5", "AHauntedArmy" },
            { "B4A3FF01312B55B1", "Pluto" },
            { "E354E818871BD1D8", "developer9998" },
            { "FBE3EE50747CB892", "Lunakitty" },
            { "339E0D392565DC39", "kishark" },
            { "F08CE3118F9E793E", "TurboAlligator" },
            { "D6E20BE9655C798", "TTTPIG 1" },
            { "71AA09D13C0F408D", "TTTPIG 2" },
            { "1D6E20BE9655C798", "TTTPIG 3" },
            { "22A7BCEFFD7A0BBA", "TTTPIG 4" },
            { "C3878B068886F6C3", "ZZEN" },
            { "6F79BE7CB34642AC", "CodyO'Quinn" },
            { "5AA1231973BE8A62", "Apollo" },
            { "7F31BEEC604AE189", "ElectronicWall 1" },
            { "42C809327652ECDD", "ElectronicWall 2" },
            { "ECDE8A2FF8510934", "Antoca" },
            { "80279945E7D3B57D", "Jolyne" },
            { "7E44E8337DF02CC1", "Nunya" },
            { "DE601BC40DB68CE0", "Graic" },
            { "F5B5C64914C13B83", "HatGirl" },
            { "660814E013F31EFA", "HOLLOWZZGT" },
            { "2E408ED946D55D51", "Haunted" },
            { "D345FE394607F946", "Bzzz the 18th" },
            { "498D4C2F23853B37", "POGTROLL" },
            { "BC9764E1EADF8BE0", "Circuit" },
            { "D0CB396539676DD8", "FrogIlla" },
            { "A1A99D33645E4A94", "STEAMVRAVTS / YEAT" },
            { "CA8FDFF42B7A1836", "Brokenstone" },
            { "CBCCBBB6C28A94CF", "PTMstar" },
            { "6DC06EEFFE9DBD39", "Lucio" },
            { "4ACA3C76B334B17F", "Wihz" },
            { "41988726285E534E", "Colussus" },
            { "571776944B6162F1", "CubCub" },
            { "FB5FCEBC4A0E0387", "PepsiDee" },
            { "645222265FB972B", "Chaotic Asriel" },
            { "BC99FA914F506AB8", "Lemming 1" },
            { "3A16560CA65A51DE", "Lemming 2" },
            { "59F3FE769DE93AB9", "Lemming 3" },
            { "EE9FB127CF7DBBD5", "NOTMARK" },
            { "54DCB69545BE0800", "Biffbish" },
            { "A04005517920EB0", "K9" },
            { "3CB4F61C87A5AF24", "Octoburr/Evelyn Standalone" },
            { "4994748F8B361E31", "Octoburr/Evelyn PC" },
            { "5CCCAA8A225A468B", "furina" },
            { "5ACE0508B3B95588", "ACEGT" },
            { "1CF4862F9A7B0D39", "ACEGT" },
            { "1879DA7F4096C99A", "GUY" }
        };

        private bool isDragging = false;
        private Vector2 dragDelta = Vector2.zero;
        private Texture2D logoTexture;

        void Start()
        {
            // Load the image from the Resources folder
            logoTexture = Resources.Load<Texture2D>("logo.png"); // Make sure the image file is named "logo.png" and placed in the Resources folder
        }

        void OnGUI()
        {
            GUI.backgroundColor = Color.black; // Set background color to black

            if (showGUI)
            {
                // Close button
                if (GUI.Button(new Rect(750, 10, 40, 20), "X"))
                {
                    showGUI = false;
                }

                // Data Logger button
                if (GUI.Button(new Rect(50, 70, 150, 30), "Data Logger"))
                {
                    showDataLogger = !showDataLogger;
                }

                // Show data logger section if Data Logger button is pressed
                if (showDataLogger)
                {
                    // Lobby button
                    if (GUI.Button(new Rect(50, 110, 150, 30), "Lobby"))
                    {
                        currentTab = currentTab == "Lobby" ? "" : "Lobby";
                        if (currentTab == "Lobby") LoadPlayerDetails();
                    }

                    // Special Players button
                    if (GUI.Button(new Rect(50, 150, 150, 30), "Special Players"))
                    {
                        currentTab = currentTab == "SpecialPlayers" ? "" : "SpecialPlayers";
                        if (currentTab == "SpecialPlayers") LoadPlayerDetails();
                    }

                    // Show lobby tab content
                    if (currentTab == "Lobby")
                    {
                        int yPos = 230;
                        foreach (var player in playerDetails)
                        {
                            if (GUI.Button(new Rect(50, yPos, 360, 30), $"{player.Value} ({player.Key})"))
                            {
                                selectedPlayerID = player.Key;
                            }
                            yPos += 40;
                        }

                        if (!string.IsNullOrEmpty(selectedPlayerID) && playerDetails.ContainsKey(selectedPlayerID))
                        {
                            string details = GetPlayerDetails(selectedPlayerID);
                            GUI.TextArea(new Rect(420, 70, 400, 400), details);
                        }
                    }

                    // Show special players tab content
                    if (currentTab == "SpecialPlayers")
                    {
                        int yPos = 230;
                        foreach (var player in playerDetails)
                        {
                            if (specialPlayerIDs.ContainsKey(player.Key))
                            {
                                if (GUI.Button(new Rect(50, yPos, 360, 30), $"{specialPlayerIDs[player.Key]} ({player.Key})"))
                                {
                                    selectedPlayerID = player.Key;
                                }
                                yPos += 40;
                            }
                        }

                        if (!string.IsNullOrEmpty(selectedPlayerID) && playerDetails.ContainsKey(selectedPlayerID))
                        {
                            string
details = GetPlayerDetails(selectedPlayerID);
                            GUI.TextArea(new Rect(420, 70, 400, 400), details);
                        }
                    }
                }
                // Drag window functionality
                GUI.DragWindow(new Rect(0, 0, 10000, 20));
            }
            else
            {
                // Show a label when GUI is closed
                GUI.Label(new Rect(10, 10, 200, 20), "CTRL + C to open Data Logger GUI");
                // Opening GUI with keyboard shortcut
                Event e = Event.current;
                if (e != null && e.isKey && e.keyCode == KeyCode.C && e.control)
                {
                    showGUI = true;
                }
            }

            // Draw logo
            if (logoTexture != null)
            {
                GUI.DrawTexture(new Rect(10, Screen.height - 110, logoTexture.width, logoTexture.height), logoTexture);
            }
        }

        private void LoadPlayerDetails()
        {
            playerDetails.Clear();
            foreach (var player in PhotonNetwork.PlayerList)
            {
                playerDetails[player.UserId] = player.NickName;
            }
        }

        private string GetPlayerDetails(string playerID)
        {
            foreach (var player in PhotonNetwork.PlayerList)
            {
                if (player.UserId == playerID)
                {
                    VRRig vrrig = GorillaGameManager.instance.FindPlayerVRRig(player);
                    string cosmetics = vrrig.concatStringOfCosmeticsAllowed;
                    Color playerColor = vrrig.playerColor;
                    int r = (int)(playerColor.r * 255);
                    int g = (int)(playerColor.g * 255);
                    int b = (int)(playerColor.b * 255);

                    int cosmeticCount = cosmetics.Split(new char[] { '.' }, System.StringSplitOptions.RemoveEmptyEntries).Length;
                    bool hasSpecialCosmetic = false;

                    foreach (string id in specialCosmeticIds)
                    {
                        if (cosmetics.Contains(id))
                        {
                            hasSpecialCosmetic = true;
                            break;
                        }
                    }

                    return $"Usename: {player.NickName}\n" +
                           $"Player ID: {player.UserId}\n" +
                           $"Properties: {ToStringFull(player.CustomProperties)}\n" +
                           $"Cosmetics: {cosmetics}\n" +
                           $"Special Cosmetics: {(hasSpecialCosmetic ? "Yes" : "No")}\n" +
                           $"Cosmetic Amount: {cosmeticCount}\n" +
                           $"Color Code: {r},{g},{b}";
                }
            }
            return "Player details not found.";
        }

        private string ToStringFull(ExitGames.Client.Photon.Hashtable hashtable)
        {
            List<string> entries = new List<string>();
            foreach (System.Collections.DictionaryEntry entry in hashtable)
            {
                entries.Add($"{entry.Key}: {entry.Value}");
            }
            return string.Join(", ", entries);
        }

        private readonly string[] specialCosmeticIds = { "LBAAK", "LBADE" };
    }
}