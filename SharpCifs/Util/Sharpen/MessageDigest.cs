using System;
using System.Linq;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;

namespace SharpCifs.Util.Sharpen
{
    public abstract class MessageDigest
    {
        public void Digest(byte[] buffer, int o, int len)
        {
            byte[] d = Digest();
            d.CopyTo(buffer, o);
        }

        public byte[] Digest(byte[] buffer)
        {
            Update(buffer);
            return Digest();
        }

        public abstract byte[] Digest();
        public abstract int GetDigestLength();
        public static MessageDigest GetInstance(string algorithm)
        {
            switch (algorithm.ToLower())
            {
                case "sha-1":
                    return new HashMessageDigest(HashAlgorithmNames.Sha1);
                case "md5":
                    return new HashMessageDigest(HashAlgorithmNames.Md5);
            }
            throw new NotSupportedException(string.Format("The requested algorithm \"{0}\" is not supported.", algorithm));
        }

        public abstract void Reset();
        public abstract void Update(byte[] b);
        public abstract void Update(byte b);
        public abstract void Update(byte[] b, int offset, int len);
    }

    class HashMessageDigest : MessageDigest
    {
        HashAlgorithmProvider _provider;
        CryptographicHash _hash;

        public HashMessageDigest(string algorithm)
        {
            _provider = HashAlgorithmProvider.OpenAlgorithm(algorithm);
            Init();
        }

        public override byte[] Digest()
        {
            byte[] hash;
            CryptographicBuffer.CopyToByteArray(_hash.GetValueAndReset(), out hash);
            return hash;
        }

        public override int GetDigestLength()
        {
            return (int)_provider.HashLength;
        }

        private void Init()
        {
            _hash = _provider.CreateHash();
        }

        public override void Reset()
        {
            Init();
        }

        public override void Update(byte[] input)
        {
            _hash.Append(CryptographicBuffer.CreateFromByteArray(input));
        }

        public override void Update(byte input)
        {
            Update(new[] { input });
        }

        public override void Update(byte[] input, int index, int count)
        {
            if (count < 0)
                throw new ArgumentException("count < 0!");
            Update(new ArraySegment<byte>(input, index, count).ToArray());
        }
    }
}
