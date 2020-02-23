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

    public void GetLevelsInfo(System.Action<object, int []> callback)
    {
        GetUserDataRequest req = new GetUserDataRequest();
        req.PlayFabId = playFabId;

        int[] li = new int[20];
        for (int i = 0; i < li.Length; i++)
            li[i] = -1;

        PlayFabClientAPI.GetUserData(req, (userDataResult) =>
        {
            Debug.Log("Got the result");
            if (userDataResult.Data != null && userDataResult.Data.ContainsKey("Level"))
            {
                UserDataRecord udr;
                userDataResult.Data.TryGetValue("Level", out udr);
                string[] d = udr.Value.ToString().Split(':');
                Debug.Log("Finished = " + d.Length);
                for (int i = 0; i < li.Length; i++)
                {
                    Debug.Log("I = " + i.ToString());
                    li[i] = int.Parse(d[i]);
                }
            }
            callback(userDataResult, li);
        }, (error) =>
        {
            Debug.Log("Errror " + error);
            callback(error, li);
        });
    }

    public void SetLevelInfo(int [] levels)
    {
        UpdateUserDataRequest req = new UpdateUserDataRequest();
        req.Data = new Dictionary<string, string>();
        string levelInfo = "";
        for(int i=0;i<levels.Length;i++)
        {
            levelInfo += levels[i].ToString() + ":";
        }
        Debug.Log("LevelInfo " + levelInfo);
        req.Data.Add("Level", levelInfo);

        PlayFabClientAPI.UpdateUserData(req, (userDataResult) => {
             Debug.Log("Finishes the result");
        }, (error) => {
             Debug.Log("Error " + error.ErrorMessage);
        });
    }

    public void Register(string username, string password, System.Action<bool,object> callback)
    {
        RegisterPlayFabUserRequest req = new RegisterPlayFabUserRequest();
        req.Username = username;
        req.Password = password;
        req.TitleId = PlayFabSettings.TitleId;
        req.RequireBothUsernameAndEmail = false;

        PlayFabClientAPI.RegisterPlayFabUser(req, (register) => { callback(true, register); }, (error) => { callback(false, error); });

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

    public void Login(string username, string password, System.Action<bool,object> callback)
    {
        LoginWithPlayFabRequest req = new LoginWithPlayFabRequest();
        req.Username = username;
        req.Password = password;
        req.TitleId = PlayFabSettings.TitleId;
        PlayFabClientAPI.LoginWithPlayFab(req, (login) => { callback(true,login); }, (error) => { callback(false,error); });
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
