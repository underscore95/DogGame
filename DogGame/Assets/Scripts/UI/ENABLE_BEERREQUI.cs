using UnityEngine;

public class ENABLE_BEERREQUI : MonoBehaviour
{
    bool playerEntered;
    [SerializeField] CLOTHTRACKER_UI CCUI;
    [SerializeField] COINCOUNTER_UI COINCOUNTER_UI;
    [SerializeField] IWANTBEERUI BEER;
    [SerializeField] IWANTBEERUI SPRINT;

    [SerializeField] UI_BUBBLE_DISPLAY LACKMONEY;
    [SerializeField] UI_BUBBLE_DISPLAY LACKCLOTHES;

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
                
                if (LACKMONEY.isActiveAndEnabled)
                {
                    LACKMONEY.SetBubbleVisibility(true);
                }
                if (LACKCLOTHES.isActiveAndEnabled)
                {
                    LACKCLOTHES.SetBubbleVisibility(true);

                }
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

            if (LACKMONEY.isActiveAndEnabled)
            {
                LACKMONEY.SetBubbleVisibility(false);
            }
            if (LACKCLOTHES.isActiveAndEnabled) 
            {
                LACKCLOTHES.SetBubbleVisibility(false);

            }
        }
    }

    public void DisableMoneyWarning()
    {
        LACKMONEY.gameObject.SetActive(false);
    }

    public void DisableClothesWarning()
    {
        LACKCLOTHES.gameObject.SetActive(false);
    }


}
