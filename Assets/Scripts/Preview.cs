

    using UnityEngine;
    using UnityEngine.UI;
    using System.Collections;
    using VRTK;

    public class Preview : VRTK_InteractableObject
    {
        public string myString;
        public Text myText;
        public float fadeTime;
        public bool displayInfo;
        public Image h;
        public Color i;
        public Color j;

        

        public override void StartUsing(GameObject usingObject)
        {
            base.StartUsing(usingObject);
            displayInfo = true;
            Debug.Log("StartUsing");
        }

        public override void StopUsing(GameObject usingObject)
        {
            base.StopUsing(usingObject);
            displayInfo = false;
            Debug.Log("StopUsing");
        }

        void FadeText()

        {


            if (displayInfo)
            {

                myText.text = myString;
                myText.color = Color.Lerp(myText.color, j, fadeTime * Time.deltaTime);
                h.color = i;

            }

            else
            {

                myText.color = Color.Lerp(myText.color, Color.clear, fadeTime * Time.deltaTime);
                h.color = Color.Lerp(myText.color, Color.clear, fadeTime * Time.deltaTime);

            }




        }

        protected void Start()
        {
            i = new Color(.2f, .3f, .8f, .5f);
            j = new Color(0, 0, 0);

            myText.color = Color.clear;
        }

        protected override void Update()
        {
            base.Update();
            FadeText();
        }
    }
