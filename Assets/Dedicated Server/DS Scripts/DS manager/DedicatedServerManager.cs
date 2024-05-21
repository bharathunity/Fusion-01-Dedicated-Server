using Fusion;
using Fusion.Photon.Realtime;
using Fusion.Sockets;
using System;
using System.Threading.Tasks;
using UnityEngine;

#if UNITY_SERVER
public class DedicatedServerManager : MonoBehaviour
{



    [SerializeField] private NetworkRunner _networkRunner;
    [SerializeField] private string _session;
    [SerializeField] private string _ip;
    [SerializeField] private ushort _port;

    public enum Region
    {
        US,
        EU,
        JP,
        ASIA,
        SA,
        CN

    }



    [SerializeField] private Region ServerRegion;

    // Start is called before the first frame update
    void Start()
    {
        Task startSimulation = StartSimulation(_networkRunner, _session, ServerRegion.ToString().ToUpper());

        Console.WriteLine($"Server starting simulation status \t {startSimulation.Status}");
    }

    // Update is called once per frame
    void Update()
    {

    }

    private Task<StartGameResult> StartSimulation(
        NetworkRunner runner,
        string SessionName,
        string customRegion
    )
    {

        // Build Custom Photon Config
        var photonSettings = PhotonAppSettings.Instance.AppSettings.GetCopy();

        if (string.IsNullOrEmpty(customRegion) == false)
        {
            photonSettings.FixedRegion = customRegion.ToLower();
        }

        // Start Runner
        return runner.StartGame(new StartGameArgs()
        {
            SessionName = SessionName,                                      // Custom Session Name
            GameMode = GameMode.Server,                                     // Game mode always set to Server
            Address = NetAddress.CreateFromIpPort(_ip, _port),              // NetAddress.Any(port),             // EndPoint to bind: 0.0.0.0:port
            CustomPhotonAppSettings = photonSettings,                       // Custom Photon App Settings
                                                                            // other arguments
        });
    }
}
#endif
