using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : MonoBehaviour
{
    private static Factory instance;

    public static Factory Instance
    {
        get { return instance; }
        set { instance = value; }
    }

    Dictionary<string, GameObject> dict;
    Dictionary<string, AudioClip> dict_AudioClip;
    private void Awake()
    {
        instance = this;
        dict = new Dictionary<string, GameObject>();
        dict_AudioClip = new Dictionary<string, AudioClip>();

    }
    public GameObject Load(string resourcesPath)
    {
        if (!dict.ContainsKey(resourcesPath))
        {
            dict.Add(resourcesPath, Resources.Load<GameObject>(resourcesPath));
        }
        return dict[resourcesPath];
    }
    public AudioClip LoadAudioSource(string resourcesPath)
    {
        if (!dict_AudioClip.ContainsKey(resourcesPath))
        {
            dict_AudioClip.Add(resourcesPath, Resources.Load<AudioClip>(resourcesPath));
        }
        return dict_AudioClip[resourcesPath];
    }

}