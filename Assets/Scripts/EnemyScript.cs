using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EnemyScript : MonoBehaviour, IColliders
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    //Implement Interface function
    public void Hit()
    {
        Debug.Log("Enemy Hit!");
    }

    private void FixedUpdate()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
