using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeEnemyScript : MonoBehaviour
{

    private Transform player;
    private RoomScript room;
    private bool immaGetcha;
    [SerializeField] float speed;
    private BoxCollider2D coll;

    // Start is called before the first frame update
    void Start()
    {
        coll = GetComponent<BoxCollider2D>();
    }

    IEnumerator CStartMoveDelay()
    {
        yield return new WaitForSeconds(1);
        immaGetcha = true;
        coll.enabled = true;
    }
    public void Initialise(Transform playerRef, RoomScript roomRef)
    {
        player = playerRef;
        room = roomRef;
    }

    public void StartHunting()
    {
        StartCoroutine(CStartMoveDelay());
        room.EyeReady();
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            IColliders i = other.GetComponent<IColliders>();
            if (i != null)
            {
                i.Hit();
                Destroy(gameObject);
            }
        }
    }







    // Update is called once per frame
    void Update()
    {
        if (immaGetcha)
        {
            if (player != null)
            {
                Vector3 direction = (player.position - transform.position).normalized;
                Vector3 newPosition = transform.position + direction * speed * Time.deltaTime;
                transform.position = newPosition;
            }
        }
    }
}
