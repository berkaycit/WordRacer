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
    public class FriendsPopup : Popup
    {
        public GameObject card;

        private Transform cartHolder;
        PopupManager mPopupManager;
        Client client;

        public async void Friends()
        {
            float offset = 0;

            var friends = await client.Auth.GetFriends();
            var friendsRequest = await client.Auth.GetFriendRequests();

            for (var i = 0; i < friends.users.Length; i++){
                GameObject mCard = Instantiate(card, new Vector2(cartHolder.transform.position.x, cartHolder.transform.position.y), Quaternion.identity, cartHolder);
                mCard.GetComponent<RectTransform>().localPosition = new Vector2(mCard.transform.localPosition.x, mCard.transform.localPosition.y - offset);

                GameObject level = mCard.transform.GetChild(1).gameObject;
                level.GetComponent<Text>().text = "Level: " + friends.users[i].metadata.point + "";
                GameObject name = mCard.transform.GetChild(2).gameObject;
                name.GetComponent<Text>().text = friends.users[i].displayName;

                offset += 200;

                //Debug.Log(friends.users[i]._id + " coins: " + friends.users[i].metadata.coins);

            }

            for(var i = 0; i<friendsRequest.users.Length; i++){

                GameObject mCard = Instantiate(card, new Vector2(cartHolder.transform.position.x, cartHolder.transform.position.y), Quaternion.identity, cartHolder);
                mCard.GetComponent<RectTransform>().localPosition = new Vector2(mCard.transform.localPosition.x, mCard.transform.localPosition.y - offset);

                GameObject accept = mCard.transform.GetChild(4).gameObject;
                GameObject decline = mCard.transform.GetChild(5).gameObject;
                accept.SetActive(true);
                decline.SetActive(true);

                mCard.GetComponent<Cart>().cartID = friendsRequest.users[i]._id;

                GameObject level = mCard.transform.GetChild(1).gameObject;
                level.GetComponent<Text>().text = "Level: " + friendsRequest.users[i].metadata.point + "";
                GameObject name = mCard.transform.GetChild(2).gameObject;
                name.GetComponent<Text>().text = friendsRequest.users[i].displayName;

                offset += 200;

            }

        }

        protected override void Start()
        {
            if (mPopupManager == null)
                mPopupManager = Manager.Instance.Popup;

            client = Manager.Instance.client;
            cartHolder = this.gameObject.transform.GetChild(1).GetChild(0);

            Friends();

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

