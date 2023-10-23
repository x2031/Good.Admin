using Good.Admin.Common.DataAccess;
using SqlSugar;

namespace Good.Admin.API
{
    public class MyContext
    {

        private static MutiDBOperate connectObject => GetMainConnectionDb();

        public static string ConnId = connectObject.ConnId;

        /// <summary>
        /// 连接字符串         
        /// </summary>
        public static MutiDBOperate GetMainConnectionDb()
        {
            var mainConnetctDb = BaseDBConfig.MutiConnectionString.allDbs.Find(x => x.ConnId == MainDb.CurrentDbConnId);
            if (BaseDBConfig.MutiConnectionString.allDbs.Count > 0)
            {
                if (mainConnetctDb == null)
                {
                    mainConnetctDb = BaseDBConfig.MutiConnectionString.allDbs[0];
                }
            }
            else
            {
                throw new Exception("请确保appsettigns.json中配置连接字符串,并设置Enabled为true;");
            }

            return mainConnetctDb;
        }
        /// <summary>
        /// 连接字符串         
        /// </summary>
        public static string ConnectionString { get; set; } = connectObject.Connection;
        /// <summary>
        /// 数据库类型         
        /// </summary>
        public static DbType DbType { get; set; } = (DbType)connectObject.DbType;
        /// <summary>
        /// 数据连接对象         
        /// </summary>
        public SqlSugarScope Db { get; private set; }

        /// <summary>
        /// 功能描述:构造函数     
        /// </summary>
        public MyContext(ISqlSugarClient sqlSugarClient)
        {
            if (string.IsNullOrEmpty(ConnectionString))
                throw new ArgumentNullException("数据库连接字符串为空");

            Db = sqlSugarClient as SqlSugarScope;

        }


        #region 实例方法
        /// <summary>
        /// 功能描述:获取数据库处理对象        
        /// </summary>
        /// <returns>返回值</returns>
        public SimpleClient<T> GetEntityDB<T>() where T : class, new()
        {
            return new SimpleClient<T>(Db);
        }
        /// <summary>
        /// 功能描述:获取数据库处理对象        
        /// </summary>
        /// <param name="db">db</param>
        /// <returns>返回值</returns>
        //public SimpleClient<T> GetEntityDB<T>(SqlSugarClient db) where T : class, new()
        //{
        //    return new SimpleClient<T>(db);
        //}



        #endregion

        #region 根据数据库生成实体
        public void CreateEntityByTable()
        {
            Db.DbFirst.IsCreateAttribute().CreateClassFile(@"D:\Codes\Good.Admin\src\Good.Admin.Entity\Base_Manage", "Good.Admin.Entity");
        }
        #endregion


        #region 根据实体类生成数据库表
        /// <summary>
        /// 功能描述:根据实体类生成数据库表        
        /// </summary>
        /// <param name="blnBackupTable">是否备份表</param>
        /// <param name="lstEntitys">指定的实体</param>
        public void CreateTableByEntity<T>(bool blnBackupTable, params T[] lstEntitys) where T : class, new()
        {
            Type[] lstTypes = null;
            if (lstEntitys != null)
            {
                lstTypes = new Type[lstEntitys.Length];
                for (int i = 0; i < lstEntitys.Length; i++)
                {
                    T t = lstEntitys[i];
                    lstTypes[i] = typeof(T);
                }
            }
            CreateTableByEntity(blnBackupTable, lstTypes);
        }

        /// <summary>
        /// 功能描述:根据实体类生成数据库表        
        /// </summary>
        /// <param name="blnBackupTable">是否备份表</param>
        /// <param name="lstEntitys">指定的实体</param>
        public void CreateTableByEntity(bool blnBackupTable, params Type[] lstEntitys)
        {
            if (blnBackupTable)
            {
                Db.CodeFirst.BackupTable().InitTables(lstEntitys); //change entity backupTable            
            }
            else
            {
                Db.CodeFirst.InitTables(lstEntitys);
            }
        }
        #endregion


        #region 静态方法

        ///// <summary>
        ///// 功能描述:获得一个DbContext        
        ///// </summary>
        ///// <returns></returns>
        //public static MyContext GetDbContext()
        //{
        //    return new MyContext();
        //}

        /// <summary>
        /// 功能描述:设置初始化参数        
        /// </summary>
        /// <param name="strConnectionString">连接字符串</param>
        /// <param name="enmDbType">数据库类型</param>
        public static void Init(string strConnectionString, DbType enmDbType = SqlSugar.DbType.SqlServer)
        {
            ConnectionString = strConnectionString;
            DbType = enmDbType;
        }

        /// <summary>
        /// 功能描述:创建一个链接配置        
        /// </summary>
        /// <param name="blnIsAutoCloseConnection">是否自动关闭连接</param>
        /// <param name="blnIsShardSameThread">是否夸类事务</param>
        /// <returns>ConnectionConfig</returns>
        public static ConnectionConfig GetConnectionConfig(bool blnIsAutoCloseConnection = true, bool blnIsShardSameThread = false)
        {
            ConnectionConfig config = new ConnectionConfig()
            {
                ConnectionString = ConnectionString,
                DbType = DbType,
                IsAutoCloseConnection = blnIsAutoCloseConnection,
                ConfigureExternalServices = new ConfigureExternalServices()
                {
                    //DataInfoCacheService = new HttpRuntimeCache()
                },
                //IsShardSameThread = blnIsShardSameThread
            };
            return config;
        }

        /// <summary>
        /// 功能描述:获取一个自定义的DB        
        /// </summary>
        /// <param name="config">config</param>
        /// <returns>返回值</returns>
        public static SqlSugarScope GetCustomDB(ConnectionConfig config)
        {
            return new SqlSugarScope(config);
        }
        /// <summary>
        /// 功能描述:获取一个自定义的数据库处理对象        
        /// </summary>
        /// <param name="sugarClient">sugarClient</param>
        /// <returns>返回值</returns>
        public static SimpleClient<T> GetCustomEntityDB<T>(SqlSugarScope sugarClient) where T : class, new()
        {
            return new SimpleClient<T>(sugarClient);
        }
        /// <summary>
        /// 功能描述:获取一个自定义的数据库处理对象        
        /// </summary>
        /// <param name="config">config</param>
        /// <returns>返回值</returns>
        public static SimpleClient<T> GetCustomEntityDB<T>(ConnectionConfig config) where T : class, new()
        {
            SqlSugarScope sugarClient = GetCustomDB(config);
            return GetCustomEntityDB<T>(sugarClient);
        }
        #endregion
    }

}
