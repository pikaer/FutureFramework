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

        private readonly IBottleBiz bottle = SingletonProvider<BottleBiz>.Instance;

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
                if (uId > 0)
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
                if (uId > 0)
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
        public async Task SubScribeMessage(long uId, long partnerUId)
        {
            try
            {
                if (partnerUId>0)
                {
                    var userHub = userBiz.OnLineUser(partnerUId);
                    if (userHub != null && userHub.IsOnLine)
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
