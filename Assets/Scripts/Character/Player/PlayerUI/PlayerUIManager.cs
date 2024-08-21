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
        [HideInInspector] public PlayerUICharacterMenuManager playerUICharacterMenuManager;
        [HideInInspector] public PlayerUIEquipmentManager playerUIEquipmentManager;

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
            playerUICharacterMenuManager = GetComponentInChildren<PlayerUICharacterMenuManager>();
            playerUIEquipmentManager = GetComponentInChildren<PlayerUIEquipmentManager>();

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
                StartCoroutine(RestartAsClient());
            }
        }

        private IEnumerator RestartAsClient()
        {
            // Shut down the network manager
            NetworkManager.Singleton.Shutdown();

            // Wait for one frame or more to ensure the network has fully shut down
            yield return null;

            // Optionally, wait for another frame to be safe
            yield return new WaitForSeconds(0.1f);

            Debug.Log("Attempting to start the client...");

            // Start the network manager as a client
            NetworkManager.Singleton.StartClient();

            Debug.Log("Client start attempted.");
        }

        public void CloseAllMenuWindows()
        {
            playerUICharacterMenuManager.CloseCharacterMenu();
            playerUIEquipmentManager.CloseEquipmentManagerMenu();
        }

    }
}

