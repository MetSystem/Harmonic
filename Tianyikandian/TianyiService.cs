using Power;
using System;
using System.Threading.Tasks;
using Tianyi.Option;

namespace Tianyi
{
    public class TianyiService
    {
        private TianyiOptions TianyiOption { get; set; }

        public TianyiService(string appKey, string appSecret, string account)
        {
            TianyiOption = new TianyiOptions()
            {
                AppKey = appKey,
                Account = account,
                AppSecret = appSecret
            };
        }

        public string GetCheckSum(string nonce, string appSecret, string curTime)
        {
            return SecurityHelper.Sha1(nonce + appSecret + curTime).ToLower();
        }

        /// <summary>
        /// 获取设备列表
        /// </summary>
        public void GetExtTerminalList()
        {
            var appSecret = TianyiOption.AppSecret;
            TimeSpan timeSpan = (DateTime.UtcNow - new DateTime(1970, 1, 1));
            string curTime = ((int)timeSpan.TotalSeconds).ToString();
            var nonce = StringHelper.Random(32);
            var checkNum = GetCheckSum(nonce, appSecret, curTime);
            var data = WebServiceHelper.CallWebService<queryUserMarkOnlineRateReq, queryUserMarkOnlineRateRes>("http://www.189eyes.com:9000/cxf/ExtMobileService", new queryUserMarkOnlineRateReq()
            {
                AppKey = TianyiOption.AppKey,
                Nonce = nonce,
                CurTime = curTime,
                CheckNum = checkNum,
                Account = TianyiOption.Account,
            });
        }

        public string GetExtGetPlayUrlHX(string devId, int channelNo)
        {
            var appSecret = TianyiOption.AppSecret;
            TimeSpan timeSpan = (DateTime.UtcNow - new DateTime(1970, 1, 1));
            string curTime = ((int)timeSpan.TotalSeconds).ToString();
            var nonce = StringHelper.Random(32);
            var checkNum = GetCheckSum(nonce, appSecret, curTime);
            ExtMobileServicesPortTypeClient client = new ExtMobileServicesPortTypeClient();
            var response = client.extGetPlayUrlHX(new extGetPlayUrlHXReq()
            {
                AppKey = TianyiOption.AppKey,
                Nonce = nonce,
                CurTime = curTime,
                CheckNum = checkNum,
                StreamType = 1,
                DevID = devId,
                ChannelNo = channelNo,
                Account = TianyiOption.Account,
                ClientType = ""
            });
            return response.RelayPlayUrl;
        }
    }
}
