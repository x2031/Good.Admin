using Good.Admin.Common.Helper;
using Good.Admin.Common.Primitives;


namespace Good.Admin.Common.DataAccess
{
    public class BaseDBConfig
    {
        /* 之前的单库操作已经删除，如果想要之前的代码，可以查看我的GitHub的历史记录
         * 目前是多库操作，默认加载的是appsettings.json设置为true的第一个db连接。
         */
        public static (List<MutiDBOperate> allDbs, List<MutiDBOperate> slaveDbs) MutiConnectionString => MutiInitConn();
        private static string DifDBConnOfSecurity(params string[] conn)
        {
            foreach (var item in conn)
            {
                try
                {
                    if (File.Exists(item))
                    {
                        return File.ReadAllText(item).Trim();
                    }
                }
                catch (Exception) { }
            }

            return conn[conn.Length - 1];
        }

        /// <summary>
        /// 获取多库链接字符串
        /// </summary>
        /// <returns></returns>

        public static (List<MutiDBOperate>, List<MutiDBOperate>) MutiInitConn()
        {
            var listdatabase = new List<MutiDBOperate>();
            listdatabase = Appsettings.app<MutiDBOperate>("dbs_secrets");
            if (listdatabase == null || listdatabase.Count == 0)
            {
                listdatabase = Appsettings.app<MutiDBOperate>("DBS")
              .Where(i => i.Enabled).ToList();
            }
            //foreach (var i in listdatabase)
            //{
            //    SpecialDbString(i);
            //}
            var listdatabaseSimpleDB = new List<MutiDBOperate>();//单库
            var listdatabaseSlaveDB = new List<MutiDBOperate>();//从库

            // 单库，且不开启读写分离，只保留一个
            if (!Appsettings.app(new string[] { "CQRSEnabled" }).ObjToBool() && !Appsettings.app(new string[] { "MutiDBEnabled" }).ObjToBool())
            {
                if (listdatabase.Count == 1)
                {
                    return (listdatabase, listdatabaseSlaveDB);
                }
                else
                {
                    var dbFirst = listdatabase.FirstOrDefault(d => d.ConnId == Appsettings.app(new string[] { "MainDB" }).ObjToString());
                    if (dbFirst == null)
                    {
                        dbFirst = listdatabase.FirstOrDefault();
                    }

                    listdatabaseSimpleDB.Add(dbFirst);
                    return (listdatabaseSimpleDB, listdatabaseSlaveDB);
                }
            }
            // 读写分离，且必须是单库模式，获取从库
            if (Appsettings.app(new string[] { "CQRSEnabled" }).ObjToBool() && !Appsettings.app(new string[] { "MutiDBEnabled" }).ObjToBool())
            {
                if (listdatabase.Count > 1)
                {
                    listdatabaseSlaveDB = listdatabase.Where(d => d.ConnId != Appsettings.app(new string[] { "MainDB" }).ObjToString()).ToList();
                }
            }
            return (listdatabase, listdatabaseSlaveDB);

        }
    }

    public class MutiDBOperate
    {
        /// <summary>
        /// 连接启用开关
        /// </summary>
        public bool Enabled { get; set; }
        /// <summary>
        /// 连接ID
        /// </summary>
        public string ConnId { get; set; }
        /// <summary>
        /// 从库执行级别，越大越先执行
        /// </summary>
        public int HitRate { get; set; }
        /// <summary>
        /// 连接字符串
        /// </summary>
        public string Connection { get; set; }
        /// <summary>
        /// 数据库类型
        /// </summary>
        public DataBaseType DbType { get; set; }
    }

}
