using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PointerTravel : MonoBehaviour {

    public static GameObject pointer = null;

    void Awake()
    {
        // DontDestroyOnLoad(cameraRig);
        // DontDestroyOnLoad(VRTK);
        if (pointer == null)
        {
            pointer = gameObject;
        }
        else if (pointer != gameObject)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

    }

    
}
