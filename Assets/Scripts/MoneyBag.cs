using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyBag : MonoBehaviour, IColliders
{
    // Start is called before the first frame update
    void Start()
    {
        
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
