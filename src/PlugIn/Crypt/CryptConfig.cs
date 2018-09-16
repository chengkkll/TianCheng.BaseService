using System;
using System.Collections.Generic;
using System.Text;

namespace TianCheng.BaseService.PlugIn.Crypt
{
    /// <summary>
    /// 配置
    /// </summary>
    public static class CryptConfig
    {
        /// <summary>
        /// 每天发送短信验证码上限
        /// </summary>
        public const int DAY_SMS_VALIDATE_CODE_CELING = 10;

        /// <summary>
        /// 短信验证码有效时间(单位：分)
        /// </summary>
        public const int SMS_VALIDATA_CODE_VALID_TIME = 10;

        /// <summary>
        /// RSA私钥
        /// </summary>
        public const string RSA_PRIVATE_KEY = "<RSAKeyValue><Modulus>td5EYiTUd08FrYd8pmTXU3V98ZhNdzuYDcT19WitQGZfk0IbACnnEHVkmBns2/vIa9qF0G9nnHid7Cj+WnX9BFz9vx0fZaVPrGbxAlow/aIXY+gszCldL0IxF8nxhp/Uh4hrRWa8gaklcV9hw8z+lcG0L2qYcMejSXiWQQ9FxSs=</Modulus><Exponent>AQAB</Exponent><P>6+7GvlZ9vAExmgiiEm5EucU1+Ei95tc/TKAynfR4nR14sLx3xpFEJ0SIxAAogc7Fun3U/0JazwYv4t+DHk+5gQ==</P><Q>xVZI8shtRJS7ogsxO6SqWHqOOY8s0PKfcsdDUpm81fJ3w9OIJ/8X+IsoUcqqIe0HTThxUKNNo1zX64tCNZrcqw==</Q><DP>nW18a/EmgNaDFHcCPi4Z6aNv+bYAERI7iJO0crV37c6Gg9eeTH8N7O3MHIzGeqdQRLpF7/WExnMobMgbo5QrAQ==</DP><DQ>nh4lFWPDKdCDB7QwHroyQ/LvQu+V2VaOrEk9iFHnHQbLL/2ue13KtzvJcsAQ0l46G4W+Rf6TlvItkG5k/s63QQ==</DQ><InverseQ>QbPNr2dCGmKxGXUO6z0s53XhMslEd843K+GW6Qoee/8sSRY9KxyXRyawle4EKGtFp83grqiwfTZd4zJeaYlrtg==</InverseQ><D>A0hq1ZaymyC7CoHalypU1LXXOygzpcZWnVED25UzI2l1qjPURvF6sUpdMX2uT95ApOnB5pxMM3/d4ehvhvSAR68P22LIs24LyE6exX2jgUHJc6GX8KXbMeXae+hvqYR0XAHIwH1F1rue56Ssa9gB05u/4JhRx9n6GfVXaZldMQE=</D></RSAKeyValue>";

        /// <summary>
        /// RSA公钥
        /// </summary>
        public const string RSA_PUBLIC_KEY = "<RSAKeyValue><Modulus>td5EYiTUd08FrYd8pmTXU3V98ZhNdzuYDcT19WitQGZfk0IbACnnEHVkmBns2/vIa9qF0G9nnHid7Cj+WnX9BFz9vx0fZaVPrGbxAlow/aIXY+gszCldL0IxF8nxhp/Uh4hrRWa8gaklcV9hw8z+lcG0L2qYcMejSXiWQQ9FxSs=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";

        /// <summary>
        /// RSA标准格式私钥
        /// </summary>
        public const string RSA_NORMAL_PRIVATE_KEY = "MIICdwIBADANBgkqhkiG9w0BAQEFAASCAmEwggJdAgEAAoGBALXeRGIk1HdPBa2HfKZk11N1ffGYTXc7mA3E9fVorUBmX5NCGwAp5xB1ZJgZ7Nv7yGvahdBvZ5x4newo/lp1/QRc/b8dH2WlT6xm8QJaMP2iF2PoLMwpXS9CMRfJ8Yaf1IeIa0VmvIGpJXFfYcPM/pXBtC9qmHDHo0l4lkEPRcUrAgMBAAECgYADSGrVlrKbILsKgdqXKlTUtdc7KDOlxladUQPblTMjaXWqM9RG8XqxSl0xfa5P3kCk6cHmnEwzf93h6G+G9IBHrw/bYsizbgvITp7FfaOBQclzoZfwpdsx5dp76G+phHRcAcjAfUXWu57npKxr2AHTm7/gmFHH2foZ9VdpmV0xAQJBAOvuxr5WfbwBMZoIohJuRLnFNfhIvebXP0ygMp30eJ0deLC8d8aRRCdEiMQAKIHOxbp91P9CWs8GL+Lfgx5PuYECQQDFVkjyyG1ElLuiCzE7pKpYeo45jyzQ8p9yx0NSmbzV8nfD04gn/xf4iyhRyqoh7QdNOHFQo02jXNfri0I1mtyrAkEAnW18a/EmgNaDFHcCPi4Z6aNv+bYAERI7iJO0crV37c6Gg9eeTH8N7O3MHIzGeqdQRLpF7/WExnMobMgbo5QrAQJBAJ4eJRVjwynQgwe0MB66MkPy70LvldlWjqxJPYhR5x0Gyy/9rntdyrc7yXLAENJeOhuFvkX+k5byLZBuZP7Ot0ECQEGzza9nQhpisRl1Dus9LOd14TLJRHfONyvhlukKHnv/LEkWPSscl0cmsJXuBChrRafN4K6osH02XeMyXmmJa7Y=";

        /// <summary>
        /// RSA标准格式公钥 
        /// </summary>
        public const string RSA_NORMAL_PUBLIC_KEY = "MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQC13kRiJNR3TwWth3ymZNdTdX3xmE13O5gNxPX1aK1AZl+TQhsAKecQdWSYGezb+8hr2oXQb2eceJ3sKP5adf0EXP2/HR9lpU+sZvECWjD9ohdj6CzMKV0vQjEXyfGGn9SHiGtFZryBqSVxX2HDzP6VwbQvaphwx6NJeJZBD0XFKwIDAQAB";
    }
}
