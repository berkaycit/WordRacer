using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.helloteam.wordracer.scene;
using com.helloteam.wordracer.manager;

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

    public async void Pressed()
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

                    //Oyuncu oyunu klavyeden kazandı
                    if (game.totOpenedWords == game.answerArr.Count - 1)
                    {
                        string userAnswer = new string(game.userAnser);

                        if (Manager.Instance.room != null)
                            await Manager.Instance.room.Send(new { command = "prediction", answer = userAnswer });

                        Debug.Log("kazandi");

                        //Manager.Instance.Popup.Open("Winner");

                        //if (Manager.Instance.room != null)
                        //TODO: servere mesaj gönder
                    }

                }
            }

            if (decreaseHealth)
                game.DecreaseHealth();
            
        }
        else
        {
            foreach(Word answerWord in game.answerArr)
            {
                if (!answerWord.opened && !answerWord.filled)
                {
                    answerWord.SetWord(id);

                    //Oyuncu kelimelerin tamamını yazdı
                    if (game.totOpenedWords == game.answerArr.Count -1)
                    {
                        string userAnswer = new string(game.userAnser);

                        Debug.Log("kazandi");
                        //servera oyuncunun cevabını gönderiyoruz ve kontrol ediyoruz
                        if (Manager.Instance.room != null)
                            await Manager.Instance.room.Send(new { command = "prediction", answer = userAnswer});
                    }


                    break;
                }
            }
        }

    }



}
