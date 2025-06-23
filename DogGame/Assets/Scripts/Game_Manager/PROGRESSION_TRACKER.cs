using System;
using UnityEngine;
using UnityEngine.Events;

public class PROGRESSION_TRACKER : MonoBehaviour
{
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
        public void DoComplete()
        {

            if (!completed) { completed = true;

                Debug.Log("QUEST " + id + ": " + name + " Has Completed!");
                foreach (var qevent in  completeQuestEvents)
                {
                qevent.Invoke();
                }
                

            }
        }

        public void OnActivate()
        {
            if (!activated) { 

                Debug.Log("QUEST " + id + ": " + name + " Has Activated!");
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
