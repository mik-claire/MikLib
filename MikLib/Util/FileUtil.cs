using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MikLib.Util
{
    public static class FileUtil
    {
        /// <summary>
        /// 指定されたファイル・ディレクトリの親ディレクトリ名を取得する
        /// </summary>
        /// <param name="fullPath">対象のファイル・ディレクトリの絶対パス</param>
        /// <returns>親ディレクトリの名前</returns>
        public static string GetParentFolderName(string fullPath)
        {
            char[] delimiter = new char[] { '\\' };
            string[] splitedDir = fullPath.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);

            if (splitedDir.Length < 2)
            {
                return splitedDir[0];
            }

            return splitedDir[splitedDir.Length - 1];
        }

        /// <summary>
        /// ディレクトリをコピー
        /// </summary>
        /// <param name="srcPath">コピー元のディレクトリパス</param>
        /// <param name="dstPath">コピー元のディレクトリパス</param>
        public static void DirectoryCopy(string srcPath, string dstPath)
        {
            DirectoryInfo srcDir = new DirectoryInfo(srcPath);
            DirectoryInfo dstDir = new DirectoryInfo(dstPath);

            // コピー先のディレクトリがなければ作成
            if (!dstDir.Exists)
            {
                dstDir.Create();
                dstDir.Attributes = srcDir.Attributes;
            }

            // ファイルコピー
            foreach (FileInfo fi in srcDir.GetFiles())
            {
                // 同じファイルが存在していたら上書き
                fi.CopyTo(Path.Combine(dstDir.FullName, fi.Name), true);
            }

            //ディレクトリのコピー（再帰を使用）
            foreach (DirectoryInfo di in srcDir.GetDirectories())
            {
                DirectoryCopy(di.FullName, Path.Combine(dstDir.FullName, di.Name));
            }
        }

        /// <summary>
        /// CSVからデータを読み込み
        /// </summary>
        /// <param name="filePath">読み込むファイルのパス</param>
        /// <param name="delimiter">区切り文字 (1文字)</param>
        /// <param name="existsHeader">ヘッダーありのデータか否か</param>
        /// <returns>読んだデータ</returns>
        public static List<string[]> ReadCSV(string filePath, char delimiter, bool existsHeader = false)
        {
            StreamReader sr = null;
            List<string[]> rows = new List<string[]>();

            try
            {
                sr = new StreamReader(filePath, Encoding.Default);

                if (existsHeader)
                {
                    sr.ReadLine();
                }

                while (-1 < sr.Peek())
                {
                    string[] row = sr.ReadLine().Split(delimiter);
                    rows.Add(row);
                }

                return rows;
            }
            finally
            {
                if (sr != null)
                {
                    sr.Close();
                }
            }
        }

    }
}
