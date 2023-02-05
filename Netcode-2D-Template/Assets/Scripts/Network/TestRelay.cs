using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using Unity.Netcode;
using Unity.Networking.Transport.Relay;
using Unity.Netcode.Transports.UTP;
using TMPro;

public class TestRelay : MonoBehaviour
{
    [SerializeField] private GameObject joinCodeInputField;
    [SerializeField] private GameObject generatedCode;

    private async void Start()
    {
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Signed In : " + AuthenticationService.Instance.PlayerId);
        };
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    public async void CreateRelay() {
        generatedCode.GetComponent<TMP_Text>().text = "Loading...";
        try { 
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(3);
            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            Debug.Log("Join code is : " + joinCode);
            generatedCode.GetComponent<TMP_Text>().text = joinCode;
            RelayServerData relayServerData = new RelayServerData(allocation, "dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
            NetworkManager.Singleton.StartHost();
        } catch (RelayServiceException e) {
            Debug.Log(e);
        }
    }

    public async void JoinRelay(string joinCode) {
        try {
            Debug.Log("Joining Relay with join code : " + joinCode);
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
            RelayServerData relayServerData = new RelayServerData(joinAllocation, "dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
            NetworkManager.Singleton.StartClient();
        } catch (RelayServiceException e) {
            Debug.Log(e);
        }
    }

    public void SetJoinCodeFromButton() { 
        string joinCodeInput = joinCodeInputField.GetComponent<TMP_InputField>().text;
        if(joinCodeInput != null) {
            JoinRelay(joinCodeInput);
        }
    }
}
