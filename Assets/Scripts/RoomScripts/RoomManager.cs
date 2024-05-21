using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class RoomManager : MonoBehaviour
{
    //make an array of all the room layouts to spawn in and out
    //Get a reference to the current room
    public GameObject[] roomPrefabs;
    private RoomScript currentRoom;

    // Player/Game mode variables
    private int playerLives = 3;
    private Vector2 inputVector;
    private int slotsCount = 0;
    private bool slotsTime = false;
    public GameObject UIManager;

    //ref to the slots manager so that we know when they finish up to transition away
    [SerializeField] GameObject slotsManager;
    private SlotsScript slots;
    //bool so that the slots only take the input when appropriate
    public bool slotReadyToPull = false;


    private UIManager UIRef;

    //reference to animator and sprite renderer
    private SpriteRenderer spriteRen;
    private Animator animator;

    //variable to see whether we're in the menus / attract mode or in the actual game loop
    private bool gameTime = false;
    private bool startingRoom = true;

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
    private int difficultyValue = 2;




    //Debug coroutine for pausing player
    //private bool playerPaused;
    IEnumerator CDebugPause()
    {
        //playerPaused = true;
        playerScript.MumSaysNo();
        yield return new WaitForSeconds(3);
        playerScript.MumLetMePlay();
        //playerPaused = false;
    }

    //-----------------------------------------------------------------------------------------------------------------------------------------------
    //Input events
    //-----------------------------------------------------------------------------------------------------------------------------------------------
    
    public void OnMove(InputAction.CallbackContext context)
    {
        inputVector = context.ReadValue<Vector2>();
        float inputY = Mathf.RoundToInt(inputVector.y);
        if (slotReadyToPull)
        {
            if (inputY == -1)
            {
                slots.PullSlots();
                slotReadyToPull = false;
            }
        }
    }

    public void SlotReadyToPull()
    {
        slotReadyToPull = true;
    }


    //-----------------------------------------------------------------------------------------------------------------------------------------------
    // Start is called before the first frame update
    //-----------------------------------------------------------------------------------------------------------------------------------------------
    void Start()
    {
        
        UIRef = UIManager.GetComponent<UIManager>();
        spriteRen = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        slots = slotsManager.GetComponent<SlotsScript>();
        slots.Initialise(this);

        //DELETE AFTER IMPLEMENTING MENU
        StartGame();
        
    }

    //start the game
    private void StartGame()
    {
        gameTime = true;
        animator.SetBool("GameTime", true);
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
        //if the player has made it through three rooms without dying, they get to spin the slots
        if (!firstRoom)
        {
            slotsCount++;
            if (slotsCount >= 1)
            {
                slotsTime = true;
                slotsCount = 0;
                gameTime = false;
                animator.SetBool("GameTime", false);
            }
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
            slotsCount = 0;
            if (currentRoom != null)
            {
                currentRoom.ClearEnemies();
            }
            spawnInt = 0;
            PlayTransition(5);
        }

        startingRoom = firstRoom;
        if (currentRoom != null)
        {
            currentRoom.PauseEnemies();
            playerScript.MumSaysNo();
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
        startingRoom = false;
        //spawn the player
        SpawnPlayer();
        
    }


    


    public void ExitReached(int exit)
    {
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


    //function to play a room transition
    public void PlayTransition(int CardinalDirection)
    {
        switch (CardinalDirection)
        {
            case 1:
                //play up animation
                animator.SetInteger("Direction", 1);
                break;
            case 2:
                //play right animation
                animator.SetInteger("Direction", 2);
                break;
            case 3:
                //play down animation
                animator.SetInteger("Direction", 3);
                break;
            case 4:
                //play left animation
                animator.SetInteger("Direction", 4);
                break;
            case 5:
                //play centre transition
                animator.SetInteger("Direction", 5);
                break;
        }
    }


    //Function triggers when the inverse transition anim is finished- meaning its time to enable all the enemies and player
    public void TransitionDone()
    {
        if (gameTime)
        {
            playerScript.MumLetMePlay();
            if (currentRoom != null)
            {
                currentRoom.ResumeEnemies();
            }

           
        }
    }

    //Make the room swap happen AFTER the transition starts so that it isnt jarring
    public void TransitionMidway()
    {
        if (gameTime)
        {
            //destroy the player so they can't do whatever while the screen is transitioning
            if (player != null)
            {
                Destroy(player);
            }

            //Set the room index to be the starting room, then create the room
            if (startingRoom)
            {
                NewRoom(0, startingRoom);
            }
            else
            {
                //IMPORTANT - SWITCH THIS OUT FOR RANDOM ROOM FUCNTION WHEN ITS MADE
                difficultyValue++;
                NewRoom(1, startingRoom);
            }
        }
        else
        {
            if (slotsTime)
            {
                //Start the slots
                StartSlots();
            }
        }
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

    //method called when player dies ---------------------------------------------------------------------------------------------------------IMPORTANT
    public void PlayerHit()
    {
        playerLives--;
        UIRef.UpdateUI(0, true, false, playerLives);
    }





    //Triggered after the player's death animation is done playing
    public void Death()
    {
        if (playerLives <= 0)
        {
            GameOver();
            ResetLevel();
        }
        else
        {
            ResetLevel();
        }
    }

    


    private void ResetLevel()
    {
        difficultyValue = 2;
        TransitionRoom(true);
    }


















    //-----------------------------------------------------------------------------------------------------------------------------------
    //-----------------------------------------------------------------------------------------------------------------------------------
    // GAME SCREENS, ATTRACT MODE AND MENU LOGIC PAST THIS POINT
    //-----------------------------------------------------------------------------------------------------------------------------------
    //-----------------------------------------------------------------------------------------------------------------------------------

    public void GameOver()
    {

    }








    //-----------------------------------------------------------------------------------------------------------------------------------
    //SLOT MACHINE LOGIC ----------------------------------------------------------------------------------------------------------------
    //-----------------------------------------------------------------------------------------------------------------------------------

    //Start the slots
    private void StartSlots()
    {
        slots.BeginSlots();
    }

    public void SlotsDone(int score)
    {
        animator.SetBool("SlotsDone", true);
    }

}
