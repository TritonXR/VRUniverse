using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class YearTravel : MonoBehaviour {

    public static GameObject vrtk = null;

    void Awake()
    {
        // DontDestroyOnLoad(cameraRig);
        // DontDestroyOnLoad(VRTK);
        if (vrtk == null)
        {
            vrtk = gameObject;
        }
        else if (vrtk != gameObject)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

    }

    
}
