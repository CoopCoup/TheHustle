using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private int comboCount = 0;
    private SoundManager soundManager;
    private int combo = 1;
    private int score = 0;
    [SerializeField] GameObject HUDRef;
    private HUDScript HUD;

    //High Score stuff
    private int HighScore = 10000;



    private void Awake()
    {
        soundManager = FindObjectOfType<SoundManager>();
    }




    public void ShowScore(bool destroyHUD)
    {
        HUD.UpdateHighScore(score, HighScore, destroyHUD);
        WipeScore();
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
                combo = 1;
            }
        }
          

        if (scoreToAdd != 0)
        {
            score += scoreToAdd;
            soundManager.PlaySound("Cash");
        }

        HUD.UpdateHUD(playerLives, score, combo);
        if (score > HighScore)
        {
            HighScore = score;
        }
        
    }

    public void WipeScore()
    {
        score = 0;
        combo = 1;
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
