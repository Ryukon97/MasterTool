using UnityEngine;
using UnityEngine.SceneManagement;

public class SenceManager :MonoBehaviour
{
    public void NextSence()
    {
        SceneManager.LoadScene(1);
    }
}
