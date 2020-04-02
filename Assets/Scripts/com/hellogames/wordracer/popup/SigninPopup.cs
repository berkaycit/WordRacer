using UnityEngine;
using System.Collections;
using com.helloteam.wordracer.manager;
using com.helloteam.wordracer.popup;
using com.helloteam.wordracer.animation;
using com.helloteam.wordracer.scene;
using com.helloteam.wordracer.conf;
using com.helloteam.wordracer.util;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Colyseus;


namespace com.helloteam.wordracer.popup
{
    public class SigninPopup : Popup
    {
        PopupManager mPopupManager;
        Client client;
        string username, mail, pass;

        protected override void Start()
        {
            if (mPopupManager == null)
                mPopupManager = Manager.Instance.Popup;

            client = Manager.Instance.client;

            base.Start();
        }


        public async override void Clicked(string action)
        {
            if (mPopupManager == null)
                mPopupManager = Manager.Instance.Popup;

            switch (action)
            {
                case ButtonConfig.POPUP_CLOSE:
                    Manager.Instance.Popup.Close();
                    break;

                case "email":

                    break;

                case "save":
                    await client.Auth.Login(mail, pass);
                    client.Auth.Username = username;
                    client.Auth.Save();
                    if (client.Auth.HasToken){
                        SceneManager.LoadScene(1);
                    }

                    break;


                case "exit":
                    gameObject.SetActive(false);
                    break;
            }

            base.Clicked(action);
        }

        public void MailChanged(string text) => mail = text;
        public void PassChanged(string text) => pass = text;
        public void UsernameChanged(string text) => username = text;

    }

}

