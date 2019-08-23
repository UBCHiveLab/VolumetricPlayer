using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System;

public class Animator : MonoBehaviour {

    // ANIMATOR
    // no memory leak

    // file name
    static string fileDir = "Model/Test5";
    static int start = 1;
    static int end = 2620;

    // frames per second, suject to rounding by int
    static float fps = 24;
    static float spf = 1 / fps;

    // offsets, experimental    
    static float posx = -3.5f;
    static float posy = 1f;
    static float posz = -11f;
    static float rotx = 0f;
    static float roty = -120f;
    static float rotz = 0f;

    Vector3 pos = new Vector3(posx, posy, posz);
    Quaternion rot = Quaternion.Euler(rotx, roty, rotz);

    // model cap to limit memory use
    static int framesQueueMax = 50;

    // frames queue
    List<GameObject> framesQueue = new List<GameObject>();
    
    // Use this for initialization
    void Start () {
        StartCoroutine("LoadFrames");
        StartCoroutine("DispFrames");
    }

    IEnumerator LoadFrames()
    {
        ResourceRequest frameToLoad;
        GameObject frameLoaded;
        GameObject frameInstantiated;
        for (int i = start; i < end + 1; i++)
        {
            if (framesQueue.Count < framesQueueMax)// check
            {
                //Debug.Log(fileDir + i);
                frameToLoad = Resources.LoadAsync(fileDir + i, typeof(GameObject));
                while(!frameToLoad.isDone)
                {
                    yield return null;
                }
                frameLoaded = frameToLoad.asset as GameObject;
                frameInstantiated = Instantiate(frameLoaded, pos, rot, this.transform);
                frameInstantiated.SetActive(false);
                //Debug.Log("loaded " + frameInstantiated.name);
                framesQueue.Add(frameInstantiated);
                
                Resources.UnloadUnusedAssets(); // very important, prevents memory leak
                //Debug.Log("queue size: " + framesQueue.Count);
            } else
            {
                i--;
                yield return null;
            }
        }
        yield return null;
    }

    IEnumerator DispFrames()
    {
        GameObject frameToDisplay;
        for (int j = start; j < end + 1; j++)
        {
            if (framesQueue.Count > 0)
            {
                frameToDisplay = framesQueue[0];
                framesQueue.RemoveAt(0);
                frameToDisplay.SetActive(true);
                //Debug.Log("displayed " + frameToDisplay.name);
                yield return new WaitForSeconds(spf);

                DestroyImmediate(frameToDisplay);
                frameToDisplay = null;

                //Debug.Log("queue size: " + framesQueue.Count);
            } else
            {
                j--;
                yield return null;
            }
        }
        yield return null;
    }
}
