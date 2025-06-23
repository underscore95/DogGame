using UnityEngine;

public class QUEST_REFERENCER : MonoBehaviour
{
    public GameObject GM;
    public QUEST_EVENTS QEVENTS;
    public PROGRESSION_TRACKER QUESTS;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GM = GameObject.Find("Game_Manager");
        QEVENTS = GM.GetComponent<QUEST_EVENTS>();
        QUESTS = GM.GetComponent<PROGRESSION_TRACKER>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
