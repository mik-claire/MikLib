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
    }
}
