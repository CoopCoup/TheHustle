using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    private int combo = 0;
    private int score = 0;
    
    public void UpdateUI(int scoreToAdd, bool updateCombo, bool addCombo, int playerHearts)
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
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
