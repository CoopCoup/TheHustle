using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomScript : MonoBehaviour
{
    private RoomManager roomManager;
    private Transform playerRef;
    public GameObject enemyPrefab;
    public GameObject leftExit;
    public GameObject rightExit;
    public GameObject upExit;
    public GameObject downExit;

    private int enemiesKilled = 0;


    public GameObject leftExitSeal;
    public GameObject rightExitSeal;
    public GameObject upExitSeal;
    public GameObject downExitSeal;

    [SerializeField] private GameObject[] enemySpawns;
    private List<GameObject> enemyInstances = new List<GameObject>();
    private int difficultyInt;
    private int difficulty;

    private int exitInt;
    
    //Create an enum for the different exits so that the room manager has an easier time communicating which exit to seal off (or vice versa)
    public RoomExits roomExits;
    public enum RoomExits
    {
        up, down, left, right
    }

    // Start is called before the first frame update
    void Start()
    {
    
    }

    //Function to get a reference to the room manager to set up a communication, I'm fine with these being dependent on one another as the two have completely shared functionalities
    public void Initialise(RoomManager manager, Transform player, int difficultyValue)
    {
        difficultyInt = difficultyValue;
        difficulty = difficultyValue;
        roomManager = manager;
        playerRef = player;
        SpawnEnemies();
        PauseEnemies();
    }

    //Spawn the enemies after the player is spawned
    public void SpawnEnemies()
    {
        foreach (GameObject enemySpawn in enemySpawns)
        {
            if (difficultyInt > 0)
            {
                GameObject enemyInstance = Instantiate(enemyPrefab, enemySpawn.transform.position, Quaternion.identity);
                EnemyScript enemyScript = enemyInstance.GetComponent<EnemyScript>();
                enemyScript.Initialise(playerRef, difficulty, this);
                enemyInstances.Add(enemyInstance);
                difficultyInt--;
            }

            
        }
    }
    
    public void EnemyDeath(int enemyScoreValue)
    {
        roomManager.UpdateScore(enemyScoreValue);
        enemiesKilled++;
        roomManager.UpdateCombo(true);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Function called by the room manager to seal the appropriate doors in the room
    public void SealExits(Enum RoomExits)
    {
        rightExitSeal.SetActive(false);
        leftExitSeal.SetActive(false);
        upExitSeal.SetActive(false);
        downExitSeal.SetActive(false);

        switch (RoomExits)
        {
            case RoomScript.RoomExits.right:
            rightExitSeal.SetActive(true); break;

            case RoomScript.RoomExits.left:
                leftExitSeal.SetActive(true); break;

            case RoomScript.RoomExits.up:
                upExitSeal.SetActive(true); break;

            case RoomScript.RoomExits.down:
                downExitSeal.SetActive(true); break;
        }
    }

    //function to pause all enemies
    public void PauseEnemies()
    {
        foreach (GameObject enemyInstance in enemyInstances)
        {
            if (enemyInstance != null)
            {
                EnemyScript enemyScript = enemyInstance.GetComponent<EnemyScript>();
                enemyScript.PauseEnemy();
            }

        }
    }

    //Resume Enemies
    public void ResumeEnemies()
    {
        foreach (GameObject enemyInstance in enemyInstances)
        {
            if (enemyInstance != null)
            {
                EnemyScript enemyScript = enemyInstance.GetComponent<EnemyScript>();
                enemyScript.ResumeEnemy();
            }

        }
    }

    //Function for when one of the exits collides with the player
    public void ExitCollided(GameObject exit)
    {
        ClearEnemies();   
        
        //Destroy eye enemy

        if (exit == leftExit)
        {
            exitInt = 4;
            roomManager.ExitReached(exitInt);
        }

        if (exit == rightExit)
        {
            exitInt = 2;
            roomManager.ExitReached(exitInt);
        }

        if (exit == upExit)
        {
            exitInt = 1;
            roomManager.ExitReached(exitInt);
        }

        if (exit == downExit)
        {
            exitInt = 3;
            roomManager.ExitReached(exitInt);
        }

        
    }
    
    //clear all enemies from the room
    public void ClearEnemies()
    {
        foreach (GameObject enemyInstance in enemyInstances)
        {
            Destroy(enemyInstance);
        }
    }
}