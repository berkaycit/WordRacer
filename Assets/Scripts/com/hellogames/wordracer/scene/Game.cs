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

        private Client client;
        private string[] keyboardContentTR;
        private string answer;
        private float constTime, currentTime, speed;
        private bool timeStarted = false;
        private GameObject deleteWord;
        private List<GameObject> keys;


        //TODO: serveri açınca sil
        private void TempInit()
        {
            mHealth1 = 5; mHealth2 = 5;
            answer = "Kastamonu";

        }

        private void GameInit()
        {
            client = Manager.Instance.client;
            answerArr = new List<Word>();
            keys = new List<GameObject>();
            openedWords = 0; totOpenedWords = 0;
            constTime = 15f; currentTime = 0f; speed = 0.65f;
        }

        private void KeyboardInit()
        {
            keyboardContentTR = new string[] {"A", "B", "C", "Ç", "D", "E", "F", "G", "Ğ",
            "H", "I", "İ", "J", "K", "L", "M", "N", "O", "Ö", "P", "R", "S", "Ş", "T", "U", "Ü", "V", "Y", "Z"};
        }

        protected override void Awake()
        {
            base.Awake();
            Manager.Instance.Init();

            GameInit();
            KeyboardInit();
            TempInit();
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

                Manager.Instance.room.OnMessage += (message) =>
                {
                    Debug.Log("message received from server");
                    Debug.Log(message);
                };
            }

            CreateKeyboard(keyboardContentTR);
            CreateAnswer(answer);

        }

        protected override void Update()
        {
            if (timeStarted)
            {
                if(constTime <= currentTime){
                    timeStarted = false;
                    TimeUp();
                }

                oTime.GetComponentInChildren<Image>().GetComponent<Animator>().SetFloat("Turn", currentTime);
                oTime.GetComponentInChildren<TextMeshProUGUI>().GetComponent<Animator>().SetFloat("Time", currentTime);

                currentTime += Time.deltaTime * speed;
                time.text = (constTime - currentTime).ToString("#.");
            }
        }

        private void CreateAnswer(string answer)
        {
            foreach(char key in answer)
            {
                //newKey.GetComponentInChildren<TextMeshProUGUI>().text = key.ToString();
                GameObject newKey = Instantiate(word, wordSpace.transform, false);
                newKey.GetComponent<Word>().word = key.ToString().ToUpper();
                answerArr.Add(newKey.GetComponent<Word>());
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

        public void DecreaseHealth()
        {
            if(mHealth1 > 0){
                mHealth1--;
                health1.SetText(mHealth1.ToString());
            }else {
                //TODO: kazanan kişi
            }

        }

        private void TimeUp()
        {
            if (deleteWord != null)
                Destroy(deleteWord);

            foreach (GameObject key in keys)
                key.GetComponent<KeyboardWord>().predicting = false;
        }

        public void GuessButton()
        {
            //TODO: hangi oyuncu bastıysa karşıdaki oyuncuyu kilitle
            //TODO: doğru cevap gelmezse karşıdaki oyuncuyu aç ve bu oyuncunun butona basmasını kilitle
            //TODO: tekrar aynı kişi basmasın
            timeStarted = true;
            currentTime = 0f;

            //guessBtn.GetComponent<Button>().interactable = false;
            //guessBtn.GetComponent<Image>().SetTransparency(155f / 255f);

            DisableAll(false);

            foreach(GameObject key in keys)
                key.GetComponent<KeyboardWord>().predicting = true;

        }

        private void DisableAll(bool myself)
        {
            //beni tamamen disable et
            if (myself){
                guessBtn.GetComponentInChildren<TextMeshProUGUI>().alpha = 155f / 255f;
                keyboardSpace.GetComponent<CanvasGroup>().alpha = 155f / 255f;
                keyboardSpace.GetComponent<CanvasGroup>().interactable = false;
                guessBtn.GetComponent<Button>().interactable = false;
            }
            else{
                guessBtn.GetComponentInChildren<TextMeshProUGUI>().alpha = 155f / 255f;
                guessBtn.GetComponent<Button>().interactable = false;
                deleteWord = Instantiate(keyboardDelete, keyboardSpace.transform, false);

            }

        }

    }
}


