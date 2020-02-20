using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class PlayFabManager
{
    private string playFabId;
    private static PlayFabManager singleton;

    private PlayFabManager()
    {

    }

    public static PlayFabManager instance
    {
        get
        {
            if (singleton == null)
                singleton = new PlayFabManager();
            return singleton;
        }
    }

    public void Login(string username, string password)
    {

        LoginWithPlayFabRequest req = new LoginWithPlayFabRequest();
        req.Username = username;
        req.Password = password;
        req.TitleId = PlayFabSettings.TitleId;
        PlayFabClientAPI.LoginWithPlayFab(req, OnLoginSuccess, OnLoginFailure);
    }

    private void OnLoginSuccess(LoginResult login)
    {
        playFabId = login.PlayFabId;
        Debug.Log("New Account = " + login.NewlyCreated.ToString());
        Debug.Log("Login succeeded");
    }

    private void OnLoginFailure(PlayFabError error)
    {
        Debug.Log("Login failed");
    }
}
