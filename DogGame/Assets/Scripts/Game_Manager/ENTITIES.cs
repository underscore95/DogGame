using System;
using System.Linq;
using UnityEngine;

public class ENTITIES : MonoBehaviour
{
    public GameObject[] OBJ_NPCS;
    public NPC[] NPCS;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        OBJ_NPCS = GameObject.FindGameObjectsWithTag("NPC");
        NPCS = new NPC[OBJ_NPCS.Length];

       for (int i = 0; i < OBJ_NPCS.Length; i++)
        {
            NPCS[i] = OBJ_NPCS[i].GetComponent<NPC>();
        }
     
      
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
