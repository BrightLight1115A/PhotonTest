using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using Photon.Pun;
using Photon.Realtime;

namespace Com.MyCompany.MyGame
{
    /// <summary>
    /// player name input field
    /// <summary>
    // [RequireComponent(typeof(InputField))]
    public class PlayerNameInputField : MonoBehaviour
    {

        #region Private Constants

        const string playerNamePrefKey = "PlayerName";

        #endregion

        #region MonoBehavior CallBacks

        // Start is called before the first frame update
        void Start()
        {
            string defaultName = string.Empty;
            TMP_InputField _inputField = gameObject.GetComponent<TMP_InputField>();
            if(_inputField != null)
            {
                if(PlayerPrefs.HasKey(playerNamePrefKey))
                {
                    defaultName = PlayerPrefs.GetString(playerNamePrefKey);
                    _inputField.text = defaultName;
                }
            }

            PhotonNetwork.NickName = defaultName;
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        #endregion

        #region Public Methods

        public void SetPlayerName(string value)
        {
            // important
            if(string.IsNullOrEmpty(value))
            {
                Debug.LogError("PlayerName is null or empty");
            }
            PhotonNetwork.NickName = value;

            PlayerPrefs.SetString(playerNamePrefKey, value);
        }

        #endregion
    }
    
}
