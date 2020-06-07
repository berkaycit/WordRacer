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
    public class WinPopup : Popup
    {
        public Text point, name1, name2, answer;

        PopupManager mPopupManager;
        Client client;
        string otherPlayerID;

        protected override void Start()
        {
            if (mPopupManager == null)
                mPopupManager = Manager.Instance.Popup;

            client = Manager.Instance.client;

            if (Manager.Instance.room.State.players[Manager.Instance.mPlayers[0]].userId.Equals(client.Auth._id))
                otherPlayerID = Manager.Instance.mPlayers[1];
            else
                otherPlayerID = Manager.Instance.mPlayers[0];

            name1.text = client.Auth.DisplayName;
            name2.text = Manager.Instance.room.State.players[otherPlayerID].displayName;
            answer.text = Manager.Instance.room.State.answer;
            point.text = "+10";

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

                case "home":
                    Manager.Instance.Popup.Close();
                    SceneManager.LoadScene(1);
                    break;

                case "add":
                    await Manager.Instance.client.Auth.SendFriendRequest(Manager.Instance.room.State.players[otherPlayerID].userId);
                    break;

                case "exit":
                    gameObject.SetActive(false);
                    break;
            }

            base.Clicked(action);
        }

    }

}

