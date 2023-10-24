using ICSharpCode.SharpZipLib.Zip;

namespace Good.Admin.Common
{
    /// <summary>
    /// 文件压缩帮助类
    /// </summary>
    public class FileZipHelper
    {
        /// <summary>
        /// 压缩一个文件
        /// </summary>
        /// <param name="file">文件信息</param>
        /// <returns></returns>
        public static byte[] ZipFile(FileEntry file)
        {
            return ZipFile(new List<FileEntry> { file });
        }

        /// <summary>
        /// 压缩多个文件
        /// </summary>
        /// <param name="files">文件信息列表</param>
        /// <returns></returns>
        public static byte[] ZipFile(List<FileEntry> files)
        {
            using (var ms = new MemoryStream())
            {
                using (var zipStream = new ZipOutputStream(ms))
                {
                    files.ForEach(aFile =>
                    {
                        var fileBytes = aFile.FileBytes;
                        var entry = new ZipEntry(aFile.FileName)
                        {
                            DateTime = DateTime.Now,
                            IsUnicodeText = true
                        };
                        zipStream.PutNextEntry(entry);
                        zipStream.Write(fileBytes, 0, fileBytes.Length);
                        zipStream.CloseEntry();
                    });

                    zipStream.IsStreamOwner = false;
                    zipStream.Finish();
                    zipStream.Close();
                    ms.Position = 0;

                    return ms.ToArray();
                }
            }
        }
    }
}
