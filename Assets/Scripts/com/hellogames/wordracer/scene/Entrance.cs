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
using com.helloteam.wordracer.conf;
using Colyseus;


namespace com.helloteam.wordracer.scene
{
    public class Entrance : Scene
    {
        public int sceneID;
        private int soundStatus;

        public Entrance(){

        }

        protected override void Awake(){
            base.Awake();
            Manager.Instance.Init();

        }

        protected override void Start(){
            base.Start();
        }

        protected override void Update(){
            base.Update();
        }

        public async override void OnClick(string action){
            base.OnClick(action);

            switch (action)
            {
                case "anonymousin":
                    await Manager.Instance.client.Auth.Login();
                    if (Manager.Instance.client.Auth.HasToken){
                        SceneManager.LoadScene(1);
                    }

                    //Manager.Instance.Sound.PlayEffect(Sounds.PLAY_BUTTON);
                    //sceneID = 2;
                    //animator.SetBool("boolStartGame", true);
                    break;

                case "facebookin":
                    Manager.Instance.client.Auth.Logout();
                    //SceneManager.LoadScene(1);
                    break;

                case "signin":
                    //Manager.Instance.Sound.PlayEffect(Sounds.BUTTON_SOUND);
                    //sceneID = 1;
                    //animator.SetBool("boolStartGame", true);
                    Manager.Instance.Popup.Open("Login");
                    break;

                case "signup":
                    //Manager.levelNumber = 1;
                    //SceneManager.LoadScene(1);
                    Manager.Instance.Popup.Open("Signup");
                    break;


                case null:
                    break;
            }
        }
    }

}