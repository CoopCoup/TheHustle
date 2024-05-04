using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ExitScript : MonoBehaviour
{
    private GameObject thisExit;
    public GameObject roomPrefab;
    private RoomScript roomPrefabScript;
    
    // Start is called before the first frame update
    void Start()
    {
        thisExit = this.GameObject();
        roomPrefabScript = roomPrefab.GetComponent<RoomScript>();
    }

    //Trigger Event for when the player touches the exit
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            roomPrefabScript.ExitCollided(thisExit);
        }
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
