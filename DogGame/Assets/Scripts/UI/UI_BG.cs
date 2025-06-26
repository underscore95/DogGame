using UnityEngine;

public class UI_BG : MonoBehaviour
{
    UI_FX FX;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        FX = GetComponent<UI_FX>();
    }

    private void OnEnable()
    {
        if (FX != null)
        {
            FX.ColorPulse(6f, FX.defaultColor, 0f);

        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
