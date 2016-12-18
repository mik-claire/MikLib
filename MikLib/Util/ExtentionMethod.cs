using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MikLib.Util
{
    public static class ExtentionMethod
    {
        /// <summary>
        /// Copy directory recursively.
        /// </summary>
        /// <param name="srcDi">Copy source directoryInfo</param>
        /// <param name="destDirectoryName">New directory name that copy destination</param>
        /// <returns></returns>
        public static DirectoryInfo CopyTo(this DirectoryInfo srcDi, string destDirectoryName)
        {
            DirectoryInfo destDi = new DirectoryInfo(destDirectoryName);

            // Create destination directory if destination directory was not existed.
            if (!destDi.Exists)
            {
                destDi.Create();
                destDi.Attributes = srcDi.Attributes;
            }

            // Copy files
            foreach (FileInfo fi in srcDi.GetFiles())
            {
                // Do NOT overwrite files
                FileInfo destFi = new FileInfo(Path.Combine(destDi.FullName, fi.Name));
                if (destFi.Exists)
                {
                    continue;
                }

                fi.CopyTo(destFi.FullName);
            }

            // Copy directories recursively
            foreach (DirectoryInfo di in srcDi.GetDirectories())
            {
                CopyTo(di, Path.Combine(destDi.FullName, di.Name));
            }

            return destDi;
        }

        /// <summary>
        /// Copy directory recursively.
        /// </summary>
        /// <param name="srcDi">Copy source directoryInfo</param>
        /// <param name="destDirectoryName">New directory name that copy destination</param>
        /// <param name="overwrite">Overwrite files when this argument is true</param>
        /// <returns>DirectoryInfo of copy destination directory</returns>
        public static DirectoryInfo CopyTo(this DirectoryInfo srcDi, string destDirectoryName, bool overwrite)
        {
            DirectoryInfo destDi = new DirectoryInfo(destDirectoryName);

            // Create destination directory if destination directory was not existed.
            if (!destDi.Exists)
            {
                destDi.Create();
                destDi.Attributes = srcDi.Attributes;
            }

            // Copy files
            foreach (FileInfo fi in srcDi.GetFiles())
            {
                // Overwrite files when 'overwrite' argument is true
                FileInfo destFi = new FileInfo(Path.Combine(destDi.FullName, fi.Name));
                if (!overwrite &&
                    destFi.Exists)
                {
                    continue;
                }

                fi.CopyTo(destFi.FullName, overwrite);
            }

            // Copy directories recursively
            foreach (DirectoryInfo di in srcDi.GetDirectories())
            {
                CopyTo(di, Path.Combine(destDi.FullName, di.Name), overwrite);
            }

            return destDi;
        }

        /// <summary>
        /// Copy directory recursively.
        /// </summary>
        /// <param name="srcDi">Copy source directoryInfo</param>
        /// <param name="destDirectoryName">New directory name that copy destination</param>
        /// <param name="overwrite">Overwrite files when this argument is true</param>
        /// <returns>DirectoryInfo of copy destination directory</returns>
        public static DirectoryInfo CopyNewestTo(this DirectoryInfo srcDi, string destDirectoryName)
        {
            DirectoryInfo destDi = new DirectoryInfo(destDirectoryName);

            // Create destination directory if destination directory was not existed.
            if (!destDi.Exists)
            {
                destDi.Create();
                destDi.Attributes = srcDi.Attributes;
            }

            // Copy files
            foreach (FileInfo fi in srcDi.GetFiles())
            {
                // Overwrite files if source file is newly than destination file.
                FileInfo destFi = new FileInfo(Path.Combine(destDi.FullName, fi.Name));
                if (destFi.Exists)
                {
                    continue;
                }

                if (fi.LastWriteTime <= destFi.LastWriteTime)
                {
                    continue;
                }

                fi.CopyTo(destFi.FullName, true);
            }

            // Copy directories recursively
            foreach (DirectoryInfo di in srcDi.GetDirectories())
            {
                CopyTo(di, Path.Combine(destDi.FullName, di.Name));
            }

            return destDi;
        }

        /// <summary>
        /// Delete files and directories in specified directory.
        /// </summary>
        /// <param name="root"></param>
        public static void Clean(this DirectoryInfo root)
        {
            if (!root.Exists)
            {
                return;
            }

            foreach (DirectoryInfo di in root.GetDirectories())
            {
                if (!di.Exists)
                {
                    continue;
                }

                di.Clean();
            }

            foreach (FileInfo fi in root.GetFiles())
            {
                if (!fi.Exists)
                {
                    continue;
                }

                fi.Delete();
            }
        }

        /// <summary>
        /// Returns a new string in which all occurrences of a specified string in the current instance are replaced with another specified string.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="before">The string to be replaced.</param>
        /// <param name="after">The string to replace all occurrences of oldValue.</param>
        /// <param name="count">The max count to replace.</param>
        /// <returns>A string that is equivalent to the current string except that all instances of oldValue are replaced with newValue. If oldValue is not found in the current instance, the method returns the current instance unchanged.</returns>
        public static string Replace(this string str, string before, string after, int count)
        {
            if (count < 1)
            {
                return str;
            }

            int startIndex = 0;
            for (int i = 0; i < count; i++)
            {
                int index = str.IndexOf(before, startIndex);
                if (index == -1)
                {
                    break;
                }

                str = str.Remove(index, before.Length);
                str = str.Insert(index, after);
                startIndex = index + after.Length;
            }

            return str;
        }
    }
}
