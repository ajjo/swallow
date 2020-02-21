using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;

public class RegisterSignin : MonoBehaviour
{
    public Text username;
    public Text password;
    public GameManager gameManager;
    public Toggle save;
    public Text error;

    public bool canLogin()
    {
        return PlayerPrefs.HasKey("SAVED_INFO");
    }

    public void Login()
    {
        string un = username.text;
        string pwd = password.text;

        // Get the username and password from player prefs
        if (canLogin())
        {
            string creds = PlayerPrefs.GetString("SAVED_INFO");
            string [] c = creds.Split(':');
            un = c[0];
            pwd = c[1];
        }

        if(un.Length > 4 && pwd.Length > 4)
        {
            Debug.Log("Attempting to login");
            PlayFabManager.instance.Login(un, pwd, LoginStatus);
        }

        // Save player prefs
        if(save.isOn && !canLogin())
        {
            string info = un + ":" + pwd;
            PlayerPrefs.SetString("SAVED_INFO", info);
        }
    }

    public void ChangeSaveStatus(bool isOn)
    {
        Debug.Log("ISOn  = " + isOn.ToString());
        save.isOn = isOn;
    }

    private void LoginStatus(bool status, System.Object obj)
    {
        if(status)
        {
            LoginResult lg = (LoginResult)obj;              
            gameManager.StartGame();
        } else
        {
            PlayFabError err = (PlayFabError)obj;
            error.text = err.ErrorMessage;
        }
    }

    public void Register()
    {
        if (!string.IsNullOrEmpty(username.text) && username.text.Length > 4 && !string.IsNullOrEmpty(password.text) && password.text.Length > 4)
        {
            Debug.Log("Attempting to register");
            PlayFabManager.instance.Register(username.text, password.text, RegisterStatus);
        }
    }

    private void RegisterStatus(bool status, System.Object obj)
    {
        if (status)
        {
            RegisterPlayFabUserResult result = (RegisterPlayFabUserResult)obj;
            gameManager.StartGame();
        } else
        {
            PlayFabError err = (PlayFabError)obj;
            error.text = err.ErrorMessage;
        }
    }
}
