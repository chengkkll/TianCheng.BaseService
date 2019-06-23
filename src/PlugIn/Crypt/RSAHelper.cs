using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace TianCheng.BaseService.PlugIn.Crypt
{
    /// <summary>
    /// RSA操作的常用方法
    /// </summary>
    public class RSAHelper
    {
        /// <summary>
        /// 生成RSA秘钥对
        /// </summary>
        public static RSAKey GenerateRSAKey()
        {
            //创建RSA对象
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(1024);
            return new RSAKey(rsa.ToXmlString(true), rsa.ToXmlString(false));
        }

        /// <summary>
        /// 使用RSA实现加密
        /// </summary>
        /// <param name="publicKey">RSA公钥</param>
        /// <param name="data">加密数据</param>
        /// <returns></returns>
        public static string RSAEncrypt(string publicKey, string data)
        {
            ////创建RSA对象并载入[公钥]
            //RSACryptoServiceProvider rsaPublic = new RSACryptoServiceProvider();
            //RSAExtension.FromXmlString(rsaPublic, publicKey);
            ////对数据进行加密
            //byte[] publicValue = rsaPublic.Encrypt(Encoding.UTF8.GetBytes(data), false);
            //string publicStr = Convert.ToBase64String(publicValue);//使用Base64将byte转换为string
            //return publicStr;

            using (var rsaProvider = new RSACryptoServiceProvider())
            {
                var inputBytes = Encoding.UTF8.GetBytes(data);//有含义的字符串转化为字节流
                RSAExtension.FromXmlString(rsaProvider, publicKey);//载入公钥
                int bufferSize = (rsaProvider.KeySize / 8) - 11;//单块最大长度
                var buffer = new byte[bufferSize];
                using (MemoryStream inputStream = new MemoryStream(inputBytes),
                     outputStream = new MemoryStream())
                {
                    while (true)
                    { //分段加密
                        int readSize = inputStream.Read(buffer, 0, bufferSize);
                        if (readSize <= 0)
                        {
                            break;
                        }

                        var temp = new byte[readSize];
                        Array.Copy(buffer, 0, temp, 0, readSize);
                        var encryptedBytes = rsaProvider.Encrypt(temp, false);
                        outputStream.Write(encryptedBytes, 0, encryptedBytes.Length);
                    }
                    return Convert.ToBase64String(outputStream.ToArray());//转化为字节流方便传输
                }
            }
        }

        /// <summary>
        /// 使用RSA实现解密
        /// </summary>
        /// <param name="privateKey">RSA私钥</param>
        /// <param name="data">解密数据</param>
        /// <returns></returns>
        public static string RSADecrypt(string privateKey, string data)
        {
            ////C#默认只能使用[私钥]进行解密(想使用[私钥加密]可使用第三方组件BouncyCastle来实现)
            ////创建RSA对象并载入[私钥]
            //RSACryptoServiceProvider rsaPrivate = new RSACryptoServiceProvider();
            //RSAExtension.FromXmlString(rsaPrivate, privateKey);
            ////对数据进行解密
            //byte[] privateValue = rsaPrivate.Decrypt(Convert.FromBase64String(data), false);//使用Base64将string转换为byte
            //string privateStr = Encoding.UTF8.GetString(privateValue);
            //return privateStr;
            using (RSACryptoServiceProvider rsaProvider = new RSACryptoServiceProvider())
            {
                var inputBytes = Convert.FromBase64String(data);
                RSAExtension.FromXmlString(rsaProvider, privateKey);
                int bufferSize = rsaProvider.KeySize / 8;
                var buffer = new byte[bufferSize];
                using (MemoryStream inputStream = new MemoryStream(inputBytes),
                     outputStream = new MemoryStream())
                {
                    while (true)
                    {
                        int readSize = inputStream.Read(buffer, 0, bufferSize);
                        if (readSize <= 0)
                        {
                            break;
                        }

                        var temp = new byte[readSize];
                        Array.Copy(buffer, 0, temp, 0, readSize);
                        var rawBytes = rsaProvider.Decrypt(temp, false);
                        outputStream.Write(rawBytes, 0, rawBytes.Length);
                    }
                    return Encoding.UTF8.GetString(outputStream.ToArray());
                }
            }
        }


    }
}
