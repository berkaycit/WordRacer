using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using com.helloteam.wordracer.scene;
using UnityEngine.UI;

public class Word : MonoBehaviour
{
    public string word;
    public int id;
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
        if (!opened)
        {
            opened = true; game.openedWords++; game.totOpenedWords++;
            game.userAnser[id] = word[0];
            GetComponentInChildren<TextMeshProUGUI>().text = word;
        }
    }

    public void SetWord(string word)
    {
        if (!filled)
        {
            filled = true; game.totOpenedWords++;
            game.userAnser[id] = word[0];
            GetComponentInChildren<TextMeshProUGUI>().text = word;
        }
    }

    public void DeleteWord()
    {
        if (filled)
        {
            filled = false; game.totOpenedWords--;
            GetComponentInChildren<TextMeshProUGUI>().text = "";
        }
    }

}
