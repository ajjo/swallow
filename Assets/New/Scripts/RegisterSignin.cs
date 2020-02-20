using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegisterSignin : MonoBehaviour
{
    public Text username;
    public Text password;

    public void Login()
    {
        if(!string.IsNullOrEmpty(username.text) && username.text.Length > 4 && !string.IsNullOrEmpty(password.text) && password.text.Length > 4)
        {
            Debug.Log("Attempting to login");
            PlayFabManager.instance.Login(username.text, password.text);
        }
    }
}
