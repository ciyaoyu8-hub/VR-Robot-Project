using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetPointCtrl : MonoBehaviour
{
    public Text text;
    public Image image;
    public void SetText(string str)
    {
        text.text = str;
    }
    public void SetArriveColor()
    {
        image.color = Color.red;
    }
}
