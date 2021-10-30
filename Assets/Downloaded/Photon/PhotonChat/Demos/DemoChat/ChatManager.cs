// --------------------------------------------------------------------------------------------------------------------
// <copyright company="Exit Games GmbH"/>
// <summary>Demo code for Photon Chat in Unity.</summary>
// <author>developer@exitgames.com</author>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

using Photon.Chat;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using AuthenticationValues = Photon.Chat.AuthenticationValues;
#if PHOTON_UNITY_NETWORKING
using Photon.Pun;
using TMPro;
#endif

// 채팅 서버에 연결하고 대화를 동기화, 보내기, 받기 수행
// 각 동작을 데이터 매니저를 통해 수행
namespace Photon.Chat
{
    /// <summary>
    /// This simple Chat UI demonstrate basics usages of the Chat Api
    /// </summary>
    /// <remarks>
    /// The ChatClient basically lets you create any number of channels.
    ///
    /// some friends are already set in the Chat demo "DemoChat-Scene", 'Joe', 'Jane' and 'Bob', simply log with them so that you can see the status changes in the Interface
    ///
    /// Workflow:
    /// Create ChatClient, Connect to a server with your AppID, Authenticate the user (apply a unique name,)
    /// and subscribe to some channels.
    /// Subscribe a channel before you publish to that channel!
    ///
    ///
    /// Note:
    /// Don't forget to call ChatClient.Service() on Update to keep the Chatclient operational.
    /// </remarks>
    public class ChatManager : MonoBehaviour, IChatClientListener
    {
        public string[] ChannelsToJoinOnConnect; // set in inspector. Demo channels to join automatically.

        public string[] FriendsList;

        public int HistoryLengthToFetch; // set in inspector. Up to a certain degree, previously sent messages can be fetched for context

        public string UserName { get; set; }

        private string selectedChannelId; // mainly used for GUI/input

        public ChatClient chatClient;

#if !PHOTON_UNITY_NETWORKING
        [SerializeField]
#endif
        protected internal ChatAppSettings chatAppSettings;

        public GameObject ConnectingLabel;
        public GameObject ChatOutputPanel;
        public TMP_InputField InputFieldChat;   // set in inspector

        public ScrollRect scroll;
        public RectTransform CurrentChannelText;     // set in inspector
        public TMP_Text CurrentChannelName;     // set in inspector
        public Toggle ChannelToggleToInstantiate; // set in inspector


        public GameObject FriendListUiItemtoInstantiate;

        private readonly Dictionary<string, Toggle> channelToggles = new Dictionary<string, Toggle>();

        private readonly Dictionary<string, FriendItem> friendListItemLUT = new Dictionary<string, FriendItem>();

        public bool ShowState = true;
        public Text StateText; // set in inspector

        public GameObject MyMsgFactory;
        public GameObject OpMsgFactory;
        public string lastMsgId = "";

        // private static string WelcomeText = "Welcome to chat. Type \\help to list commands.";
        private static string HelpText = "\n    -- HELP --\n" +
            "To subscribe to channel(s) (channelnames are case sensitive) :  \n" +
                "\t<color=#E07B00>\\subscribe</color> <color=green><list of channelnames></color>\n" +
                "\tor\n" +
                "\t<color=#E07B00>\\s</color> <color=green><list of channelnames></color>\n" +
                "\n" +
                "To leave channel(s):\n" +
                "\t<color=#E07B00>\\unsubscribe</color> <color=green><list of channelnames></color>\n" +
                "\tor\n" +
                "\t<color=#E07B00>\\u</color> <color=green><list of channelnames></color>\n" +
                "\n" +
                "To switch the active channel\n" +
                "\t<color=#E07B00>\\join</color> <color=green><channelname></color>\n" +
                "\tor\n" +
                "\t<color=#E07B00>\\j</color> <color=green><channelname></color>\n" +
                "\n" +
                "To send a private message: (username are case sensitive)\n" +
                "\t\\<color=#E07B00>msg</color> <color=green><username></color> <color=green><message></color>\n" +
                "\n" +
                "To change status:\n" +
                "\t\\<color=#E07B00>state</color> <color=green><stateIndex></color> <color=green><message></color>\n" +
                "<color=green>0</color> = Offline " +
                "<color=green>1</color> = Invisible " +
                "<color=green>2</color> = Online " +
                "<color=green>3</color> = Away \n" +
                "<color=green>4</color> = Do not disturb " +
                "<color=green>5</color> = Looking For Group " +
                "<color=green>6</color> = Playing" +
                "\n\n" +
                "To clear the current chat tab (private chats get closed):\n" +
                "\t<color=#E07B00>\\clear</color>";


        public void Start()
        {
            DontDestroyOnLoad(this.gameObject);

            this.StateText.text = "";
            this.StateText.gameObject.SetActive(false);
            this.ChatOutputPanel.gameObject.SetActive(false);
            this.ConnectingLabel.SetActive(false);

            if (string.IsNullOrEmpty(this.UserName))
            {
                this.UserName = "user" + Environment.TickCount % 99; //made-up username
            }

#if PHOTON_UNITY_NETWORKING
            this.chatAppSettings = PhotonNetwork.PhotonServerSettings.AppSettings.GetChatSettings();
#endif

            bool appIdPresent = !string.IsNullOrEmpty(this.chatAppSettings.AppIdChat);


            if (!appIdPresent)
            {
                Debug.LogError("You need to set the chat app ID in the PhotonServerSettings file in order to continue.");
            }
        }



        /// <summary>To avoid that the Editor becomes unresponsive, disconnect all Photon connections in OnDestroy.</summary>
        public void OnDestroy()
        {
            if (this.chatClient != null)
            {
                this.chatClient.Disconnect();
            }
        }

        /// <summary>To avoid that the Editor becomes unresponsive, disconnect all Photon connections in OnApplicationQuit.</summary>
        public void OnApplicationQuit()
        {
            if (this.chatClient != null)
            {
                this.chatClient.Disconnect();
            }
        }
        public void LoadChat(string _channelId)
        {
            // 현재 메시지 갯수와 Firestore의 메시지 갯수가 같다면
            // 갱신할 것이 없으므로 종료한다.
            List<CustomMsg> msgs = DataManager.instance.chatCache[_channelId];
            msgs.Sort((a, b) => a.Time.CompareTo(b.Time));
            if (msgs == null && CurrentChannelText.transform.childCount == msgs.Count)
            {
                return;
            }

            // 이미 메시지가 있는 상태에서
            // 현재 채널과 다른 채널이라면 메시지 내역을 비운다
            Transform[] childList = CurrentChannelText.GetComponentsInChildren<RectTransform>(true);
            if (childList != null && _channelId != selectedChannelId)
            {
                for (int i = 1; i < childList.Length; i++)
                {
                    if (childList[i] != transform)
                        Destroy(childList[i].gameObject);
                }
            }
            // 오프라인이 메시지가 더 적은 경우
            // 다른 부분만 추가될 수 있도록 함
            if (childList.Length < msgs.Count)
            {
                for (int i = childList.Length; i < msgs.Count; i++)
                {
                    AppendMsg(msgs[i]);
                }
            } // 온라인이 메시지가 더 적은 경우(아직 보내지 못한 상태?)
            else
            {

            }

            

            // foreach (var msg in msgs.OrderBy(x => x.Time))
            // {
            //     childList[i];
            // }
            // 채널 이름을 유저가 설정해놓은 채널 이름으로 변경
            this.CurrentChannelName.text = DataManager.instance.userCache.MyChannels[_channelId];
            //this.CurrentChannelText.text = previousMsg;
            Debug.Log("[Chat] " + "메시지 불러오기 성공 : ");

        }
        public void AppendMsg(CustomMsg _msg)
        {
            GameObject mBox = null;
            if (_msg.Sender == this.UserName)
            {
                mBox = Instantiate(MyMsgFactory);
            }
            else
            {
                mBox = Instantiate(OpMsgFactory);
            }
            mBox.GetComponent<MsgFactory>().SetMsg(_msg);
            mBox.transform.SetParent(this.CurrentChannelText.transform);
        }
        public void Update()
        {
            if (this.chatClient != null)
            {
                this.chatClient.Service(); // make sure to call this regularly! it limits effort internally, so calling often is ok!
            }

            // check if we are missing context, which means we got kicked out to get back to the Photon Demo hub.
            if (this.StateText == null)
            {
                Destroy(this.gameObject);
                return;
            }

            //this.StateText.gameObject.SetActive(this.ShowState); // this could be handled more elegantly, but for the demo it's ok.

        }


        public void OnEnterSend()
        {
            if (Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter))
            {
                string nowtime = DateTime.Now.ToString((DataManager.TimeFormat));
                this.SendChatMessage(this.InputFieldChat.text, nowtime);
                this.InputFieldChat.text = "";
            }
        }

        public void OnClickSend()
        {
            if (this.InputFieldChat != null)
            {
                string nowtime = DateTime.Now.ToString((DataManager.TimeFormat));
                this.SendChatMessage(this.InputFieldChat.text, nowtime);
                this.InputFieldChat.text = "";
            }
        }


        public int TestLength = 2048;
        private byte[] testBytes = new byte[2048];

        private void SendChatMessage(string inputLine, string time = "")
        {
            if (string.IsNullOrEmpty(inputLine))
            {
                return;
            }
            if ("test".Equals(inputLine))
            {
                if (this.TestLength != this.testBytes.Length)
                {
                    this.testBytes = new byte[this.TestLength];
                }

                this.chatClient.SendPrivateMessage(this.chatClient.AuthValues.UserId, this.testBytes, true);
            }


            bool doingPrivateChat = this.chatClient.PrivateChannels.ContainsKey(this.selectedChannelId);
            string privateChatTarget = string.Empty;
            if (doingPrivateChat)
            {
                // the channel name for a private conversation is (on the client!!) always composed of both user's IDs: "this:remote"
                // so the remote ID is simple to figure out

                string[] splitNames = this.selectedChannelId.Split(new char[] { ':' });
                privateChatTarget = splitNames[1];
            }
            //UnityEngine.Debug.Log("selectedChannelName: " + selectedChannelName + " doingPrivateChat: " + doingPrivateChat + " privateChatTarget: " + privateChatTarget);

            if (inputLine[0].Equals('\\'))
            {
                string[] tokens = inputLine.Split(new char[] { ' ' }, 2);
                if (tokens[0].Equals("\\help"))
                {
                    this.PostHelpToCurrentChannel();
                }
                if (tokens[0].Equals("\\state"))
                {
                    int newState = 0;


                    List<string> messages = new List<string>();
                    messages.Add("i am state " + newState);
                    string[] subtokens = tokens[1].Split(new char[] { ' ', ',' });

                    if (subtokens.Length > 0)
                    {
                        newState = int.Parse(subtokens[0]);
                    }

                    if (subtokens.Length > 1)
                    {
                        messages.Add(subtokens[1]);
                    }

                    this.chatClient.SetOnlineStatus(newState, messages.ToArray()); // this is how you set your own state and (any) message
                }
                else if ((tokens[0].Equals("\\subscribe") || tokens[0].Equals("\\s")) && !string.IsNullOrEmpty(tokens[1]))
                {
                    this.chatClient.Subscribe(tokens[1].Split(new char[] { ' ', ',' }));

                    DataManager.instance.SubscribeChannel(tokens[1]);
                }
                else if ((tokens[0].Equals("\\unsubscribe") || tokens[0].Equals("\\u")) && !string.IsNullOrEmpty(tokens[1]))
                {
                    this.chatClient.Unsubscribe(tokens[1].Split(new char[] { ' ', ',' }));
                }
                else if (tokens[0].Equals("\\clear"))
                {
                    if (doingPrivateChat)
                    {
                        this.chatClient.PrivateChannels.Remove(this.selectedChannelId);
                    }
                    else
                    {
                        ChatChannel channel;
                        if (this.chatClient.TryGetChannel(this.selectedChannelId, doingPrivateChat, out channel))
                        {
                            channel.ClearMessages();
                        }
                    }
                }
                else if (tokens[0].Equals("\\msg") && !string.IsNullOrEmpty(tokens[1]))
                {
                    string[] subtokens = tokens[1].Split(new char[] { ' ', ',' }, 2);
                    if (subtokens.Length < 2) return;

                    string targetUser = subtokens[0];
                    string message = subtokens[1];
                    this.chatClient.SendPrivateMessage(targetUser, message);
                }
                else if ((tokens[0].Equals("\\join") || tokens[0].Equals("\\j")) && !string.IsNullOrEmpty(tokens[1]))
                {
                    string[] subtokens = tokens[1].Split(new char[] { ' ', ',' }, 2);

                    // If we are already subscribed to the channel we directly switch to it, otherwise we subscribe to it first and then switch to it implicitly
                    if (this.channelToggles.ContainsKey(subtokens[0]))
                    {
                        this.ShowChannel(subtokens[0]);
                    }
                    else
                    {
                        this.chatClient.Subscribe(new string[] { subtokens[0] });
                    }
                }
                else
                {
                    Debug.Log("[Chat] " + "The command '" + tokens[0] + "' is invalid.");
                }
            }
            else
            {
                if (doingPrivateChat)
                {
                    this.chatClient.SendPrivateMessage(privateChatTarget, inputLine);
                    AppendMsg(new CustomMsg(UserName, time, inputLine));
                    DataManager.instance.AppendMsg(this.selectedChannelId, new CustomMsg(UserName, time, inputLine));

                }
                else
                {
                    this.chatClient.PublishMessage(this.selectedChannelId, inputLine);
                    AppendMsg(new CustomMsg(UserName, time, inputLine));
                    DataManager.instance.AppendMsg(this.selectedChannelId, new CustomMsg(UserName, time, inputLine));
                }
            }
        }

        public void PostHelpToCurrentChannel()
        {
            //this.CurrentChannelText.text += HelpText;
        }

        public void DebugReturn(ExitGames.Client.Photon.DebugLevel level, string message)
        {
            if (level == ExitGames.Client.Photon.DebugLevel.ERROR)
            {
                Debug.LogError(message);
            }
            else if (level == ExitGames.Client.Photon.DebugLevel.WARNING)
            {
                Debug.LogWarning(message);
            }
            else
            {
                Debug.Log(message);
            }
        }
        public void Connect(string _id)
        {
            this.UserName = _id;

            this.chatClient = new ChatClient(this);
#if !UNITY_WEBGL
            this.chatClient.UseBackgroundWorkerForSending = true;
#endif
            this.chatClient.AuthValues = new AuthenticationValues(_id);
            this.chatClient.ConnectUsingSettings(this.chatAppSettings);

            this.ChannelToggleToInstantiate.gameObject.SetActive(false);
            Debug.Log("[Chat] " + "Connecting as: " + this.UserName);
            this.ConnectingLabel.SetActive(true);


        }
        public void UpdateRooms()
        {
            UserData userCache = DataManager.instance.userCache;
            if (userCache != null && userCache.MyChannels != null)
            {
                if (userCache.MyChannels.Count == 0)
                {
                    DataManager.instance.SubscribeChannel(DataManager.REGION_CHANNEL_ID);
                }
                foreach (string channelId in userCache.MyChannels.Keys)
                {
                    this.chatClient.Subscribe(channelId, this.HistoryLengthToFetch);
                    DataManager.instance.LoadAllMessages(channelId);
                }
            }
            scroll.verticalNormalizedPosition = 0;
        }

        public void OnConnected()
        {
            UpdateRooms();
            // if (this.ChannelsToJoinOnConnect != null && this.ChannelsToJoinOnConnect.Length > 0)
            // {
            //     this.chatClient.Subscribe(this.ChannelsToJoinOnConnect, this.HistoryLengthToFetch);
            // }

            this.ConnectingLabel.SetActive(false);
            // 기본으로 열리는 채널
            //LoadChat(DataManager.REGION_CHANNEL_ID);
            //this.ChatPanel.gameObject.SetActive(true);

            if (this.FriendsList != null && this.FriendsList.Length > 0)
            {
                this.chatClient.AddFriends(this.FriendsList); // Add some users to the server-list to get their status updates

                // add to the UI as well
                foreach (string _friend in this.FriendsList)
                {
                    if (this.FriendListUiItemtoInstantiate != null && _friend != this.UserName)
                    {
                        this.InstantiateFriendButton(_friend);
                    }

                }

            }

            if (this.FriendListUiItemtoInstantiate != null)
            {
                this.FriendListUiItemtoInstantiate.SetActive(false);
            }

            
            this.chatClient.SetOnlineStatus(ChatUserStatus.Online); // You can set your online state (without a mesage).
        }

        public void OnDisconnected()
        {
            this.ConnectingLabel.SetActive(false);
        }

        public void OnChatStateChange(ChatState state)
        {
            // use OnConnected() and OnDisconnected()
            // this method might become more useful in the future, when more complex states are being used.

            this.StateText.text = state.ToString();
        }

        public void OnSubscribed(string[] channels, bool[] results)
        {
            // in this demo, we simply send a message into each channel. This is NOT a must have!
            foreach (string channel in channels)
            {
                string nowtime = DateTime.Now.ToString(("[yyyy-MM-dd HH:mm]"));
                Debug.Log("[Chat] " + $"login sucussed in channel <{channel}>");
                //this.chatClient.PublishMessage(channel, nowtime + "login success."); // you don't HAVE to send a msg on join but you could.

                if (this.ChannelToggleToInstantiate != null)
                {
                    this.InstantiateChannelButton(channel);

                }
            }

            Debug.Log("[Chat] " + "OnSubscribed: " + string.Join(", ", channels));

            /*
            // select first subscribed channel in alphabetical order
            if (this.chatClient.PublicChannels.Count > 0)
            {
                var l = new List<string>(this.chatClient.PublicChannels.Keys);
                l.Sort();
                string selected = l[0];
                if (this.channelToggles.ContainsKey(selected))
                {
                    ShowChannel(selected);
                    foreach (var c in this.channelToggles)
                    {
                        c.Value.isOn = false;
                    }
                    this.channelToggles[selected].isOn = true;
                    AddMessageToSelectedChannel(WelcomeText);
                }
            }
            */

            // Switch to the first newly created channel
            this.ShowChannel(channels[0]);
        }

        /// <inheritdoc />
        public void OnSubscribed(string channel, string[] users, Dictionary<object, object> properties)
        {
            Debug.LogFormat("OnSubscribed: {0}, users.Count: {1} Channel-props: {2}.", channel, users.Length, properties.ToStringFull());
        }

        private void InstantiateChannelButton(string channelId)
        {
            if (this.channelToggles.ContainsKey(channelId))
            {
                Debug.Log("[Chat] " + "Skipping creation for an existing channel toggle.");
                return;
            }

            Toggle cbtn = (Toggle)Instantiate(this.ChannelToggleToInstantiate);
            cbtn.gameObject.SetActive(true);
            cbtn.GetComponentInChildren<ChannelSelector>().SetChannel(channelId);
            cbtn.transform.SetParent(this.ChannelToggleToInstantiate.transform.parent, false);
            cbtn.GetComponentInChildren<TMP_Text>().text = DataManager.instance.userCache.MyChannels[channelId];
            this.channelToggles.Add(channelId, cbtn);
        }

        private void InstantiateFriendButton(string friendId)
        {
            GameObject fbtn = (GameObject)Instantiate(this.FriendListUiItemtoInstantiate);
            fbtn.gameObject.SetActive(true);
            FriendItem _friendItem = fbtn.GetComponent<FriendItem>();

            _friendItem.FriendId = friendId;

            fbtn.transform.SetParent(this.FriendListUiItemtoInstantiate.transform.parent, false);

            this.friendListItemLUT[friendId] = _friendItem;
        }


        public void OnUnsubscribed(string[] channels)
        {
            foreach (string channelId in channels)
            {
                if (this.channelToggles.ContainsKey(channelId))
                {
                    Toggle t = this.channelToggles[channelId];
                    Destroy(t.gameObject);

                    this.channelToggles.Remove(channelId);

                    Debug.Log("[Chat] " + "Unsubscribed from channel '" + channelId + "'.");

                    // Showing another channel if the active channel is the one we unsubscribed from before
                    if (channelId == this.selectedChannelId && this.channelToggles.Count > 0)
                    {
                        IEnumerator<KeyValuePair<string, Toggle>> firstEntry = this.channelToggles.GetEnumerator();
                        firstEntry.MoveNext();

                        this.ShowChannel(firstEntry.Current.Key);

                        firstEntry.Current.Value.isOn = true;
                    }
                }
                else
                {
                    Debug.Log("[Chat] " + "Can't unsubscribe from channel '" + channelId + "' because you are currently not subscribed to it.");
                }
            }
        }

        public void OnGetMessages(string channelId, string[] senders, object[] messages)
        {

            // string time = "";
            // string text = "";
            // string timeFormat = "[yyyy-MM-dd HH:mm]";
            // for (int i = 0; i < senders.Length; i++)
            // {
            //     string msg = messages[i].ToString();
            //     if (msg.Length >= DataManager.TimeFormat.Length)
            //     {
            //         time = msg.Substring(0, DataManager.TimeFormat.Length);
            //         text = msg.Substring(DataManager.TimeFormat.Length);

            //     }
            //     else
            //     {
            //         Debug.Log("[Chat] " + "Can't Find timeFormat in received text");
            //         time = timeFormat;
            //         text = msg;
            //     }
            //     DataManager.instance.AppendMsg(channelId, new CustomMsg(senders[i], time, text));

            // }

            // update text
            Debug.Log("[Chat] : " + messages.ToString());
            this.ShowChannel(this.selectedChannelId);
            
            //throw new System.NotImplementedException();
        }

        public void OnPrivateMessage(string sender, object message, string channelId)
        {
            // as the ChatClient is buffering the messages for you, this GUI doesn't need to do anything here
            // you also get messages that you sent yourself. in that case, the channelName is determinded by the target of your msg
            this.InstantiateChannelButton(channelId);

            byte[] msgBytes = message as byte[];
            if (msgBytes != null)
            {
                Debug.Log("[Chat] " + "Message with byte[].Length: " + msgBytes.Length);
            }
            if (this.selectedChannelId.Equals(channelId))
            {
                this.ShowChannel(channelId);
            }
        }

        /// <summary>
        /// New status of another user (you get updates for users set in your friends list).
        /// </summary>
        /// <param name="user">Name of the user.</param>
        /// <param name="status">New status of that user.</param>
        /// <param name="gotMessage">True if the status contains a message you should cache locally. False: This status update does not include a
        /// message (keep any you have).</param>
        /// <param name="message">Message that user set.</param>
        public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
        {

            Debug.LogWarning("status: " + string.Format("{0} is {1}. Msg:{2}", user, status, message));

            if (this.friendListItemLUT.ContainsKey(user))
            {
                FriendItem _friendItem = this.friendListItemLUT[user];
                if (_friendItem != null) _friendItem.OnFriendStatusUpdate(status, gotMessage, message);
            }
        }

        public void OnUserSubscribed(string channel, string user)
        {
            Debug.LogFormat("OnUserSubscribed: channel=\"{0}\" userId=\"{1}\"", channel, user);
        }

        public void OnUserUnsubscribed(string channel, string user)
        {
            Debug.LogFormat("OnUserUnsubscribed: channel=\"{0}\" userId=\"{1}\"", channel, user);
        }

        /// <inheritdoc />
        public void OnChannelPropertiesChanged(string channel, string userId, Dictionary<object, object> properties)
        {
            Debug.LogFormat("OnChannelPropertiesChanged: {0} by {1}. Props: {2}.", channel, userId, Extensions.ToStringFull(properties));
        }

        public void OnUserPropertiesChanged(string channel, string targetUserId, string senderUserId, Dictionary<object, object> properties)
        {
            Debug.LogFormat("OnUserPropertiesChanged: (channel:{0} user:{1}) by {2}. Props: {3}.", channel, targetUserId, senderUserId, Extensions.ToStringFull(properties));
        }

        /// <inheritdoc />
        public void OnErrorInfo(string channel, string error, object data)
        {
            Debug.LogFormat("OnErrorInfo for channel {0}. Error: {1} Data: {2}", channel, error, data);
        }

        public void AddMessageToSelectedChannel(string msg)
        {
            ChatChannel channel = null;
            bool found = this.chatClient.TryGetChannel(this.selectedChannelId, out channel);
            if (!found)
            {
                Debug.Log("[Chat] " + "AddMessageToSelectedChannel failed to find channel: " + this.selectedChannelId);
                return;
            }

            if (channel != null)
            {
                channel.Add("Bot", msg, 0); //TODO: how to use msgID?
            }
        }



        public void ShowChannel(string channelId)
        {
            if (string.IsNullOrEmpty(channelId))
            {
                return;
            }

            ChatChannel channel = null;
            bool found = this.chatClient.TryGetChannel(channelId, out channel);
            if (!found)
            {
                Debug.Log("[Chat] " + "ShowChannel failed to find channel: " + channelId);
                return;
            }

            LoadChat(channelId);
            this.selectedChannelId = channelId;
            scroll.verticalNormalizedPosition = 0;

            Debug.Log("[Chat] " + "ShowChannel: " + this.selectedChannelId);

            foreach (KeyValuePair<string, Toggle> pair in this.channelToggles)
            {
                pair.Value.isOn = pair.Key == channelId ? true : false;
            }
        }
    }
}