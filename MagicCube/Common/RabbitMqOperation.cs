
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Security.Cryptography;
using System.IO;
using System.Threading;


namespace MagicCube.Common
{
    public class RabbitMqOperation
    {
        //#region "变量&&委托"
        //ConnectionFactory factory = new ConnectionFactory();
        //IConnection conn;
        //IModel ch;
        //public Action<string> RecieveMessageAction;
        //private void RecieveMessageTrigger(string str)
        //{
        //    if(RecieveMessageAction != null)
        //    {
        //        RecieveMessageAction(str);
        //    }
        //}
        //#endregion

        //#region "常量"
        //private  string userName = "m%A0%3E%D5%5B%E6%DA%C9%AF3y%11D%F1%C0%C4%C51A5%A3%3B%DC%A7Q5%BCz%D3%BE%AF%AF%AB%D5%8E%A4%D9%81W%D2+%B7%E2C%E2%19%D7%28l%3D%8A%AA%9D70%C8%98%E7%CC%B1%FFF%BD%1E%CA%D4%9C%A6%23Q%DA%DF%7B%E737%BFi%E9%8DV%CE%1B%3B4%B3%F6%1A%14%BA%89%15lzok%3B%22.%FA%5Bz%21%FCP%EA_j%2B%8B5%CF3%82%C3%24%94%09h%0E%D1qS%A3%D8%5B0%0A";
        //private  string password = "%27%F1%9B%00%CEt%F7%E9%EE%F6%08%E0%D3%9F%85%ABN%3E%3D%B8%EC%3F%E7%B5%3B%9A%07%A7J%F8%EC%CC%21%A0K%3F%02%94%F8R%3Bj%1B%A3y%0B%86%21%B4%04%0F%00%0C%16n8%B1%86%16%BF%B5So%23%9B%A8%AA%DC%D2%C318K%0E%FBb%F8m%0A%13%C1%D4D%C9%0AU%03%60%EE%F5%FE%AF%F1%DD%7E%DE%DFn%A5%AB%0E%23L%2Ci%97%5B%2AC%14%B6%E3%B7%97%03%B7%AB%8Am3%9B%D0%A9%BAz%BC%D1%C2";
        //private  string virturalHost = "/enterprise_user";

        //private  string exchangeName = "ex_job_downLoad";
        //private string editExchangeName = string.Empty;
        //private  int userId;
        //public int UserId
        //{
        //    get { return userId; }
        //    set { userId = value; }
        //}
        //private const string userType = "ENTERPRISE_USER";
        //#endregion

        //#region "构造函数"
        //private static RabbitMqOperation instance;
        //private static object lockObj = new object();
        //private static RabbitMqOperation Instance
        //{
        //    get
        //    {
        //        if(instance == null)
        //        {
        //            lock(lockObj)
        //            {
        //                if (instance == null)
        //                    instance = new RabbitMqOperation();
        //            }
        //        }
        //        return instance;
        //    }
        //}
        //public RabbitMqOperation()
        //{
        //    this.InitalCustmerMq();
        //}
        //public RabbitMqOperation(string userName, string password, string virturalHost, string exchangeName, int userId)
        //{
        //    this.userName = userName;
        //    this.password = password;
        //    this.virturalHost = virturalHost;
        //    this.exchangeName = exchangeName;
        //    this.userId = userId;
        //    this.InitalCustmerMq();
        //}
        //public RabbitMqOperation(int userId, string exchangeName)
        //{
        //    this.exchangeName = exchangeName;
        //    this.userId = userId;
        //    this.InitalCustmerMq();
        //}
        //public RabbitMqOperation(int userId)
        //{
        //    this.userId = userId;
        //    this.InitalCustmerMq();
        //}
        //#endregion

        //#region "公共方法"
        ///// <summary>
        ///// 启动mq接收线程
        ///// </summary>
        //public void StartRetrievingMessage()
        //{
        //    conn = factory.CreateConnection();
        //    ch = conn.CreateModel();
        //    string queueName = this.MD5Encrypt(userId.ToString() + "_" + userType) + "_" + exchangeName;
        //    editExchangeName = queueName;
        //    ch.ExchangeDeclare(editExchangeName, ExchangeType.Fanout);
        //    ch.QueueDeclare(queueName, false, false, false, null);
        //    ch.QueueBind(queueName, editExchangeName, queueName, null);
        //    var consumer = new EventingBasicConsumer(ch);
        //    consumer.Received += this.ReTrievingCallback;
        //    ch.BasicConsume(queue: queueName,
        //                         noAck: true,
        //                         consumer: consumer);



        //    ////普通使用方式BasicGet
        //    ////noAck = true，不需要回复，接收到消息后，queue上的消息就会清除
        //    ////noAck = false，需要回复，接收到消息后，queue上的消息不会被清除，直到调用channel.basicAck(deliveryTag, false); queue上的消息才会被清除 而且，在当前连接断开以前，其它客户端将不能收到此queue上的消息
        //    //BasicGetResult res = ch.BasicGet("q1", false/*noAck*/);
        //    //if (res != null)
        //    //{
        //    //    bool t = res.Redelivered;
        //    //    t = true;
        //    //    Console.WriteLine(System.Text.UTF8Encoding.UTF8.GetString(res.Body));
        //    //    ch.BasicAck(res.DeliveryTag, false);
        //    //}
        //    //else
        //    //{
        //    //    Console.WriteLine("No message！");
        //    //}
        //}
        ///// <summary>
        ///// 关闭mq连接
        ///// </summary>
        //public void Close()
        //{
        //    try
        //    {
        //        if (ch != null)
        //            ch.Close();
        //        if (conn != null)
        //            conn.Close();
        //    }
        //    catch(Exception e) 
        //    {
        //        Console.WriteLine("关闭错误信息:" + e.Message);
        //    }
        //}

        //public void Listen(object obj)
        //{
        //    try
        //    {
        //        using (conn = factory.CreateConnection())
        //        {
        //            using (ch = conn.CreateModel())
        //            {
        //                string queueName = this.MD5Encrypt(userId.ToString() + "_" + userType) + "_" + exchangeName;
        //                editExchangeName = queueName;
        //                ch.ExchangeDeclare(editExchangeName, ExchangeType.Fanout);
        //                ch.QueueDeclare(queueName, false, false, false, null);
        //                ch.QueueBind(queueName, editExchangeName, queueName, null);

        //                Console.WriteLine("RabbitMQListening...");

        //                //在队列上定义一个消费者
        //                QueueingBasicConsumer consumer = new QueueingBasicConsumer(ch);
        //                ch.BasicConsume(queueName, true, consumer);

        //                while (true)
        //                {
                            
        //                    try
        //                    {
        //                        //阻塞函数，获取队列中的消息
        //                        BasicDeliverEventArgs ea = (BasicDeliverEventArgs)consumer.Queue.Dequeue();
        //                        byte[] body = ea.Body;
        //                        string message = Encoding.UTF8.GetString(body);
        //                        RecieveMessageTrigger(message);
        //                        //ch.BasicAck(ea.DeliveryTag, false); 
        //                    }
        //                    //catch (EndOfStreamException e)
        //                    //{
        //                    //    Console.WriteLine("RabbitMq错误： " + e.Message);
        //                    //    Thread.Sleep(200);
        //                    //}
        //                    catch(Exception ex)
        //                    {
        //                        Console.WriteLine("RabbitMq错误：" + ex.Message);
        //                        break;
        //                    } 
        //                }
        //            }
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        Console.WriteLine("RabbitMq错误：" + ex.Message);
        //    }
         
        //}

        ////public void Send()
        ////{
        ////    //定义要发送的数据
        ////    RequestMessage message = new RequestMessage() { MessageId = Guid.NewGuid(), Message = "this is a 请求。" };

        ////    //创建一个 AMQP 连接
        ////    using (IConnection connection = factory.CreateConnection())
        ////    {
        ////        using (IModel channel = connection.CreateModel())
        ////        {
        ////            //在MQ上定义一个队列
        ////            channel.QueueDeclare("esbtest.rmq.consoleserver", false, false, false, null);

        ////            //序列化消息对象，RabbitMQ并不支持复杂对象的序列化，所以对于自定义的类型需要自己序列化
        ////            XmlSerializer xs = new XmlSerializer(typeof(RequestMessage));
        ////            using (MemoryStream ms = new MemoryStream())
        ////            {
        ////                xs.Serialize(ms, message);
        ////                byte[] bytes = ms.ToArray();
        ////                //指定发送的路由，通过默认的exchange直接发送到指定的队列中。
        ////                channel.BasicPublish("", "esbtest.rmq.consoleserver", null, bytes);
        ////            }

        ////            Console.WriteLine(string.Format("Request Message Sent, Id:{0}, Message:{1}", message.MessageId, message.Message));
        ////        }
        ////    }
        ////}


        //#endregion

        //#region "对内功能函数"
        ///// <summary>
        ///// 初始化mq工厂
        ///// </summary>
        //private void InitalCustmerMq()
        //{
        //    factory.UserName = userName;
        //    factory.Password = password;
        //    factory.VirtualHost = virturalHost;
        //    factory.HostName = ConfUtil.RabbitMQServer;
        //    factory.Port = ConfUtil.RabbitMQPort;
        //}
        ///// <summary>
        ///// MQ消息接收回调函数
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void ReTrievingCallback(object sender, BasicDeliverEventArgs e)
        //{
        //    var body = e.Body;
        //    string message = Encoding.UTF8.GetString(body);
        //    RecieveMessageTrigger(message);
        //}
        /////   <summary>
        /////   给一个字符串进行MD5加密
        /////   </summary>
        /////   <param   name="strText">待加密字符串</param>
        /////   <returns>加密后的字符串</returns>
        //private string MD5Encrypt(string strText)
        //{
        //    MD5 md5 = new MD5CryptoServiceProvider();
        //    byte[] result = md5.ComputeHash(System.Text.Encoding.Default.GetBytes(strText));
        //    string strTemp = string.Empty;
        //    foreach(byte item in result)
        //    {
        //        strTemp +=item.ToString("X2");
        //    }
        //    return strTemp.ToLower();
        //} 
        //#endregion
    }
}
