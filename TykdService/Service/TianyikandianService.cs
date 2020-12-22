using System;
using System.Threading.Tasks;
using TykdService.Tykd;

namespace TykdService
{
    public class TianyikandianService
    {
        public string AppKey { get; set; } = System.Configuration.ConfigurationSettings.AppSettings.Get("AppKey");
        public string AppSecret { get; set; } = System.Configuration.ConfigurationSettings.AppSettings.Get("AppSecret");
        public string Account { get; set; } = System.Configuration.ConfigurationSettings.AppSettings.Get("Account");

        public string GetCheckSum(string nonce, string appSecret, string curTime)
        {
            return SecurityHelper.Sha1(nonce + appSecret + curTime).ToLower();
        }

        /// <summary>
        /// 获取设备列表
        /// </summary>
        public string GetExtTerminalList()
        {
            TimeSpan timeSpan = (DateTime.UtcNow - new DateTime(1970, 1, 1));
            string curTime = ((int)timeSpan.TotalSeconds).ToString();
            var nonce = StringHelper.Random(32);
            var checkNum = GetCheckSum(nonce, AppSecret, curTime);

            Tykd.ExtMobileServicesPortTypeClient client = new Tykd.ExtMobileServicesPortTypeClient();
            var data = client.extTerminalList(new extTerminalListReq
            {
                AppKey = AppKey,
                Nonce = nonce,
                CurTime = curTime,
                CheckNum = checkNum,
                Account = Account,
                CurrPage = 0,
                PageSize = 10,
                SpDataFlag = "0"
            });

            GetExtGetPlayUrlHX("8128B493002", 0);
            return null;
        }

        public void GetExtGetPlayUrlHX(string devId, int channelNo)
        {
            TimeSpan timeSpan = (DateTime.UtcNow - new DateTime(1970, 1, 1));
            string curTime = ((int)timeSpan.TotalSeconds).ToString();
            var nonce = StringHelper.Random(32);
            var checkNum = GetCheckSum(nonce, AppSecret, curTime);
            ExtMobileServicesPortTypeClient client = new ExtMobileServicesPortTypeClient();
            Task<extGetPlayUrlHXResponse> responseTask = client.extGetPlayUrlHXAsync(new extGetPlayUrlHXReq()
            {
                AppKey = AppKey,
                Nonce = nonce,
                CurTime = curTime,
                CheckNum = checkNum,
                StreamType = 1,
                DevID = devId,
                ChannelNo = channelNo,
                Account = Account,
                ClientType = ""
            });
            var response = responseTask.Result.extGetPlayUrlHXRes;

        }
    }
}
