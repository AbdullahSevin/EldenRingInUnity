using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AS
{
    public class Item : ScriptableObject
    {
        [Header("Item Information")]
        public string name;
        public Sprite itemIcon;
        [TextArea] public string itemDescription;
        public int itemID;





    }
}
