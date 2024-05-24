using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyBag : MonoBehaviour, IColliders
{
    
    private SoundManager soundManager;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        soundManager = FindObjectOfType<SoundManager>();
    }

    public void Hit()
    {
        Destroy(gameObject);
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
