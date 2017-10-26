using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using WebSocketSharp;
using System.IO;
using System.Xml;
using System.Windows;
using System.Xml.Linq;

namespace MagicCube.Util
{
    public class TextChatUtil
    {
        private static TextChatUtil instance;
        private WebSocket websocket;
        private string username;
        private string password;
        private string token;
        private DateTime tokenExpireTime;
        private string rest_token;
        private DateTime rest_tokenExpireTime;

        public static TextChatUtil GetInstance()
        {
            if (instance == null)
            {
                instance = new TextChatUtil();
            }

            return instance;
        }

        private TextChatUtil()
        {

        }

        //初始化接收信息侦听
        public void InitXmppWebSocket(string username, string password)
        {
            this.username = username;
            this.password = password;
            this.rest_token = GetRestToken();
            //if (websocket != null)
            //{
            //    websocket.Close();
            //}
            websocket = new WebSocket(ConfUtil.EMCHAT_URL);
            websocket.OnOpen += new EventHandler(HandleWsOpen);
            websocket.OnClose += new EventHandler<WebSocketSharp.CloseEventArgs>(HandleWsClose);
            websocket.OnError += new EventHandler<WebSocketSharp.ErrorEventArgs>(HandleWsError);        
            websocket.OnMessage += new EventHandler<WebSocketSharp.MessageEventArgs>(HandleWsMessage);
         
            websocket.Connect();
        }

        public void ConnectWebsocket()
        {
            websocket.Connect();
        }

        //登录打开侦听
        public void HandleWsOpen(object sender, EventArgs e)
        {
            
            websocket.Send("<open xmlns='urn:ietf:params:xml:ns:xmpp-framing' to='" + ConfUtil.EMCHAT_DOMAIN_NAME + "' version='1.0'/>");
            string authzid = string.Format("{0}#{1}_{2}@{3}",ConfUtil.EMCHAT_ORG_NAME,ConfUtil.EMCHAT_APP_NAME,username,ConfUtil.EMCHAT_DOMAIN_NAME);
            string authcid = string.Format("{0}#{1}_{2}", ConfUtil.EMCHAT_ORG_NAME, ConfUtil.EMCHAT_APP_NAME, username);
            //string pass = "$t$" + this.token;
            this.token = this.GetAccessToken(username, password);
            string pass = "$t$" + token;
            string authContent = string.Format("{0}\u0000{1}\u0000{2}", authzid, authcid, pass);
            byte[] bAuthContent = Encoding.ASCII.GetBytes(authContent);
            string sAuthContent = Convert.ToBase64String(bAuthContent);
            websocket.Send(string.Format("<auth xmlns='urn:ietf:params:xml:ns:xmpp-sasl' mechanism='PLAIN'>{0}</auth>", sAuthContent));
            websocket.Send(string.Format("<open xmlns='urn:ietf:params:xml:ns:xmpp-framing' to='{0}' version='1.0'/>",ConfUtil.EMCHAT_DOMAIN_NAME));
            websocket.Send("<iq type='set' id='_bind_auth_2' xmlns='jabber:client'><bind xmlns='urn:ietf:params:xml:ns:xmpp-bind'><resource>webimxm</resource></bind></iq>");
            websocket.Send("<iq type='set' id='_session_auth_2' xmlns='jabber:client'><session xmlns='urn:ietf:params:xml:ns:xmpp-session'/></iq>");
            websocket.Send("<presence xmlns='jabber:client'/>");
        }

        //关闭事件
        public delegate void OnWsCloseDelegate(int returnCode, string reason);
        public OnWsCloseDelegate onWsCloseHandler = null;
        public void HandleWsClose(object sender, CloseEventArgs e)
        {
            if (onWsCloseHandler != null)
            {
                try
                {
                    onWsCloseHandler(e.Code, e.Reason);
                }
                catch
                {
 
                }

                
            }
        }

        //接收消息事件
        public delegate void OnRecvMessageDelegate(string sendUserName, string recvUserName, string message,int recordid,string time);
        public OnRecvMessageDelegate onRecvMessageHandler = null;
        public void HandleWsMessage(object sender, MessageEventArgs e)
        {
            //TODO:log
            if (onRecvMessageHandler == null)
            {
                return;
            }
            try
            {
                string xml = e.Data;
                string jsonData = JsonUtil.GetJsonTextFromXml(xml);
                RespTxtObj respTxtObj = JsonUtil.ToObject<RespTxtObj>(jsonData);
                if (respTxtObj==null ||respTxtObj.bodies == null || respTxtObj.bodies.Count == 0)
                {
                    return;
                }
                MsgObj msgObj = respTxtObj.bodies[0];
                if ("txt".Equals(msgObj.type))
                {                   
                    XmlDocument document = new XmlDocument();
                    document.LoadXml(xml);

                    XmlNodeList nodeList = document.GetElementsByTagName("message");
                    string id = string.Empty;
                    string from = string.Empty;
                    foreach (XmlNode pXmlNode in nodeList)
                    {
                        id = pXmlNode.Attributes["id"].Value;
                        from = pXmlNode.Attributes["to"].Value;

                    }
                    string sendtime = GetCreatetime();
                    try
                    {
                        sendtime = respTxtObj.ext["sendTime"];
                    }
                    catch
                    {

                    }

                    onRecvMessageHandler(respTxtObj.from, respTxtObj.to, msgObj.msg, Convert.ToInt32(respTxtObj.ext["contactRecordId"]), sendtime);
                    websocket.Send("<message from='" + from + "' to='easemob.com' id='" + id + "'><received xmlns='urn:xmpp:receipts' id='" + id + "'></received></message>");
                }
                else
                {
                    //onRecvMessageHandler(respTxtObj.from, respTxtObj.to, msgObj.type,-1);
                }
            }
            catch
            {
                
            }
        }

        //错误事件
        public delegate void OnWsErrorDelegate(string errorMessage);
        public OnWsErrorDelegate onWsErrorHandler = null;
        public void HandleWsError(object sender, WebSocketSharp.ErrorEventArgs e)
        {
            try
            {
                if (onWsErrorHandler != null)
                {
                    onWsErrorHandler(e.Message);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("HandleWsError" + ex.ToString());
            }
        }

        //关闭文本接收
        public void Close()
        {
            if (this.websocket!=null)
            {
                this.websocket.Close();
            }
        
            this.token = null;
        }

        /// <summary>
        /// 使用app的client_id 和 client_secret登陆并获取授权token
        /// </summary>
        /// <returns></returns>
        public string GetRestToken()
        {
            string clientID = ConfUtil.EMCHAT_CLIENT_ID;
            string clientSecret = ConfUtil.EMCHAT_CLIENT_SECRET;

            if (string.IsNullOrEmpty(clientID) || string.IsNullOrEmpty(clientSecret)) { return string.Empty; }
            string cacheKey = clientID + clientSecret;

            if (!string.IsNullOrWhiteSpace(this.rest_token) && this.rest_tokenExpireTime.CompareTo(DateTime.Now) > 0)
            {
                return this.rest_token;
            }

            string postUrl = ConfUtil.EMCHAT_REST_URL + "token";
            StringBuilder _build = new StringBuilder();
            _build.Append("{");
            _build.AppendFormat("\"grant_type\": \"client_credentials\",\"client_id\": \"{0}\",\"client_secret\": \"{1}\"", clientID, clientSecret);
            _build.Append("}");
            string postResultStr = HttpUtil.SendAndReceiveMessage(postUrl, _build.ToString());
            this.rest_token = string.Empty;
            int expireSeconds = 0;
            try
            {
                JObject jo = JObject.Parse(postResultStr);
                rest_token = jo.GetValue("access_token").ToString();
                int.TryParse(jo.GetValue("expires_in").ToString(), out expireSeconds);
                //设置缓存时效
                if (!string.IsNullOrWhiteSpace(this.rest_token) && expireSeconds > 0)
                {
                    this.rest_tokenExpireTime = DateTime.Now.AddSeconds(expireSeconds);
                }
                else
                {
                    throw new Exception("更新Token失败！");
                }
            }
            catch 
            {
               
            }
            return rest_token;
        }

         //<summary>
         //使用用户名和密码登陆并获取授权token
         //</summary>
         //<returns>token</returns>
        public string GetAccessToken(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            { return string.Empty; }

            if (!string.IsNullOrWhiteSpace(this.token) && this.tokenExpireTime.CompareTo(DateTime.Now) > 0)
            {
                return this.token;
            }

            string postUrl = ConfUtil.EMCHAT_REST_URL + "token";
            StringBuilder sendMessage = new StringBuilder("{");
            sendMessage.AppendFormat("\"grant_type\": \"password\", \"username\":\"{0}\", \"password\":\"{1}\"", username, password);
            //sendMessage.AppendFormat("\"grant_type\": \"client_credentials\", \"client_id\":\"{0}\", \"client_secret\":\"{1}\"", ConfUtil.EMCHAT_CLIENT_ID, ConfUtil.EMCHAT_CLIENT_SECRET);
            sendMessage.Append("}");
            string postResultStr = HttpUtil.SendAndReceiveMessage(postUrl, sendMessage.ToString());
            this.token = string.Empty;
            int expireSeconds = 0;
            try
            {
                JObject jo = JObject.Parse(postResultStr);
                token = jo.GetValue("access_token").ToString();
                int.TryParse(jo.GetValue("expires_in").ToString(), out expireSeconds);
                //设置缓存时效
                if (!string.IsNullOrWhiteSpace(this.token) && expireSeconds > 0)
                {
                    this.tokenExpireTime = DateTime.Now.AddSeconds(expireSeconds);
                }
                else
                {
                    throw new Exception("更新Token失败！");
                }
            }
            catch
            {
               
                
            }
            return token;
        }


        //发送文本消息
        public bool Send(string sendUserName, string recvUserName, string message,string contactRecordId, ref string timestamp)
        {
            timestamp = string.Empty;
            //方式1，消息对象赋值后利用JsonUtil转换
            //方式2，直接使用Json对象构建消息
            JObject jsonBody = new JObject();
            jsonBody.Add("target_type", "users");
            JArray l = new JArray() { recvUserName };
            jsonBody.Add("target", l);
            JObject jsonMsg = new JObject();
            jsonMsg.Add("type", "txt");
            jsonMsg.Add("msg", message);
            JObject jsonExt = new JObject();
            jsonExt.Add("contactRecordId",Convert.ToInt32(contactRecordId));
            jsonExt.Add("sendTime", GetCreatetime());
            jsonExt.Add("extUniqueKey", System.Guid.NewGuid());
            jsonBody.Add("msg", jsonMsg);
            jsonBody.Add("ext", jsonExt);
            if (!string.IsNullOrWhiteSpace(sendUserName))
            {
                jsonBody.Add("from", sendUserName);
            }
            string reqBody = JsonUtil.ToJsonString(jsonBody);
            string respBody = HttpUtil.SendAndReceiveMessage(ConfUtil.EMCHAT_REST_URL + "messages", reqBody, rest_token);
            JObject respJObj = JObject.Parse(respBody);

            timestamp = respJObj.GetValue("timestamp").ToString();
            string respData = respJObj.GetValue("data").ToString();
            respJObj = JObject.Parse(respData);
            string sendResult = respJObj.GetValue(recvUserName).ToString();
            if ("success".Equals(sendResult))
            {
                return true; 
            }
            else
            {
                return false;
            }
        }

        /// <summary>  
        /// 时间戳Timestamp  
        /// </summary>  
        /// <returns></returns>  
        private  string GetCreatetime()
        {
            //TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1,0, 0, 0, 0));
            double dbResult = (DateTime.Now - startTime).TotalSeconds*1000;
            long intResult = (long)dbResult;
            return intResult.ToString(); 
            //return Convert.ToInt64(ts.TotalSeconds).ToString();  
        }

    }
}
