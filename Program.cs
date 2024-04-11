using Judge;
using Admin;
using Worker;
using Newtonsoft.Json;
using System.Text;


namespace mainProcess
{
    public class LogIn  //��¼��ʽ
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
                Console.WriteLine("������������������������������������������������������������������������������������������������������������");
                Console.WriteLine("��        ���������:                                 ��");
                Console.WriteLine("��        ��������Կ:                                 ��");
                Console.WriteLine("��        over��������:                               ��");
                Console.WriteLine("������������������������������������������������������������������������������������������������������������");
                name = Console.ReadLine() ?? "";
                Key = Console.ReadLine() ?? "";
                if (name.Equals("over",StringComparison.CurrentCultureIgnoreCase) || Key.Equals("over",StringComparison.CurrentCultureIgnoreCase))
                {
                    Console.WriteLine("�������");
                    Environment.Exit(0);
                    //return;  //��������
                }
                if (IdentityJudge.IsWorkerOrAdmin(name,Key) == "wrong")
                {
                    Console.WriteLine("��֤����");
                    Console.Clear();
                    continue;
                }
                else if (IdentityJudge.IsWorkerOrAdmin(name,Key) == "admin")
                {
                    Console.Clear();
                    Console.WriteLine("\\\\��ӭ��������Ա////");
                    LogIn logInIt = AddInformation(name);
                    WriteLog(logInIt);
                    Functions.Chose();
                    //��������̨
                }
                else if (IdentityJudge.IsWorkerOrAdmin(name,Key) == "worker")
                {
                    Console.Clear();
                    Console.WriteLine("\\\\��ӭ��������Ա////");
                    LogIn logInIt = AddInformation(name);
                    WriteLog(logInIt);
                    WorkerFunctions.FunctionChose(name);
                    //������������
                }
                else
                {
                    Console.WriteLine("��Կ����");
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
                return new List<LogIn>(); // �ļ������ڣ����ؿ��б�
            }
            string json = File.ReadAllText(logFilePath);
            return JsonConvert.DeserializeObject<List<LogIn>>(json);
        }

        public static void WriteLog(LogIn inLog)
        {
            List<LogIn> logs = GetLogsFile();
            logs.Add(inLog);
            //д��
            string init = JsonConvert.SerializeObject(logs, Formatting.Indented);
            File.WriteAllText(logFilePath, init, new UTF8Encoding(false));
            //Console.WriteLine("��־д��ɹ� �ǵ�ɾ��");
        }
    }
    
}