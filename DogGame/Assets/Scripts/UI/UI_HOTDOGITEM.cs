using UnityEngine;
using UnityEngine.UI;

public class UI_HOTDOGITEM : MonoBehaviour
{
    [SerializeField] Sprite HotdogCollectedSprite;
    [SerializeField] Sprite HotdogUncollectedSprite;
    [SerializeField] Color CollectedCol;
    [SerializeField] Color UncollectedCol;
    Image UI_Img;
    bool collected;
    int id;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        UI_Img = GetComponent<Image>();
        UI_Img.sprite = HotdogUncollectedSprite;
        UI_Img.color = UncollectedCol;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCollect()
    {
        collected = true;
        UI_Img.sprite = HotdogCollectedSprite;
        UI_Img.color = CollectedCol;
        UI_Img.transform.position = new Vector3(UI_Img.transform.position.x, UI_Img.transform.position.y +200f, UI_Img.transform.position.z);

    }
}
