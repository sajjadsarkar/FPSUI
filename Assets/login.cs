using UnityEngine;
using UnityEngine.SceneManagement;

public class login : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    public void Login()
    {
        SceneManager.LoadScene("Menu");
    }

}
