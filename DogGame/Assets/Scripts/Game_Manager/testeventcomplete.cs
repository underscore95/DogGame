using UnityEngine;

public class testeventcomplete : MonoBehaviour
{
    QUEST_REFERENCER qr;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        qr = GetComponent<QUEST_REFERENCER>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (!qr.QUESTS.Game_Quests[0].completed)
        {
            qr.QUESTS.Game_Quests[0].DoComplete();
        }
    }
}
