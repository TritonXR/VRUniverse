namespace VRTK.Examples.Utilities { 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * OBSOLETE
 * TO BE REMOVED FROM FINAL PRODUCT
 */

public class LoadNewYear : MonoBehaviour {

    //public KeyCode MyKey;
    public string sceneName;
       

    private bool canPress;
    private uint controllerIndex;

    //public string MyTrigger;
    public float duration = 5.0F;
    public Color color0 = Color.red;
    public Color color1 = Color.blue;
    public Light lt;
    public GameObject lightObject;
    public GameObject particleWarpObject;
    public ParticleSystem hyperspeed;
    public AudioSource hyperspeedSound;

    public GameObject cameraRig;

    /*
    private void Awake()
    {
        canPress = false;

            //RadialMenu yearChanger = GameObject.Find("Radial Menu");
        Invoke("ResetPress", 1f);
        DynamicGI.UpdateEnvironment();
       

    }*/

    // Use this for initialization
    void Start () {

            hyperspeedSound = gameObject.GetComponent<AudioSource>();

            lightObject = GameObject.Find("Directional Light");
            lt = lightObject.GetComponent<Light>();
            particleWarpObject = GameObject.Find("Warp");
            hyperspeed = particleWarpObject.GetComponent<ParticleSystem>();
            hyperspeed.Pause();

    }

    /*private bool ForwardPressed()
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
    }*/

    // Update is called once per frame
    void Update () {
            //var leftHand = VRTK_DeviceFinder.GetControllerLeftHand(true);
            //var rightHand = VRTK_DeviceFinder.GetControllerRightHand(true);
            //controllerIndex = VRTK_DeviceFinder.GetControllerIndex(leftHand);
            //Debug.Log("test");

            //if (ForwardPressed() || Input.GetKeyUp(KeyCode.Space))
            if (Input.GetKeyDown(KeyCode.Space))
            {
                /*var nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
                if (nextSceneIndex >= SceneManager.sceneCountInBuildSettings)
                {
                    nextSceneIndex = 0;
                }*/
                //SceneManager.LoadScene(nextSceneIndex);
                Debug.Log("Starting the travel coroutine");
                StartCoroutine(Travel("2016"));
                
            }


            /*if (Input.GetKeyUp(MyKey))
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        }*/
	}

        IEnumerator Travel(string sceneName)
        {

            hyperspeed.Play();
            Debug.Log("play warp");
            if (lt != null)
            {
                hyperspeedSound.Play();
                for (float i = 0; i < 2; i += Time.deltaTime)
                {
                    lt.intensity = Mathf.Lerp(1f, 0.5f, i / 2.0f);
                    yield return null;
                }
            }
            yield return new WaitForSecondsRealtime(1);
            if (lt == null)
            {
                cameraRig.transform.position = gameObject.transform.position;
            }
            hyperspeedSound.Stop();
            SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
            yield return new WaitForSecondsRealtime(1);
            lightObject = GameObject.Find("Directional Light");
            if (lightObject != null)
            {
                //hyperspeedSound.Play();
                lt = lightObject.GetComponent<Light>();
                for (float i = 0; i < 2; i += Time.deltaTime)
                {
                    lt.intensity = Mathf.Lerp(0.5f, 1f, i / 2.0f);
                    yield return null;
                }
            }
            yield return new WaitForSecondsRealtime(1);
            //hyperspeedSound.Stop();
            hyperspeed.Stop();
            Debug.Log("stop warp");
            //yield return null; 

        }

        public void loadHomeScene()
        {
            //var leftHand = VRTK_DeviceFinder.GetControllerLeftHand(true);
            //controllerIndex = VRTK_DeviceFinder.GetControllerIndex(leftHand);

            //if (ForwardPressed() || Input.GetKeyUp(KeyCode.Space))
            //{
            //SceneManager.LoadScene("MainUniverse", LoadSceneMode.Single);
            StartCoroutine(Travel("MainUniverse"));
            Debug.Log("load home");
            //}
        }

        public void load2015()
        {
            //var leftHand = VRTK_DeviceFinder.GetControllerLeftHand(true);
            // controllerIndex = VRTK_DeviceFinder.GetControllerIndex(leftHand);

            //if (ForwardPressed() || Input.GetKeyUp(KeyCode.Space))
            //{
            //SceneManager.LoadScene("2015", LoadSceneMode.Single);
            StartCoroutine(Travel("2015"));
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
            //SceneManager.LoadScene("2016", LoadSceneMode.Single);
            StartCoroutine(Travel("2016"));
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
            //SceneManager.LoadScene("2017", LoadSceneMode.Single);
            StartCoroutine(Travel("2017"));
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
            //SceneManager.LoadScene("2018", LoadSceneMode.Single);
            StartCoroutine(Travel("2018"));
            Debug.Log("load 2018");
            // }
        }
    }

    
}
