using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    private int combo = 0;
    private int score = 0;
    [SerializeField] GameObject HUDRef;
    private HUDScript HUD;

    
    public void UpdateUI(int scoreToAdd, bool updateCombo, bool addCombo, int playerLives)
    {

        if (updateCombo)
        {
            if (addCombo)
            {
                combo++;
                Debug.Log(combo);
            }
            else
            {
                combo--;
            }
        }
          

        if (scoreToAdd != 0)
        {
            score += scoreToAdd;
            Debug.Log(score);
        }

        HUD.UpdateHUD(playerLives, score, combo);
    }


    public void ResetHearts()
    {

    }


    // Start is called before the first frame update
    void Start()
    {
        HUD = HUDRef.GetComponent<HUDScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
