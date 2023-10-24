using SqlSugar;

namespace Good.Admin.Common
{
    public class SqlsugarHelper
    {

        public static string GetWholeSql(SugarParameter[] paramArr, string sql)
        {
            foreach (var param in paramArr)
            {
                sql = sql.Replace(param.ParameterName, param.Value.ObjToString());
            }
            return sql;
        }

        public static string GetParas(SugarParameter[] pars)
        {
            string key = "【SQL参数】：";
            foreach (var param in pars)
            {
                key += $"{param.ParameterName}:{param.Value}\n";
            }

            return key;
        }
    }
}
