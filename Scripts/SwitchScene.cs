using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Valve.VR.InteractionSystem;

public class SwitchScene : MonoBehaviour
{
    public string SceneName;
    Button LoadSceneBtn;
    public GameObject Player1;
    private void Start()
    {
        LoadSceneBtn = GetComponent<Button>();
        LoadSceneBtn.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(SceneName);
            if (Player.instance != null)
            {
                Destroy(Player1);
            }
        });
    }
}
