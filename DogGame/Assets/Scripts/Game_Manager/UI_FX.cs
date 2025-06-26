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
    Color defaultColorStatic;
    float colorBackTime;
    public PLAYER_INPUTS pi;
    public Image img;
    public bool isImg;
    bool fadeOut;
    public bool NotAffectedByTimescale;
    float fadeoutalpha;
    public float defaultAlpha;
    public bool overrideScaleRotation;
    Vector3 overrideScale;
    Vector3 overrideRotation;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        overrideScaleRotation = false;
        if (isImg) 
        { 
        img = GetComponent<Image>();
        }
        if (img != null) { defaultColor = img.color; }
        defaultPos = transform.position;
        defaultScale = transform.localScale;
        defaultRot = transform.rotation;
        defaultColorStatic = defaultColor;
    }

    private void Awake()
    {
        
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
    public void EndFadeOut(float spd) 
    {
        fadeOut = false;
        colorBackTime = spd;
        defaultColor.a = defaultColorStatic.a;
    }

    void MoveTowardsDefault()
    {
        float timeScale = NotAffectedByTimescale ? Time.unscaledDeltaTime :  Time.deltaTime;

        if (fadeOut)
        { defaultColor.a = fadeoutalpha; }    

        transform.position = Vector3.Lerp(transform.position, defaultPos, moveBackTime * timeScale);
        if (!overrideScaleRotation)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, defaultScale, scaleBackTime * timeScale);
            transform.rotation = Quaternion.Lerp(transform.rotation, defaultRot, rotBackTime * timeScale);
        }
        if (img != null) { img.color = Color.Lerp(img.color, defaultColor, colorBackTime * timeScale);}
       
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

    public void OverrideScaleRotation(Vector3 scale, Vector3 euler)
    {
        overrideScaleRotation = true;
        transform.localScale = scale;
        transform.localRotation = Quaternion.Euler(euler);
    }

    public void StopOverrideScaleRotation()
    {
        overrideScaleRotation = false;
        transform.localScale = defaultScale;
        transform.localRotation = defaultRot;
    }

    public void ScalePulse(Vector3 scale, Vector3 euler, float scaleSpd, float rotSpd, bool multScale)
    {
        if (!NotAffectedByTimescale || Time.timeScale == 1)
        {



       
        if (multScale)
            {
                transform.localScale = new Vector3(transform.localScale.x * scale.x, transform.localScale.y * scale.y, transform.localScale.z * scale.z);
            }
            else
            {
                transform.localScale = new Vector3(scale.x, scale.y, +scale.z);
            }
            if (euler != Vector3.zero)
            {
                transform.rotation = Quaternion.Euler(euler);
            }
            scaleBackTime = scaleSpd;
            rotBackTime = rotSpd;
        }
    }
}
