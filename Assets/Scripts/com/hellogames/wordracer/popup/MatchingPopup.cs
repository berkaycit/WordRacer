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
using com.helloteam.wordracer.conf;
using Colyseus;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


namespace com.helloteam.wordracer.popup
{
    public class MatchingPopup : Popup
    {
        PopupManager mPopupManager;
        Client client;
        public Text name1, name2, point1, point2, countDown;
        public String photo1, photo2;

        //TODO: destroy popoup load next scene

        protected async override void Start()
        {
            if (mPopupManager == null)
                mPopupManager = Manager.Instance.Popup;

            client = Manager.Instance.client;

            name1.text = client.Auth.DisplayName;
            point1.text = client.Auth.Metadata.point;

            name2.text = "Searching";
            point2.text = "???";

            Dictionary<string, object> options = new Dictionary<string, object>();
            options.Add("userId", client.Auth._id);

            Manager.Instance.room = await client.JoinOrCreate<State>("my_room", options);

            Manager.Instance.room.State.players.OnChange += (Player player, string key) =>
            {
                //Debug.Log("player have changes at " + key);

                IEnumerator myDict = Manager.Instance.room.State.players.Keys.GetEnumerator();
                List<string> mPlayers = new List<string>();

                Manager.Instance.room.State.OnChange += (changes) =>
                {
                    changes.ForEach((obj) =>
                    {
                        if (obj.Field.ToString().Equals("phase") && obj.Value.ToString().Equals("placed"))
                        {
                            countDown.text = 3.ToString();

                            foreach (string mkey in Manager.Instance.room.State.players.Keys)
                            {
                                mPlayers.Add(mkey);
                            }

                            Manager.Instance.mPlayers = mPlayers;

                            string otherPlayerID;

                            if (Manager.Instance.room.State.players[mPlayers[0]].userId.Equals(client.Auth._id))
                                otherPlayerID = Manager.Instance.mPlayers[1];
                            else
                                otherPlayerID = Manager.Instance.mPlayers[0];

                            name2.text = Manager.Instance.room.State.players[otherPlayerID].displayName;
                            point2.text = Manager.Instance.room.State.players[otherPlayerID].point.ToString();

                            for(int i = 1; i<4; i++)
                            {
                                float num = i;
                                Invoke("CountDown", num);
                            }


                        }

                    });
                };

            };

            base.Start();
        }

        public void CountDown()
        {
            int number = int.Parse(countDown.text);
            number -= 1;
            countDown.text = number.ToString();

            if(number <= 0)
            {
                Debug.Log("load scene");
                Destroy(gameObject);
                SceneManager.LoadScene(2);
            }

        }


        public async override void Clicked(string action)
        {
            if (mPopupManager == null)
                mPopupManager = Manager.Instance.Popup;

            switch (action)
            {
                case ButtonConfig.POPUP_CLOSE:
                    Manager.Instance.Popup.Close();
                    Destroy(gameObject);
                    break;

                case "exit":
                    gameObject.SetActive(false);
                    break;

            }

            base.Clicked(action);
        }

    }

}

