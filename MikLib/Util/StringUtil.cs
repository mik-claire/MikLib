using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MikLib.Util
{
    public static class StringUtil
    {
        /// <summary>
        /// UTF8のボム情報
        /// </summary>
        private static readonly byte[] bom = Encoding.UTF8.GetPreamble();

        /// <summary>
        /// UTF8エンコーディング
        /// </summary>
        private static readonly Encoding utf8 = new UTF8Encoding(false);

        /// <summary>
        /// Shift-JISエンコーディング
        /// </summary>
        private static readonly Encoding shiftJis = Encoding.GetEncoding("Shift-JIS");

        /// <summary>
        /// 文字列のバイナリからBOMを外す
        /// </summary>
        /// <param name="doc">BOMが付いているかもしれない文字列のバイナリデータ</param>
        /// <returns>BOM抜き文字列のバイナリデータ</returns>
        public static byte[] RemoveBom(byte[] src)
        {
            if (src.Length < 3)
            {
                return new byte[0];
            }

            bool withBom = true;
            for (int i = 0; i < bom.Length; i++)
            {
                if (bom[i] == src[i])
                {
                    continue;
                }

                withBom = false;
                break;
            }

            if (!withBom)
            {
                return src;
            }

            byte[] encodedBytes = new byte[src.Length - 3];
            for (int i = 0; i < encodedBytes.Length; i++)
            {
                encodedBytes[i] = src[i + 3];
            }

            return encodedBytes;
        }

        /// <summary>
        /// 文字列をUTF8からSJISに変換する
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static string ConvertToSjisFromUtf8(string src)
        {
            byte[] srcBytes = Encoding.UTF8.GetBytes(src);
            srcBytes = RemoveBom(srcBytes);

            byte[] dstBytes = Encoding.Convert(utf8, shiftJis, srcBytes);

            string dst = shiftJis.GetString(dstBytes);
            return dst;
        }
    }
}
