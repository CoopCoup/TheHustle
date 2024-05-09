using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    //make an array of all the room layouts to spawn in and out
    //Get a reference to the current room
    public GameObject[] roomPrefabs;
    private RoomScript currentRoom;

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





    //-----------------------------------------------------------------------------------------------------------------------------------------------
    // Start is called before the first frame update
    //-----------------------------------------------------------------------------------------------------------------------------------------------
    void Start()
    {
        spawnInt = 0;

        TransitionRoom(true);
        
    }



    //Spawns the player. duh
   private void SpawnPlayer()
    {
        player = Instantiate(playerPrefab, spawnPoints[spawnInt].transform.position, Quaternion.identity);
        currentRoom.Initialise(this, player.transform);
    }



    //tidy little function to organise the order that we have to run all the OTHER functions - create room, then get rid of player, then animation, then spawn player in correct spot
    private void TransitionRoom(bool firstRoom)
    {
        //Set the room index to be the starting room, then create the room
        if (firstRoom)
        {
            NewRoom(0, firstRoom);
        }
        else
        {
            //IMPORTANT - SWITCH THIS OUT FOR RANDOM ROOM FUCNTION WHEN ITS MADE
            NewRoom(1, firstRoom);
        }

        //play the transition animation and spawn the player
        StartCoroutine(CTransitionAnim(firstRoom));
    }
    

    //function to get the random room prefab index will go here
    //a random range between 1 and the length of the room prefab array to prevent the starting room from popping up 
    //private int GetRandomRoom()

    //Function to create a new room
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

        //Give the current Room prefab a reference to the room manager so that they can communicate
        if (currentRoom != null)
        {
            
        }
        else
        {
            //If this returns null there's been an error and either the room prefab doesn't exist or it doesn't have a room script on it
            Debug.Log("No script found on this room");
        }

        if (!firstRoom)
        {
            currentRoom.SealExits(exitToSeal);
        }
    }





    public void ExitReached(int exit)
    {   
        switch (exit)
        {
            case 1:
                Debug.Log("Up Exit Reached");
                lastExit = RoomScript.RoomExits.up;
                exitToSeal = RoomScript.RoomExits.down;
                spawnInt = 3;
                break;

            case 2:
                Debug.Log("Right Exit Reached");
                lastExit = RoomScript.RoomExits.right;
                exitToSeal = RoomScript.RoomExits.left;
                spawnInt = 4;
                break;

            case 3:
                Debug.Log("Down Exit Reached");
                lastExit = RoomScript.RoomExits.down;
                exitToSeal = RoomScript.RoomExits.up;
                spawnInt = 1;
                break;

            case 4:
                Debug.Log("Left Exit Reached");
                lastExit = RoomScript.RoomExits.left;
                exitToSeal = RoomScript.RoomExits.right;
                spawnInt = 2;
                break;
        }

        TransitionRoom(false);
    }

    
    
    
    //coroutine that destroys the player so no funny business can go down while the transit is happening, plays the transition, then spawns the player in the correct spot
    IEnumerator CTransitionAnim(bool firstRoom)
    {
       if (player != null)
        {
            Destroy(player);
        }

       if (!firstRoom)
        {
            switch (lastExit)
            {
                case RoomScript.RoomExits.up:
                    //play up exit anim
                    break;

                case RoomScript.RoomExits.right:
                    //play right exit anim
                    break;

                case RoomScript.RoomExits.down:
                    //play down exit anim
                    break;

                case RoomScript.RoomExits.left:
                    //play left exit anim
                    break;
            }
        }
        else
        {
            //play opening anim
        }

        yield return new WaitForSeconds(1);
        SpawnPlayer();
        
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
