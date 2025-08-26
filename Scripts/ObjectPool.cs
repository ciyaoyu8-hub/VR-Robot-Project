using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    private static ObjectPool instance;

    public static ObjectPool Instance
    {
        get { return instance; }
        set { instance = value; }
    }

    Dictionary<string, List<GameObject>> dict_path_list_GO;
    public GameObject Create(string path)
    {
        return Create(path, Vector3.zero, Quaternion.identity);
    }
    public GameObject Create(string path, Transform parent)
    {
        return Create(path, Vector3.zero, Quaternion.identity, 1, parent);
    }
    public GameObject Create(string path, Vector3 pos, float size)
    {
        return Create(path, pos, Quaternion.identity, size, null);
    }
    public GameObject Create(string path, Vector3 pos, Vector3 euler)
    {
        return Create(path, pos, Quaternion.Euler(euler));
    }
    public GameObject Create(string path, Vector3 pos, Quaternion qua)
    {
        return Create(path, pos, qua, 1, null);
    }
    public GameObject Create(string path, Vector3 pos, Quaternion qua, float size, Transform parent)
    {
        GameObject go = LoadGO(path, parent);
        go.transform.localPosition = pos;
        go.transform.localRotation = qua;
        go.transform.localScale = Vector3.one * size;
        go.SetActive(true);
        return go;
    }


    public void Destroy(string path)
    {
        if (dict_path_list_GO.ContainsKey(path))
        {
            foreach (var item in dict_path_list_GO[path])
            {
                Destroy(item);
            }
            dict_path_list_GO.Remove(path);
        }
    }
    public void Destroy()
    {
        foreach (var item in dict_path_list_GO.Keys)
        {
            Destroy(item);
        }
    }
    public void Recycle(GameObject go)
    {
        go.SetActive(false);
    }
    public Coroutine Recycle(GameObject go, float delay)
    {
        return StartCoroutine(DelayRecycle(go, delay));
    }
    IEnumerator DelayRecycle(GameObject go, float delay)
    {
        yield return new WaitForSeconds(delay);
        go.SetActive(false);
    }
    /// <summary>
    /// 取消延时,并立即释放
    /// </summary>
    public void CancelDelay2Recycle(KeyValuePair<GameObject, Coroutine> keyValuePair)
    {
        if (keyValuePair.Key.activeSelf)
        {
            StopCoroutine(keyValuePair.Value);
            keyValuePair.Key.SetActive(false);
        }

    }
    private void Awake()
    {
        instance = this;
        dict_path_list_GO = new Dictionary<string, List<GameObject>>();

    }
    void LoadGO(out GameObject go, string path)
    {
        go = Instantiate(Factory.Instance.Load(path));
        go.SetActive(false);
        dict_path_list_GO[path].Add(go);
    }
    GameObject LoadGO(string path, Transform parent)
    {
        GameObject go = null;
        if (!dict_path_list_GO.ContainsKey(path))
        {
            dict_path_list_GO.Add(path, new List<GameObject>());
            LoadGO(out go, path);
        }
        else
        {
            foreach (var item in dict_path_list_GO[path])
            {
                if (!item.activeSelf)
                {
                    go = item;

                    break;
                }
            }
            if (go == null)
            {
                LoadGO(out go, path);
            }
        }
        go.transform.SetParent(parent);
        return go;
    }
}