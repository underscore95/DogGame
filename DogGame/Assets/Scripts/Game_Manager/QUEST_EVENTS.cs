using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QUEST_EVENTS : MonoBehaviour
{
    PROGRESSION_TRACKER PT;
    ENTITIES ENT;
    GameObject CS_OBJ;
    PLAYER_CAMSTATEMACHINE CS;
    WORLD_EVENTS WE;
    bool endGame;
    float secondsTilGameFinish;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CS_OBJ = GameObject.Find("CameraSystem");
        WE = GetComponent<WORLD_EVENTS>();
        CS = CS_OBJ.GetComponent<PLAYER_CAMSTATEMACHINE>();
        PT = GetComponent<PROGRESSION_TRACKER>();
        ENT = GetComponent<ENTITIES>();
    }

    // Update is called once per frame
    void Update()
    {



    }
    public void ActivateNPCAndAddQuest(int npcId, int questID)
    {
        NPC _npc = ENT.FindNPC(npcId);
        _npc._activated = true;
        _npc._hasTraded = false;
        _npc.AttachQuest(questID);
    }

    public void BeginGoToBar()
    {
        ActivateNPCAndAddQuest(0, 0);
    }



    public void EndGoToBar() 
    {
        PT.Game_Quests[1].OnActivate();
    }

    public void BeginGoToShirtMan()
    {
        ActivateNPCAndAddQuest(1, 1);


    }

    public void EndGoToShirtMan()
    {
        PT.Game_Quests[2].OnActivate();
    }

    public void BeginGoToSunglassesLady()
    {
        ActivateNPCAndAddQuest(2, 2);

    }

    public void EndGoToSunglassesLady()
    {
        PT.Game_Quests[3].OnActivate();
    }

    public void BeginGoToMuscleGuy()
    {
        ActivateNPCAndAddQuest(3, 3);

    }

    public void EndGoToMuscleGuy()
    {
        PT.Game_Quests[4].OnActivate();

    }

    public void BeginGoToFlowerLady()
    {
        ActivateNPCAndAddQuest(4, 4 );

    }

    public void EndGoToFlowerLady()
    {
        PT.Game_Quests[5].OnActivate();


    }

    public void BeginGetTheBeer()
    {
        CS.StartCutscene(4f, WE.Bridge, Camera.main.transform);
        ActivateNPCAndAddQuest(0, 5);
    }

    public void EndGetTheBeer()
    {
        Debug.Log("GAME IS FINISHED");
    }

    public void EndGame()
    {
        endGame = true;

    }
}
