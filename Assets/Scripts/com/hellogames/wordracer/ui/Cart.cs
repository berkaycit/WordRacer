using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Colyseus;
using com.helloteam.wordracer.manager;
using com.helloteam.wordracer.popup;

public class Cart : MonoBehaviour
{
    public string cartID;

    FriendsPopup friendsPopup;

    private Client client;

    private void Start(){

        client = Manager.Instance.client;
        friendsPopup = FindObjectOfType<FriendsPopup>();
    }

    public async void AcceptFriend(){

        await client.Auth.AcceptFriendRequest(cartID);
        friendsPopup.Friends();
        Destroy(gameObject);
    }

    public async void DeclineFriend(){

        await client.Auth.DeclineFriendRequest(cartID);
        friendsPopup.Friends();
        Destroy(gameObject);
    }

}
