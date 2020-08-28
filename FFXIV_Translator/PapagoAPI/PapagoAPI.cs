using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FFXIV_Translator.PapagoAPIs
{
    public class PapagoAPI
    {
        private static readonly string clientID = "<your client id>";
        private static readonly string clientSecret = "<your client secret>";

        public enum LangCode
        {
            None = -1,
            KR,
            EN,
            JP,

            Count
        }

        public static async Task<string> Translate(string text, LangCode source, LangCode target)
        {
            try
            {
                return await Translate(text, LangCodeToString(source), LangCodeToString(target));
            }
            catch (PapagoAPIException e)
            {
                return e.Message;
            }
        }

        private static async Task<string> Translate(string text, string source, string target)
        {
            text = text.Replace("\r\n", " \\n ");

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

                    return jObject["message"]["result"]["translatedText"].ToString().Replace(" \\n ", "\n");
                    //{"message":{"@type":"response","@service":"naverservice.nmt.proxy","@version":"1.0.0","result":{"srcLangType":"ko","tarLangType":"en","translatedText":"Hello","engineType":"PRETRANS","pivot":null}}}
                }
            }
            catch (WebException e)
            {
                HttpWebResponse response = e.Response as HttpWebResponse;
                if ((int)response.StatusCode == 429)
                {
                    throw new PapagoAPIException("하루 or 초당 호출 한도를 초과했습니다.");
                }
                else
                {
                    throw new PapagoAPIException(e.Message);
                }
            }
        }

        public static async Task<string> Translate(string text, LangCode target)
        {
            try
            {
                return await Translate(text, LangCodeToString(target));
            }
            catch (PapagoAPIException e)
            {
                return e.Message;
            }
        }

        private static async Task<string> Translate(string text, string target)
        {
            try
            {
                string source = await Detect(text);
                if (source.Equals(target) || source.Equals("unk"))
                    return text;
                if (!TryStringToLangCode(source, out LangCode code))
                    return text;

                return await Translate(text, source, target);
            }
            catch (PapagoAPIException e)
            {
                return e.Message;
            }
        }

        private static async Task<string> Detect(string text)
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
                    throw new PapagoAPIException("하루 or 초당 호출 한도를 초과했습니다.");
                }
                else
                {
                    throw new PapagoAPIException(e.Message);
                }
            }
        }

        public static string LangCodeToString(LangCode code)
        {
            switch (code)
            {
                case LangCode.KR:
                    return "ko";
                case LangCode.EN:
                    return "en";
                case LangCode.JP:
                    return "ja";
                default:
                    throw new NotImplementedException();
            }
        }

        public static LangCode StringToLangCode(string str)
        {
            str = str.ToLower();
            switch (str)
            {
                case "ko":
                    return LangCode.KR;
                case "en":
                    return LangCode.EN;
                case "ja":
                    return LangCode.JP;
                default:
                    throw new NotImplementedException();
            }
        }

        public static bool TryStringToLangCode(string str, out LangCode code)
        {
            try
            {
                code = StringToLangCode(str);
            }
            catch
            {
                code = LangCode.None;
                return false;
            }

            return true;
        }
    }
}
