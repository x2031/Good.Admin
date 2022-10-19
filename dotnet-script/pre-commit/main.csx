#r "nuget:Newtonsoft.Json, 13.0.1"

using System;
using System.Text;
using System.IO;
using Newtonsoft.Json.Linq;

string currentdir= Environment.CurrentDirectory;
string appsettingspath=currentdir+@"\src\Good.Admin.API\appsettings.json";
Console.WriteLine("当前工作目录:"+currentdir);
Console.WriteLine("配置文件路径:"+appsettingspath);
var isexist= File.Exists(appsettingspath);
if (isexist)
{
    //替换密钥
    var appsettings=File.ReadAllText(appsettingspath);
    if(string.IsNullOrEmpty(appsettings)){
        return;
    };

    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine("数据库连接字符串已替换");



    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("现在的数据库字符串是:1231231231312");
    Console.ResetColor();
}
else
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("配置文件不存在");
    Console.ResetColor();
}
