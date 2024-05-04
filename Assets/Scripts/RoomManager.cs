using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    //make an array of all the room layouts to spawn in and out
    //Get a reference to the current room
    public GameObject[] roomPrefabs;
    private RoomScript currentRoom;
    
    
    // Start is called before the first frame update
    void Start()
    {
        //Instantiate the starting room prefab
        GameObject roomPrefab = roomPrefabs[0];
        GameObject roomPrefabInstance = Instantiate(roomPrefab, Vector3.zero, Quaternion.identity);

        //Set the current room as the script component for the room prefab, letting us have access to the 
        currentRoom = roomPrefabInstance.GetComponent<RoomScript>();

        //Give the current Room prefab a reference to the room manager so that they can communicate
        if (currentRoom != null)
        {
            currentRoom.Initialise(this);
        }
        else
        {
            //If this returns null there's been an error and either the room prefab doesn't exist or it doesn't have a room script on it
            Debug.Log("No script found on this room");
        }

        //currentRoom.SealExits(RoomScript.RoomExits.right);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
