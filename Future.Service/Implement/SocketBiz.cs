using Future.Model.Entity.Hubs;
using Future.Service.Interface;
using Future.Utility;
using Infrastructure;
using System;

namespace Future.Service.Implement
{
    public class SocketBiz : ISocketBiz
    {
        private readonly IUserBiz userBiz = SingletonProvider<UserBiz>.Instance;

        public void OnConnected(string connectionId, long uId, long partnerUId, int connetType)
        {
            try
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
                            ConnectionId = connectionId,
                            IsOnLine = true,
                            CreateTime = DateTime.Now,
                            UpdateTime = DateTime.Now
                        };
                        userBiz.InsertOnLineUserAsync(onLineUser);
                    }
                    else
                    {
                        onLineUser.ConnectionId = connectionId;
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
                            ConnectionId = connectionId,
                            IsOnLine = true,
                            CreateTime = DateTime.Now,
                            UpdateTime = DateTime.Now
                        };
                        userBiz.InsertChatListHubAsync(chatListHub);
                    }
                    else
                    {
                        chatListHub.ConnectionId = connectionId;
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
                                ConnectionId = connectionId,
                                IsOnLine = true,
                                CreateTime = DateTime.Now,
                                UpdateTime = DateTime.Now
                            };
                            userBiz.InsertOnChatHubAsync(userHub);
                        }
                        else
                        {
                            userHub.ConnectionId = connectionId;
                            userHub.IsOnLine = true;
                            userHub.PartnerUId = partnerUId;
                            userHub.UpdateTime = DateTime.Now;
                            userBiz.UpdateOnChatHubAsync(userHub);
                        }
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                LogHelper.ErrorAsync("SocketBiz.OnConnected", ex);
            }

          
        }

        public void OnDisconnected(string connectionId, long uId, int connetType)
        {
            try
            {
                if (uId > 0)
                {
                    if (connetType == 0)
                    {
                        var onLineUser = userBiz.OnLineUser(uId);
                        if (onLineUser != null)
                        {
                            onLineUser.ConnectionId = connectionId;
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
                            chatListHub.ConnectionId = connectionId;
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
                            userHub.ConnectionId = connectionId;
                            userHub.IsOnLine = false;
                            userHub.UpdateTime = DateTime.Now;
                            userHub.LastOnLineTime = DateTime.Now;
                            userBiz.UpdateOnChatHubAsync(userHub);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                LogHelper.ErrorAsync("SocketBiz.OnDisconnected", ex);
            }
        }
    }
}
