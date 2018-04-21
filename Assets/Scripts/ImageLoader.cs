using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public delegate void PassRawBytes(byte[] rawData);
public delegate void PassSprite(Sprite planetImage);

public class ImageLoader : MonoBehaviour {
    public static ImageLoader instance;

    private List<byte[]> receivedDataQueue;
    private List<PassSprite> callbacks;
    private Thread loadWorker;

    // Use this for initialization
    void Start () {
        if(instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }

        receivedDataQueue = new List<byte[]>();
        callbacks = new List<PassSprite>();
        loadWorker = null;
	}
	
	// Update is called once per frame
	void Update () {
		
        lock(receivedDataQueue)
        {
            if(receivedDataQueue.Count > 0)
            {
                Debug.Log("Handling queued texture");
                if(callbacks.Count > 0)
                {
                    Texture2D texture = new Texture2D(0, 0);
                    texture.LoadImage(receivedDataQueue[0]);

                    Rect rect = new Rect(0, 0, texture.width, texture.height);
                    Sprite newSprite = Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f), 100.0f, 0, SpriteMeshType.FullRect);

                    callbacks[0](newSprite);

                    receivedDataQueue.RemoveAt(0);
                    callbacks.RemoveAt(0);
                }
                else
                {
                    Debug.Log("Clearing data queue");
                    receivedDataQueue.Clear();
                }
            }
        }
	}

    public void LoadImages(List<string> imagePaths, List<PassSprite> spriteCallbacks)
    {
        Debug.Log("Loading Images");
        if(loadWorker != null)
        {
            loadWorker.Abort();
            loadWorker = null;
        }

        if (receivedDataQueue.Count > 0) receivedDataQueue.Clear();
        if (callbacks.Count > 0) callbacks.Clear();

        foreach(PassSprite callback in spriteCallbacks)
        {
            callbacks.Add(callback);
        }

        ImageLoadWorker worker = new ImageLoadWorker(imagePaths, ReceiveTexture);

        loadWorker = new Thread(new ThreadStart(worker.LoadPlanets));
        loadWorker.Start();
    }

    public void CancelLoading()
    {
        if(loadWorker != null)
        {
            loadWorker.Abort();
            loadWorker = null;
        }
    }

    public void ReceiveTexture(byte[] rawData)
    {
        lock(receivedDataQueue)
        {
            receivedDataQueue.Add(rawData);
        }
    }

    public static ImageLoader GetInstance()
    {
        return instance;
    }
}

public class ImageLoadWorker
{
    private List<string> imagePaths;
    private PassRawBytes dataReturnCallback;

    public ImageLoadWorker(List<string> paths, PassRawBytes callback)
    {
        dataReturnCallback = callback;
        imagePaths = paths;
    }

    public void LoadPlanets()
    {
        try
        {
            while(imagePaths.Count > 0)
            {
                byte[] bytes = File.ReadAllBytes(imagePaths[0]);

                imagePaths.RemoveAt(0);

                dataReturnCallback(bytes);
            }
        }
        finally
        {
            //no cleanup really required
        }
     }
 }