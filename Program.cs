using Judge;
using Admin;
using Worker;
using Newtonsoft.Json;
using System.Text;


namespace mainProcess
{
    public class LogIn  //登录格式
    {
        public DateTime Time {get; set;}
        public string Operating {get; set;}
        public string WorkerId{get; set;}
    }
    public static class Program
    {
        static void Main()
        {
            Chose();
        }

        public static void Chose()
        {
            
            string name;
            string Key;
            do
            {
                Console.WriteLine("┌────────────────────────────────────────────────────┐");
                Console.WriteLine("│        请输入身份:                                 │");
                Console.WriteLine("│        请输入密钥:                                 │");
                Console.WriteLine("│        over结束程序:                               │");
                Console.WriteLine("└────────────────────────────────────────────────────┘");
                name = Console.ReadLine() ?? "";
                Key = Console.ReadLine() ?? "";
                if (name.Equals("over",StringComparison.CurrentCultureIgnoreCase) || Key.Equals("over",StringComparison.CurrentCultureIgnoreCase))
                {
                    Console.WriteLine("程序结束");
                    Environment.Exit(0);
                    //return;  //结束程序
                }
                if (IdentityJudge.IsWorkerOrAdmin(name,Key) == "wrong")
                {
                    Console.WriteLine("认证错误");
                    Console.Clear();
                    continue;
                }
                else if (IdentityJudge.IsWorkerOrAdmin(name,Key) == "admin")
                {
                    Console.Clear();
                    Console.WriteLine("\\\\欢迎回来管理员////");
                    LogIn logInIt = AddInformation(name);
                    WriteLog(logInIt);
                    Functions.Chose();
                    //进入管理后台
                }
                else if (IdentityJudge.IsWorkerOrAdmin(name,Key) == "worker")
                {
                    Console.Clear();
                    Console.WriteLine("\\\\欢迎回来收银员////");
                    LogIn logInIt = AddInformation(name);
                    WriteLog(logInIt);
                    WorkerFunctions.FunctionChose(name);
                    //进入收银工作
                }
                else
                {
                    Console.WriteLine("密钥错误");
                    Console.Clear();
                    continue;
                }
            } while (true);
        }

        public static LogIn AddInformation(string name)
        {
            LogIn logIn = new LogIn
            {
                Time = DateTime.Now,
                Operating = "LogIn",
                WorkerId = name
            };
            return logIn;
        }

        public static string logFilePath = Path.Combine("..", "..", "..", "Data", "log", "login.json");
        public static List<LogIn> GetLogsFile()
        {
            //logFilePathReturn = logFilePath;
            if (!File.Exists(logFilePath))
            {
                return new List<LogIn>(); // 文件不存在，返回空列表
            }
            string json = File.ReadAllText(logFilePath);
            return JsonConvert.DeserializeObject<List<LogIn>>(json);
        }

        public static void WriteLog(LogIn inLog)
        {
            List<LogIn> logs = GetLogsFile();
            logs.Add(inLog);
            //写入
            string init = JsonConvert.SerializeObject(logs, Formatting.Indented);
            File.WriteAllText(logFilePath, init, new UTF8Encoding(false));
            //Console.WriteLine("日志写入成功 记得删除");
        }
    }
    
}