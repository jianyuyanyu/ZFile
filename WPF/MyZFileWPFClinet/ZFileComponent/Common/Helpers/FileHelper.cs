using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Component.Common.Helpers
{

    public enum FileSpliEnmu
    {
        Byte,
        KB,
        MB,
        GB
    }

   public class FileHelper
    {
        #region 分割文件
        /// <summary>
        /// 分割文件
        /// </summary>
        /// <param name="Flag">分割单位</param>
        /// <param name="intFlag">分割大小</param>

        /// <param name="strFile">要分割的文件</param>
        /// <param name="PBar">进度条显示</param>
        public static List<byte[]> SplitFile(FileSpliEnmu Flag, int intFlag, string strFile)
        {
            int iFileSize = 0;
            List<byte[]> ByteList = new List<byte[]>();
            //根据选择来设定分割的小文件的大小
            switch (Flag)
            {
                case FileSpliEnmu.Byte:
                    iFileSize = intFlag;
                    break;
                case FileSpliEnmu.KB:
                    iFileSize = intFlag * 1024;
                    break;
                case FileSpliEnmu.MB:
                    iFileSize = intFlag * 1024 * 1024;
                    break;
                case FileSpliEnmu.GB:
                    iFileSize = intFlag * 1024 * 1024 * 1024;
                    break;
            }
            //以文件的全路径对应的字符串和文件打开模式来初始化FileStream文件流实例
            FileStream SplitFileStream = new FileStream(strFile, FileMode.Open);
            //以FileStream文件流来初始化BinaryReader文件阅读器
            BinaryReader SplitFileReader = new BinaryReader(SplitFileStream);
            //每次分割读取的最大数据
            byte[] TempBytes;
            //小文件总数
            int iFileCount = (int)(SplitFileStream.Length / iFileSize);      
            if (SplitFileStream.Length % iFileSize != 0) iFileCount++;
            string[] TempExtra = strFile.Split('.');
            //循环将大文件分割成多个小文件
            for (int i = 0; i < iFileCount; i++)
            {
                TempBytes = SplitFileReader.ReadBytes(iFileSize);
                ByteList.Add(TempBytes);
            }
            //关闭大文件阅读器
            SplitFileReader.Close();
            SplitFileStream.Close();
            File.Delete(strFile);
            return ByteList;
        }
        #endregion

        public static int GetFileSplitCount(FileSpliEnmu Flag, int intFlag, string strFile)
        {
            int iFileSize = 0;
            //根据选择来设定分割的小文件的大小
            switch (Flag)
            {
                case FileSpliEnmu.Byte:
                    iFileSize = intFlag;
                    break;
                case FileSpliEnmu.KB:
                    iFileSize = intFlag * 1024;
                    break;
                case FileSpliEnmu.MB:
                    iFileSize = intFlag * 1024 * 1024;
                    break;
                case FileSpliEnmu.GB:
                    iFileSize = intFlag * 1024 * 1024 * 1024;
                    break;
            }
            //以文件的全路径对应的字符串和文件打开模式来初始化FileStream文件流实例
            FileStream SplitFileStream = new FileStream(strFile, FileMode.Open);
            int iFileCount = (int)(SplitFileStream.Length / iFileSize);
            if (SplitFileStream.Length % iFileSize != 0) iFileCount++;
            SplitFileStream.Close();
            return iFileCount;
        }
    }




}


