using UnityEngine;

public class ENABLE_BEERREQUI : MonoBehaviour
{
    bool playerEntered;
    [SerializeField] CLOTHTRACKER_UI CCUI;
    [SerializeField] COINCOUNTER_UI COINCOUNTER_UI;
    [SerializeField] IWANTBEERUI BEER;
    [SerializeField] IWANTBEERUI SPRINT;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       // BEER = FindAnyObjectByType<IWANTBEERUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Player")
        {
            if (!playerEntered) 
            {
                BEER.disappear = true;
                SPRINT.disappear = true;
                Debug.Log("ENTER");
                playerEntered = true;
                CCUI.Show();
                COINCOUNTER_UI.Show();

            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("EXIT");

            playerEntered = false;
            CCUI.Hide();
            COINCOUNTER_UI.Hide();
        }
    }

   
}
