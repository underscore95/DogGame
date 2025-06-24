using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class UI_QUESTPOPUP : MonoBehaviour
{

    [SerializeField] public TextMeshProUGUI Text;
    public float stayTime;
    public bool activated;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (activated)
        {
            Text.rectTransform.position = new Vector3(0f, 200f, 0f);
        }
        StartCoroutine(WaitForDestroy());
    }

    IEnumerator WaitForDestroy()
    {
        yield return new WaitForSeconds(stayTime);
        Destroy(gameObject);
    }

    public void ChangePos()
    {
        Text.rectTransform.position += new Vector3(0f, 200f, 0f);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
