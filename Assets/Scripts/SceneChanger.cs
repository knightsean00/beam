using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public void Scene1() {
        SceneManager.LoadScene("LevelProto");
    }

    public void Credits() {
        SceneManager.LoadScene("Credits");
    }

    public void Title() {
        SceneManager.LoadScene("Title");
    }
}
