/*
This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.If not, see<http://www.gnu.org/licenses/>.
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;

#pragma warning disable 0618

namespace OdinSdk.BaseLib.Cryption
{
    //-----------------------------------------------------------------------------------------------------------------------------
    //
    //-----------------------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class CCryption : IDisposable
    {
        //-----------------------------------------------------------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// 
        /// </summary>
        private CCryption()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p_crypto_key"></param>
        public CCryption(string p_crypto_key = "")
        {
            SelectedKey = IsExistKey(p_crypto_key) == false ? DefaultKey : p_crypto_key;
            CreateTransform();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p_rgbkey"></param>
        /// <param name="p_vector"></param>
        public CCryption(string p_rgbkey, string p_vector)
        {
            SelectedKey = "";
            CreateTransform(p_rgbkey, p_vector);
        }

        //-----------------------------------------------------------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// 
        /// </summary>
        public const string DefaultKey = "V1006151";

        private static readonly string[,] CryptoString =
        {
            {"V0503201", "vszQeBFqA9tojdJlfOLAalPLdi+pWZRFk42kG0KpC70=", "mxfQcjp6cJBA5yRnl+C1DA=="},
            {"V0510141", "DXzT3SeaKHq1LRxExKQfJvzhUxJ112NAj4zQoMJ5zek=", "gVF1AAo0QubIX57bowDcDA=="},
            {"V0511071", "Mxk2ACWrwz+UWfUkiw8cPkteHYo7TphzIgSOni4cBQE=", "QTdEP2iDrlOyvp6rdA9kFg=="},
            {"V0708091", "q8ln1+RkYfJbqQ6s6S8W0wGsDRhE2TRsiCCHybFua9A=", "gu+nT4ANRARXz+blqvHi4w=="},
            {"V1006151", "Dr/uVjYLK1hkoD1YNIP+NtuXKfIpzXxKvu2pR6AD/AU=", "y7XlqDlAGHqvULRM5Ng3vw=="},
            {"V1006152", "KdGSxDdsPp5TNGZsdFIOpIa/x0KKTum0lrMs9uXWmPY=", "6EoMJlSr1bbAMfkFJ9K8BQ=="}
        };

        /// <summary>
        /// 선택 되어진 암호화 키
        /// </summary>
        public string SelectedKey
        {
            get;
            private set;
        }

        private static object m_syncRoot = null;

        /// <summary>
        /// 액세스를 동기화하는 데 사용할 수 있는 개체를 가져옵니다.
        /// </summary>
        public object SyncRoot
        {
            get
            {
                if (m_syncRoot == null)
                    m_syncRoot = new object();

                return m_syncRoot;
            }
        }

        private static readonly Lazy<List<string>> m_keyVersions = new Lazy<List<string>>(() =>
        {
            var _keyVersions = new List<string>();

            for (int _x = CryptoString.GetLowerBound(0); _x <= CryptoString.GetUpperBound(0); _x++)
                _keyVersions.Add(CryptoString[_x, 0]);

            return _keyVersions;
        });

        /// <summary>
        /// 
        /// </summary>
        public static List<string> KeyVersions
        {
            get
            {
                return m_keyVersions.Value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p_crypto_key"></param>
        /// <returns></returns>
        public static bool IsExistKey(string p_crypto_key)
        {
            return KeyVersions.Contains(p_crypto_key);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetRandomKey()
        {
            var _random = new Random();

            var _offset = _random.Next(CryptoString.GetLowerBound(0), CryptoString.GetUpperBound(0));

            return KeyVersions[_offset];
        }

        //-----------------------------------------------------------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------------------------------------------------------
        private static Aes m_rcManaged;

        private static Aes CryptManager
        {
            get
            {
                if (m_rcManaged == null)
                    m_rcManaged = Aes.Create();

                return m_rcManaged;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ICryptoTransform Encryptor
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public ICryptoTransform Decryptor
        {
            get;
            set;
        }

        private void CreateTransform()
        {
            var _offset = KeyVersions.IndexOf(SelectedKey);
            CreateTransform(CryptoString[_offset, 1], CryptoString[_offset, 2]);
        }

        private void CreateTransform(string p_rgbkey, string p_vector)
        {
            lock (SyncRoot)
            {
                byte[] _rgbKey = Convert.FromBase64String(p_rgbkey);
                byte[] _rgbIV = Convert.FromBase64String(p_vector);

                Encryptor = CryptManager.CreateEncryptor(_rgbKey, _rgbIV);
                Decryptor = CryptManager.CreateDecryptor(_rgbKey, _rgbIV);
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// 평문을 암호화 합니다.
        /// </summary>
        /// <param name="p_plain_text"></param>
        /// <param name="p_encoding"></param>
        /// <returns></returns>
        public string PlainToChiper(string p_plain_text, Encoding p_encoding = null)
        {
            var _result = "";

            lock (SyncRoot)
            {
                using (var _os = new MemoryStream())
                {
                    using (var _cs = new CryptoStream(_os, Encryptor, CryptoStreamMode.Write))
                    {
                        p_encoding = p_encoding ?? Encoding.UTF8;

                        using (var _sw = new StreamWriter(_cs, p_encoding))
                        {
                            _sw.Write(p_plain_text);
                        }
                    }

                    _result = Convert.ToBase64String(_os.ToArray());
                }
            }

            return _result;
        }

        /// <summary>
        /// 암호화된 내역을 복호화한다.
        /// </summary>
        /// <param name="p_chiper_text"></param>
        /// <param name="p_encoding"></param>
        /// <returns></returns>
        public string ChiperToPlain(string p_chiper_text, Encoding p_encoding = null)
        {
            var _result = p_chiper_text;

            lock (SyncRoot)
            {
                using (var _is = new MemoryStream(Convert.FromBase64String(p_chiper_text)))
                {
                    using (var _cs = new CryptoStream(_is, Decryptor, CryptoStreamMode.Read))
                    {
                        p_encoding = p_encoding ?? Encoding.UTF8;

                        using (var _sr = new StreamReader(_cs, p_encoding))
                        {
                            _result = _sr.ReadToEnd();
                        }
                    }
                }
            }

            return _result;
        }

        /// <summary>
        /// 평문 문자열을 암호화 된 문자열로 변환 합니다.
        /// </summary>
        /// <param name="p_plain_text"></param>
        /// <param name="p_compress"></param>
        /// <returns></returns>
        public string PlainToChiperText(string p_plain_text, bool p_compress = false)
        {
            return Convert.ToBase64String(PlainToChiperBytes(p_plain_text, p_compress));
        }

        /// <summary>
        /// 암호화된 문자열을 내역을 복호화 합니다.
        /// </summary>
        /// <param name="p_chiper_text"></param>
        /// <param name="p_compress"></param>
        /// <returns></returns>
        public string ChiperTextToPlain(string p_chiper_text, bool p_compress = false)
        {
            return (string)ChiperBytesToPlain(Convert.FromBase64String(p_chiper_text), p_compress);
        }

        /// <summary>
        /// 평문을 암호화 합니다.
        /// </summary>
        /// <param name="p_object"></param>
        /// <param name="p_compress"></param>
        /// <returns></returns>
        public byte[] PlainToChiperBytes(object p_object, bool p_compress = false)
        {
            var _result = new byte[0];

            lock (SyncRoot)
            {
                var _jsonSerializer = new JsonSerializer();

                using (var _mse = new MemoryStream())
                {
                    using (var _cse = new CryptoStream(_mse, Encryptor, CryptoStreamMode.Write))
                    {
                        if (p_compress == true)
                        {
                            using (var _gse = new GZipStream(_cse, CompressionMode.Compress))
                            {
                                var _bsonWriter = new BsonWriter(_gse);
                                _jsonSerializer.Serialize(_bsonWriter, p_object);

                                //var _bfe = new BinaryFormatter();
                                //_bfe.Serialize(_gse, p_object);
                            }
                        }
                        else
                        {
                            var _bsonWriter = new BsonWriter(_cse);
                            _jsonSerializer.Serialize(_bsonWriter, p_object);

                            //var _bfe = new BinaryFormatter();
                            //_bfe.Serialize(_cse, p_object);
                        }
                    }

                    _result = _mse.ToArray();
                }
            }

            return _result;
        }

        /// <summary>
        /// 암호화된 바이트열을 평문으로 변환 합니다.
        /// </summary>
        /// <param name="p_chiper_bytes"></param>
        /// <param name="p_compress"></param>
        /// <returns></returns>
        public object ChiperBytesToPlain(byte[] p_chiper_bytes, bool p_compress = false)
        {
            var _result = (object)null;

            lock (SyncRoot)
            {
                var _jsonSerializer = new JsonSerializer();

                using (var _msd = new MemoryStream(p_chiper_bytes))
                {
                    using (var _csd = new CryptoStream(_msd, Decryptor, CryptoStreamMode.Read))
                    {
                        if (p_compress == true)
                        {
                            using (var _gsd = new GZipStream(_csd, CompressionMode.Decompress))
                            {
                                var _bsonReader = new BsonReader(_gsd);
                                _result = _jsonSerializer.Deserialize(_bsonReader);

                                //var _bfd = new BinaryFormatter();
                                //_result = _bfd.Deserialize(_gsd);
                            }
                        }
                        else
                        {
                            var _bsonReader = new BsonReader(_csd);
                            _result = _jsonSerializer.Deserialize(_bsonReader);

                            //var _bfd = new BinaryFormatter();
                            //_result = _bfd.Deserialize(_csd);
                        }
                    }
                }
            }

            return _result;
        }

        //-----------------------------------------------------------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Generates a hash for the given plain text value and returns a
        /// byte[]-encoded result. Before the hash is computed, a random salt
        /// is generated and appended to the plain text. This salt is stored at
        /// the end of the hash value, so it can be used later for hash
        /// verification.
        /// </summary>
        /// <param name="p_plain_text">
        /// Plaintext value to be hashed. The function does not check whether
        /// this parameter is null.
        /// </param>
        /// <param name="p_hashAlgorithm">
        /// Name of the hash algorithm. Allowed values are: "MD5", "SHA1",
        /// "SHA256", "SHA384", and "SHA512" (if any other value is specified
        /// MD5 hashing algorithm will be used). This value is case-insensitive.
        /// </param>
        /// <param name="p_saltBytes">
        /// Salt bytes. This parameter can be null, in which case a random salt
        /// value will be generated.
        /// </param>        
        /// <returns>
        /// Hash value formatted as a encoded byte[]. for SQL varbinary field.
        /// </returns>
		public byte[] ComputeHash(string p_plain_text, string p_hashAlgorithm = "SHA256", byte[] p_saltBytes = null)
        {
            // If salt is not specified, generate it on the fly.
            if (p_saltBytes == null)
            {
                // Define min and max salt sizes.
                var _minSaltSize = 4;
                var _maxSaltSize = 8;

                // Generate a random number for the size of the salt.
                var _random = new Random();
                var _saltSize = _random.Next(_minSaltSize, _maxSaltSize);

                // Allocate a byte array, which will hold the salt.
                p_saltBytes = new byte[_saltSize];

                // Initialize a random number generator.
                //var _rng = new RNGCryptoServiceProvider();
                var _rng = RandomNumberGenerator.Create();

                // Fill the salt with cryptographically strong byte values.
                //_rng.GetNonZeroBytes(p_saltBytes);
                _rng.GetBytes(p_saltBytes);
            }

            // Convert plain text into a byte array.
            byte[] _plainTextBytes = Encoding.UTF8.GetBytes(p_plain_text);

            // Allocate array, which will hold plain text and salt.
            byte[] _plainTextWithSaltBytes = new byte[_plainTextBytes.Length + p_saltBytes.Length];

            // Copy plain text bytes into resulting array.
            for (int i = 0; i < _plainTextBytes.Length; i++)
                _plainTextWithSaltBytes[i] = _plainTextBytes[i];

            // Append salt bytes to the resulting array.
            for (int i = 0; i < p_saltBytes.Length; i++)
                _plainTextWithSaltBytes[_plainTextBytes.Length + i] = p_saltBytes[i];

            // Because we support multiple hashing algorithms, we must define
            // hash object as a common (abstract) base class. We will specify the
            // actual hashing algorithm class later during object creation.
            HashAlgorithm _hashAlgorithm;

            // Initialize appropriate hashing algorithm class.
            switch (p_hashAlgorithm.ToUpper())
            {
                case "SHA1":
                    _hashAlgorithm = SHA1.Create();
                    break;

                case "SHA256":
                    _hashAlgorithm = SHA256.Create();
                    break;

                case "SHA384":
                    _hashAlgorithm = SHA384.Create();
                    break;

                case "SHA512":
                    _hashAlgorithm = SHA512.Create();
                    break;

                default:
                    _hashAlgorithm = MD5.Create();
                    break;
            }

            // Compute hash value of our plain text with appended salt.
            byte[] _hashBytes = _hashAlgorithm.ComputeHash(_plainTextWithSaltBytes);

            // Create array which will hold hash and original salt bytes.
            byte[] _result = new byte[_hashBytes.Length + p_saltBytes.Length];

            // Copy hash bytes into resulting array.
            for (int i = 0; i < _hashBytes.Length; i++)
                _result[i] = _hashBytes[i];

            // Append salt bytes to the result.
            for (int i = 0; i < p_saltBytes.Length; i++)
                _result[_hashBytes.Length + i] = p_saltBytes[i];

            // Return the result.
            return _result;
        }

        /// <summary>
        /// Compares a hash of the specified plain text value to a given hash
        /// value. Plain text is hashed with the same salt value as the original
        /// hash.
        /// </summary>
        /// <param name="plain_text">
        /// Plain text to be verified against the specified hash. The function
        /// does not check whether this parameter is null.
        /// </param>
        /// <param name="hash_algorithm">
        /// Name of the hash algorithm. Allowed values are: "MD5", "SHA1", 
        /// "SHA256", "SHA384", and "SHA512" (if any other value is specified,
        /// MD5 hashing algorithm will be used). This value is case-insensitive.
        /// </param>
        /// <param name="hash_value">
        /// byte[]-encoded hash value produced by ComputeHash function. This value
        /// includes the original salt appended to it.
        /// </param>
        /// <returns>
        /// If computed hash mathes the specified hash the function the return
        /// value is true; otherwise, the function returns false.
        /// </returns>
        public bool VerifyHash(string plain_text, string hash_algorithm = "SHA256", byte[] hash_value = null)
        {
            var _result = false;

            // We must know size of hash (without salt).
            int _hashSizeInBits, _hashSizeInBytes;

            // Size of hash is based on the specified algorithm.
            switch (hash_algorithm.ToUpper())
            {
                case "SHA1":
                    _hashSizeInBits = 160;
                    break;

                case "SHA256":
                    _hashSizeInBits = 256;
                    break;

                case "SHA384":
                    _hashSizeInBits = 384;
                    break;

                case "SHA512":
                    _hashSizeInBits = 512;
                    break;

                default: // Must be MD5
                    _hashSizeInBits = 128;
                    break;
            }

            // Convert size of hash from bits to bytes.
            _hashSizeInBytes = _hashSizeInBits / 8;

            // Make sure that the specified hash value is long enough.
            if (hash_value.Length >= _hashSizeInBytes)
            {
                // Allocate array to hold original salt bytes retrieved from hash.
                byte[] _saltBytes = new byte[hash_value.Length - _hashSizeInBytes];

                // Copy salt from the end of the hash to the new array.
                for (int i = 0; i < _saltBytes.Length; i++)
                    _saltBytes[i] = hash_value[_hashSizeInBytes + i];

                // Compute a new hash String.
                byte[] _expectedHashString = ComputeHash(plain_text, hash_algorithm, _saltBytes);

                // If the computed hash matches the specified hash,
                // the plain text value must be correct.
                var _chkCharCount = 0;

                if (hash_value.Length == _expectedHashString.Length)
                {
                    for (int i = 0; i < hash_value.Length; i++)
                    {
                        if (hash_value[i] != _expectedHashString[i])
                        {
                            _chkCharCount = hash_value[i] - _expectedHashString[i];
                            break;
                        }
                    }
                }
                else
                {
                    _chkCharCount = hash_value.Length - _expectedHashString.Length;
                }

                if (_chkCharCount == 0)
                    _result = true;
            }

            return _result;
        }

        //-----------------------------------------------------------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------------------------------------------------------

        #region IDisposable Members

        /// <summary>
        /// 
        /// </summary>
        private bool IsDisposed
        {
            get;
            set;
        }

        /// <summary>
        /// Dispose of the backing store before garbage collection.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose of the backing store before garbage collection.
        /// </summary>
        /// <param name="disposing">
        /// <see langword="true"/> if disposing; otherwise, <see langword="false"/>.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (IsDisposed == false)
            {
                if (disposing == true)
                {
                    // Dispose managed resources. 
                }

                // Dispose unmanaged resources. 

                // Note disposing has been done. 
                IsDisposed = true;
            }
        }

        /// <summary>
        /// Dispose of the backing store before garbage collection.
        /// </summary>
        ~CCryption()
        {
            Dispose(false);
        }

        #endregion
    }
}
