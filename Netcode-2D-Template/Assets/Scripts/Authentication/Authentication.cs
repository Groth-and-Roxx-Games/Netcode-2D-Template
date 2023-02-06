using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using UnityEngine.SceneManagement;
using Unity.Services.Authentication;
using System;

public class Authentication : MonoBehaviour
{

    async void Start()
    {
        await UnityServices.InitializeAsync();

        if(UnityServices.State == ServicesInitializationState.Initialized) 
        {
            AuthenticationService.Instance.SignedIn += OnSignedIn;
            await AuthenticationService.Instance.SignInAnonymouslyAsync();

            if(AuthenticationService.Instance.IsSignedIn) 
            {
                string username = PlayerPrefs.GetString("Username");
                if(username == "") 
                {
                    username = "Player";
                    PlayerPrefs.SetString("Username", username);
                }

                SceneManager.LoadSceneAsync("MainMenu");
            }
        }
    }

    private void OnSignedIn()
    {
        Debug.Log($"Player ID: {AuthenticationService.Instance.PlayerId}");
        Debug.Log($"Token: {AuthenticationService.Instance.AccessToken}");
    }
}
