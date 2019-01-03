using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Crypto
{
    public class RSAConvert
    {
        /// <summary>    
        /// RSA私钥格式转换，pem->xml 
        /// </summary>    
        /// <param name="pemPrivateKey">pem格式的RSA私钥</param>    
        /// <returns>xml格式的RSA私钥</returns>   
        public static string XmlPrivateKeyByPem(string pemPrivateKey)
        {
            byte[] bytes = Convert.FromBase64String(pemPrivateKey);
            var privateKeyParam = PrivateKeyFactory.CreateKey(bytes) as RsaPrivateCrtKeyParameters;
            string modulus = Convert.ToBase64String(privateKeyParam.Modulus.ToByteArrayUnsigned());
            string exponent = Convert.ToBase64String(privateKeyParam.PublicExponent.ToByteArrayUnsigned());
            string p = Convert.ToBase64String(privateKeyParam.P.ToByteArrayUnsigned());
            string q = Convert.ToBase64String(privateKeyParam.Q.ToByteArrayUnsigned());
            string dp = Convert.ToBase64String(privateKeyParam.DP.ToByteArrayUnsigned());
            string dq = Convert.ToBase64String(privateKeyParam.DQ.ToByteArrayUnsigned());
            string qinv = Convert.ToBase64String(privateKeyParam.QInv.ToByteArrayUnsigned());
            string d = Convert.ToBase64String(privateKeyParam.Exponent.ToByteArrayUnsigned());
            string pemKey = string.Format(@"<RSAKeyValue>
                                               <Modulus>{0}</Modulus>
                                               <Exponent>{1}</Exponent>
                                               <P>{2}</P>
                                               <Q>{3}</Q>
                                               <DP>{4}</DP>
                                               <DQ>{5}</DQ>
                                               <InverseQ>{6}</InverseQ>
                                               <D>{7}</D>
                                            </RSAKeyValue>", modulus, exponent, p, q, dp, dq, qinv, d);
            return pemKey;
        }

        /// <summary>    
        /// RSA私钥格式转换，xml->pem    
        /// </summary>    
        /// <param name="xmlPrivateKey">xml格式的RSA私钥</param>    
        /// <returns>pem格式的RSA私钥</returns>   
        public string PemPrivateKeyByXml(string xmlPrivateKey)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlPrivateKey);
            BigInteger m = new BigInteger(1, Convert.FromBase64String(doc.DocumentElement.GetElementsByTagName("Modulus")[0].InnerText));
            BigInteger exp = new BigInteger(1, Convert.FromBase64String(doc.DocumentElement.GetElementsByTagName("Exponent")[0].InnerText));
            BigInteger d = new BigInteger(1, Convert.FromBase64String(doc.DocumentElement.GetElementsByTagName("D")[0].InnerText));
            BigInteger p = new BigInteger(1, Convert.FromBase64String(doc.DocumentElement.GetElementsByTagName("P")[0].InnerText));
            BigInteger q = new BigInteger(1, Convert.FromBase64String(doc.DocumentElement.GetElementsByTagName("Q")[0].InnerText));
            BigInteger dp = new BigInteger(1, Convert.FromBase64String(doc.DocumentElement.GetElementsByTagName("DP")[0].InnerText));
            BigInteger dq = new BigInteger(1, Convert.FromBase64String(doc.DocumentElement.GetElementsByTagName("DQ")[0].InnerText));
            BigInteger qinv = new BigInteger(1, Convert.FromBase64String(doc.DocumentElement.GetElementsByTagName("InverseQ")[0].InnerText));
            RsaPrivateCrtKeyParameters privateKeyParam = new RsaPrivateCrtKeyParameters(m, exp, d, p, q, dp, dq, qinv);
            PrivateKeyInfo privateKeyInfo = PrivateKeyInfoFactory.CreatePrivateKeyInfo(privateKeyParam);
            byte[] serializedPrivateBytes = privateKeyInfo.ToAsn1Object().GetEncoded();
            return Convert.ToBase64String(serializedPrivateBytes);
        }

        /// <summary>    
        /// RSA公钥格式转换，pem->xml  
        /// </summary>    
        /// <param name="pemPublicKey">pem格式的RSA公钥</param>    
        /// <returns>xml格式的RSA公钥</returns>    
        public static string XMLPublicKeyByPem(string pemPublicKey)
        {
            RsaKeyParameters publicKeyParam = (RsaKeyParameters)PublicKeyFactory.CreateKey(Convert.FromBase64String(pemPublicKey));
            return string.Format("<RSAKeyValue><Modulus>{0}</Modulus><Exponent>{1}</Exponent></RSAKeyValue>",
               Convert.ToBase64String(publicKeyParam.Modulus.ToByteArrayUnsigned()),
               Convert.ToBase64String(publicKeyParam.Exponent.ToByteArrayUnsigned()));
        }
        /// <summary>    
        /// RSA公钥格式转换，xml->pem  
        /// </summary>    
        /// <param name="xmlPublicKey">xml格式的RSA公钥</param>    
        /// <returns>pem格式的RSA公钥</returns>   
        public static string PemPublicKeyByXml(string xmlPublicKey)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlPublicKey);
            BigInteger m = new BigInteger(1, Convert.FromBase64String(doc.DocumentElement.GetElementsByTagName("Modulus")[0].InnerText));
            BigInteger p = new BigInteger(1, Convert.FromBase64String(doc.DocumentElement.GetElementsByTagName("Exponent")[0].InnerText));
            RsaKeyParameters pub = new RsaKeyParameters(false, m, p);
            SubjectPublicKeyInfo publicKeyInfo = SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(pub);
            byte[] serializedPublicBytes = publicKeyInfo.ToAsn1Object().GetDerEncoded();
            return Convert.ToBase64String(serializedPublicBytes);
        }
    }
}
