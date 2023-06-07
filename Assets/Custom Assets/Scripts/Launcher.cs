using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace Com.MyCompany.MyGame
{
    public class Launcher : MonoBehaviourPunCallbacks
    {

        #region Private Serializable Fields

            [Tooltip("The max number of players per room")]
            [SerializeField]
            private byte maxPlayersPerRoom = 4;

            [Tooltip("control panel gameObject")]
            [SerializeField]
            private GameObject controlPanel_GO;

            [Tooltip("progress text gameObject")]
            [SerializeField]
            private GameObject progressText_GO;

            bool isConnecting;

        #endregion

        #region Private Fields

            /// &lt;summary&gt;
            /// This client's version number. Users are separated from each other by gameVersion (which allows you to make breaking changes).
            /// &lt;/summary&gt;
            string gameVersion = "1";

        #endregion

        #region MonoBehaviour CallBacks

            /// &lt;summary&gt;
            /// MonoBehaviour method called on GameObject by Unity during early initialization phase.
            /// &lt;/summary&gt;
            void Awake()
            {
                // #Critical
                // this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
                PhotonNetwork.AutomaticallySyncScene = true;
            }

            /// &lt;summary&gt;
            /// MonoBehaviour method called on GameObject by Unity during initialization phase.
            /// &lt;/summary&gt;
            void Start()
            {
                // Connect();
                ToggleControlPanelAndProgreeText(true);
            }

            void Update()
            {
                
            }

        #endregion

        #region Public Methods

            /// &lt;summary&gt;
            /// Start the connection process.
            /// - If already connected, we attempt joining a random room
            /// - if not yet connected, Connect this application instance to Photon Cloud Network
            /// &lt;/summary&gt;
            public void Connect()
            {
                ToggleControlPanelAndProgreeText(false);

                // check if we are connected or not
                if(PhotonNetwork.IsConnected)
                {
                    PhotonNetwork.JoinRandomRoom();
                }
                else
                {
                    isConnecting = PhotonNetwork.ConnectUsingSettings();
                    PhotonNetwork.GameVersion = gameVersion;
                }
            }

        #endregion

        #region Private Methods

            void ToggleControlPanelAndProgreeText(bool ctrlPanelToggle_pr)
            {
                controlPanel_GO.SetActive(ctrlPanelToggle_pr);
                progressText_GO.SetActive(!ctrlPanelToggle_pr);
            }

        #endregion

        #region MonoBehaviorPunCallbacks Callbacks

            public override void OnConnectedToMaster()
            {
                Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster was called by PUN");

                if (isConnecting)
                {
                    // #Critical: The first we try to do is to join a potential existing room. If there is, good, else, we'll be called back with OnJoinRandomFailed()
                    PhotonNetwork.JoinRandomRoom();
                    isConnecting = false;
                }
            }

            public override void OnDisconnected(DisconnectCause cause)
            {
                Debug.LogWarningFormat("PUN Basics Tutorial/Launcer: OnDisconnected() was called by PUN with reason {0}", cause);

                ToggleControlPanelAndProgreeText(true);
            }

        #endregion

            public override void OnJoinRandomFailed(short returnCode, string message)
            {
                Debug.Log("PUN Basics Tutorial/Launcher: OnJoinRandomFailed() was called by PUN. No");

                PhotonNetwork.CreateRoom(null, new RoomOptions{ MaxPlayers = maxPlayersPerRoom });
            }

            public override void OnJoinedRoom()
            {
                Debug.Log("PUN Basics Tutorial/Launcher: Now this client is in a room");

                if(PhotonNetwork.CurrentRoom.PlayerCount == 1)
                {
                    Debug.Log("We load the 'Room for 1' ");

                    PhotonNetwork.LoadLevel("Room for 1");
                }
            }
    }
}

