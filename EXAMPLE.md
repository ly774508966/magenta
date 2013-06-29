object server
object player1
object player2
object player3
object broadcast

note right of server: start

player1 -> server: local connect
server -> broadcast: newplayer (p1)

player2 -> server: connect
server -> broadcast: newplayer (p2)

player1 -> server: place building (coordinates)
note right of server: can create check?

server -> broadcast: spawnthing (coordinate, properties, id, p1 owner)

player2 -> server: modify building (id)
note right of server: can modify check?

server -> player2: error notification

player3 -> server: connect

server -> broadcast: newplayer (p3)
server -> player3: send players
server -> player3: send objects
