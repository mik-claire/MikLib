using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows.Forms;

namespace MikLib
{
    public static class MikLib
    {
        /// <summary>
        /// エラーメッセージを表示
        /// </summary>
        /// <param name="ex"></param>
        public static void ShowError(Exception ex)
        {
            string message = string.Format("{0}", ex.Message);

#if DEBUG
            message += string.Format("\n{0}", ex.StackTrace);
#endif
            MessageBox.Show(message, "例外が発生しました。",
               MessageBoxButtons.OK,
               MessageBoxIcon.Error);
        }

        /// <summary>
        /// DataTableのDeepCopyCloneを取得する
        /// </summary>
        /// <param name="target">コピー元のDataTable</param>
        /// <returns>クローン</returns>
        public static DataTable DeepCopyDataTable(DataTable target)
        {
            DataTable clone = null;
            using (MemoryStream ms = new MemoryStream())
            {
                // コピー元オブジェクトをシリアライズ
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(ms, target);
                ms.Position = 0;

                // シリアライズデータをコピー先オブジェクトにデシリアライズ
                clone = (DataTable)formatter.Deserialize(ms);
            }
            
            return clone;
        }
    }
}
