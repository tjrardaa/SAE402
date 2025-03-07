using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public void LoadCreditsScene()
    {
        SceneManager.LoadScene("Credits");
    }
}
