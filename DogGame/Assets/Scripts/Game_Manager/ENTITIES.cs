using System;
using System.Linq;
using UnityEngine;

public class ENTITIES : MonoBehaviour
{
    public GameObject[] OBJ_NPCS;
    public GameObject[] OBJ_COLLECTIBLES;
    public bool[] COLLECTED;
    public NPC[] NPCS;
    public PICKUPS[] COLLECTIBLES;
    UI_HOTDOGMANAGER HDM;
    COINCOUNTER_UI CCUI;
    public int Money;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CCUI = GetComponentInChildren<COINCOUNTER_UI>();
        HDM = GetComponentInChildren<UI_HOTDOGMANAGER>();
        OBJ_NPCS = GameObject.FindGameObjectsWithTag("NPC");
        NPCS = new NPC[OBJ_NPCS.Length];

        OBJ_COLLECTIBLES = GameObject.FindGameObjectsWithTag("Pickup");
        COLLECTED = new bool[OBJ_COLLECTIBLES.Length];

       for (int i = 0; i < OBJ_NPCS.Length; i++)
        {
            NPCS[i] = OBJ_NPCS[i].GetComponent<NPC>();
        }

       for (int i = 0;i < OBJ_COLLECTIBLES.Length; i++)
        {
            COLLECTIBLES[i] = OBJ_COLLECTIBLES[i].GetComponent<PICKUPS>();
        }    
     
      
    }

    public void NotifyPickup(int id)
    {
        COLLECTED[id] = true;
       // HDM.CollectHotDog(id);
    }

    public void AddMoney(int amount)
    {
        Money += amount;
        CCUI.MoneyAdded(Money);
    }

    public NPC FindNPC(int id)
    {
        for (int i = 0;i < NPCS.Length;i++) 
        { 
        if (NPCS[i].id == id) return NPCS[i];
        }
        return null;
    }
   


    // Update is called once per frame
    void Update()
    {
        
    }
}
