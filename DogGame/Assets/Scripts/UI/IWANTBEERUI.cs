using System.Collections;
using UnityEngine;

public class IWANTBEERUI : MonoBehaviour
{
   public UI_BUBBLE_DISPLAY UIB;
    public bool disappear;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UIB = GetComponent<UI_BUBBLE_DISPLAY>();
        if (!disappear)
        {
            UIB.SetBubbleVisibility(true);
        }
        else
        { UIB.SetBubbleVisibility(false); Destroy(gameObject, 3f); }
    }

    // Update is called once per frame
    void Update()
    {
        if (!disappear)
        {
            UIB.SetBubbleVisibility(true);
        }
        else
        { UIB.SetBubbleVisibility(false); Destroy(gameObject, 3f); }
    }

    
}
