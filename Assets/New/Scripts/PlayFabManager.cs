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

    public void Register(string username, string password, System.Action<bool> callback)
    {
        RegisterPlayFabUserRequest req = new RegisterPlayFabUserRequest();
        req.Username = username;
        req.Password = password;
        req.TitleId = PlayFabSettings.TitleId;
        req.RequireBothUsernameAndEmail = false;

        PlayFabClientAPI.RegisterPlayFabUser(req, (register) => { callback(true);  }, (error) => { callback(false); });

        //PlayFabClientAPI.RegisterPlayFabUser(req, OnRegisterSuccess, OnRegisterFailure);
    }

    private void OnRegisterSuccess(RegisterPlayFabUserResult register)
    {
        Debug.Log("Register succeeeded");
        playFabId = register.PlayFabId;
    }

    private void OnRegisterFailure(PlayFabError error)
    {
        Debug.Log("Register failed = " + error.ErrorMessage);
    }

    public void Login(string username, string password, System.Action<bool> callback)
    {

        LoginWithPlayFabRequest req = new LoginWithPlayFabRequest();
        req.Username = username;
        req.Password = password;
        req.TitleId = PlayFabSettings.TitleId;
        PlayFabClientAPI.LoginWithPlayFab(req, (login) => { callback(true); }, (error) => { callback(false); });
    }

    private void OnLoginSuccess(LoginResult login)
    {
        playFabId = login.PlayFabId;
        Debug.Log("New Account = " + login.NewlyCreated.ToString());
        Debug.Log("Login succeeded");
    }

    private void OnLoginFailure(PlayFabError error)
    {
        Debug.Log("Login failed = " + error.ErrorMessage);
    }
}
