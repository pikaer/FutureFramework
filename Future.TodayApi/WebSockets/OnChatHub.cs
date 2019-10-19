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
    /// 聊天集线器
    /// </summary>
    public class OnChatHub : Hub
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
                long partnerUId = Convert.ToInt64(Context.GetHttpContext().Request.Query["PartnerUId"]);
                if (uId > 0 && partnerUId > 0)
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
            catch (Exception ex)
            {
                LogHelper.ErrorAsync("OnLineHub.OnConnectedAsync", ex);
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
                long partnerUId = Convert.ToInt64(Context.GetHttpContext().Request.Query["PartnerUId"]);
                if (uId > 0&& partnerUId>0)
                {
                    var userHub = userBiz.OnChatHub(uId);
                    if (userHub != null)
                    {
                        userHub.ConnectionId = Context.ConnectionId;
                        userHub.IsOnLine = false;
                        userHub.PartnerUId = partnerUId;
                        userHub.UpdateTime = DateTime.Now;
                        userHub.LastOnLineTime = DateTime.Now;
                        userBiz.UpdateOnChatHubAsync(userHub);
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
    }
}
