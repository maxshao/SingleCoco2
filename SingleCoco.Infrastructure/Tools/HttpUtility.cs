using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace SingleCoco.Infrastructure.Tools
{
    public class HttpUtility
    {


        #region 请求数据with证书

        public Tuple<T, bool> ReqWithCert<T>(string url, string sslCertPath, string sslPswd, string data) where T : class
        {
            //string data = "playername=" + username;
            //byte[] dataStream = Encoding.UTF8.GetBytes(data);

            HttpWebRequest Request = (HttpWebRequest)WebRequest.Create(url);
            Request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            Request.Headers.Add("Cache-Control", "max-age=0");
            Request.KeepAlive = true;
            //Request.Headers.Add("Keep-Alive", "timeout=5, max=100");
            //Request.Headers.Add("Accept-Charset", "ISO-8859-1,utf-8;q=0.7,*;q=0.3");
            //Request.Headers.Add("Accept-Language", "es-ES,es;q=0.8");
            //Request.Headers.Add("Pragma", "no-cache");
            //Request.Headers.Add("X_ENTITY_KEY", X_ENTITY_KEY);
            Request.Method = "POST";
            Request.ContentType = "application/x-www-form-urlencoded";
            Request.ClientCertificates.Add(new X509Certificate2(sslCertPath, sslPswd, X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.UserKeySet));
            ServicePointManager.ServerCertificateValidationCallback = CertificateValidationCallBack;


            // 添加请求参数
            if (string.IsNullOrEmpty(data))
            {
                byte[] dataStream = Encoding.UTF8.GetBytes(data);
                Request.ContentLength = dataStream.Length;
                Stream newStream = Request.GetRequestStream();
                //Send the data.
                newStream.Write(dataStream, 0, dataStream.Length);
                newStream.Close();
            }

            using (HttpWebResponse Response = (HttpWebResponse)Request.GetResponse())
            {
                StreamReader reader = new StreamReader(Response.GetResponseStream());
                String retData = reader.ReadToEnd();

                JObject jObject = JObject.Parse(retData);

                JToken jresult = null;
                IDictionary<string, JToken> dictionary = jObject;

                if (dictionary.ContainsKey("error"))
                {
                    jresult = jObject["error"];
                    //TODO 写入错误日志
                    return new Tuple<T, bool>(null, false);
                }
                if (dictionary.ContainsKey("result"))
                {
                    jresult = jObject["result"];
                }
                return new Tuple<T, bool>(JsonConvert.DeserializeObject<T>(jresult.ToString()), true);
            }
        }


        public string ReqWithCert(string url, string sslCertPath, string sslPswd, string data)
        {


            HttpWebRequest Request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse Response = null;

            Request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            Request.Headers.Add("Cache-Control", "max-age=0");
            Request.KeepAlive = true;
            //Request.Headers.Add("Keep-Alive", "timeout=5, max=100");
            //Request.Headers.Add("Accept-Charset", "ISO-8859-1,utf-8;q=0.7,*;q=0.3");
            //Request.Headers.Add("Accept-Language", "es-ES,es;q=0.8");
            //Request.Headers.Add("Pragma", "no-cache");
            //Request.Headers.Add("X_ENTITY_KEY", X_ENTITY_KEY);
            Request.Method = "POST";

            Request.ContentType = "application/x-www-form-urlencoded";
            Request.ClientCertificates.Add(new X509Certificate2(sslCertPath, sslPswd, X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.UserKeySet));
            ServicePointManager.ServerCertificateValidationCallback = CertificateValidationCallBack;

            // 添加请求参数   
            if (string.IsNullOrEmpty(data))
            {
                byte[] dataStream = Encoding.UTF8.GetBytes(data);
                Request.ContentLength = dataStream.Length;
                Stream newStream = Request.GetRequestStream();
                //Send the data.
                newStream.Write(dataStream, 0, dataStream.Length);
                newStream.Close();
            }
            Response = (HttpWebResponse)Request.GetResponse();
            StreamReader reader = new StreamReader(Response.GetResponseStream());
            String retData = reader.ReadToEnd();
            return retData;
            //JObject jObject = JObject.Parse(retData);

            //JToken jresult = null;
            //IDictionary<string, JToken> dictionary = jObject;
            //if (dictionary.ContainsKey("result"))
            //{
            //    jresult = jObject["result"];

            //    if (jObject.Count > 0)
            //    {
            //        string playername = jresult["PLAYERNAME"].ToString();
            //        string kioskname = jresult["KIOSKNAME"].ToString();
            //        string kioskadminname = jresult["KIOSKADMINNAME"].ToString();
            //        string isfrozen = jresult["FROZEN"].ToString() == "1" ? "YES" : "NO";
            //    }
            //}
            //if (dictionary.ContainsKey("error"))
            //{
            //    jresult = jObject["error"];
            //}
            //retData = null;
            //jObject = null;
        }
        private static bool CertificateValidationCallBack(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            // If the certificate is a valid, signed certificate, return true.
            if (sslPolicyErrors == System.Net.Security.SslPolicyErrors.None)
            {
                return true;
            }

            // If there are errors in the certificate chain, look at each error to determine the cause.
            if ((sslPolicyErrors & System.Net.Security.SslPolicyErrors.RemoteCertificateChainErrors) != 0)
            {
                if (chain != null && chain.ChainStatus != null)
                {
                    foreach (System.Security.Cryptography.X509Certificates.X509ChainStatus status in chain.ChainStatus)
                    {
                        if ((certificate.Subject == certificate.Issuer) &&
                           (status.Status == System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.UntrustedRoot))
                        {
                            // Self-signed certificates with an untrusted root are valid. 
                            continue;
                        }
                        else
                        {
                            if (status.Status != System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.NoError)
                            {
                                // If there are any other errors in the certificate chain, the certificate is invalid,
                                // so the method returns false.
                                return false;
                            }
                        }
                    }
                }

                return true;
            }
            else
            {
                // In all other cases, return false.
                return false;
            }
        }


        #endregion








    }
}
