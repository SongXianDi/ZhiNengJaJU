using UnityEngine;
using System;
using UnityEngine.Networking;
using System.Collections;
using System.Security.Cryptography;
using System.Text;

public class JiangxiSciencesAbutment : MonoBehaviour
{
    private const int APPID = 1007;
    public string IP
    {
        get
        {
            return "http://192.168.101.99:8002";//测试IP
            //return "https://ilab-x.jxust.edu.cn/jxustapi/";//正式IP
        }
    }
    /// <summary>获取题库的Url</summary>
    public string GetQuestionUrl => IP + "/api/currency/record/topic";
    /// <summary>接收答题报表的Url</summary>
    public string ReceiveAnswerFormUrl => IP + "/api/currency/record/recTopic";
    /// <summary>接收实验信息的Url</summary>
    public string ReceiveTestInfoUrl => IP + "/api/currency/record/receive";

    /// <summary>
    /// 获取题库
    /// </summary>
    /// <returns></returns>
    public void GetQuestionBank(Action succeed, Action fail)
    {
        StartCoroutine(GetQuestion(succeed, fail));
    }

    [ContextMenu("获取题库")]
    private void GetQuestionBank()
    {
        StartCoroutine(GetQuestion());
    }

    private IEnumerator GetQuestion(Action succeed = null, Action fail = null)
    {
        WWWForm form = new WWWForm();
        form.AddField("id", APPID);
        form.AddField("des", CreateDes());
        using (UnityWebRequest webRequest = UnityWebRequest.Post(GetQuestionUrl, form))
        {
            webRequest.timeout = 10;
            yield return webRequest.SendWebRequest();
            string response = null;
            if (webRequest.isNetworkError || webRequest.isHttpError)
            {
                response = webRequest.error;
                Debug.LogError("获取题库请求失败 : " + response);
                fail?.Invoke();
            }
            else
            {
                response = webRequest.downloadHandler.text;
                Debug.Log("获取题库回传请求成功收到回包 : " + response);
                //解析题库
                if (OptionManager.Instance.SetQuestionBank(response))
                {
                    Debug.Log("获取题库解析成功");
                    succeed?.Invoke();
                }
                else
                {
                    Debug.Log("获取题库解析失败");
                    fail?.Invoke();
                }
            }
            webRequest.Dispose();
        }
    }

    /// <summary>
    /// 提交答题数据
    /// </summary>
    /// <param name="report">答题数据和之前的答题数据一样</param>OptionListClass类转json
    /// <param name="succeed">成功回调</param>
    /// <param name="fail">失败回调</param>
    public void SubmitProblem(string report, Action succeed, Action fail)
    {
        StartCoroutine(UnityWebRequestPostProblem(report, succeed, fail));
    }

    IEnumerator UnityWebRequestPostProblem(string report, Action succeed, Action fail)
    {
        WWWForm empiricalDataForm = new WWWForm();
        empiricalDataForm.AddField("id", APPID);
        empiricalDataForm.AddField("report", report);
        empiricalDataForm.AddField("des", CreateDes());
        Debug.Log("----提交答题数据数据----");
        if (!string.IsNullOrEmpty(report))
            Debug.Log("Json:" + report);
        using (UnityWebRequest webRequest = UnityWebRequest.Post(ReceiveAnswerFormUrl, empiricalDataForm))
        {
            yield return webRequest.SendWebRequest();
            string response = null;
            if (webRequest.isNetworkError || webRequest.isHttpError)
            {
                response = webRequest.error;
                Debug.LogError("答题数据回传请求失败 : " + response);
                fail?.Invoke();
            }
            else
            {
                response = webRequest.downloadHandler.text;
                Debug.Log("答题数据回传请求成功收到回包 : " + response);
                succeed?.Invoke();
            }
            webRequest.Dispose();
        }
    }
    /// <summary>
    /// 提交实验数据
    /// </summary>
    /// <param name="report">和Ilab网站的数据一样的</param>IlabData类转json
    /// <param name="token">Acctoken</param>
    /// <param name="succeed">成功回调</param>
    /// <param name="fail">失败回调</param>
    public void SubmitData(string report, string token, Action succeed, Action fail)
    {
        StartCoroutine(UnityWebRequestPostExperiment(report, token, succeed, fail));
    }

    IEnumerator UnityWebRequestPostExperiment(string report, string token, Action succeed, Action fail)
    {
        WWWForm empiricalDataForm = new WWWForm();
        empiricalDataForm.AddField("report", report);
        empiricalDataForm.AddField("des", CreateDes());
        empiricalDataForm.AddField("access_token", token);
        Debug.Log("----提交后台实验数据----");
        Debug.Log("Json:" + report);
        Debug.Log("access_token:" + token);
        using (UnityWebRequest webRequest = UnityWebRequest.Post(ReceiveTestInfoUrl, empiricalDataForm))
        {
            yield return webRequest.SendWebRequest();
            string response = null;
            if (webRequest.isNetworkError || webRequest.isHttpError)
            {
                response = webRequest.error;
                Debug.LogError("实验数据回传请求失败 : " + response);
                fail?.Invoke();
            }
            else
            {
                response = webRequest.downloadHandler.text;
                Debug.Log("实验数据回传请求成功收到回包 : " + response);
                succeed?.Invoke();
            }
            webRequest.Dispose();
        }
    }

    [ContextMenu("DES加密")]
    public static string CreateDes()
    {
        string key;
        var startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
        var timestamp = ((long)(DateTime.Now - startTime).TotalMilliseconds);
        key = "华畅股份" + timestamp;
        string des = toHex(Encrypt(key, "huaChang"));
        Debug.Log(des);
        return des;
    }

    /// <summary>
    /// 转十六进制
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static string toHex(byte[] data)
    {
        StringBuilder sb = new StringBuilder("");
        foreach (byte b in data)
        {
            sb.AppendFormat("{0:x2}", b);
        }
        return sb.ToString();
    }

    //加密
    public static byte[] Encrypt(string pToEncrypt, string sKey)
    {
        using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
        {

            byte[] inputByteArray = Encoding.UTF8.GetBytes(pToEncrypt);
            des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
            des.Mode = CipherMode.CBC;
            des.Padding = PaddingMode.PKCS7;
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
            {
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                cs.Close();
            }
            return ms.ToArray();
        }
    }

    //解密
    public static string Decrypt(string pToDecrypt, string sKey)
    {
        using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
        {
            byte[] inputByteArray = Convert.FromBase64String(pToDecrypt);
            des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
            des.Mode = CipherMode.CBC;
            des.Padding = PaddingMode.PKCS7;
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write))
            {
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                cs.Close();
            }
            string str = Encoding.UTF8.GetString(ms.ToArray());
            ms.Close();
            Debug.Log(pToDecrypt + "--通过--" + sKey + "--解密为--" + str);
            return str;
        }
    }
}
