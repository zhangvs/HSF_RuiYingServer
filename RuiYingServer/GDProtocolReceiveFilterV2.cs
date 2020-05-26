using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase.Protocol;
using SuperSocket.Facility.Protocol;//
using SuperSocket.Common;//

namespace RuiYingServer
{
    /// <summary>
    /// 广东规约过滤器V2,(帧格式为GDProtocolRequestInfo)
    /// </summary>
    public class GDProtocolReceiveFilterV2 : FixedHeaderReceiveFilter<GDProtocolRequestInfo>
    {
        public GDProtocolReceiveFilterV2()
            : base(16)
        {

        }

        /// <summary>
        /// 获取数据域和结尾字节长度
        /// </summary>
        /// <param name="header"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        protected override int GetBodyLengthFromHeader(byte[] header, int offset, int length)
        {
            //length为头部(包含两字节的length)长度

            //获取高位
            byte high = header[offset + length - 1];
            //获取低位
            byte low = header[offset + length - 2];
            int len = (int)high * 256 + low;
            return len + 2;//结尾有2个字节
        }

        /// <summary>
        /// 实现帧内容解析
        /// </summary>
        /// <param name="header"></param>
        /// <param name="bodyBuffer"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        protected override GDProtocolRequestInfo ResolveRequestInfo(ArraySegment<byte> header, byte[] bodyBuffer, int offset, int length)
        {
            GDProtocolRequestInfo res = new GDProtocolRequestInfo();
            string entireFrame = BytesToHexStr(header.Array) + BytesToHexStr(bodyBuffer.CloneRange(offset, length));
            //res.EntireFrame = entireFrame;
            res.DeviceLogicalCode = entireFrame.Substring(2, 8);
            res.Seq = entireFrame.Substring(10, 4);
            res.ControlCode = entireFrame.Substring(16, 2);
            res.Length = entireFrame.Substring(18, 4);
            int dataLen = int.Parse(HEXtoDEC(ReverseHexString(res.Length)));
            res.Data = entireFrame.Substring(22, dataLen * 2);
            res.Cs = entireFrame.Substring(22 + dataLen * 2, 2);
            return res;
        }

        /// <summary>
        /// 高低对调
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        string ReverseHexString(string str)
        {
            char[] buff = new char[str.Length];
            for (int i = 0; i < str.Length; i += 2)
            {
                buff[i] = str[str.Length - i - 2];
                buff[i + 1] = str[str.Length - 1 - i];
            }
            string s = new string(buff);
            return s;
        }

        /// <summary>
        /// 16进制转10进制
        /// </summary>
        /// <param name="HEX"></param>
        /// <returns></returns>
        string HEXtoDEC(string HEX)
        {
            return Convert.ToInt64(HEX, 16).ToString();
        }

        /// <summary>
        /// 转化bytes成16进制的字符
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        string BytesToHexStr(byte[] bytes)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    returnStr += bytes[i].ToString("X2");
                }
            }
            return returnStr;
        }

        // 16进制字符串转字节数组   格式为 string sendMessage = "00 01 00 00 00 06 FF 05 00 64 00 00";
        private static byte[] HexStrTobyte(string hexString)
        {
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
                hexString += " ";
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2).Trim(), 16);
            return returnBytes;
        }


        // 字节数组转16进制字符串   
        public static string byteToHexStr(byte[] bytes)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    returnStr += bytes[i].ToString("X2");//ToString("X2") 为C#中的字符串格式控制符
                }
            }
            return returnStr;
        }
        //字节数组转16进制更简单的，利用BitConverter.ToString方法
        //string str0x = BitConverter.ToString(result, 0, result.Length).Replace("-"," ");
    }
}