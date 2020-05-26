using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;
using System;

namespace RuiYingServer
{
    public class GDProtocolSessionV2 : AppSession<GDProtocolSessionV2, GDProtocolRequestInfo>
    {
        protected override void HandleException(Exception e)
        {

        }
    }
}