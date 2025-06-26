using System;
using UnityEngine;
using UnityEngine.UI;

public class UI_FX : MonoBehaviour
{
      Vector3 defaultScale;
    public Vector3 defaultPos;
    public Quaternion defaultRot;
    public Color defaultColor;
    float moveBackTime;
    float scaleBackTime;
    float rotBackTime;
    float colorBackTime;
    public PLAYER_INPUTS pi;
    public Image img;
    public bool isImg;
    bool fadeOut;

    float fadeoutalpha;
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
        
       // { MoveIn(Vector3.down * 800f, 5f); ColorPulse(3f, Color.white, 0f); }
        MoveTowardsDefault();
    }

    public void BeginFadeOut(float alpha, float spd)
    {
        fadeOut = true;
        colorBackTime = spd;
        fadeoutalpha = alpha;
    }
    public void EndFadeOut(float spd, float alpha) 
    {
        fadeOut = false;
        colorBackTime = spd;
        defaultColor.a = alpha;
    }

    void MoveTowardsDefault()
    {
        if (fadeOut)
        { defaultColor.a = fadeoutalpha; }    

        transform.position = Vector3.Lerp(transform.position, defaultPos, moveBackTime * Time.deltaTime);
        transform.localScale = Vector3.Lerp(transform.localScale, defaultScale, scaleBackTime * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, defaultRot, rotBackTime * Time.deltaTime) ;
        if (img != null) { img.color = Color.Lerp(img.color, defaultColor, colorBackTime * Time.deltaTime);}
    }

    public void ChangeDefaultColor(Color col, float alpha)
    {
        defaultColor = col;
        defaultColor.a = alpha;
    }
    public void ChangeDefaultPos(Vector3 pos)
    {
        defaultPos = pos ;
    }


    public void ColorPulse(float pulseSpd, Color color, float alpha)
    {
        img.color = color;
        colorBackTime = pulseSpd;
        img.color = new Color(img.color.r, img.color.g, img.color.b, alpha);
    }

    public void MoveIn(Vector3 pos, float spd)
    {
        transform.position += pos;
        moveBackTime = spd;
    }

    public void ScalePulse(Vector3 scale, Vector3 euler, float scaleSpd, float rotSpd)
    {
        transform.localScale = new Vector3(transform.localScale.x * scale.x, transform.localScale.y * scale.y, transform.localScale.z * scale.z);
        if (euler != Vector3.zero)
        {
            transform.rotation = Quaternion.Euler(euler);
        }
        scaleBackTime = scaleSpd;
        rotBackTime = rotSpd;
    }
}
