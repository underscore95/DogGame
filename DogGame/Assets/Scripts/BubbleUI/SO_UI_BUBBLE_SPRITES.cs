using UnityEngine;

[CreateAssetMenu(fileName = "SO_UI_BUBBLE_SPRITES", menuName = "Scriptable Objects/SO_UI_BUBBLE_SPRITES")]
public class SO_UI_BUBBLE_SPRITES : ScriptableObject
{
    public string bubbleName;
    public Sprite[] sprites;
    public int animFPS;
}
