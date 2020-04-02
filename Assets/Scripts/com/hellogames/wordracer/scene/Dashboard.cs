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
using TMPro;
using System;
using com.helloteam.wordracer.popup;

namespace com.helloteam.wordracer.scene
{
    public class Dashboard : Scene
    {

        public TextMeshProUGUI name, level, diamons, coins;
        public int sceneID;
        public GameObject card;


        private FriendsPopup friendsPopup;
        private int soundStatus;
        private Client client;

        public Dashboard(){

        }

        protected override void Awake(){
            base.Awake();
            Manager.Instance.Init();
            client = Manager.Instance.client;
        }

        private async void Init(){
            level.SetText("Level: " + client.Auth.Metadata.point);

            if (client.Auth.IsAnonymous){
                name.SetText("Guest");
            }
            else{
                name.SetText(client.Auth.Username);
            }

            diamons.SetText(client.Auth.Metadata.diamonds);
            coins.SetText(client.Auth.Metadata.point);

            //await client.Auth.AcceptFriendRequest("5e5e41186a3cc474e1479f22");

        }


        protected override void Start(){
            base.Start();

            Init();

        }

        protected override void Update(){
            base.Update();
        }

        public override void OnClick(string action){
            base.OnClick(action);

            switch (action){

                case "friends":
                    Manager.Instance.Popup.Open("Friends");
                    break;

                case "matching":
                    Manager.Instance.Popup.Open("Matching");
                    break;

                case null:
                    break;
            }
        }
    }

}