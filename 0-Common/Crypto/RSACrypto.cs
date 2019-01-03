using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Crypto
{
    /// <summary>
    /// RSA算法
    /// version 1.0.0
    /// </summary>
    public class RSACrypto
    {
        /// <summary>
        /// RSA产生密钥
        /// </summary>
        /// <param name="PrivateKey">私钥</param>
        /// <param name="PublicKey">公钥</param>
        public void CreateKeys(out string privateKey, out string publicKey)
        {
            try
            {
                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                privateKey = rsa.ToXmlString(true);
                publicKey = rsa.ToXmlString(false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string Encrypt(string publicKey, string data)
        {
            try
            {
                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                rsa.FromXmlString(publicKey);
                var _data = rsa.Encrypt(Encoding.UTF8.GetBytes(data), false);
                return Convert.ToBase64String(_data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string Encrypt(string data)
        {
            var encryptBytes = new RSACryptoServiceProvider(new CspParameters()).Encrypt(Encoding.UTF8.GetBytes(data), false);
            return Convert.ToBase64String(encryptBytes);
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="privateKey"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public string Decrypt(string privateKey, string data)
        {
            try
            {
                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                rsa.FromXmlString(privateKey);
                var _data = rsa.Decrypt(Convert.FromBase64String(data.Replace(" ", "+")), false);
                return Encoding.UTF8.GetString(_data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
