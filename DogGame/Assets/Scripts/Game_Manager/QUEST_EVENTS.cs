using UnityEngine;

public class QUEST_EVENTS : MonoBehaviour
{
    PROGRESSION_TRACKER PT;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PT = GetComponent<PROGRESSION_TRACKER>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BeginGoToBar()
    {
    }

    public void EndGoToBar() 
    {
        PT.Game_Quests[1].OnActivate();
    }

    public void BeginGoToShirtMan()
    {

    }

    public void EndGoToShirtMan()
    {
       // PT.Game_Quests[2].OnActivate();
    }
}
