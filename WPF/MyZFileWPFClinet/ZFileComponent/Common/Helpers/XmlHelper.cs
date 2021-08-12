using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Component.Common.Helpers
{
    public class XmlHelper
    {


        /// <summary>
        /// 将自定义对象序列化为XML文件保存
        /// </summary>
        /// <param name="myObject">自定义对象实体</param>
        /// <param name="FilePath">地址</param>
        public static bool SerializeToXmlFile<T>(T myObject, string FilePath)
        {
            TextWriter writer = null;
            try
            {
                if (myObject == null)
                    throw new ArgumentNullException("XMLObjet is null");
                using (writer = new StreamWriter(FilePath))
                {
                    XmlSerializer serializer = new XmlSerializer(myObject.GetType());
                    XmlWriterSettings settings = new XmlWriterSettings();
                    serializer.Serialize(writer, myObject);
                    writer.Close();
                    return true;
                }
            }
            catch (Exception)
            {


                return false;
            }
            finally
            {
                if (writer != null)
                {

                    writer.Dispose();
                }

            }
        }

        /// <summary>
        /// 将XML转换成实体对象
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="strXML">XML</param>
        public static T SerializerXMLToObject<T>(string strXML) where T : class
        {
            StreamReader sr = null;

            if (!File.Exists(strXML))
            {
                throw new ArgumentNullException("path is null");
            }
            try
            {
                using (sr = new StreamReader(strXML))
                {
                    XmlSerializer xs = new XmlSerializer(typeof(T));
                    T obj = xs.Deserialize(sr) as T;
                    sr.Close();
                    return obj;
                }
            }
            catch (Exception)
            {

                return null;
            }
            finally
            {
                if (sr != null)
                {

                    sr.Dispose();
                }
            }
        }
    }
}
