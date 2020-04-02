using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Word : MonoBehaviour
{
    public string word;

    public void DisplayWord()
    {
        gameObject.GetComponentInChildren<TextMeshProUGUI>().text = word;
    }

}
