using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private int comboCount = 0;
    private int combo = 0;
    private int score = 0;
    [SerializeField] GameObject HUDRef;
    private HUDScript HUD;

    //High Score stuff
    private int HighScore = 10000;

    public void ShowScore(bool destroyHUD)
    {
        HUD.UpdateHighScore(score, HighScore, destroyHUD);
    }


    public void UpdateUI(int scoreToAdd, bool updateCombo, bool addCombo, int playerLives)
    {

        if (updateCombo)
        {
            if (addCombo)
            {
                comboCount++;
                if (comboCount >= 3)
                {
                    combo++;
                    comboCount = 0;
                }
                    
            }
            else
            {
                score = (score * combo);
                combo = 0;
            }
        }
          

        if (scoreToAdd != 0)
        {
            score += scoreToAdd;
        }

        HUD.UpdateHUD(playerLives, score, combo);
        if (score > HighScore)
        {
            HighScore = score;
        }
        
    }


    public void ResetHearts()
    {

    }

    public void ResetComboCount()
    {
        comboCount = 0;
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
