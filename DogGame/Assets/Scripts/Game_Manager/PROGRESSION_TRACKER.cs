using System;
using UnityEngine;
using UnityEngine.Events;

public class PROGRESSION_TRACKER : MonoBehaviour
{
    public GameObject QuestPopupUI;
    UI_QUESTPOPUP QUESTPOPUP;
    ENTITIES en;
    public float requiredWinMoney;

    

   

    [System.Serializable]
    public struct Game_Quest
    {
        public int id;
        public string name;
        public bool activateOnStart;
        public bool activated;
        public bool completed;
        public UnityEvent[] activateQuestEvents;
        public UnityEvent[] completeQuestEvents;
        UI_QUESTPOPUP QPOPUP;
        


        public void DoComplete()
        {

            if (!completed) { completed = true;

               // AddQuestUI("QUEST " + id + ": " + name + " Has Completed!", false);
                foreach (var qevent in  completeQuestEvents)
                {
                    


                    qevent.Invoke();
                   
                }
                

            }
        }

        public void AddQuestUI(string text, bool activate)
        {
           
        }

        public void OnActivate()
        {
            if (!activated) {

                AddQuestUI("QUEST " + id + ": " + name + " Has Activated!", true);
                activated = true;
                foreach (var qevent in activateQuestEvents)
                {

                    qevent.Invoke();
                }
            }
            
        }
    }
    public Game_Quest[] Game_Quests;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        ActivateEvents();
    }

    public void AddQuestPopup()
    {

    }

    void ActivateEvents()
    {
        for (int i = 0; i < Game_Quests.Length; i++)
        {
            if (Game_Quests[i].activateOnStart)
            {
                Game_Quests[i].OnActivate();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RUNTHIS()
    {
        Debug.Log("RAN");
    }
}
