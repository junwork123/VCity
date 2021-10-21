SimpleNet 0.1 is an easy to use Networking library for Unity. 

Requirements:
 Unity 2018.4 LTS
 Runtime .Net 4.x(Project Settings > Player > Other Settings)
 
 Documentation:
    
 
 Package Layout:
 - SimpleNet
    ->Examples
        ->Lobby
            ->Prefabs
                - Player Prefab.prefab
            ->Scripts
                ->LobbyClient.cs
                ->LobbyServer.cs
                ->Movement.cs
            - LobbyExample.scene
        ->Messenger
            - MySimpleClient.cs
            - MySimpleServer.cs
            - SimpleMessaging.scene
    ->Scripts
        - NetClient.cs
        - NetServer.cs
        
        
 Starter Instructions:
 
 Server Instructions:
    1. Create new C# script
    2. Change derived class from MonoBehavior to NetServer
    3. Create override methods for OnClientConnected(TcpClient client),
       OnMessageArrive(string message), and OnClientDisconnected(TcpClient client,int netId);
 
 Client Instructions:
    1. Create new C# script
    2. Change derived class from MonoBehavior to NetClient
    3. Create override methods for OnConnectedToServer(int networkId),
        OnMessageRecieve(string message) and OnUserDisconnectedFromServer(int networkId);