using System.Collections;
using UnityEngine;

public class UI_HOTDOGMANAGER : MonoBehaviour
{
    public UI_HOTDOGITEM[] HOTDOGS;
    public UI_FX[] HOTDOGFX;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HOTDOGFX = new UI_FX[HOTDOGS.Length];
        GetUIFX();
        BeginEntrance();
    }

    void GetUIFX()
    {
        for (int i = 0; i < HOTDOGS.Length; i++)
        {
            HOTDOGFX[i] = HOTDOGS[i].GetComponent<UI_FX>();
        }
        BeginEntrance() ;
    }

    void BeginEntrance()
    {
        StartCoroutine(DelayedEntrance(HOTDOGFX[0], 0));
       
    }

    IEnumerator DelayedEntrance(UI_FX hotdog, int i)
    {
        yield return new WaitForSeconds(0.3f);
       hotdog.MoveIn(Vector3.down * 800f, 6f);
        
        if (i < HOTDOGS.Length)
        { StartCoroutine(DelayedEntrance(HOTDOGFX[i + 1], i + 1)); }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CollectHotDog(int i)
    {
        HOTDOGS[i].OnCollect();
    }
}
