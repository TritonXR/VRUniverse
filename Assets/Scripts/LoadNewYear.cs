namespace VRTK.Examples.Utilities { 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNewYear : MonoBehaviour {

    //public KeyCode MyKey;
    public string sceneName;
       

    private bool canPress;
    private uint controllerIndex;

    public string MyTrigger;
    public float duration = 5.0F;
    public Color color0 = Color.red;
    public Color color1 = Color.blue;
    public Light lt;
    public GameObject lightObject;

    /*
    private void Awake()
    {
        canPress = false;

            //RadialMenu yearChanger = GameObject.Find("Radial Menu");
        Invoke("ResetPress", 1f);
        DynamicGI.UpdateEnvironment();
       

    }

    // Use this for initialization
    void Start () {
            lightObject = GameObject.Find("Directional Light");
            lt = lightObject.GetComponent<Light>();
    }

    private bool ForwardPressed()
    {
        if (controllerIndex >= uint.MaxValue)
        {
            return false;
        }
        if (canPress && VRTK_SDK_Bridge.IsTouchpadPressedOnIndex(controllerIndex))
        {
            return true;
        }
        return false;
    }

    private void ResetPress()
    {
        canPress = true;
    }

    // Update is called once per frame
    void Update () {
            var leftHand = VRTK_DeviceFinder.GetControllerLeftHand(true);
            //var rightHand = VRTK_DeviceFinder.GetControllerRightHand(true);
            controllerIndex = VRTK_DeviceFinder.GetControllerIndex(leftHand); */
            
            /*if (ForwardPressed() || Input.GetKeyUp(KeyCode.Space))
            {
                var nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
                if (nextSceneIndex >= SceneManager.sceneCountInBuildSettings)
                {
                    nextSceneIndex = 0;
                }
                SceneManager.LoadScene(nextSceneIndex);
            }*/


            /*if (Input.GetKeyUp(MyKey))
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        }
	}*/

        /*IEnumerator Travel()
        {
            GetComponent<Animator>().SetTrigger(MyTrigger);
            float t = Mathf.PingPong(Time.time, duration) / duration;
            lt.color = Color.Lerp(color0, color1, t);
            yield return new WaitForSecondsRealtime(3);
            SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
            //yield return null; 

        }*/

        public void loadHomeScene()
        {
            //var leftHand = VRTK_DeviceFinder.GetControllerLeftHand(true);
            //controllerIndex = VRTK_DeviceFinder.GetControllerIndex(leftHand);

            //if (ForwardPressed() || Input.GetKeyUp(KeyCode.Space))
            //{
                SceneManager.LoadScene("MainUniverse", LoadSceneMode.Single);
            Debug.Log("load home");
            //}
        }

        public void load2015()
        {
            //var leftHand = VRTK_DeviceFinder.GetControllerLeftHand(true);
           // controllerIndex = VRTK_DeviceFinder.GetControllerIndex(leftHand);

            //if (ForwardPressed() || Input.GetKeyUp(KeyCode.Space))
            //{
                SceneManager.LoadScene("2015", LoadSceneMode.Single);
            Debug.Log("load 2015");
            //}
        }

        public void load2016()
        {
           // var leftHand = VRTK_DeviceFinder.GetControllerLeftHand(true);
            //var rightHand = VRTK_DeviceFinder.GetControllerRightHand(true);
          //  controllerIndex = VRTK_DeviceFinder.GetControllerIndex(leftHand);

            //if (ForwardPressed() || Input.GetKeyUp(KeyCode.Space))
            //{
                SceneManager.LoadScene("2016", LoadSceneMode.Single);
            Debug.Log("load 2016");
            //}
        }

        public void load2017()
        {
            //var leftHand = VRTK_DeviceFinder.GetControllerLeftHand(true);
            //var rightHand = VRTK_DeviceFinder.GetControllerRightHand(true);
           // controllerIndex = VRTK_DeviceFinder.GetControllerIndex(leftHand);

            //if (ForwardPressed() || Input.GetKeyUp(KeyCode.Space))
            //{
                SceneManager.LoadScene("2017", LoadSceneMode.Single);
            Debug.Log("load 2017");
            //}
        }

        public void load2018()
        {
            //var leftHand = VRTK_DeviceFinder.GetControllerLeftHand(true);
            //var rightHand = VRTK_DeviceFinder.GetControllerRightHand(true);
           // controllerIndex = VRTK_DeviceFinder.GetControllerIndex(leftHand);

           // if (ForwardPressed() || Input.GetKeyUp(KeyCode.Space))
            //{
                SceneManager.LoadScene("2018", LoadSceneMode.Single);
            Debug.Log("load 2018");
            // }
        }
    }

    
}
