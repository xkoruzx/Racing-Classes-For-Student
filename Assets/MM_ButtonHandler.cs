using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MM_ButtonHandler : MonoBehaviour
{
    // Start is called before the first frame update

    public Button playButton, quitButton;

    void Start()
    {
        Cursor.visible = false;
    }

    // Update is called once per frame
    void QuitGame()
    {
        Application.Quit();
    }

    void NextScene()
    {
        SceneManager.LoadScene("CharacterSelect(Today)");
    }

}
