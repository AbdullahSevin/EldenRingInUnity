using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AS
{
    public class UI_StatBar : MonoBehaviour
    {
        private Slider slider;
        private RectTransform rectTransform;

        [Header("Bar Options")]
        [SerializeField] protected bool scaleBarLengthWithStats = true;
        [SerializeField] protected float widthScaleMultiplayer = 1f;


        // VARIABLE TO SCALE THE BAR SIZE DEPENDING ON STAT (HIGHER STAT = LONGER BAR ACROSS SCREEN)
        // SECONDARY BAR BEHIND MAY BAR FOR POLISH EFFECT (YELLOW BAR THAT SHOWS HOW MUCH AN ACTION/DAMAGE TAKES AWAY FROM CURRENT STAT)

        protected virtual void Awake()
        {
            slider = GetComponent<Slider>();
            rectTransform = GetComponent<RectTransform>();
        }

        public virtual void SetStat(int newValue)
        {
            slider.value = newValue;
        }

        public virtual void SetMaxStat(int maxValue)
        {
            slider.maxValue = maxValue;
            slider.value = maxValue; // THIS DOESN'T MAKE ANY SENSE IS IT, IF A PLAYER USES ALL STAMINA AND INCREASES ENDURANCE STAT POINT THEN HIS STAMINA WILL BE FILLED?

            if (scaleBarLengthWithStats)
            {
                rectTransform.sizeDelta = new Vector2(maxValue * widthScaleMultiplayer, rectTransform.sizeDelta.y);

                // RESETS THE POSITION OF THE BARS BASED ON THEIR LAYOUT GOUPS'S SETTINGS
                PlayerUIManager.instance.playerUIHudManager.RefreshHUD();
            }
        }
    }

}

