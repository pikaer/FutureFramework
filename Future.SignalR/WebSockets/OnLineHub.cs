using Future.Service.Implement;
using Future.Service.Interface;
using Future.Utility;
using Infrastructure;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace Future.SignalR.WebSockets
{
    /// <summary>
    /// 在线用户集线器
    /// </summary>
    public class OnLineHub : Hub
    {
        private readonly IUserBiz userBiz = SingletonProvider<UserBiz>.Instance;

        private readonly ISocketBiz socketBiz = SingletonProvider<SocketBiz>.Instance;

        /// <summary>
        /// 成功连接
        /// </summary>
        /// <returns></returns>
        public override async Task OnConnectedAsync()
        {
            try
            {
                await base.OnConnectedAsync();

                //用户连接信息同步到数据库目的：不同页面之间共享用户行为信息
                long uId = Convert.ToInt64(Context.GetHttpContext().Request.Query["UId"]);

                long partnerUId = Convert.ToInt64(Context.GetHttpContext().Request.Query["PartnerUId"]);

                //0:Online 1:OnChatList 2:OnChat
                int connetType = Convert.ToInt16(Context.GetHttpContext().Request.Query["ConnetType"]);

                socketBiz.OnConnected(Context.ConnectionId, uId, partnerUId, connetType);
            }
            catch (Exception ex)
            {
                LogHelper.ErrorAsync("OnLineHub.OnConnectedAsync", ex);
            }
        }

        /// <summary>
        /// 断开连接
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public override async Task OnDisconnectedAsync(Exception exception = null)
        {
            try
            {
                long uId = Convert.ToInt64(Context.GetHttpContext().Request.Query["UId"]);

                int connetType = Convert.ToInt16(Context.GetHttpContext().Request.Query["ConnetType"]);

                socketBiz.OnDisconnected(Context.ConnectionId, uId,connetType);
            }
            catch (Exception ex)
            {
                LogHelper.ErrorAsync("OnLineHub.OnDisconnectedAsync", ex);
            }
            finally
            {
                await base.OnDisconnectedAsync(exception);
            }
        }

        /// <summary>
        /// 订阅消息
        /// </summary>
        public async Task SubScribeMessage(long partnerUId)
        {
            try
            {
                if (partnerUId>0)
                {
                    var onLineUser = userBiz.OnLineUser(partnerUId);
                    if (onLineUser != null && onLineUser.IsOnLine)
                    {
                        //当对方正在互动列表页面停留,通知对方刷新页面
                        await Clients.Client(onLineUser.ConnectionId).SendAsync("receive");
                    }

                    var chatListHub = userBiz.ChatListHub(partnerUId);
                    if (chatListHub != null && chatListHub.IsOnLine)
                    {
                        //当对方正在互动列表页面停留,通知对方刷新页面
                        await Clients.Client(chatListHub.ConnectionId).SendAsync("receive");
                    }

                    var userHub = userBiz.OnChatHub(partnerUId);
                    if (userHub != null&& userHub.IsOnLine)
                    {
                        //当对方正在互动列表页面停留,通知对方刷新页面
                        await Clients.Client(userHub.ConnectionId).SendAsync("receive");
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.ErrorAsync("ChatListHub.SubScribeMessage", ex);
            }
        }
    }
}
