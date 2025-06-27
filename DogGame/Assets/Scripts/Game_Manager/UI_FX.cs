using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    public bool fadeOut;
    public bool NotAffectedByTimescale;
    public float fadeoutalpha;
    public float defaultAlpha;
    public bool overrideScaleRotation;
    Vector3 overrideScale;
    Vector3 overrideRotation;
    //Because of multi frame images
    Color storedColor;
    public bool isText;
    public TextMeshProUGUI text;
    bool rotating;
    Vector3 rotAmount;
    Vector3 rotEular;
    float defaultRotStatic;
    public bool overridePosChanges;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        overrideScaleRotation = false;
        if (isImg)
        {
            img = GetComponent<Image>();
        }
        if (img != null) { defaultColor = img.color; storedColor = defaultColor; }
        if (isText)
        {
            text = GetComponent<TextMeshProUGUI>();
            { defaultColor = text.color; storedColor = defaultColor; }
        }
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
        if (rotating)
        { rotEular += (rotAmount * Time.unscaledDeltaTime); }
        
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
    public void Rotating(Vector3 axis, float amount)
    {
        rotating = true;
        rotAmount = axis * amount;
    }

    public void EndRotating()
    {
        rotating = false;

    }

    void MoveTowardsDefault()
    {
        float timeScale = NotAffectedByTimescale ? Time.unscaledDeltaTime :  Time.deltaTime;

        if (fadeOut)
        { defaultColor.a = fadeoutalpha; }

        if (!overridePosChanges)
        {
            transform.position = Vector3.Lerp(transform.position, defaultPos, moveBackTime * timeScale);
        }
        if (!overrideScaleRotation)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, defaultScale, scaleBackTime * timeScale);
            if (rotating)
            {
                transform.rotation = Quaternion.Euler(rotEular);
            }
            else
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, defaultRot, rotBackTime * timeScale);
            }
        }
        if (isText) {
            storedColor = Color.Lerp(storedColor, defaultColor, colorBackTime * timeScale);

            text.color = storedColor; text.alpha = storedColor.a; }
        else
        if (img != null)
        {
             storedColor = Color.Lerp(storedColor, defaultColor, colorBackTime * timeScale);
          //  img.color = Color.Lerp(storedColor, defaultColor, colorBackTime * timeScale);

            
          img.color = storedColor; 
        }
       
    }

    public void ChangeDefaultColor(Color col, float alpha)
    {
        defaultColor = col;
        storedColor = col;
        defaultColor.a = alpha;
        storedColor.a = alpha;
    }
    public void ChangeDefaultPos(Vector3 pos)
    {
        defaultPos = pos ;
    }


    public void ColorPulse(float pulseSpd, Color color, float alpha)
    {
        if (isText)
        {
            text.color = color;
            storedColor = color;
            colorBackTime = pulseSpd;
            text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
            storedColor = text.color;

        }
        else
        {

            img.color = color;
            storedColor = color;
            colorBackTime = pulseSpd;
            img.color = new Color(img.color.r, img.color.g, img.color.b, alpha);
            storedColor = img.color;
        }
    }

    public void MoveIn(Vector3 pos, float spd)
    {
        if (!overridePosChanges)
        {
            transform.position += pos;
        }
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
