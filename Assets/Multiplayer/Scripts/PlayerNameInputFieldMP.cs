using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

namespace Com.CodaKid.KartRacing
{

    
    public class PlayerNameInputFieldMP : MonoBehaviour
    {
        InputField _inputField;

        #region Private Constants

        const string playerNamePrefKey = "PlayerName";

        #endregion

        #region MonoBehaviourCallbacks

        // Start is called before the first frame update
        void Start()
        {
            string defaultName = string.Empty;
            _inputField = this.GetComponent<InputField>();
            if (_inputField != null)
            {
                if (PlayerPrefs.HasKey(playerNamePrefKey))
                {
                    defaultName = PlayerPrefs.GetString(playerNamePrefKey);
                    _inputField.text = defaultName;
                }
            }

            PhotonNetwork.NickName = defaultName;
        }

        #endregion

        #region Public Methods

        public void SetPlayerName()
        {
            if (string.IsNullOrEmpty(_inputField.text))
            {
                Debug.LogError("Player name is null or empty");
                return;
            }
            PhotonNetwork.NickName = _inputField.text;

            PlayerPrefs.SetString(playerNamePrefKey, _inputField.text);
            //Debug.Log("Player name set to " + _inputField.text);
        }

        #endregion
    }
}

