using System;
using System.Security.Cryptography;

namespace ExpanseManagerServiceLibrary.Services.Security
{
    /*
        ref -> https://stackoverflow.com/questions/4181198/how-to-hash-a-password/10402129#10402129
    */
    public sealed class PasswordHasher
    {
        private const int SaltSize = 16, HashSize = 20, HashIter = 10000 /* min. val. 10 000*/;
        private readonly byte[] _salt, _hash;

        public byte[] Salt { get { return (byte[])_salt.Clone(); } }
        public byte[] Hash { get { return (byte[])_hash.Clone(); } }

        public PasswordHasher(string password)
        {
            new RNGCryptoServiceProvider().GetBytes(_salt = new byte[SaltSize]);
            _hash = new Rfc2898DeriveBytes(password, _salt, HashIter).GetBytes(HashSize);
        }

        public PasswordHasher(byte[] hashBytes)
        {
            Array.Copy(hashBytes, 0, _salt = new byte[SaltSize], 0, SaltSize);
            Array.Copy(hashBytes, SaltSize, _hash = new byte[HashSize], 0, HashSize);
        }

        public PasswordHasher(byte[] salt, byte[] hash)
        {
            Array.Copy(salt, 0, _salt = new byte[SaltSize], 0, SaltSize);
            Array.Copy(hash, 0, _hash = new byte[HashSize], 0, HashSize);
        }

        public byte[] ToArray()
        {
            byte[] hashBytes = new byte[SaltSize + HashSize];
            Array.Copy(_salt, 0, hashBytes, 0, SaltSize);
            Array.Copy(_hash, 0, hashBytes, SaltSize, HashSize);

            return hashBytes;
        }

        public bool Verify(string password)
        {
            byte[] hashedPassword = new Rfc2898DeriveBytes(password, _salt, HashIter).GetBytes(HashSize);
            for (int i = 0; i < HashSize; ++i)
            {
                if (hashedPassword[i] != _hash[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}
