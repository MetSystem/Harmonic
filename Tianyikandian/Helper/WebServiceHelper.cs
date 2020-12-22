using System;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using Tianyi.Soap;

namespace Power
{
    public class WebServiceHelper
    {
        public static TReponse CallWebService<TRequest, TReponse>(string url, TRequest tRequest)
        {
            string soapText =
         @"<?xml version=""1.0"" encoding =""utf -8"" ?>
<soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
  <soap:Body>
    <extTerminalListReq xmlns=""http://www.sttri.com.cn/ns1MobileServices/"">
      <AppKey>0</AppKey>
      <Nonce>0</Nonce>
      <CurTime>0</CurTime>
      <CheckNum>0</CheckNum>
      <Account>0</Account>
      <CurrPage>0</CurrPage>
      <PageSize>0</PageSize>
      <OrderFlag>0</OrderFlag>
      <DevName>0</DevName>
      <SortFlag>0</SortFlag>
      <SpDataFlag>0</SpDataFlag>
      <DevID>0</DevID>
      <OnlineFlag>0</OnlineFlag>
    </extTerminalListReq>
  </soap:Body>
</soap:Envelope>";

            soapText=@"<?xml version=""1.0"" encoding=""utf-8""?>
<soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
  <soap:Body>
    <queryUserMarkOnlineRateReq xmlns=""http://www.sttri.com.cn/ns1MobileServices/"">
      <AppKey />
      <Nonce />
      <CurTime />
      <CheckNum />
      <Account />
      <UserMark />
    </queryUserMarkOnlineRateReq>
  </soap:Body>
</soap:Envelope>";

            var requestName = typeof(TReponse).Name;
            var requestData = new SoapRoot<TRequest>() {  Body = tRequest  };


            soapText = XmlSerializeHelper.Serialize<SoapRoot<TRequest>>(requestData);
            soapText = soapText.Replace("<Body>", $"<soap:Body><{requestName}>");
            soapText = soapText.Replace("</Body>", $"</{requestName}></soap:Body>");

            using (WebClient webClient = new WebClient())
            {
                webClient.Proxy = null;
                byte[] postDatabyte = Encoding.GetEncoding("UTF-8").GetBytes(soapText);

                webClient.Headers.Add("Content-Type", "text/xml");
                byte[] responseData = webClient.UploadData(url, "POST", postDatabyte);
                //解码 
                string responseStr = Encoding.GetEncoding("UTF-8").GetString(responseData);

                var reponse = XmlSerializeHelper.DeSerialize<SoapRoot<TReponse>>(responseStr);
                return reponse.Body;
            }
        }
    }
}
