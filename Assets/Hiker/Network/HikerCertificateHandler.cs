using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Security.Cryptography.X509Certificates;

class HikerCertificateHandler : CertificateHandler
{
    // Encoded RSAPublicKey
    private static string PUB_KEY = "xx";

    protected override bool ValidateCertificate(byte[] certificateData)
    {
        X509Certificate2 certificate = new X509Certificate2(certificateData);
        string pk = certificate.GetPublicKeyString();
        
        //if (pk.Equals(PUB_KEY))
            return true;

        // Bad dog
        Debug.Log("Bad dog");
        Debug.Log(pk);
        return false;
        //return true;
    }
}
