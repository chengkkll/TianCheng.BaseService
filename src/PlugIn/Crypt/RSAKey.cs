using System;
using System.Collections.Generic;
using System.Text;

namespace TianCheng.BaseService.PlugIn.Crypt
{
    /// <summary>
    /// RSA秘钥对
    /// </summary>
    public struct RSAKey
    {
        /// <summary>
        /// 公钥
        /// </summary>
        public string PublicKey { get; set; }

        /// <summary>
        /// 私钥
        /// </summary>
        public string PrivateKey { get; set; }

        /// <summary>
        /// RSA秘钥对
        /// </summary>
        /// <param name="privateKey">私钥</param>
        /// <param name="publicKey">公钥</param>
        public RSAKey(string privateKey, string publicKey)
        {
            PublicKey = publicKey;
            PrivateKey = privateKey;
        }
    }
}
