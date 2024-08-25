using System.Collections;
using UnityEngine;


namespace AS
{
    public class PlayerUICharacterMenuManager : MonoBehaviour
    {

        [Header("Menu")]
        [SerializeField] GameObject menu;

        public void OpenCharacterMenu()
        {
            PlayerUIManager.instance.menuWindowIsOpen = true;
            menu.SetActive(true);
        }

        //  THIS ISFINE BUT IF YOU R USING THE A BUTTON TO COSE MENUS YOU WILL JUMP AS YOU CLOSE THE MENU   
        public void CloseCharacterMenu()
        {
            
            PlayerUIManager.instance.menuWindowIsOpen = false;
            menu.SetActive(false);
        }

        public void CloseCharacterMenuAfterFixedFrame()
        {
            StartCoroutine(WaitThenCloseMenu());

        }


        private IEnumerator WaitThenCloseMenu()
        {
            yield return new WaitForFixedUpdate();
            PlayerUIManager.instance.menuWindowIsOpen = false;
            menu.SetActive(false);
        }



    }

}
