using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Com.CodaKid.CodaKart;
using Photon.Pun;

public class ButtonHandler : MonoBehaviour
{
    public Button backButton, goButton, noButton, yesButton, startMPButton;
        
    public GameObject confirmPanel, player, connectingPanel;

    public Text connectText;

    // Start is called before the first frame update
    void Start()
    {
        goButton.onClick.AddListener(delegate { PopupConfirm(); });
        noButton.onClick.AddListener(delegate { Cancel(); });
        yesButton.onClick.AddListener(delegate { Proceed(); });
        backButton.onClick.AddListener(delegate { Back(); });
        startMPButton.onClick.AddListener(delegate { StartGame(); });

    }

    void StartGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            GameManagerMP gameManager = GameObject.Find("GameManager").GetComponent<GameManagerMP>();
            gameManager.LoadGame();
        }
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
