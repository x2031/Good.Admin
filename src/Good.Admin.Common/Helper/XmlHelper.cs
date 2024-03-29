﻿using System.Xml.Serialization;

namespace Good.Admin.Common
{
    /// <summary>
    /// XML文档操作帮助类
    /// </summary>
    public class XmlHelper
    {
        /// <summary>
        /// 序列化为XML字符串
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public static string Serialize(object obj)
        {
            var type = obj.GetType();
            var Stream = new MemoryStream();
            var xml = new XmlSerializer(type);
            try
            {
                //序列化对象
                xml.Serialize(Stream, obj);
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            Stream.Position = 0;
            var sr = new StreamReader(Stream);
            var str = sr.ReadToEnd();

            sr.Dispose();
            Stream.Dispose();

            return str;
        }
    }
}
