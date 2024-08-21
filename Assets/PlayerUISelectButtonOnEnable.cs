using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace AS
{
    public class PlayerUISelectButtonOnEnable : MonoBehaviour
    {

        private Button button;

        private void Awake()
        {
            button = GetComponent<Button>();
        }

        private void OnEnable()
        {
            button.Select();
            button.OnSelect(null);
        }

    }
}

