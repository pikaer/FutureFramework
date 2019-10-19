using Future.Model.Entity.Hubs;
using Future.Service.Implement;
using Future.Service.Interface;
using Future.Utility;
using Infrastructure;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace Future.TodayApi.WebSockets
{
    /// <summary>
    /// 聊天列表集线器
    /// </summary>
    public class ChatListHub:Hub
    {
        private readonly IUserBiz userBiz = SingletonProvider<UserBiz>.Instance;

        /// <summary>
        /// 成功连接
        /// </summary>
        /// <returns></returns>
        public override async Task OnConnectedAsync()
        {
            try
            {
                //用户连接信息同步到数据库目的：不同页面之间共享用户行为信息
                long uId = Convert.ToInt64(Context.GetHttpContext().Request.Query["UId"]);
                if (uId > 0)
                {
                    var chatListHub = userBiz.ChatListHub(uId);
                    if (chatListHub == null)
                    {
                        chatListHub = new ChatListHubEntity()
                        {
                            ChatListHubId = Guid.NewGuid(),
                            UId = uId,
                            ConnectionId = Context.ConnectionId,
                            IsOnLine = true,
                            CreateTime = DateTime.Now,
                            UpdateTime = DateTime.Now
                        };
                        userBiz.InsertChatListHubAsync(chatListHub);
                    }
                    else
                    {
                        chatListHub.ConnectionId = Context.ConnectionId;
                        chatListHub.IsOnLine = true;
                        chatListHub.UpdateTime = DateTime.Now;
                        userBiz.UpdateChatListHubAsync(chatListHub);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.ErrorAsync("ChatListHub.OnConnectedAsync", ex);
            }
            finally
            {
                await base.OnConnectedAsync();
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
                if (uId > 0)
                {
                    var chatListHub = userBiz.ChatListHub(uId);
                    if (chatListHub != null)
                    {
                        chatListHub.ConnectionId = Context.ConnectionId;
                        chatListHub.IsOnLine = false;
                        chatListHub.UpdateTime = DateTime.Now;
                        chatListHub.LastOnLineTime = DateTime.Now;
                        userBiz.UpdateChatListHubAsync(chatListHub);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.ErrorAsync("ChatListHub.OnDisconnectedAsync", ex);
            }
            finally
            {
                await base.OnDisconnectedAsync(exception);
            }
        }
    }
}
