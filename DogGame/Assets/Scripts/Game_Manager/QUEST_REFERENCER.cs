using UnityEngine;

public class QUEST_REFERENCER : MonoBehaviour
{
    [HideInInspector]
    public GameObject GM;
    [HideInInspector]
    public QUEST_EVENTS QEVENTS;
    [HideInInspector]
    public PROGRESSION_TRACKER QUESTS;
    [HideInInspector]
    public ENTITIES EN;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GM = GameObject.Find("Game_Manager");
        QEVENTS = GM.GetComponent<QUEST_EVENTS>();
        QUESTS = GM.GetComponent<PROGRESSION_TRACKER>();
        EN = GM.GetComponent<ENTITIES>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
