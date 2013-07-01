using UnityEngine;
using System.Collections;
using System.Collections.Generic;


// Example usage of network code
//
public class GameManagerStuff : MonoBehaviour
{
    // Object list held by server, and sent to clients when requested.
    //
    public List<GameObject> game_objects;
    public List<GamePlayer> players;



    // GAME GUI /////////////////////////
    //
    void OnGUI ()
    {
        // Status indicators
        //
        if (Network.isClient) { GUILayout.Label ("Connected as Client."); };
        if (Network.isServer) { GUILayout.Label ("Hosting as Server.");   };

        if (GUILayout.Button ("Host"   )) { host_game (); }
        if (GUILayout.Button ("Connect")) { connect ();   }
    }



    // SETUP CLIENT OR SERVER ////////////
    //
    void host_game ()
    {
      PurpleNetwork.LaunchServer ();

      // Server listeners
      PurpleNetwork.AddListener<BuildRequestMessage>  ("BuildRequest",    build_request_callback);

      // Local player needs events too
      client_events ();
    }


    void connect ()
    {
      PurpleNetwork.ConnectTo ("127.0.0.1");
      client_events ();
    }


    void client_events ()
    {
      // Client listeners
      PurpleNetwork.AddListener<ConnectedToServer>   ("ConnectedToServer",  connected_to_server_callback);
      PurpleNetwork.AddListener<PlayerMessage>       ("AddPlayer",          add_player_callback);
      PurpleNetwork.AddListener<BuildRequestError>   ("BuildRequestError",  build_error_callback);
      PurpleNetwork.AddListener<PlayerBuildsMessage> ("PlayerBuildsObject", player_builds_object_callback);
    }



    // GAME LOGIC ////////////////////////
    //
    void Update ()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            BuildRequestMessage build_request = new BuildRequestMessage();
            build_request.type = "Factory"; build_request.x = 5; build_request.y = 10;

            PurpleNetwork.ToServer ("BuildRequest", build_request);
        }
    }



    // Game specific methods for Server
    //
    void build_request_callback(object build_request)
    {
      Debug.Log("YOU WANNA BUILD");
    }

    void player_connected_callback(object player_connected)
    {
    }


    // Game specific methods for Client
    //

    // request game state and register name on connect

    void connected_to_server_callback (object connected_to_server)
    {
    }

    void add_player_callback (object player)
    {
    }

    void build_error_callback (object build_request_error)
    {
    }

    void player_builds_object_callback (object player_builds)
    {
    }
}
