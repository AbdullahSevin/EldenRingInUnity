using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;


namespace AS
{
    public class PlayerUIManager : MonoBehaviour
    {
        public static PlayerUIManager instance;

        [Header("NETWORK JOIN")]
        [SerializeField] bool startGameAsClient;

        [HideInInspector] public PlayerUIHudManager playerUIHudManager;
        [HideInInspector] public PlayerUIPopUpManager playerUIPopUpManager;

        [Header("UI FLAGS")]
        public bool menuWindowIsOpen = false;    //  INVENTORY SCREEN, EQUIPMENT MENU, BLACKSMITH MENU ETC
        public bool popUpWindowIsOpen = false;   //  ITEM PICKUP, DIALOG PUP UP ETC.


        public void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);

            }

            playerUIHudManager = GetComponentInChildren<PlayerUIHudManager>();
            playerUIPopUpManager = GetComponentInChildren<PlayerUIPopUpManager>();

        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            if (startGameAsClient)
            {
                startGameAsClient = false;
                // WE MUST FIRST SHUT DOWN THE NETWORK BECAUSE WE HAVE STARTED AS A HOST DURING THE TITLESCREEN
                NetworkManager.Singleton.Shutdown();
                // WE THEN RESTART AS A CLIENT
                NetworkManager.Singleton.StartClient();
            }
        }

    }
}

