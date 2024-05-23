using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Unity.Burst.Intrinsics;
using UnityEngine.SocialPlatforms.Impl;

public class HUDScript : MonoBehaviour
{
    [SerializeField] private GameObject heart1;
    [SerializeField] private GameObject heart2;
    [SerializeField] private GameObject heart3;

    [SerializeField] private TMPro.TextMeshProUGUI scoreCombo;
    [SerializeField] private TMPro.TextMeshProUGUI playerScore;
    [SerializeField] private TMPro.TextMeshProUGUI highScore;

    // Start is called before the first frame update
    void Start()
    {
        scoreCombo.text = "0  X0";
    }


    public void UpdateHUD(int lives, int score, int combo)
    {
        switch (lives)
        {
            case 0:
                SpriteRenderer heart1Ren = heart1.GetComponent<SpriteRenderer>();
                heart1Ren.enabled = false;
                SpriteRenderer heart2Ren = heart2.GetComponent<SpriteRenderer>();
                heart2Ren.enabled = false;
                SpriteRenderer heart3Ren = heart3.GetComponent<SpriteRenderer>();
                heart3Ren.enabled = false;
                break;

            case 1:
                heart1Ren = heart1.GetComponent<SpriteRenderer>();
                heart1Ren.enabled = true;
                heart2Ren = heart2.GetComponent<SpriteRenderer>();
                heart2Ren.enabled = false;
                heart3Ren = heart3.GetComponent<SpriteRenderer>();
                heart3Ren.enabled = false;
                break;

            case 2:
                heart1Ren = heart1.GetComponent<SpriteRenderer>();
                heart1Ren.enabled = true;
                heart2Ren = heart2.GetComponent<SpriteRenderer>();
                heart2Ren.enabled = true;
                heart3Ren = heart3.GetComponent<SpriteRenderer>();
                heart3Ren.enabled = false;
                break;

            case 3:
                heart1Ren = heart1.GetComponent<SpriteRenderer>();
                heart1Ren.enabled = true;
                heart2Ren = heart2.GetComponent<SpriteRenderer>();
                heart2Ren.enabled = true;
                heart3Ren = heart3.GetComponent<SpriteRenderer>();
                heart3Ren.enabled = true;
                break;
        }
        
        scoreCombo.text = score.ToString() + "  X" + combo.ToString();
    }

    public void UpdateHighScore(int playerScoreRef, int highScoreRef, bool destroy)
    {
        if (destroy)
        {
            playerScore.text = "";
            highScore.text = "";
        }
        else
        {
            playerScore.text = "$" + playerScoreRef.ToString();
            highScore.text = "$" + highScoreRef.ToString();
        }
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
