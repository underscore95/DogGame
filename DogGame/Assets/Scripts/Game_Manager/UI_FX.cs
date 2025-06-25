using System;
using UnityEngine;
using UnityEngine.UI;

public class UI_FX : MonoBehaviour
{
    Vector3 defaultScale;
    Vector3 defaultPos;
    Quaternion defaultRot;
    Color defaultColor;
    float moveBackTime;
    float scaleBackTime;
    float rotBackTime;
    float colorBackTime;
    public PLAYER_INPUTS pi;
    Image img;
    public bool isImg;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (isImg) 
        { 
        img = GetComponent<Image>();
        }
        if (img != null) { defaultColor = img.color; }
        defaultPos = transform.position;
        defaultScale = transform.localScale;
        defaultRot = transform.rotation;
    }

    private T TryGetComponent<T>()
    {
        throw new NotImplementedException();
    }

    // Update is called once per frame
    void Update()
    {
        if (pi.IA_Fetch.WasPressedThisFrame())
        // { ScalePulse(new Vector3(10f, 5f, 5f),new Vector3(0, 0, 90), 5, 5); ColorPulse(5f, Color.white); }
        { MoveIn(Vector3.down * 800f, 5f); }
        MoveTowardsDefault();
    }

    void MoveTowardsDefault()
    {
        transform.position = Vector3.Lerp(transform.position, defaultPos, moveBackTime * Time.deltaTime);
        transform.localScale = Vector3.Lerp(transform.localScale, defaultScale, scaleBackTime * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, defaultRot, rotBackTime * Time.deltaTime) ;
        if (img != null) { img.color = Color.Lerp(img.color, defaultColor, colorBackTime * Time.deltaTime); }
    }

    public void ColorPulse(float pulseSpd, Color color)
    {
        img.color = color;
        colorBackTime = pulseSpd;
    }

    public void MoveIn(Vector3 pos, float spd)
    {
        transform.position += pos;
        moveBackTime = spd;
    }

    public void ScalePulse(Vector3 scale, Vector3 euler, float scaleSpd, float rotSpd)
    {
        transform.localScale += scale;
        if (euler != Vector3.zero)
        {
            transform.rotation = Quaternion.Euler(euler);
        }
        scaleBackTime = scaleSpd;
        rotBackTime = rotSpd;
    }
}
