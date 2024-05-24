using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotsScript : MonoBehaviour
{
   private RoomManager roomManager;
    private SoundManager soundManager;

    [SerializeField] private GameObject EyeL;
    [SerializeField] private GameObject EyeC;
    [SerializeField] private GameObject EyeR;
    [SerializeField] private GameObject Joystick;

    private Animator LAnimator;
    private Animator CAnimator;
    private Animator RAnimator;
    private Animator JAnimator;

    private List<GameObject> eyes = new List<GameObject>();

    //int that holds the result of the slot - 1 for money, 2 for skull
    private int odds;
    private int winnings = 0;


    private int slotsCount = 0;


    private void Awake()
    {
        soundManager = FindObjectOfType<SoundManager>();
    }


    //coroutine when the slots are done 
    IEnumerator CSlotsFinished()
    {
        yield return new WaitForSeconds(1.5f);
        SlotsClose();
    }


    IEnumerator RunSlots(int odds)
    {
        yield return new WaitForSeconds(.5f);
        slotsCount++;
        IterateSlots(slotsCount, odds);
    }
    
    private void IterateSlots(int slotsCount, int odds)
    {
        switch (slotsCount)
        {
            case 1:
                LAnimator.SetBool("SlotPull", true);
                RollASlot();
                break;

            case 2:
                CAnimator.SetBool("SlotPull", true);
                RollASlot();
                break;

            case 3:
                RAnimator.SetBool("SlotPull", true);
                RollASlot();
                break;

            case 4:
                switch (odds)
                {
                    case 1:
                        LAnimator.SetBool("Money", true);
                        winnings++;
                        break;
                    case 2:
                        LAnimator.SetBool("Skull", true);
                        break;
                }
                RollASlot();
                break;

            case 5:
                switch (odds)
                {
                    case 1:
                        CAnimator.SetBool("Money", true);
                        winnings++;
                        break;
                    case 2:
                        CAnimator.SetBool("Skull", true);
                        break;
                }
                RollASlot();
                break;

            case 6:
                switch (odds)
                {
                    case 1:
                        RAnimator.SetBool("Money", true);
                        winnings++;
                        break;
                    case 2:
                        RAnimator.SetBool("Skull", true);
                        break;
                }
                StartCoroutine(CSlotsFinished());
                break;
        }
    }
    
    
    //Get a ref to the room manager to notify it when the slots are done with and the room transition can resume
    public void Initialise(RoomManager roomManRef)
    {
        roomManager = roomManRef;
    }

    public void PullSlots()
    {
        JAnimator.SetBool("Pull", true);
        soundManager.PlaySound("Roulette");
    }

    public void SlotsFinished()
    {
        roomManager.SlotsDone(winnings);
        slotsCount = 0;
        winnings = 0;
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


    //joystick has been pulled down, start spinning the slots
    public void StartSlots()
    {
        JAnimator.SetBool("End", true);
        RollASlot();

    }

    private void RollASlot()
    {
        odds = Random.Range(1, 3);
        StartCoroutine(RunSlots(odds));
    }


    private void SlotsClose()
    {
        LAnimator.SetBool("SlotDone", true);
        CAnimator.SetBool("SlotDone", true);
        RAnimator.SetBool("SlotDone", true);
    }



    // Start is called before the first frame update
    void Start()
    {
        LAnimator = EyeL.GetComponent<Animator>();
        eyes.Add(EyeL);

        CAnimator = EyeC.GetComponent<Animator>();
        eyes.Add(EyeC);

        RAnimator = EyeR.GetComponent<Animator>();
        eyes.Add(EyeR);

        JAnimator = Joystick.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
