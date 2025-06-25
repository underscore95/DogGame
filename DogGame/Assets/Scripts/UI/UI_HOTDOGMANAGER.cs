using UnityEngine;

public class UI_HOTDOGMANAGER : MonoBehaviour
{
    public UI_HOTDOGITEM[] HOTDOGS;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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
