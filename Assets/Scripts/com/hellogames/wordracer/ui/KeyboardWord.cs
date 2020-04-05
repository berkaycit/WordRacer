using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.helloteam.wordracer.scene;

public class KeyboardWord : MonoBehaviour
{
    public string id;
    public bool predicting;

    private Game game;
    private bool decreaseHealth = true;

    private void Awake()
    {
        predicting = false;
    }

    public void Pressed()
    {
        if (game == null)
            game = FindObjectOfType<Game>();

        if (!predicting)
        {
            foreach (Word answerWord in game.answerArr)
            {
                if (answerWord.word.Equals(id))
                {
                    answerWord.DisplayWord();
                    decreaseHealth = false;
                }
            }

            if (decreaseHealth) game.DecreaseHealth();
        }
        else
        {
            foreach(Word answerWord in game.answerArr)
            {
                if (!answerWord.opened && !answerWord.filled)
                {
                    answerWord.SetWord(id);
                    break;
                }
            }
        }

    }



}
