using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotsScript : MonoBehaviour
{
   private RoomManager roomManager;

    [SerializeField] private GameObject EyeL;
    [SerializeField] private GameObject EyeC;
    [SerializeField] private GameObject EyeR;
    [SerializeField] private GameObject Joystick;

    private Animator LAnimator;
    private Animator CAnimator;
    private Animator RAnimator;
    private Animator JAnimator;

    private int odds;

    private int slotsCount = 0;


   IEnumerator SlotsDelay()
    {
        yield return new WaitForSeconds(1);
    }
    
    
    
    
    //Get a ref to the room manager to notify it when the slots are done with and the room transition can resume
    public void Initialise(RoomManager roomManRef)
    {
        roomManager = roomManRef;
    }

    public void PullSlots()
    {
        JAnimator.SetBool("Pull", true);
    }

    public void SlotsFinished()
    {
        roomManager.SlotsDone(3);
    }

    public void BeginSlots()
    {
        LAnimator.SetBool("SlotStart", true);
        CAnimator.SetBool("SlotStart", true);
        RAnimator.SetBool("SlotStart", true);
        JAnimator.SetBool("Start", true);
    }

    public void JoystickReady()
    {
        roomManager.SlotReadyToPull();
    }


    // Start is called before the first frame update
    void Start()
    {
        LAnimator = EyeL.GetComponent<Animator>();
        CAnimator = EyeC.GetComponent<Animator>();
        RAnimator = EyeR.GetComponent<Animator>();
        JAnimator = Joystick.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
