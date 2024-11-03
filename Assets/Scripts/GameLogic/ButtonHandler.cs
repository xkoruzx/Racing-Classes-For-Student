using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonHandler : MonoBehaviour
{
    public Button backButton, goButton, noButton, yesButton, startMPButton;
        
    public GameObject confirmPanel, player;

    public Text connectText;

    // Start is called before the first frame update
    void Start()
    {
        goButton.onClick.AddListener(delegate { PopupConfirm(); });
        noButton.onClick.AddListener(delegate { Cancel(); });
        yesButton.onClick.AddListener(delegate { Proceed(); });
        backButton.onClick.AddListener(delegate { Back(); });

    }

    void Back()
    {
        SceneManager.LoadScene("MainMenu");
    }

    void PopupConfirm()
    {
        confirmPanel.SetActive(true);
    }

    void Proceed()
    {
        confirmPanel.SetActive(false);
        connectingPanel.SetActive(true);
    }

    void Cancel()
    {
        confirmPanel.SetActive(false);
    }
}
