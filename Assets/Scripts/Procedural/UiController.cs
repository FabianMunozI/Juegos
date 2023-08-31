using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SVS
{
    public class UiController : MonoBehaviour
    {
        public static UiController instance;
        public Slider slider;
        public TextMeshProUGUI loadingText;
        public GameObject loadingPanel;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }else if (instance != this)
            {
                Destroy(gameObject);
            }
        }

        public void ResetScreen()
        {
            loadingPanel.SetActive(true);
            loadingText.text = "0%";
            slider.value = 0;
        }

        public void SetLoadValue(float value)
        {
            loadingText.text = (int)(value*100) + " %";
            slider.value = value;
        }

        public void HideLoadScreen()
        {
            loadingPanel.SetActive(false);
        }
    }
}


