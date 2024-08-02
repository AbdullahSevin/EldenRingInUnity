using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace AS
{
    public class UI_Match_Scoll_Wheel_To_Selected_Button : MonoBehaviour
    {
        [SerializeField] GameObject currentSelected;
        [SerializeField] GameObject previouslySelected;
        [SerializeField] RectTransform currentSelectedTransform;


        [SerializeField] RectTransform contentPanel;
        [SerializeField] ScrollRect scrollRect;

        private void Update()
        {
            currentSelected = EventSystem.current.currentSelectedGameObject;

            if (currentSelected != null)
            {
                if (currentSelected != previouslySelected)
                {
                    previouslySelected = currentSelected;
                    currentSelectedTransform = currentSelected.GetComponent<RectTransform>();
                    SnapTo(currentSelectedTransform);
                }
            }
        }

        private void SnapTo(RectTransform target)
        {
            Canvas.ForceUpdateCanvases();

            Vector2 newPosition = 
                (Vector2)scrollRect.transform.InverseTransformPoint(contentPanel.position) // THIS
                -                                                                          // MINUS
                (Vector2)scrollRect.transform.InverseTransformPoint(target.position);      // THIS

            // WE ONLY WANT TO LOCK THE POSITION ON THE Y AXIS (UP AND DOWN)
            newPosition.x = 0;

            contentPanel.anchoredPosition = newPosition;
        }


    }

}
