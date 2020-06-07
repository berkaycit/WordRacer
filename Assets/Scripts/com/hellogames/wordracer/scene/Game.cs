using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using com.helloteam.wordracer.scene;
using com.helloteam.wordracer.popup;
using com.helloteam.wordracer.manager;
using com.helloteam.wordracer.conf;
using com.helloteam.wordracer.util;
using UnityEngine.SceneManagement;
using TMPro;
using Colyseus;

namespace com.helloteam.wordracer.scene
{
    public static class Extensions
    {
        public static void SetTransparency(this UnityEngine.UI.Image p_image, float p_transparency)
        {
            if (p_image != null)
            {
                UnityEngine.Color __alpha = p_image.color;
                __alpha.a = p_transparency;
                p_image.color = __alpha;
            }
        }
    }

    public class Game : Scene
    {
        //TODO: health ve answer i şifreli hash yap. public den çıkart
        public TextMeshProUGUI name1, name2, health1, health2, time, question;
        public GameObject keyboardSpace, wordSpace, guessBtn, word, keyboardWord, keyboardDelete, oTime;
        public int mHealth1, mHealth2, openedWords, totOpenedWords;
        public List<Word> answerArr;
        public Sprite sDeleteWord;
        public char[] userAnser;

        private Client client;
        private string[] keyboardContentTR;
        private string answer;
        private float constTime, currentTime, speed;
        private bool timeStarted = false, disabled = false;
        private GameObject deleteWord, canvasPopups;
        private List<GameObject> keys;

        //TODO: serveri açınca sil
        private void TempInit()
        {
            mHealth1 = 5; mHealth2 = 5;
            answer = "Ankara";

        }

        private void GameInit()
        {
            client = Manager.Instance.client;

            if(Manager.Instance.room != null){
                userAnser = new char[Manager.Instance.room.State.answer.Length];
            }else{
                userAnser = new char[answer.Length];
            }

            answerArr = new List<Word>();
            keys = new List<GameObject>();
            openedWords = 0; totOpenedWords = -1;
            constTime = 15f; currentTime = 0f; speed = 0.65f;
        }

        private void KeyboardInit()
        {
            keyboardContentTR = new string[] {"A", "B", "C", "Ç", "D", "E", "F", "G", "Ğ",
            "H", "I", "İ", "J", "K", "L", "M", "N", "O", "Ö", "P", "R", "S", "Ş", "T", "U", "Ü", "V", "W", "Y", "Z"};
        }

        protected override void Awake()
        {
            base.Awake();
            Manager.Instance.Init();

            canvasPopups = GameObject.Find("CanvasPopups");
            if (canvasPopups == null){
                Instantiate(Resources.Load("Prefabs/CanvasPopups"));
            }
            Manager.Instance.Popup.Init();

            if (Manager.Instance.room != null){
                GameStarted();
            }
            else{
                TempInit();
            }

            GameInit();
            KeyboardInit();
        }

        protected override void Start()
        {
            //TODO: internet problemi var bir önceki sayfaya gönder
            if (client != null)
            {
                name1.SetText(Manager.Instance.room.State.players[Manager.Instance.mPlayers[0]].displayName);
                name2.SetText(Manager.Instance.room.State.players[Manager.Instance.mPlayers[1]].displayName);
                health1.SetText(Manager.Instance.room.State.players[Manager.Instance.mPlayers[0]].healt.ToString());
                health2.SetText(Manager.Instance.room.State.players[Manager.Instance.mPlayers[1]].healt.ToString());
                question.SetText(Manager.Instance.room.State.question);
                answer = Manager.Instance.room.State.answer;

                Manager.Instance.room.OnMessage += (message) =>
                {
                    Debug.Log("message received from server");
                    Debug.Log(message);
                };
            }

            CreateKeyboard(keyboardContentTR);
            CreateAnswer(answer);
        }

        IEnumerator TimeStarted()
        {
            while (timeStarted)
            {
                if(Manager.Instance.room == null)
                {
                    if (constTime <= currentTime)
                    {
                        timeStarted = false;
                        TimeUp();
                    }
                    time.text = (constTime - currentTime).ToString("#.");
                }

                oTime.GetComponentInChildren<Image>().GetComponent<Animator>().SetFloat("Turn", currentTime);
                oTime.GetComponentInChildren<TextMeshProUGUI>().GetComponent<Animator>().SetFloat("Time", currentTime);
                currentTime += Time.deltaTime * speed;

                yield return null;
            }
        }

        private void CreateAnswer(string answer)
        {
            int i = 0;
            foreach(char key in answer)
            {
                GameObject newKey = Instantiate(word, wordSpace.transform, false);
                newKey.GetComponent<Word>().word = key.ToString().ToUpper();
                newKey.GetComponent<Word>().id = i;
                //eğer boşluk varsa onu görünmez yapıyorum
                if (string.IsNullOrWhiteSpace(key.ToString())){
                    newKey.GetComponent<Image>().color = Color.grey;
                    newKey.GetComponent<Image>().SetTransparency(100f / 255f);
                }
                else
                    answerArr.Add(newKey.GetComponent<Word>());
                
                i++;
            }
        }

        private void CreateKeyboard(string[] keyboardContent)
        {
            foreach(string key in keyboardContent)
            {
                GameObject newKey = Instantiate(keyboardWord, keyboardSpace.transform, false);
                newKey.GetComponentInChildren<TextMeshProUGUI>().text = key;
                newKey.GetComponent<KeyboardWord>().id = key;
                keys.Add(newKey);
            }
        }

        public async void DecreaseHealth()
        {
            if (Manager.Instance.room != null)
            {
                await Manager.Instance.room.Send(new { command = "minushealt" });
            }
            else
            {
                if (mHealth1 > 0){
                    mHealth1--;
                    health1.SetText(mHealth1.ToString());
                    }else {
                        //TODO: kazanan kişi
                    }
            }
        }

        private void TimeUp()
        {
            if (deleteWord != null)
                Destroy(deleteWord);

            foreach (GameObject key in keys)
                key.GetComponent<KeyboardWord>().predicting = false;
        }

        public async void GuessButton()
        {
            //TODO: doğru cevap gelmezse karşıdaki oyuncuyu aç ve bu oyuncunun butona basmasını kilitle
            //TODO: tekrar aynı kişi basmasın
            timeStarted = true;
            StartCoroutine(TimeStarted());
            currentTime = 0f;

            if(Manager.Instance.room != null){
                await Manager.Instance.room.Send(new { command = "predicting" });
            }
            else{
                DisableAll(false);
            }
        }

        private void DisableAll(bool myself)
        {
            if (!disabled)
            {
                disabled = true;

                //beni tamamen disable et
                if (myself)
                {
                    guessBtn.GetComponentInChildren<TextMeshProUGUI>().alpha = 155f / 255f;
                    keyboardSpace.GetComponent<CanvasGroup>().alpha = 155f / 255f;
                    keyboardSpace.GetComponent<CanvasGroup>().interactable = false;
                    guessBtn.GetComponent<Button>().interactable = false;

                }
                else
                {
                    guessBtn.GetComponentInChildren<TextMeshProUGUI>().alpha = 155f / 255f;
                    guessBtn.GetComponent<Button>().interactable = false;
                    deleteWord = Instantiate(keyboardDelete, keyboardSpace.transform, false);

                    foreach (GameObject key in keys)
                        key.GetComponent<KeyboardWord>().predicting = true;

                }
            }
        }

        public async void GameStarted()
        {
            Manager.Instance.room.State.OnChange += (changes) =>
            {
                changes.ForEach((obj) =>
                {
                    health1.SetText(Manager.Instance.room.State.players[Manager.Instance.mPlayers[0]].healt.ToString());
                    health2.SetText(Manager.Instance.room.State.players[Manager.Instance.mPlayers[1]].healt.ToString());

                    switch (Manager.Instance.room.State.phase)
                    {
                        case "predicting":
                            time.SetText(Manager.Instance.room.State.time + "");
                            if (Manager.Instance.room.State.time <= 0)
                                TimeUp();

                            //benim olduğum durum
                            if(Manager.Instance.room.State.playerTurn == client.Auth._id){
                                DisableAll(false);
                            }
                            else{
                                DisableAll(true);
                            }

                            break;

                        case "finished":
                            Debug.Log("winning player: " + Manager.Instance.room.State.winnigPlayer +
                            " benim id: " + client.Auth._id + " kaybeden id" + Manager.Instance.room.State.losingPlayer);
                            //kazanan kişi
                            if(Manager.Instance.room.State.winnigPlayer.Equals(client.Auth._id))
                            {
                                Manager.Instance.Popup.Open("Winner");
                            }
                            //kaybeden kişi
                            else
                            {
                                Manager.Instance.Popup.Open("Fail");
                            }

                            break;
                    }

                });
            };
        }


    }
}


