using UnityEngine;
using System.Collections;

public class GameManagerStuff : MonoBehaviour
{
    void Start ()
    {
        // Lets register with the network to handle game events.
        //

        // Event from basic server.
        PurpleNetwork.AddListener ("AddPlayer",    add_player_callback);

        // User created event with link transfer message
        PurpleNetwork.AddListener ("LinkTransfer", link_transfer_callback);

        // User created event with no arguments and a second listener
        PurpleNetwork.AddListener ("Ping", ping_callback);
        PurpleNetwork.AddListener ("Ping", ping_two_callback);

        // TODO listen only on server for 'request game state' which will be in charge of sending commands directly  to one player
    }

    void Update ()
    {
        // Lets send a game event.
        //
        if (Input.GetButtonDown("Fire1"))
        {

            object link_transfer_message = new LinkTransferMessage();
            PurpleNetwork.Broadcast ("LinkTransfer", link_transfer_message);
        }

        // TODO example creating object.. getting id, and it going out to everyone
        // since this will have to happen for all clients on join to keep ids in sync
    }


    // Game specific methods
    //

    void add_player_callback() // FIXME player message argument next
    {
        Debug.Log ("player callback!");
    }


    void link_transfer_callback()
    {
        Debug.Log ("link transfer callback!");
    }


    void ping_callback()
    {
        Debug.Log ("ping callback!");
    }

    void ping_two_callback()
    {
        Debug.Log ("the other ping callback!");
    }
}


// GAME NETWORK MESSAGES
//

[System.Serializable]
public class LinkTransferMessage
{
    public int    first_id;
    public int    second_id;
    public string link_type;
}
