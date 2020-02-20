using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegisterSignin : MonoBehaviour
{
    public Text username;
    public Text password;
    public GameManager gameManager;

    public void Login()
    {
        if(!string.IsNullOrEmpty(username.text) && username.text.Length > 4 && !string.IsNullOrEmpty(password.text) && password.text.Length > 4)
        {
            Debug.Log("Attempting to login");
            PlayFabManager.instance.Login(username.text, password.text, LoginStatus);
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
