using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyClient : NetClient
{
    public GameObject playerObject;

    GameObject myPlayer;

    // Start is called before the first frame update
    void Start()
    {
        StartClient();
    }

    // Update is called once per frame
    void Update()
    {
        if (myPlayer != null&&initializedOnNetwork)
        {
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

           //myPlayer.transform.position += x * myPlayer.transform.right*Time.deltaTime;
            //myPlayer.transform.position += z * myPlayer.transform.forward*Time.deltaTime;

            if (Mathf.Abs(x) > 0 || Mathf.Abs(z) > 0)
            {
                //SendPosition(myPlayer);
                SendTransform(myPlayer);
            }

            if (Input.GetKeyDown(KeyCode.Escape)) {
                this.Disconnect();
            }

        }

    }

    //method which is run when the client connects to the server.
    public override void OnConnectedToServer(int networkId)
    {
        base.OnConnectedToServer(networkId);

        base.SendNetMessage("Joined:"+networkID.ToString()+"|");
    }

    //method which is run when the client recieves a message from the server.
    public override void OnMessageReceive(string message)
    {
        base.OnMessageReceive(message);
        ParseMessage(message);

    }

    //Parses the message into a usable format. 
    private void ParseMessage(string message)
    {
        //checks if messages have stacked
        if (message.Contains("|"))
        {
            string[] messages = message.Split('|');
            foreach(string msg in messages) {
                ParseMessage(msg);
            }
            return;

        }
        //checks if message has colon format
        if (message.Contains(":"))
        {
            //checks if message is detialing that a player joined.
            if (message.Split(':')[0].Equals("Joined")) {


                GameObject player = Instantiate(playerObject);

                player.name = message.Split(':')[1];
                int playerNetId;
                if (int.TryParse(player.name, out playerNetId))
                {
                    if (playerNetId == this.networkID)
                    {
                        this.myPlayer = player;

                        player.GetComponent<Movement>().movementEnabled = true;

                    }
                }
                

            }

            //checks if the message is detailing position of an object.
            if (message.Split(':')[0].Equals("position"))
            {
                if(debug)
                    print("message recieved:" + message);

                string objName = message.Split(':')[1];

                if (GameObject.Find(objName)!=null)
                {

                    if (int.Parse(objName) != this.networkID)
                    {
                        string[] psnString = message.Split(':')[2].Split(',');

                        if (psnString.Length == 3)
                        {
                            float x = float.Parse(psnString[0]);
                            float y = float.Parse(psnString[1]);
                            float z = float.Parse(psnString[2]);

                            GameObject obj = GameObject.Find(objName);
                            obj.GetComponent<Movement>().SetTarget(new Vector3(x, y, z));

                        }

                    }
                }
                else
                {
                    GameObject newInstance = Instantiate(playerObject);
                    newInstance.name = objName;

                }


            }
            //checks if the message is detailing the transform of an object.
            if (message.Split(':')[0].Equals("transform"))
            {
                if (message.Split(':').Length == 3)
                {
                    if (debug)
                        print("transform message recieved:" + message);

                    string objName = message.Split(':')[1];

                    if (GameObject.Find(objName) != null)
                    {

                        if (int.Parse(objName) != this.networkID)
                        {

                            string[] psnString = message.Split(':')[2].Split(',');
                            print("coordinates:" + message.Split(':')[2]);
                            if (psnString.Length == 9)
                            {
                                float px = float.Parse(psnString[0]);
                                float py = float.Parse(psnString[1]);
                                float pz = float.Parse(psnString[2]);
                                float rx = float.Parse(psnString[3]);
                                float ry = float.Parse(psnString[4]);
                                float rz = float.Parse(psnString[5]);
                                float sx = float.Parse(psnString[6]);
                                float sy = float.Parse(psnString[7]);
                                float sz = float.Parse(psnString[8].Replace("|", ""));

                                GameObject obj = GameObject.Find(objName);
                                obj.GetComponent<Movement>().SetTarget(new Vector3(px, py, pz));
                                obj.transform.eulerAngles = new Vector3(rx, ry, rz);
                                obj.transform.localScale = new Vector3(sx, sy, sz);
                            }
                        }
                    }
                    else
                    {
                        GameObject newInstance = Instantiate(playerObject);
                        newInstance.name = objName;

                    }
                }

            }
        }
    }

    //Sends position data of the given object to the server.
    private void SendPosition(GameObject obj)
    {
        float x = obj.transform.position.x;
        float y = obj.transform.position.y;
        float z = obj.transform.position.z;

        //format position:[name]:x,y,z
        SendNetMessage("position:"+obj.name+":"+x.ToString()+","+y.ToString()+","+z.ToString()+"|");
    }

    //Sends all Transform data of the given object to the server.
    private void SendTransform(GameObject obj)
    {
        float px = obj.transform.position.x;
        float py = obj.transform.position.y;
        float pz = obj.transform.position.z;

        float rx = obj.transform.eulerAngles.x;
        float ry = obj.transform.eulerAngles.y;
        float rz = obj.transform.eulerAngles.z;

        float sx = obj.transform.localScale.x;
        float sy = obj.transform.localScale.y;
        float sz = obj.transform.localScale.z;

        //format transform:[name]:px,py,pz,rx,ry,rz,sx,sy,sz
        SendNetMessage("transform:" + obj.name + ":" + px + "," + py + "," + pz + "," +
        rx + "," + ry + "," + rz + ","
            + sx + "," + sy+"," + sz+"|");
    }

}
