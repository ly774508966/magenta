using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;

public class PurpleNetwork : MonoBehaviour
{
    public int    number_of_players = 32;
    public int    port_number       = 25000;
    public string password          = "a password";
    public NetworkViewID last_view_id;

    private Dictionary<string, PurpleNetCallback> event_listeners = new Dictionary<string, PurpleNetCallback>();

    // TODO Server Listeners? (or just if(server))
    // TODO Direct Responders
    // TODO broadcast player connects and disconnects to others so they may request info.
    // TODO send directly to server
    //
    // FIXME probably reduce to one listen/broadcast (vs server special) and just have if conditions in game manager that chose to listen or broadcast based on hosting/client

    // LEFT AT: figuring out how to subscribe to both kinds of messages (with/without args)
    // adding realistic connect/add/reconnect/etc

    void Start ()
    {
        instance = this;
    }



    // DEBUG GUI /////////////////////////
    //
    void OnGUI ()
    {
        if (GUILayout.Button ("Host"   )) { launch_server ();         }
        if (GUILayout.Button ("Connect")) { connect_to ("127.0.0.1"); }
        if (Network.isClient) { GUILayout.Label ("Connected as Client."); };
        if (Network.isServer) { GUILayout.Label ("Hosting as Server.");   };

        if (GUILayout.Button ("New ID")) { last_view_id = Network.AllocateViewID (); }

        GUILayout.Label ("View: " + last_view_id.ToString() );
    }



    // SINGLETON /////////////////////////
    //
    private static NetworkView network_view;
    private static PurpleNetwork instance;
    public  static PurpleNetwork Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject game_object = new GameObject ("PurpleNetworkManager"); // Name is unimportant
                instance     = game_object.AddComponent<PurpleNetwork> ();
                network_view = game_object.AddComponent<NetworkView>   ();
            }
            return instance;
        }
    }



    // SERVER ////////////////////////////
    //
    public void launch_server()
    {
        Debug.Log ("Launching Server");
        Network.incomingPassword = password;
        bool use_nat = !Network.HavePublicAddress();
        Network.InitializeServer (number_of_players, port_number, use_nat);
    }


    // SERVER EVENTS
    void OnServerInitialized()  { Debug.Log ("Server Initialized."); }
    void OnPlayerDisconnected() { Debug.Log ("Player Disconnected"); }
    void OnPlayerConnected()    { Debug.Log ("Player Connected");    }



    // CLIENT ////////////////////////////
    //
    public void connect_to(string server_host)
    {
        Network.Connect(server_host, port_number, password);
        Debug.Log ("Connecting to Server");
    }


    // CLIENT EVENTS
    void OnConnectedToServer()      { Debug.Log ("Connected to server");      }
    void OnDisconnectedFromServer() { Debug.Log ("Disconnected from server"); }



    // EVENT DISPATCH ////////////////////
    //
    private void add_listener (string event_name, PurpleNetCallback listener)
    {
        Debug.Log ("ADD LISTENER " + event_name);

        if (!event_listeners.ContainsKey (event_name))
        {
          event_listeners.Add(event_name, null);
        }

        // delegates can be chained using addition
        event_listeners[event_name] += listener;
    }


    // SEND
    private void broadcast (string event_name, object message = null)
    {
        Debug.Log ("BROADCAST " + event_name);

        if (message == null) // FIXME ugly, just write two signatures and two rpcs? else rpc has an if == ""
        {
          network_view.RPC("receive_broadcast", RPCMode.All, event_name);
        }
        else
        {
          network_view.RPC("receive_broadcast_with_arg", RPCMode.All, event_name, JsonMapper.ToJson(message));
        };
    }



    // RECEIVE
    [RPC]
    void receive_broadcast_with_arg(string event_name, string json_message, NetworkMessageInfo info)
    {
        // TODO use <T> to induce object its coming back as instead of making function use the mapper?
        event_listeners[event_name]();//message);
    }

    [RPC]
    void receive_broadcast(string event_name, NetworkMessageInfo info)
    {
        event_listeners[event_name]();
    }



    // SINGLETON STATIC METHODS ///////////
    //
    public static void AddListener (string event_name, PurpleNetCallback listener)
    {
        Instance.add_listener (event_name, listener);
    }


    public static void Broadcast (string event_name, object message = null)
    {
        Instance.broadcast (event_name, message);
    }

}



// DELEGATES FOR CALLBACK

public delegate void PurpleNetCallback();          // Without message
public delegate void PurpleNetCallback<T>(T arg1); // With message
