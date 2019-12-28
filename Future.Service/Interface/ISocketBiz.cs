namespace Future.Service.Interface
{
    public interface ISocketBiz
    {
        /// <summary>
        /// 开启连接
        /// </summary>
        /// <param name="connectionId">连接Id</param>
        /// <param name="uId">用户Id</param>
        /// <param name="partnerUId">用户Id</param>
        /// <param name="connetType">0:Online 1:OnChatList 2:OnChat</param>
        void OnConnected(string connectionId, long uId, long partnerUId, int connetType);

        /// <summary>
        /// 断开连接
        /// </summary>
        /// <param name="connectionId">连接Id</param>
        /// <param name="uId">用户Id</param>
        /// <param name="connetType">0:Online 1:OnChatList 2:OnChat</param>
        void OnDisconnected(string connectionId, long uId,int connetType);
    }
}
