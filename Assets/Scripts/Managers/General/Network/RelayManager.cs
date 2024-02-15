using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

public class RelayManager : MonoBehaviour
{
    public static RelayManager Instance;
    private bool isRelayReady = false;

    public void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private async void Start()
    {
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Signed In " + AuthenticationService.Instance.PlayerId);
        };
        await AuthenticationService.Instance.SignInAnonymouslyAsync();

        isRelayReady = true;
    }

#if DEDICATED_SERVER

    public async Task CreateRelay()
    {
        try
        {
            while (!isRelayReady) await Task.Delay(100);

            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(2);

            string relayCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            Debug.Log("Creating Relay with " + relayCode);
            SenderManager.Instance.SendRelayCode(relayCode);

            RelayServerData relayServerData = new RelayServerData(allocation, "dtls");

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
        }
        catch (RelayServiceException e)
        {
            Debug.Log(e);
        }
    }

#else

    public async Task JoinRelay(string relayCode)
    {
        try
        {
            while (!isRelayReady) await Task.Delay(100);

            Debug.Log("Joining Relay with " + relayCode);

            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(relayCode);

            RelayServerData relayServerData = new RelayServerData(joinAllocation, "dtls");

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
        }
        catch (RelayServiceException e)
        {
            Debug.Log(e);
        }
    }

#endif
}
