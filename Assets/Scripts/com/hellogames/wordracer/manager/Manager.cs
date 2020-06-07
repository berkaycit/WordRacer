﻿using UnityEngine;
using System.Collections.Generic;

using com.helloteam.wordracer.util;
using com.helloteam.wordracer.scene;
using UnityEngine.SceneManagement;
using System;
using com.helloteam.wordracer.conf;
using Colyseus;

namespace com.helloteam.wordracer.manager
{
    public class Manager : Singleton<Manager>
    {
        private SoundManager sound;
        private PopupManager popup;
        public Client client;
        public Room<State> room;
        public List<string> mPlayers;

        void Awake()
        {
            mPlayers = new List<string>();
            Screen.sleepTimeout = SleepTimeout.NeverSleep;

            //sound = new SoundManager(gameObject);
            popup = new PopupManager();
        }

        void Start()
        {
            client = new Client("ws://6240aea7.ngrok.io/");

#if UNITY_ANDROID
            string appId = Config.ANDROID_APPID;
#elif UNITY_IPHONE
            string appId = Config.IOS_APPID;
#else
            string appId = "unexpected_platform";
#endif

        }

        public SoundManager Sound
        {
            get
            {
                return sound;
            }
        }

        public PopupManager Popup
        {
            get
            {
                return popup;
            }
        }

        public void Init(){
        }

        public void Create(){
        }




    }

}