using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class Indicator : MonoBehaviour
{
    public TextMeshProUGUI textUI;

    public static List<Indicator> indicators = new List<Indicator>();

    public string Text
    {
        get
        {
            return textUI.text;
        }

        set
        {
            textUI.text = value;
        }
    }

    public float lifeTime = 1f;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }
}
