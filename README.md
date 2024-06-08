Photon Data Tracker Advanced
Photon Data Tracker Advanced is a mod designed for use with Photon Unity Networking (PUN) in Gorilla Tag, aimed at tracking and logging player data and cosmetics in multiplayer environments.

Features
Player Data Tracking: Logs player information including username, player ID, custom properties, and cosmetics.
Special Cosmetics Flagging: Identifies players with specific special cosmetic items.
Cosmetic Counting: Calculates and logs the number of cosmetics each player has.
Color Code Logging: Includes RGB color code for each player's avatar.
Persistent Logging: Appends player data across different game sessions without overwriting existing data.
How to Use
Setup:

Import the mod into your Gorilla Tag project.
Ensure Photon PUN is set up and configured in your project.
Integration:

Attach the PunCallbacks script to a GameObject in your scene.
Ensure the GorillaGameManager is correctly set up to access player data.
Logging:

Player data is logged automatically when players join a room or when new player data is detected.
Data Storage:

Player information is stored in the DataLogger directory.
Detailed player data is stored in separate files within the Rooms directory.
Player IDs and usernames are logged in the player.IDs file.
Example
Here's an example of what the logged data might look like:

yaml
Copy code
Usename: PlayerName
Player ID: ABC123
Properties: { customProperty1 = value, customProperty2 = value }
Cosmetics: ITEM1.ITEM2.ITEM3
Special Cosmetics: No
Cosmetic Amount: 3
Color Code: 255,128,0
License
This mod is licensed under the MIT License - see the LICENSE file for details.

# **Acknowledgments**
Photon Unity Networking (PUN): Multiplayer game framework for Gorilla Tag.
Gorilla Tag Game: Multiplayer game based on gorilla parkour.
This is a spin off of a different mod called data logger the person that made this mod does not have a GitHub or I would’ve linked it this is just a more upgraded version of it. It gives you more data. Happy moding
