using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AS
{
    public class PortalManager : MonoBehaviour
    {
        public Vector3 portalOutPosition;

        private void Awake()
        {
            portalOutPosition = transform.GetChild(0).position;
        }

        private void OnTriggerEnter(Collider other)
        {
            other.gameObject.transform.position = portalOutPosition;
        }




    }

}
