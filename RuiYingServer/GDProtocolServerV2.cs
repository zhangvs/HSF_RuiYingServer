using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;

namespace RuiYingServer
{
    public class GDProtocolServerV2 : AppServer<GDProtocolSessionV2, GDProtocolRequestInfo>
    {
        public GDProtocolServerV2()
            : base(new DefaultReceiveFilterFactory<GDProtocolReceiveFilterV2, GDProtocolRequestInfo>()) //使用默认的接受过滤器工厂 (DefaultReceiveFilterFactory)
        {
        }
    }
}
