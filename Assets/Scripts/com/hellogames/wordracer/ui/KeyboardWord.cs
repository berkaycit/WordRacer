using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.helloteam.wordracer.scene;

public class KeyboardWord : MonoBehaviour
{
    public string id;

    private Game game;
    private bool decreaseHealth = true;

    public void Pressed()
    {
        if (game == null)
            game = FindObjectOfType<Game>();

        foreach (Word answerKey in game.answerArr)
        {
            if (answerKey.word.Equals(id)){
                answerKey.DisplayWord();
                decreaseHealth = false;
            }
        }

        if (decreaseHealth) game.DecreaseHealth();

    }



}
