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
using TMPro;
using UnityEngine.SceneManagement;
using Colyseus;


namespace com.helloteam.wordracer.popup
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

    public class LoginPopup : Popup
    {
        PopupManager mPopupManager;
        Client client;
        string mail, pass;

        private static bool remember = false;

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

                case "remember":
                    /*
                    if (!remember){
                        remember = true;
                        gameObject.GetComponent<Image>().SetTransparency(255f);
                    }else{
                        remember = false;
                        gameObject.GetComponent<Image>().SetTransparency(0f);
                    }
                    */

                    break;

                case "enter":
                    await client.Auth.Login(mail, pass);
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

    }

}

