using Newtonsoft.Json.Linq;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FFXIV_Translator.PapagoAPI
{
    public class PapagoAPI
    {
        private static readonly string clientID = "<your client id>";
        private static readonly string clientSecret = "<your client secret>";

        public static async Task<string> Translate(string text, string source, string target)
        {
            HttpWebRequest request = WebRequest.Create("https://openapi.naver.com/v1/papago/n2mt") as HttpWebRequest;
            request.Headers.Add("X-Naver-Client-Id", clientID);
            request.Headers.Add("X-Naver-Client-Secret", clientSecret);
            request.Method = "POST";

            byte[] byteDataParams = Encoding.UTF8.GetBytes("source=" + source + "&target=" + target + "&text=" + text);
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteDataParams.Length;
            Stream st = request.GetRequestStream();
            st.Write(byteDataParams, 0, byteDataParams.Length);
            st.Close();

            try
            {
                using (HttpWebResponse response = await Task.Factory.FromAsync(request.BeginGetResponse, request.EndGetResponse, null) as HttpWebResponse)
                using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
                {
                    string result = await streamReader.ReadToEndAsync();
                    JObject jObject = JObject.Parse(result);
                    return jObject["message"]["result"]["translatedText"].ToString();
                    //{"message":{"@type":"response","@service":"naverservice.nmt.proxy","@version":"1.0.0","result":{"srcLangType":"ko","tarLangType":"en","translatedText":"Hello","engineType":"PRETRANS","pivot":null}}}
                }
            }
            catch (WebException e)
            {
                HttpWebResponse response = e.Response as HttpWebResponse;
                if ((int)response.StatusCode == 429)
                {
                    return "하루 or 초당 호출 한도를 초과했습니다.";
                }
                else
                {
                    return e.Message;
                }
            }
        }

        public static async Task<string> Translate(string text, string target)
        {
            string source = await Detect(text);
            if (source.Equals(target))
                return text;

            return await Translate(text, source, target);
        }

        public static async Task<string> Detect(string text)
        {
            HttpWebRequest request = WebRequest.Create("https://openapi.naver.com/v1/papago/detectLangs") as HttpWebRequest;
            request.Headers.Add("X-Naver-Client-Id", clientID);
            request.Headers.Add("X-Naver-Client-Secret", clientSecret);
            request.Method = "POST";

            byte[] byteDataParams = Encoding.UTF8.GetBytes("query=" + text);
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteDataParams.Length;
            Stream st = request.GetRequestStream();
            st.Write(byteDataParams, 0, byteDataParams.Length);
            st.Close();

            try
            {
                using (HttpWebResponse response = await Task.Factory.FromAsync(request.BeginGetResponse, request.EndGetResponse, null) as HttpWebResponse)
                using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
                {
                    string result = await streamReader.ReadToEndAsync();
                    JObject jObject = JObject.Parse(result);

                    return jObject["langCode"].ToString();
                    //{"langCode":"ko"}
                }
            }
            catch (WebException e)
            {
                HttpWebResponse response = e.Response as HttpWebResponse;
                if ((int)response.StatusCode == 429)
                {
                    return "하루 or 초당 호출 한도를 초과했습니다.";
                }
                else
                {
                    return e.Message;
                }
            }
        }
    }
}
