using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    //make an array of all the room layouts to spawn in and out
    //Get a reference to the current room
    public GameObject[] roomPrefabs;
    private RoomScript currentRoom;

    // Player/Game mode variables
    private int playerLives;
    public GameObject UIManager;
    private UIManager UIRef;

    //variable to see whether we're in the menus / attract mode or in the actual game loop
    private bool gameTime = false;

    //the last exit the player touched
    private RoomScript.RoomExits lastExit;

    //the exit to seal behind the player
    private RoomScript.RoomExits exitToSeal;

    //Set the points the player can be spawned at
    public GameObject[] spawnPoints;
    private int spawnInt;

    //player reference to spawn them in
    public GameObject playerPrefab;
    private GameObject player;
    private PlayerMovement playerScript;

    //Increase the difficulty value each time you enter a new room. 
    private int difficultyValue = 1;

    //-----------------------------------------------------------------------------------------------------------------------------------------------
    // Start is called before the first frame update
    //-----------------------------------------------------------------------------------------------------------------------------------------------
    void Start()
    {
        
        UIRef = UIManager.GetComponent<UIManager>();
        
        
    }

    //start the game
    private void StartGame()
    {
        gameTime = true;
        spawnInt = 0;

        TransitionRoom(true);
    }

    //Spawns the player. duh
   private void SpawnPlayer()
    {
        player = Instantiate(playerPrefab, spawnPoints[spawnInt].transform.position, Quaternion.identity);
        playerScript = player.GetComponent<PlayerMovement>();
        playerScript.GetManagerRef(this);
        currentRoom.Initialise(this, player.transform, difficultyValue);
    }



    //tidy little function to organise the order that we have to run all the OTHER functions - create room, then get rid of player, then animation, then spawn player in correct spot
    private void TransitionRoom(bool firstRoom)
    {
        //Play the transition anim
        
        if (!firstRoom)
        {
            switch (lastExit)
            {
                case RoomScript.RoomExits.up:
                    //play up exit anim
                    PlayTransition(1);
                    break;

                case RoomScript.RoomExits.right:
                    //play right exit anim
                    PlayTransition(2);
                    break;

                case RoomScript.RoomExits.down:
                    //play down exit anim
                    PlayTransition(3);
                    break;

                case RoomScript.RoomExits.left:
                    //play left exit anim
                    PlayTransition(4);
                    break;
            }
        }
        else
        {
            PlayTransition(5);
        }




        //destroy the player so they can't do whatever while the screen is transitioning
        if (player != null)
        {
            Destroy(player);
        }

        //Set the room index to be the starting room, then create the room
        if (firstRoom)
        {
            NewRoom(0, firstRoom);
        }
        else
        {
            //IMPORTANT - SWITCH THIS OUT FOR RANDOM ROOM FUCNTION WHEN ITS MADE
            difficultyValue++;
            NewRoom(1, firstRoom);
        }
    }

    
    
    //Function to create a new room ---------------------------------------------------------------------------------------------
    private void NewRoom(int roomIndex, bool firstRoom)
    {
        //Destroy the current room
        if (currentRoom != null)
        {
            Destroy(currentRoom.gameObject);
        }

        //Instantiate the room prefab
        GameObject roomPrefab = roomPrefabs[roomIndex];
        GameObject roomPrefabInstance = Instantiate(roomPrefab, Vector3.zero, Quaternion.identity);

        //Set the current room as the script component for the room prefab, letting us have access to the 
        currentRoom = roomPrefabInstance.GetComponent<RoomScript>();

        //seal the exit behind the player
        if (!firstRoom)
        {
            currentRoom.SealExits(exitToSeal);
        }

        //spawn the player
        SpawnPlayer();
    }





    public void ExitReached(int exit, bool addCombo)
    {
        //if the player didnt clear all the enemies last room, clear their combo when entering the new one
        if (!addCombo)
        {
            UpdateCombo(false);
        }

        switch (exit)
        {
            case 1:
                lastExit = RoomScript.RoomExits.up;
                exitToSeal = RoomScript.RoomExits.down;
                spawnInt = 3;
                break;

            case 2:
                lastExit = RoomScript.RoomExits.right;
                exitToSeal = RoomScript.RoomExits.left;
                spawnInt = 4;
                break;

            case 3:
                lastExit = RoomScript.RoomExits.down;
                exitToSeal = RoomScript.RoomExits.up;
                spawnInt = 1;
                break;

            case 4:
                lastExit = RoomScript.RoomExits.left;
                exitToSeal = RoomScript.RoomExits.right;
                spawnInt = 2;
                break;
        }

        TransitionRoom(false);
    }

    //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //METHODS FOR UPDATING UI
    //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    
    //MetHod called when the player kills every enemy on the screen or just generally increases their combo
    public void UpdateCombo(bool addCombo)
    {
        UIRef.UpdateUI(0, true, addCombo, playerLives);
    }

    //Method called when the player gains score (through either killing an enemy or getting a pickup)
    public void UpdateScore(int scoreGained)
    {
        UIRef.UpdateUI(scoreGained, false, false, playerLives);
    }


    //function to play a room transition
    public void PlayTransition(int CardinalDirection)
    {
        switch (CardinalDirection)
        {
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
            case 5:
                //play centre transition
                break;
        }
    }

    //Function triggers when the inverse animation starts
    public void SpawnEnemiesInTransit()
    {
        if (gameTime)
        {
            currentRoom.SpawnEnemies();
        }
            
    }

    //Function triggers when the inverse transition anim is finished- meaning its time to enable all the enemies and player
    public void TransitionDone()
    {
        if (gameTime)
        {
            playerScript.MumLetMePlay();
            
        }
    }

}
