using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapUICtrl : MonoBehaviour
{
    private void LateUpdate()
    {
        if (transform.rotation != Quaternion.identity)
        {

            transform.rotation = Quaternion.Euler(90,- 90, 0);
        }
    }
}
