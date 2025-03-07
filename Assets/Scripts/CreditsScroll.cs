using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsScroll : MonoBehaviour
{
    public float scrollSpeed = 50f;
    public Transform endPoint;

    void Update()
    {
        if (transform.position.y < endPoint.position.y)
        {
            transform.Translate(Vector3.up * scrollSpeed * Time.deltaTime);
        }
        else
        {
            SceneManager.LoadScene("MainMenu"); 
        }
    }
}
