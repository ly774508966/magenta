using UnityEngine;

// GAME NETWORK MESSAGES
//
// In this example these are all messages, but could just as easily be your other classes in the game.
// As long as the network message can be passed through by injecting it into your object.

public class BuildRequestMessage : PurpleMessage
{
    public int x;
    public int y;
    public string type;
}


public class PlayerMessage : PurpleMessage
{
  public NetworkPlayer player;
  public string player_name;
}


public class PlayerConnectMessage : PurpleMessage
{
}

public class ConnectedToServer : PurpleMessage
{
}

public class BuildRequestError : PurpleMessage
{
}

public class PlayerBuildsMessage : PurpleMessage
{
}
