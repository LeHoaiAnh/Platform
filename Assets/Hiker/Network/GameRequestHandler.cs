using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameRequestHandler : DownloadHandlerScript
{
    protected override byte[] GetData()
    {
        return base.GetData();
    }

    protected override void CompleteContent()
    {
        base.CompleteContent();
    }

    protected override float GetProgress()
    {
        return base.GetProgress();
    }

    protected override string GetText()
    {
        return base.GetText();
    }

#if UNITY_2019_4_OR_NEWER
    protected override void ReceiveContentLengthHeader(ulong contentLength)
    {
        base.ReceiveContentLengthHeader(contentLength);
    }
#else
    protected override void ReceiveContentLength(int contentLength)
    {
        base.ReceiveContentLength(contentLength);
    }
#endif

    protected override bool ReceiveData(byte[] data, int dataLength)
    {
        return base.ReceiveData(data, dataLength);
    }
}
