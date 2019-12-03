using Future.Model.Entity.Hubs;
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
                if (uId > 0)
                {
                    #region 在线信息
                    if (connetType == 0)
                    {
                        var onLineUser = userBiz.OnLineUser(uId);
                        if (onLineUser == null)
                        {
                            onLineUser = new OnLineUserHubEntity()
                            {
                                OnLineId = Guid.NewGuid(),
                                UId = uId,
                                ConnectionId = Context.ConnectionId,
                                IsOnLine = true,
                                CreateTime = DateTime.Now,
                                UpdateTime = DateTime.Now
                            };
                            userBiz.InsertOnLineUserAsync(onLineUser);
                        }
                        else
                        {
                            onLineUser.ConnectionId = Context.ConnectionId;
                            onLineUser.IsOnLine = true;
                            onLineUser.UpdateTime = DateTime.Now;
                            userBiz.UpdateOnLineUserAsync(onLineUser);
                        }
                    }
                    #endregion

                    #region 聊天页面在线状态
                    else if (connetType == 1)
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
                    #endregion

                    #region 聊天互动功能
                    else if (connetType == 2)
                    {
                        if (partnerUId > 0)
                        {
                            var userHub = userBiz.OnChatHub(uId);
                            if (userHub == null)
                            {
                                userHub = new OnChatHubEntity()
                                {
                                    OnChatHubId = Guid.NewGuid(),
                                    UId = uId,
                                    PartnerUId = partnerUId,
                                    ConnectionId = Context.ConnectionId,
                                    IsOnLine = true,
                                    CreateTime = DateTime.Now,
                                    UpdateTime = DateTime.Now
                                };
                                userBiz.InsertOnChatHubAsync(userHub);
                            }
                            else
                            {
                                userHub.ConnectionId = Context.ConnectionId;
                                userHub.IsOnLine = true;
                                userHub.PartnerUId = partnerUId;
                                userHub.UpdateTime = DateTime.Now;
                                userBiz.UpdateOnChatHubAsync(userHub);
                            }
                        }
                    }
                    #endregion
                }
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
                if (uId > 0)
                {
                    if (connetType == 0)
                    {
                        var onLineUser = userBiz.OnLineUser(uId);
                        if (onLineUser != null)
                        {
                            onLineUser.ConnectionId = Context.ConnectionId;
                            onLineUser.IsOnLine = false;
                            onLineUser.UpdateTime = DateTime.Now;
                            onLineUser.LastOnLineTime = DateTime.Now;
                            userBiz.UpdateOnLineUserAsync(onLineUser);
                        }
                    }

                    if (connetType == 1)
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

                    if (connetType == 2)
                    {
                        var userHub = userBiz.OnChatHub(uId);
                        if (userHub != null)
                        {
                            userHub.ConnectionId = Context.ConnectionId;
                            userHub.IsOnLine = false;
                            userHub.UpdateTime = DateTime.Now;
                            userHub.LastOnLineTime = DateTime.Now;
                            userBiz.UpdateOnChatHubAsync(userHub);
                        }
                    }
                }
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
