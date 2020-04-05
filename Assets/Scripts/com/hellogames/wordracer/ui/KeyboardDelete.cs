using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.helloteam.wordracer.scene;

public class KeyboardDelete : MonoBehaviour
{
    private Game game;

    public void Pressed()
    {
        if (game == null)
            game = FindObjectOfType<Game>();

        for(int i = game.totOpenedWords; i>=0; i--)
        {
            if(!game.answerArr[i].opened && game.answerArr[i].filled)
            {
                game.answerArr[i].DeleteWord();
                break;
            }
        }
    }
}
