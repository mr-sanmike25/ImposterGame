using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
using WebSocketSharp;

public class PhotonConnection : MonoBehaviourPunCallbacks
{
    /// Date: 11/09/2024
    /// Author: Miguel Angel Garcia Elizalde y Alan Elias Carpinteyro Gastelum.
    /// Brief: Código para conectar a cuartos y servidores de Photon.

    //[SerializeField] TMP_InputField m_roomInputField;
    [SerializeField] TMP_InputField m_newNickname;

    //[SerializeField] TextMeshProUGUI m_joinRoomFailedTextMeshProUGUI;
    //[SerializeField] TextMeshProUGUI m_createRoomFailedTextMeshProUGUI;
    [SerializeField] TextMeshProUGUI m_nicknameFailedTextMeshProUGUI;

    [SerializeField] Button m_playButton;
    [SerializeField] Button m_exitButton;
    [SerializeField] Button m_backButton;
    [SerializeField] Button m_vsModeButton;
    [SerializeField] Button m_coopModeButton;

    [SerializeField] GameObject m_menuModePanel;
    [SerializeField] GameObject m_selectModePanel;

    [SerializeField] Canvas m_loadingCanvas;
    [SerializeField] Canvas m_menuCanvas;

    [SerializeField] int gameMode = 0;

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();

        //StartCoroutine(ActivateCanvasAgain());
    }

    /*IEnumerator ActivateCanvasAgain()
    {
        yield return new WaitForSeconds(2.0f);

        print("Se ha entrado al Lobby Abstracto");
        m_loadingCanvas.gameObject.SetActive(false);
        m_menuCanvas.gameObject.SetActive(true);
        PhotonNetwork.NickName = m_newNickname.text;
    }*/

    public override void OnConnectedToMaster()
    {
        print("Se ha conectado al Master");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        print("Se ha entrado al Lobby Abstracto");
        m_loadingCanvas.gameObject.SetActive(false);
        m_menuCanvas.gameObject.SetActive(true);
        PhotonNetwork.NickName = m_newNickname.text;
    }

    public override void OnJoinedRoom()
    {
        print("Se entró al room");
        PhotonNetwork.LoadLevel("Gameplay");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        /*print("Hubo un error al crear el room: " + message);
        m_createRoomFailedTextMeshProUGUI.gameObject.SetActive(true);
        m_createRoomFailedTextMeshProUGUI.text = "Hubo un error al crear el room: " + m_roomInputField.text;*/
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        print("Hubo un error al entrar el room: " + message);
        /*m_joinRoomFailedTextMeshProUGUI.gameObject.SetActive(true);
        m_joinRoomFailedTextMeshProUGUI.text = "Hubo un error al entrar al room: " + m_roomInputField.text;*/
    }

    RoomOptions NewRoomInfo()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;
        roomOptions.IsOpen = true;
        roomOptions.IsVisible = true;

        return roomOptions;
    }

    //public void joinVSDefaultRoom()
    //{
    //    if (m_newNickname.text.IsNullOrEmpty())
    //    {
    //        m_nicknameFailedTextMeshProUGUI.gameObject.SetActive(true);
    //        m_nicknameFailedTextMeshProUGUI.text = "Introduce un nombre, no lo dejes en blanco.";
    //        print("Necesita un nombre.");
    //        return;
    //    }
    //    else
    //    {
    //        PhotonNetwork.NickName = m_newNickname.text;
    //    }

    //    /*if (m_newNickname.text == "")
    //    {
    //        m_joinRoomFailedTextMeshProUGUI.text = "Este espacio no lo puedes dejar en blanco.";
    //        m_joinRoomFailedTextMeshProUGUI.gameObject.SetActive(true);
    //    }
    //    else
    //    {
    //        PhotonNetwork.JoinRoom(m_roomInputField.text);
    //    }*/

    //    PhotonNetwork.JoinRoom("VSDefaultRoom");
    //}

    public void JoinOrCreateVSDefaultRoom()
    {
        if (m_newNickname.text.IsNullOrEmpty())
        {
            m_nicknameFailedTextMeshProUGUI.gameObject.SetActive(true);
            m_nicknameFailedTextMeshProUGUI.text = "Introduce un nombre, no lo dejes en blanco.";
            print("Necesita un nombre.");
            return;
        }
        else
        {
            PhotonNetwork.NickName = m_newNickname.text;
        }

        /*if (m_roomInputField.text == "")
        {
            m_createRoomFailedTextMeshProUGUI.text = "Este espacio no lo puedes dejar en blanco.";
            m_createRoomFailedTextMeshProUGUI.gameObject.SetActive(true);
        }
        else
        {
            PhotonNetwork.CreateRoom(m_roomInputField.text, NewRoomInfo(), null);
        }*/

        PhotonNetwork.JoinOrCreateRoom("VSDefaultRoom", NewRoomInfo(), null);
    }

    //public void JoinCoopDefaultRoom()
    //{
    //    if (m_newNickname.text.IsNullOrEmpty())
    //    {
    //        m_nicknameFailedTextMeshProUGUI.gameObject.SetActive(true);
    //        m_nicknameFailedTextMeshProUGUI.text = "Introduce un nombre, no lo dejes en blanco.";
    //        print("Necesita un nombre.");
    //        return;
    //    }
    //    else
    //    {
    //        gameMode = 2;
    //        PhotonNetwork.NickName = m_newNickname.text;
    //    }

    //    /*if (m_newNickname.text == "")
    //    {
    //        m_joinRoomFailedTextMeshProUGUI.text = "Este espacio no lo puedes dejar en blanco.";
    //        m_joinRoomFailedTextMeshProUGUI.gameObject.SetActive(true);
    //    }
    //    else
    //    {
    //        PhotonNetwork.JoinRoom(m_roomInputField.text);
    //    }*/

    //    PhotonNetwork.JoinRoom("CoopDefaultRoom");
    //}

    public void JoinOrCreateCoopDefaultRoom()
    {
        if (m_newNickname.text.IsNullOrEmpty())
        {
            m_nicknameFailedTextMeshProUGUI.gameObject.SetActive(true);
            m_nicknameFailedTextMeshProUGUI.text = "Introduce un nombre, no lo dejes en blanco.";
            print("Necesita un nombre.");
            return;
        }
        else
        {
            PhotonNetwork.NickName = m_newNickname.text;
        }

        /*if (m_roomInputField.text == "")
        {
            m_createRoomFailedTextMeshProUGUI.text = "Este espacio no lo puedes dejar en blanco.";
            m_createRoomFailedTextMeshProUGUI.gameObject.SetActive(true);
        }
        else
        {
            PhotonNetwork.CreateRoom(m_roomInputField.text, NewRoomInfo(), null);
        }*/
         
        PhotonNetwork.JoinOrCreateRoom("VSDefaultRoom", NewRoomInfo(), null);
    }

    public void PlayButton()
    {
        if(!m_newNickname.text.IsNullOrEmpty())
        {
            m_nicknameFailedTextMeshProUGUI.gameObject.SetActive(false);

            m_menuModePanel.SetActive(false);
            m_selectModePanel.SetActive(true);

            //m_roomInputField.gameObject.SetActive(false);
            m_newNickname.gameObject.SetActive(false);

            m_playButton.gameObject.SetActive(false);
            m_exitButton.gameObject.SetActive(false);
            m_backButton.gameObject.SetActive(true);
            m_vsModeButton.gameObject.SetActive(true);
            m_coopModeButton.gameObject.SetActive(true);
        }
        else
        {
            m_nicknameFailedTextMeshProUGUI.gameObject.SetActive(true);
            m_nicknameFailedTextMeshProUGUI.text = "Introduce un nombre, no lo dejes en blanco.";
            return;
        }
    }

    public void ExitButton()
    {
        Application.Quit();
    }

    public void BackButton()
    {
        m_menuModePanel.SetActive(true);
        m_selectModePanel.SetActive(false);

        //m_roomInputField.gameObject.SetActive(true);
        m_newNickname.gameObject.SetActive(true);

        m_playButton.gameObject.SetActive(true);
        m_exitButton.gameObject.SetActive(true);
        m_backButton.gameObject.SetActive(false);
        m_vsModeButton.gameObject.SetActive(false);
        m_coopModeButton.gameObject.SetActive(false);

        gameMode = 0;
    }
}
