

    using UnityEngine;
    using UnityEngine.UI;
    using System.Collections;
    //using VRTK;

    public class PreviewCopy : MonoBehaviour
    {
        public Text myText;
        public Image panel;

        public void StartUsing(GameObject usingObject)
        {

            panel.enabled = true;
            myText.enabled = true;
        }

        public void StopUsing(GameObject usingObject)
        {

            panel.enabled = false;
            myText.enabled = false;
        }


        public void Start()
        {
            panel.enabled = false;
            myText.enabled = false;
        }

        public void Update()
        {

        }
    }
