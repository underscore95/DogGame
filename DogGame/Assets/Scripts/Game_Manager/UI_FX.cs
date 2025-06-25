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
        
       // { MoveIn(Vector3.down * 800f, 5f); ColorPulse(3f, Color.white, 0f); }
        MoveTowardsDefault();
    }

    void MoveTowardsDefault()
    {
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
        transform.localScale += scale;
        if (euler != Vector3.zero)
        {
            transform.rotation = Quaternion.Euler(euler);
        }
        scaleBackTime = scaleSpd;
        rotBackTime = rotSpd;
    }
}
