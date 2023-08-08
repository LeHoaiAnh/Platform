using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if BEST_HTTP
using BestHTTP;
#endif
using UnityEngine.Networking;

public class HTTPClient : MonoBehaviour
{
    protected string URL = "https://localhost:44348/game/";
    private static readonly bool ENCRYPT_DATA = true;

    public delegate void OnSendRequestSuccess();

    public const int REQUEST_TIME_OUT = 10;

    //private System.Action RetryAction { get; set; }

    public delegate void OnReceiveResponse(string responseData);
    public delegate void OnErrorResponse(string responseErr, bool isNetworkError);

    public bool IsLastConnectTimeOut { get; set; } = false;

    void OnRequestFinish(bool isErr, string errMsg, string dataRes,
        OnReceiveResponse onResponse, OnErrorResponse onFail, bool ignoreError)
    {
        if (isErr == false)
        {
            string responseStr = null;
            string iv = null;
            try
            {
                var rescontent = dataRes;
                if (string.IsNullOrEmpty(rescontent) == false)
                {
                    var res = LitJson.JsonMapper.ToObject<Dictionary<string, string>>(rescontent);
                    if (res.TryGetValue("er", out string er) && string.IsNullOrEmpty(er) == false)
                    {
                        onFail?.Invoke(er, false);
                        return;
                    }
                    else
                    {
                        if (res.TryGetValue("data", out string data))
                        {
                            responseStr = data;
                        }
                        if (res.TryGetValue("iv", out iv))
                        {

                        }
                    }
                }
            }
            catch (System.NotSupportedException)
            {
                responseStr = Localization.Get("NetworkError");
            }
            catch (System.Exception e)
            {
                onFail?.Invoke(e.ToString(), false);
                return;
            }

            if (string.IsNullOrEmpty(responseStr))
            {
                onResponse?.Invoke(string.Empty);
            }
            else if (ReadText.IsBase64String(responseStr) == false)
            {
#if DEBUG
                Debug.LogError("[ERROR] Not base64 response str: " + responseStr);
#endif
                if (ignoreError == false)
                {
                    Hiker.GUI.PopupMessage.Create(Hiker.GUI.MessagePopupType.ERROR, Localization.Get("NetworkError"));
                }
                onFail?.Invoke(Localization.Get("NetworkError"), false);
            }
            else
            {
                string decompressData = string.Empty;
                try
                {
                    decompressData = ReadText.DecompressString(responseStr, iv, ENCRYPT_DATA);
                }
                catch (System.Exception e)
                {
#if DEBUG
                    Debug.LogError("Response: " + responseStr + "Exception: " + e.Message);
#endif
                    onFail?.Invoke(e.Message, false);
                    return;
                }
                onResponse?.Invoke(decompressData);
            }
        }
        else
        {
            onFail?.Invoke(errMsg, false);
        }
    }

    void OnRequestNetworkError(
        System.Action onRetry,
        bool ignoreError,
        bool acceptOffline)
    {
        if (Hiker.GUI.PopupNetworkLoading.instance)
        {
            Hiker.GUI.PopupNetworkLoading.instance.gameObject.SetActive(false);
        }
        else if (Hiker.GUI.PopupDataSync.instance != null)
        {
            Hiker.GUI.PopupDataSync.instance.gameObject.SetActive(false);
        }

        if (Application.isPlaying)
        {
            if (ignoreError == false)
            {
                Hiker.GUI.PopupConfirm.Create("There is a problem about your connection. Please check it and try again.", () =>
                {
                    if (Hiker.GUI.PopupNetworkLoading.instance)
                    {
                        Hiker.GUI.PopupNetworkLoading.instance.gameObject.SetActive(true);
                    }
                    else if (Hiker.GUI.PopupDataSync.instance != null)
                    {
                        Hiker.GUI.PopupDataSync.instance.gameObject.SetActive(true);
                    }
                    onRetry?.Invoke();
                },
                true,
                "Retry",
                "Information");
            }
        }

    }

    void OnRequestHttpError(
        string error,
        System.Action onRetry,
        bool ignoreError)
    {
        if (Hiker.GUI.PopupNetworkLoading.instance)
        {
            Hiker.GUI.PopupNetworkLoading.instance.gameObject.SetActive(false);
        }

        if (Application.isPlaying)
        {
            if (ignoreError)
            {

            }
            else
            {
                Hiker.GUI.PopupConfirm.Create(error, () =>
                {
                    if (Hiker.GUI.PopupNetworkLoading.instance)
                    {
                        Hiker.GUI.PopupNetworkLoading.instance.gameObject.SetActive(true);
                    }
                    onRetry?.Invoke();
                },
                true,
                Localization.Get("RetryBtn"));
            }
        }
    }

    //    void SendRequestPri(string reqUrl,
    //        string func, string data,
    //        OnReceiveResponse onResponse,
    //        OnErrorResponse onFail,
    //        bool ignoreError,
    //        bool showLoading,
    //        int request_time_out)
    //    {
    //#if !BEST_HTTP
    //        if (request_time_out <= 0 || request_time_out > 3 * REQUEST_TIME_OUT) request_time_out = REQUEST_TIME_OUT;

    //        var req = GetHttpRequest(reqUrl, func, data, onResponse, onFail, ignoreError, showLoading, request_time_out);
    //        req.Timeout = new System.TimeSpan(0, 0, request_time_out);
    //        req.Send();

    //        //StartCoroutine(CoSendWebRequest(request_url, form, onResponse, onFail, ignoreError, request_time_out));
    //#else

    //#endif
    //    }
    public event System.Action onConnectionTimeOut;
    public event System.Action onConnectionRecon;

    void OnConnectionTimeOut(bool isTimeOut)
    {
        if (IsLastConnectTimeOut == false && isTimeOut)
        {
            onConnectionTimeOut?.Invoke();
        }
        if (IsLastConnectTimeOut && isTimeOut == false)
        {
            onConnectionRecon?.Invoke();
        }

        IsLastConnectTimeOut = isTimeOut;
    }

    public void RequestCheckConnection(System.Action onFinish)
    {
        // use hint dont use keep-alive connection on http/1
        // because of error connection when request GetUserInfo
        // immediately after application become foreground on pc version
        var uri = new System.Uri(URL);
        HTTPRequest request = new HTTPRequest(new System.Uri(uri.GetLeftPart(System.UriPartial.Authority)), HTTPMethods.Head, false,
            (req, res) =>
            {
                switch (req.State)
                {
                    // The request finished without any problems.
                    case HTTPRequestStates.Finished:
                        OnConnectionTimeOut(false);
                        break;
                    // The request finished with an unexpected error.
                    // The request's Exception property may contain more information about the error.
                    case HTTPRequestStates.Error:
                        bool isNetworkErr = false;
                        if (req.Exception != null && req.Exception is System.Net.Sockets.SocketException)
                        {
                            var socketException = req.Exception as System.Net.Sockets.SocketException;
                            if (socketException.SocketErrorCode == System.Net.Sockets.SocketError.ConnectionRefused)
                            {
                                isNetworkErr = true;
                            }
                            Debug.Log("HttpRequestError SocketErrorCode = " + socketException.SocketErrorCode);
                        }
                        if (isNetworkErr)
                        {
                            OnConnectionTimeOut(true);
                        }
                        break;

                    // The request aborted, initiated by the user.
                    case HTTPRequestStates.Aborted:
                        break;

                    // Connecting to the server timed out.
                    case HTTPRequestStates.ConnectionTimedOut:
                        OnConnectionTimeOut(true);
                        break;

                    // The request didn't finished in the given time.
                    case HTTPRequestStates.TimedOut:
                        break;
                }
                onFinish?.Invoke();
            }
           );

        request.Timeout = new System.TimeSpan(0, 0, 0, 10);
        request.Send();
    }

    //public struct HikerRequestCache
    //{
    //    public string url { get; set; }
    //    public string data { get; set; }
    //};

    //protected Queue<HikerRequestCache> pOfflineQueue = new Queue<HikerRequestCache>();

    protected virtual void OnApplicationPause(bool isPause)
    {
        if (isPause)
        {
            //            var queueReq = LitJson.JsonMapper.ToJson(pOfflineQueue);
            //#if ANTICHEAT
            //            CodeStage.AntiCheat.Storage.ObscuredPrefs.SetString("OfflineQueue", queueReq);
            //            CodeStage.AntiCheat.Storage.ObscuredPrefs.Save();
            //#else
            //            PlayerPrefs.SetString("OfflineQueue", queueReq);
            //            PlayerPrefs.Save();
            //#endif
        }
        else
        {
            RequestCheckConnection(null);

            //#if ANTICHEAT
            //            var queueReq = CodeStage.AntiCheat.Storage.ObscuredPrefs.GetString("OfflineQueue", string.Empty);
            //#else
            //            var queueReq = PlayerPrefs.GetString("OfflineQueue", string.Empty);
            //#endif

            //            if (string.IsNullOrWhiteSpace(queueReq) == false)
            //            {
            //                var offLineQueue = LitJson.JsonMapper.ToObject<Queue<HikerRequestCache>>(queueReq);
            //                while (offLineQueue.Count > 0)
            //                {
            //                    pOfflineQueue.Enqueue(offLineQueue.Dequeue());
            //                }
            //            }
        }
    }

#if BEST_HTTP
    HTTPRequest GetHttpRequest(string request_url,
        string data,
        OnReceiveResponse onResponse,
        OnErrorResponse onFail,
        bool ignoreError,
        bool showLoading,
        int request_time_out,
        bool accept_offline)
    {
        // use hint dont use keep-alive connection on http/1
        // because of error connection when request GetUserInfo
        // immediately after application become foreground on pc version
        HTTPRequest request = new HTTPRequest(new System.Uri(request_url), HTTPMethods.Post, false,
            (req, res) =>
            {
                switch (req.State)
                {
                    // The request finished without any problems.
                    case HTTPRequestStates.Finished:
                        if (showLoading)
                        {
                            Hiker.GUI.PopupNetworkLoading.Dismiss();
                        }
                        OnRequestFinish(false, string.Empty, res.DataAsText, onResponse, onFail, ignoreError);
                        OnConnectionTimeOut(false);
                        break;
                    // The request finished with an unexpected error.
                    // The request's Exception property may contain more information about the error.
                    case HTTPRequestStates.Error:
                        if (accept_offline)
                        {
                            if (showLoading)
                            {
                                Hiker.GUI.PopupNetworkLoading.Dismiss();
                            }
                        }
                        else
                        {
                            OnRequestNetworkError(() => req.Send(), ignoreError, accept_offline);
                        }
                        bool isNetworkErr = false;
                        if (req.Exception != null && req.Exception is System.Net.Sockets.SocketException)
                        {
                            var socketException = req.Exception as System.Net.Sockets.SocketException;
                            if (socketException.SocketErrorCode != System.Net.Sockets.SocketError.Success)
                            {
                                isNetworkErr = true;
                            }
                            Debug.Log("HttpRequestError SocketErrorCode = " + socketException.SocketErrorCode);
                        }
                        if (isNetworkErr)
                        {
                            OnConnectionTimeOut(true);
                        }
                        onFail?.Invoke(req.Exception != null ? req.Exception.Message : string.Empty, isNetworkErr);
                        break;

                    // The request aborted, initiated by the user.
                    case HTTPRequestStates.Aborted:
                        if (showLoading)
                        {
                            Hiker.GUI.PopupNetworkLoading.Dismiss();
                        }
#if DEBUG
                        Debug.LogWarning("Request Aborted!");
#endif
                        onFail?.Invoke("Request Aborted!", false);
                        break;

                    // Connecting to the server timed out.
                    case HTTPRequestStates.ConnectionTimedOut:
                        OnConnectionTimeOut(true);
                        if (accept_offline)
                        {
                            if (Hiker.GUI.PopupNetworkLoading.instance != null &&
                                Hiker.GUI.PopupNetworkLoading.instance.gameObject.activeInHierarchy)
                            {
                                Hiker.GUI.PopupNetworkLoading.Dismiss();
                            }
                        }
                        else
                        {
#if DEBUG
                            Debug.LogError("Connection Timed Out!");
#endif
                            OnRequestNetworkError(() => {
                                req.Timeout = new System.TimeSpan(0, 0, Mathf.Min(request_time_out + REQUEST_TIME_OUT, 30));
                                req.Send();
                            }, ignoreError, accept_offline);
                        }
                        onFail?.Invoke(string.Empty, true);
                        break;

                    // The request didn't finished in the given time.
                    case HTTPRequestStates.TimedOut:
#if DEBUG
                        Debug.LogError("Processing the request Timed Out!");
#endif
                        OnRequestNetworkError(() =>
                        {
                            req.Timeout = new System.TimeSpan(0, 0, Mathf.Min(request_time_out + REQUEST_TIME_OUT, 30));
                            req.Send();
                        }, ignoreError, false);
                        
                        /*OnConnectionTimeOut(true);
                        if (accept_offline)
                        {
                            if (Hiker.GUI.PopupNetworkLoading.instance != null &&
                                Hiker.GUI.PopupNetworkLoading.instance.gameObject.activeInHierarchy)
                            {
                                Hiker.GUI.PopupNetworkLoading.Dismiss();
                            }
                        }
                        else
                        {
#if DEBUG
                            Debug.LogError("Connection Timed Out!");
#endif
                            OnRequestNetworkError(() => {
                                req.Timeout = new System.TimeSpan(0, 0, Mathf.Min(request_time_out + REQUEST_TIME_OUT, 30));
                                req.Send();
                            }, ignoreError, accept_offline);
                        }
                        onFail?.Invoke(string.Empty, true); */
                        onFail?.Invoke(string.Empty, false);
                        break;
                }
            }
           );
        request.FormUsage = BestHTTP.Forms.HTTPFormUsage.UrlEncoded;

        request.Timeout = new System.TimeSpan(0, 0, 0, request_time_out);
        return request;
    }
#else
    UnityWebRequest GetUnityRequest(string request_url, WWWForm form, int request_time_out)
    {
        UnityWebRequest webRequest = UnityWebRequest.Post(request_url, form);
        webRequest.timeout = request_time_out;
        if (request_url.StartsWith("https://"))
        {
            webRequest.certificateHandler = new HikerCertificateHandler();
            webRequest.disposeCertificateHandlerOnDispose = true;
        }
        //else
        //{
        //    webRequest.certificateHandler = null;
        //    webRequest.disposeCertificateHandlerOnDispose = false;
        //}
        return webRequest;
    }


    bool isNetworkError(UnityWebRequest webRequest)
    {
#if UNITY_2019_4_OR_NEWER
        return webRequest.result == UnityWebRequest.Result.ConnectionError ||
            webRequest.result == UnityWebRequest.Result.DataProcessingError ||
            webRequest.result == UnityWebRequest.Result.ProtocolError;
#else
        return webRequest.isNetworkError;
#endif
    }

    bool isHttpError(UnityWebRequest webRequest)
    {
#if UNITY_2019_4_OR_NEWER
        return webRequest.result == UnityWebRequest.Result.Success && webRequest.responseCode != 200;
#else
        return webRequest.isHttpError;
#endif
    }

    IEnumerator CoSendWebRequest(string request_url,
        WWWForm form,
        OnReceiveResponse onResponse,
        OnErrorResponse onFail,
        bool ignoreError,
        int request_time_out)
    {
        UnityWebRequest webRequest = GetUnityRequest(request_url, form, request_time_out);
        var async = webRequest.SendWebRequest();
        yield return async;
        if (isNetworkError(webRequest) && ignoreError == false)
        {
            OnRequestNetworkError(() =>
            {
                SendRequestPri(request_url, form, onResponse, onFail, ignoreError, request_time_out + REQUEST_TIME_OUT);
            });
        }
        else if (isHttpError(webRequest) && ignoreError == false)
        {
            OnRequestHttpError(webRequest.error, () =>
            {
                SendRequestPri(request_url, form, onResponse, onFail, ignoreError, request_time_out + REQUEST_TIME_OUT);
            });
        }
        else
        {
            OnRequestFinish(webRequest, onResponse, onFail, ignoreError);
        }
        webRequest.Dispose();
    }

    void SendRequestPri(string request_url,
        WWWForm form,
        OnReceiveResponse onResponse,
        OnErrorResponse onFail,
        bool ignoreError,
        int request_time_out = 0)
    {
        if (request_time_out <= 0 || request_time_out > 3 * REQUEST_TIME_OUT) request_time_out = REQUEST_TIME_OUT;
        StartCoroutine(CoSendWebRequest(request_url, form, onResponse, onFail, ignoreError, request_time_out));
    }

    void OnRequestFinish(UnityWebRequest webRequest,
        OnReceiveResponse onResponse, OnErrorResponse onFail, bool ignoreError)
    {
        Hiker.GUI.PopupNetworkLoading.Dismiss();
        bool isOk = PostProcessResponse(webRequest, ignoreError);

        if (isOk)
        {
            string responseStr = null;
            string iv = null;
            try
            {
                var rescontent = webRequest.downloadHandler.text;
                if (string.IsNullOrEmpty(rescontent) == false)
                {
                    var res = LitJson.JsonMapper.ToObject<Dictionary<string, string>>(rescontent);
                    if (res.TryGetValue("er", out string er) && string.IsNullOrEmpty(er) == false)
                    {
                        onFail?.Invoke(er);
                        return;
                    }
                    else
                    {
                        if (res.TryGetValue("data", out string data))
                        {
                            responseStr = data;
                        }
                        if (res.TryGetValue("iv", out iv))
                        {

                        }
                    }
                }

            }
            catch (System.NotSupportedException)
            {
                responseStr = Localization.Get("NetworkError");
            }
            catch (System.Exception e)
            {
                onFail?.Invoke(e.ToString());
                return;
            }

            if (string.IsNullOrEmpty(responseStr))
            {
                onResponse?.Invoke(string.Empty);
            }
            else if (ReadText.IsBase64String(responseStr) == false)
            {
#if DEBUG
                Debug.LogError("[ERROR] Not base64 response str: " + responseStr);
#endif
                if (ignoreError == false)
                {
                    Hiker.GUI.PopupMessage.Create(Hiker.GUI.MessagePopupType.ERROR, Localization.Get("NetworkError"));
                }
                onFail?.Invoke(Localization.Get("NetworkError"));
            }
            else
            {
                string decompressData = string.Empty;
                try
                {
                    decompressData = ReadText.DecompressString(responseStr, iv, ENCRYPT_DATA);
                }
                catch (System.Exception e)
                {
#if DEBUG
                    Debug.LogError("Response: " + responseStr + "Exception: " + e.Message);
#endif
                    onFail?.Invoke(e.Message);
                    return;
                }
                onResponse?.Invoke(decompressData);
            }
        }
        else
        {
            onFail?.Invoke(webRequest != null ? webRequest.error : string.Empty);
        }
    }

    bool PostProcessResponse(UnityWebRequest request, bool ignoreError)
    {
#if UNITY_EDITOR
        if (request == null)
        {
            Debug.Log("request null");
            return false;
        }
#endif
        //var requestState = request.State;
        //if (response == null)
        //{
        //    Debug.Log("respon null");
        //    Debug.Log(request.State);
        //    requestState = HTTPRequestStates.ConnectionTimedOut;
        //}

        //Debug.Log("PostProcessRespons : " + request.State);
        if (request.result == UnityWebRequest.Result.ConnectionError) // connection err
        {
            Debug.Log(request.error);
            if (ignoreError == false)
            {
                Hiker.GUI.PopupMessage.Create(Hiker.GUI.MessagePopupType.ERROR, request.error);
            }

            return false;
        }
        else if (request.result == UnityWebRequest.Result.ProtocolError)
        {
            switch (request.responseCode)
            {
                default:
                    break;
            }
            Debug.Log(request.error);
            if (ignoreError == false)
            {
                Hiker.GUI.PopupMessage.Create(Hiker.GUI.MessagePopupType.ERROR, request.error);
            }

            return false;
        }
        else
        {
            return true;
        }
    }
#endif


    public void SendRequest(string funcName,
        string data,
        OnReceiveResponse onResponse = null,
        bool showLoading = true,
        bool ignoreError = false,
        OnErrorResponse onFail = null,
        byte[] binaryData = null,
        int customTimeOut = 0,
        bool accept_offline = false)
    {
#if DEBUG
        Debug.Log("SendRequest " + funcName);
#endif

#if VIDEO_BUILD
        showLoading = false;
#endif
        //this.RetryAction = () => this.SendRequest(funcName, data, onResponse, showLoading, ignoreError, onFail/*, delay*/);

        //bool isInSplash = (SplashScreen.Instance && SplashScreen.Instance.gameObject.activeSelf) &&
        //                 !(PopupDisplayName.Instance && PopupDisplayName.Instance.gameObject.activeSelf);
        //isInSplash = isInSplash || PopupCloudEffect.instance != null;

        if (showLoading)
        {
            if (Hiker.GUI.PopupNetworkLoading.instance != null &&
                Hiker.GUI.PopupNetworkLoading.instance.gameObject.activeSelf)
            {
                //Debug.Log("requesting...");
            }
            else
            {
                Hiker.GUI.PopupNetworkLoading.Create("Network Loading");
            }
        }

        //if (funcName != "RequestUpdateDisplayName" && TutorialManager.IsShowing)
        //{
        //    showLoading = true;
        //    onFail = null;
        //}

        string request_url = this.URL + "request";
        string iv = ENCRYPT_DATA ? HikerAes.GenerateIV() : string.Empty;

        string compressed_data = ReadText.CompressString(data, iv, ENCRYPT_DATA);

        int timeOut = customTimeOut > 0 ? customTimeOut : REQUEST_TIME_OUT;
#if UNITY_EDITOR // cap timeOut minimum 30s cho server local
        //if (GameClient.instance.targetServer == GameClient.TargetServer.LOCAL)
        {
            if (timeOut < 30)
            {
                timeOut = 30;
            }
        }
#endif

#if BEST_HTTP
        var request = GetHttpRequest(request_url, data, onResponse, onFail, ignoreError, showLoading, timeOut, accept_offline);
        request.AddField(funcName, compressed_data);
        if (ENCRYPT_DATA)
        {
            request.AddField("iv", iv);
        }

        request.Send();
#else
        WWWForm form = new WWWForm();
        form.AddField(funcName, compressed_data);
        if (ENCRYPT_DATA)
        {
            form.AddField("iv", iv);
        }

        SendRequestPri(request_url, form, onResponse, onFail, ignoreError);
#endif

    }

    /*public HTTPRequest SendRequestOffline(string funcName,
        string data,
        OnReceiveResponse onResponse = null,
        bool showLoading = true,
        bool ignoreError = false,
        OnErrorResponse onFail = null,
        byte[] binaryData = null,
        int customTimeOut = 0)
    {
#if DEBUG
        Debug.Log("SendRequest " + funcName);
#endif

#if VIDEO_BUILD
        showLoading = false;
#endif
        //this.RetryAction = () => this.SendRequest(funcName, data, onResponse, showLoading, ignoreError, onFail/*, delay#1#);

        //bool isInSplash = (SplashScreen.Instance && SplashScreen.Instance.gameObject.activeSelf) &&
        //                 !(PopupDisplayName.Instance && PopupDisplayName.Instance.gameObject.activeSelf);
        //isInSplash = isInSplash || PopupCloudEffect.instance != null;

        if (showLoading)
        {
            if (Hiker.GUI.PopupNetworkLoading.instance != null &&
                Hiker.GUI.PopupNetworkLoading.instance.gameObject.activeSelf)
            {
                //Debug.Log("requesting...");
            }
            else
            {
                Hiker.GUI.PopupNetworkLoading.Create(Localization.Get("NetworkLoading"));
            }
        }

        //if (funcName != "RequestUpdateDisplayName" && TutorialManager.IsShowing)
        //{
        //    showLoading = true;
        //    onFail = null;
        //}

        string request_url = this.URL + "request";

        string iv = ENCRYPT_DATA ? HikerAes.GenerateIV() : string.Empty;

        string compressed_data = ReadText.CompressString(data, iv, ENCRYPT_DATA);

        int timeOut = customTimeOut > 0 ? customTimeOut : REQUEST_TIME_OUT;
#if BEST_HTTP
        var request = GetHttpRequest(request_url, data, onResponse, onFail, ignoreError, showLoading, timeOut, true);
        request.AddField(funcName, compressed_data);
        if (ENCRYPT_DATA)
        {
            request.AddField("iv", iv);
        }

        /*
        request.Send();
#1#
#else
        WWWForm form = new WWWForm();
        form.AddField(funcName, compressed_data);
        if (ENCRYPT_DATA)
        {
            form.AddField("iv", iv);
        }

        SendRequestPri(request_url, form, onResponse, onFail, ignoreError);
#endif
        return request;
    }*/

    //public void RestartWithURL(string url)
    //{
    //    this.URL = url;
    //    PlayerPrefs.SetString("SERVER_URL", this.URL);
    //    //clear cached
    //    PlayerPrefs.DeleteKey("user_" + GameClient.instance.user);
    //    GUIManager.Instance.RestartGame();
    //}
}
