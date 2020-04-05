using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using com.helloteam.wordracer.scene;

public class Word : MonoBehaviour
{
    public string word;
    public bool opened, filled;

    private Game game;

    private void Awake()
    {
        opened = false;
        filled = false;

        if (game == null)
            game = FindObjectOfType<Game>();
    }

    public void DisplayWord()
    {
        opened = true; game.openedWords++; game.totOpenedWords++;
        GetComponentInChildren<TextMeshProUGUI>().text = word;
    }

    public void SetWord(string word)
    {
        filled = true; game.totOpenedWords++;
        GetComponentInChildren<TextMeshProUGUI>().text = word;
    }

    public void DeleteWord()
    {
        filled = false; game.totOpenedWords--;
        GetComponentInChildren<TextMeshProUGUI>().text = "";
    }

}
