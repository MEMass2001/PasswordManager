using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManagerApp.Classes
{
    public class IdeaCipher
    {
        private static int blockSize = 8;
        public static string cryptString(string input, string key, bool encrypt)
        {
            Stream inStream = null;
            Stream outStream = new MemoryStream();
            try
            {
                Idea idea = new Idea(key, encrypt);
                BlockStreamCrypter bsc = new BlockStreamCrypter(idea, encrypt);
                long inStreamSize;
                long inDataLen;
                long outDataLen;
                if (encrypt)
                {
                    inStream = StringToStream(input);
                    inStreamSize = inStream.Length;
                    inDataLen = inStreamSize;
                    outDataLen = (inDataLen + blockSize - 1) / blockSize * blockSize;
                }
                else
                {
                    string[] values = input.Split(' ');
                    byte[] bytes = new byte[values.Length];
                    for (int i = 0; i < values.Length; i++)
                    {
                        bytes[i] = Byte.Parse(values[i]);
                    }
                    inStream = new MemoryStream(bytes);
                    //inStream = StringToStream(input);
                    inStreamSize = inStream.Length;
                    if (inStreamSize == 0)
                    {
                        throw new IOException("Input Stream Size is empty.");
                    }
                    if (inStreamSize % blockSize != 0)
                    {
                        throw new IOException("Input stream size is not a multiple of " + blockSize + ".");
                    }
                    inDataLen = inStreamSize - blockSize;
                    outDataLen = inDataLen;
                }
                processData(inStream, inDataLen, outStream, outDataLen, bsc);
                if (encrypt)
                {
                    outStream = writeDataLength(outStream, inDataLen, bsc);
                    return string.Join(" ", StreamToBytes(outStream));
                    //return StreamToString(outStream);
                }
                else
                {
                    long outFileSize = readDataLength(inStream, bsc);
                    //if (outFileSize < 0 || outFileSize > inDataLen || outFileSize < inDataLen - blockSize + 1)
                    //{
                    //    throw new IOException("Input file is not a valid cryptogram.");
                    //}
                    //if (outFileSize != outDataLen)
                    //{
                    //    inStream.SetLength(outFileSize);
                    //}
                    return StreamToString(outStream);
                }
            }
            finally
            {
                if (inStream != null)
                {
                    inStream.Close();
                }
                if (outStream != null)
                {
                    outStream.Close();
                }
            }
            return null;
        }
        public static byte[] StreamToBytes(Stream input)
        {
            MemoryStream mems = (MemoryStream)input;
            return mems.ToArray();
        }
        public static string StreamToString(Stream stream)
        {
            stream.Position = 0;
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }
        public static Stream StringToStream(string src)
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(src);
            return new MemoryStream(byteArray);
        }

        private static Stream writeDataLength(Stream stream, long dataLength, BlockStreamCrypter bsc)
        {
            byte[] a = packDataLength(dataLength);
            bsc.crypt(a, 0);
            stream.Write(a, 0, blockSize);
            return stream;
        }

        // Packs an integer into an 8-byte block. Used to encode the file size.
        // To support larger files, we allow 13 more bits than the original IDEA V1.1 implementation.
        // But files larger than 4GB are no longer backward compatible with the old IDEA V1.1 file structure.
        private static byte[] packDataLength(long i)
        {
            if (i > 0x1FFFFFFFFFFFL) // 45 bits
            {
                throw new ArgumentException("Text too long.");
            }
            byte[] b = new byte[blockSize];
            b[7] = (byte)(i << 3);
            b[6] = (byte)(i >> 5);
            b[5] = (byte)(i >> 13);
            b[4] = (byte)(i >> 21);
            b[3] = (byte)(i >> 29);
            b[2] = (byte)(i >> 37);
            return b;
        }
        // Extracts an integer from an 8-byte block. Used to decode the file size.
        // Returns -1 if the encoded value is invalid. This means that the input file is not a valid cryptogram.

        private static long readDataLength(Stream stream, BlockStreamCrypter bsc)
        {
            byte[] buf = new byte[blockSize];
            int trLen = stream.Read(buf, 0, blockSize);
            if (trLen != blockSize)
            {
                throw new Exception("Unable to read data length suffix.");
            }
            bsc.crypt(buf, 0);
            return unpackDataLength(buf);
        }
        private static long unpackDataLength(byte[] b)
        {
            if (b[0] != 0 || b[1] != 0 || (b[7] & 7) != 0)
            {
                return -1;
            }
            return
               (long)(b[7] & 0xFF) >> 3 |
               (long)(b[6] & 0xFF) << 5 |
               (long)(b[5] & 0xFF) << 13 |
               (long)(b[4] & 0xFF) << 21 |
               (long)(b[3] & 0xFF) << 29 |
               (long)(b[2] & 0xFF) << 37;
        }
        private static void processData(Stream inStream, long inDataLen, Stream outStream, long outDataLen, BlockStreamCrypter bsc)
        {
            int bufSize = 0x200000;
            byte[] buf = new byte[bufSize];
            long filePos = 0;
            while (filePos < inDataLen)
            {
                int reqLen = (int)Math.Min(inDataLen - filePos, bufSize);
                int trLen = inStream.Read(buf, 0, reqLen);
                if (trLen != reqLen)
                {
                    throw new Exception("Incomplete data chunk read from file.");
                }
                int chunkLen = (trLen + blockSize - 1) / blockSize * blockSize;
                for (int i = trLen; i <= chunkLen; i++)
                {
                    buf[i] = 0;
                }
                for (int pos = 0; pos < chunkLen; pos += blockSize)
                {
                    bsc.crypt(buf, pos);
                }
                reqLen = (int)Math.Min(outDataLen - filePos, chunkLen);

                outStream.Write(buf, 0, reqLen);

                filePos += chunkLen;
            }
        }
        private static void xor(byte[] a, int pos, byte[] b)
        {
            for (int p = 0; p < blockSize; p++)
            {
                a[pos + p] ^= b[p];
            }
        }
        private class BlockStreamCrypter
        {
            Idea idea;
            bool encrypt;
            // data of the previous ciphertext block
            byte[] prev;
            byte[] newPrev;
            public BlockStreamCrypter(Idea idea, bool encrypt)
            {
                this.idea = idea;
                this.encrypt = encrypt;
                prev = new byte[blockSize];
                newPrev = new byte[blockSize];
            }
            public void crypt(byte[] data, int pos)
            {
                if (encrypt)
                {
                    xor(data, pos, prev);
                    idea.crypt(data, pos);
                    Array.Copy(data, pos, prev, 0, blockSize);
                }
                else
                {
                    Array.Copy(data, pos, newPrev, 0, blockSize);
                    idea.crypt(data, pos);
                    xor(data, pos, prev);
                    byte[] temp = prev;
                    prev = newPrev;
                    newPrev = temp;
                }
            }
        }
    }

    public class Idea
    {
        // Number of rounds.
        internal static int rounds = 16;
        // Internal encryption sub-keys.
        internal int[] subKey;
        public Idea(string stringKey, bool encrypt)
        {
            byte[] byteKey = generateUserKeyFromCharKey(stringKey);
            // Expands a 16-byte user key to the internal encryption sub-keys.
            int[] tempSubKey = expandUserKey(byteKey);
            if (encrypt)
            {
                subKey = tempSubKey;
            }
            else
            {
                subKey = invertSubKey(tempSubKey);
            }
        }

        private static byte[] generateUserKeyFromCharKey(string key)
        {
            int nofChar = 0x7E - 0x21 + 1;    // Number of different valid characters
            int[] a = new int[8];
            for (int p = 0; p < key.Length; p++)
            {
                int c = key[p];

                for (int i = a.Length - 1; i >= 0; i--)
                {
                    c += a[i] * nofChar;
                    a[i] = c & 0xFFFF;
                    c >>= 16;
                }
            }
            byte[] byteKey = new byte[16];
            for (int i = 0; i < 8; i++)
            {
                byteKey[i * 2] = (byte)(a[i] >> 8);
                byteKey[i * 2 + 1] = (byte)a[i];
            }
            return byteKey;
        }

        private static int[] expandUserKey(byte[] userKey)
        {
            if (userKey.Length != 16)
            {
                throw new ArgumentException("Key length must be 128 bit", "key");
            }
            int[] key = new int[rounds * 6 + 4];
            for (int i = 0; i < userKey.Length / 2; i++)
            {
                key[i] = ((userKey[2 * i] & 0xFF) << 8) | (userKey[2 * i + 1] & 0xFF);
            }
            for (int i = userKey.Length / 2; i < key.Length; i++)
            {
                key[i] = ((key[(i + 1) % 8 != 0 ? i - 7 : i - 15] << 9) | (key[(i + 2) % 8 < 2 ? i - 14 : i - 6] >> 7)) & 0xFFFF;
            }
            return key;
        }

        // Inverts decryption/encrytion sub-keys to encrytion/decryption sub-keys.
        private static int[] invertSubKey(int[] key)
        {
            int[] invKey = new int[key.Length];
            int p = 0;
            int i = rounds * 6;
            invKey[i + 0] = mulInv(key[p++]);
            invKey[i + 1] = addInv(key[p++]);
            invKey[i + 2] = addInv(key[p++]);
            invKey[i + 3] = mulInv(key[p++]);
            for (int r = rounds - 1; r >= 0; r--)
            {
                i = r * 6;
                int m = r > 0 ? 2 : 1;
                int n = r > 0 ? 1 : 2;
                invKey[i + 4] = key[p++];
                invKey[i + 5] = key[p++];
                invKey[i + 0] = mulInv(key[p++]);
                invKey[i + m] = addInv(key[p++]);
                invKey[i + n] = addInv(key[p++]);
                invKey[i + 3] = mulInv(key[p++]);
            }
            return invKey;
        }
        // Additive Inverse.
        // The argument and the result are within the range 0 .. 0xFFFF.
        private static int addInv(int x)
        {
            return (0x10000 - x) & 0xFFFF;
        }
        // Multiplicative inverse.
        // The argument and the result are within the range 0 .. 0xFFFF.
        // The following condition is met for all values of x: mul(x, mulInv(x)) == 1
        private static int mulInv(int x)
        {
            if (x <= 1)
            {
                return x;
            }
            int y = 0x10001;
            int t0 = 1;
            int t1 = 0;
            while (true)
            {
                t1 += y / x * t0;
                y %= x;
                if (y == 1)
                {
                    return 0x10001 - t1;
                }
                t0 += x / y * t1;
                x %= y;
                if (x == 1)
                {
                    return t0;
                }
            }
        }

        // Addition in the additive group.
        // The arguments and the result are within the range 0 .. 0xFFFF.
        private static int add(int a, int b)
        {
            return (a + b) & 0xFFFF;
        }
        // Multiplication in the multiplicative group.
        // The arguments and the result are within the range 0 .. 0xFFFF.
        private static int mul(int a, int b)
        {
            long r = (long)a * b;
            if (r != 0)
            {
                return (int)(r % 0x10001) & 0xFFFF;
            }
            else
            {
                return (1 - a - b) & 0xFFFF;
            }
        }

        /**
         * Encrypts or decrypts a block of 8 data bytes.
         *
         * @param data
         *    Buffer containing the 8 data bytes to be encrypted/decrypted.
         */
        public void crypt(byte[] data)
        {
            crypt(data, 0);
        }

        /**
         * Encrypts or decrypts a block of 8 data bytes.
         *
         * @param data
         *    Data buffer containing the bytes to be encrypted/decrypted.
         * @param dataPos
         *    Start position of the 8 bytes within the buffer.
         */
        public void crypt(byte[] data, int dataPos)
        {
            int x0 = ((data[dataPos + 0] & 0xFF) << 8) | (data[dataPos + 1] & 0xFF);
            int x1 = ((data[dataPos + 2] & 0xFF) << 8) | (data[dataPos + 3] & 0xFF);
            int x2 = ((data[dataPos + 4] & 0xFF) << 8) | (data[dataPos + 5] & 0xFF);
            int x3 = ((data[dataPos + 6] & 0xFF) << 8) | (data[dataPos + 7] & 0xFF);
            //
            int p = 0;
            for (int round = 0; round < rounds; round++)
            {
                int y0 = mul(x0, subKey[p++]);
                int y1 = add(x1, subKey[p++]);
                int y2 = add(x2, subKey[p++]);
                int y3 = mul(x3, subKey[p++]);
                //
                int t0 = mul(y0 ^ y2, subKey[p++]);
                int t1 = add(y1 ^ y3, t0);
                int t2 = mul(t1, subKey[p++]);
                int t3 = add(t0, t2);
                //
                x0 = y0 ^ t2;
                x1 = y2 ^ t2;
                x2 = y1 ^ t3;
                x3 = y3 ^ t3;
            }
            //
            int r0 = mul(x0, subKey[p++]);
            int r1 = add(x2, subKey[p++]);
            int r2 = add(x1, subKey[p++]);
            int r3 = mul(x3, subKey[p++]);
            //
            data[dataPos + 0] = (byte)(r0 >> 8);
            data[dataPos + 1] = (byte)r0;
            data[dataPos + 2] = (byte)(r1 >> 8);
            data[dataPos + 3] = (byte)r1;
            data[dataPos + 4] = (byte)(r2 >> 8);
            data[dataPos + 5] = (byte)r2;
            data[dataPos + 6] = (byte)(r3 >> 8);
            data[dataPos + 7] = (byte)r3;
        }
    }
}
