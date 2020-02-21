using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegisterSignin : MonoBehaviour
{
    public Text username;
    public Text password;
    public GameManager gameManager;
    public Toggle save;

    public void Login()
    {
        string un = username.text;
        string pwd = password.text;

        // Get the username and password from player prefs
        if (PlayerPrefs.HasKey("SAVED_INFO"))
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
        if(save.isOn && !PlayerPrefs.HasKey("SAVED_INFO"))
        {
            string info = un + ":" + pwd;
            PlayerPrefs.SetString("SAVED_INFO", info);
        }
    }

    private void LoginStatus(bool status)
    {
        if(status)
        {
            gameManager.StartGame();
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


    private void RegisterStatus(bool status)
    {
        if (status)
        {
            gameManager.StartGame();
        }
    }
}
