using UnityEngine;
using UnityEngine.SceneManagement;
public class Loading : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Invoke("loadGame", 1f);
    }
    public void loadGame()
    {
        SceneManager.LoadScene("Game");
    }
    // Update is called once per frame
    void Update()
    {

    }
}
