using Judge;
using Admin;
using Worker;
using Newtonsoft.Json;
using System.Text;


namespace mainProcess
{
    // public class Product
    // {
    //     public string Id { get; set; }
    //     public string Name { get; set; }
    //     public DateTime Production_date { get; set; }
    //     public DateTime Expiration_date { get; set; }
    //     public string Category { get; set; }
    //     public double Purchase_price { get; set; }
    //     public double Selling_price { get; set; }
    //     public double Gross_profit_per_unit { get; set; } //����
    //     public int Total_purchase_quantity { get; set; }  //������
    //     public int Total_sales_quantity { get; set; }  //������
    //     public int Remaining { get; set; }  //ʣ������
    // }

    // public class NewProduct
    // {
    //     public string Id { get; set; }
    //     public string Name { get; set; }
    //     public DateTime Production_date { get; set; }
    //     public DateTime Expiration_date { get; set; }
    //     public string Category { get; set; }
    //     public double Purchase_price { get; set; }
    //     public double Selling_price { get; set; }
    //     public double Gross_profit_per_unit { get; set; } //����
    //     public int Total_purchase_quantity { get; set; }  //������
    //     public int Total_sales_quantity { get; set; }  //������
    //     public double Sales {get; set; } //���۶�
    //     public int Remaining { get; set; }  //ʣ������
    // }

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
            //Changeddd();
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
                    //Console.WriteLine("�������");
                    Environment.Exit(0);
                    //return;  //��������
                }
                if (IdentityJudge.IsWorkerOrAdmin(name,Key) == "wrong")
                {
                    Console.Clear();
                    Console.WriteLine("��֤����");
                    continue;
                }
                else if (IdentityJudge.IsWorkerOrAdmin(name,Key) == "admin")
                {
                    Console.Clear();
                    Console.WriteLine("              \\\\��ӭ��������Ա////");
                    LogIn logInIt = AddInformation(name);
                    WriteLog(logInIt);
                    Functions.Chose();
                    //��������̨
                }
                else if (IdentityJudge.IsWorkerOrAdmin(name,Key) == "worker")
                {
                    Console.Clear();
                    Console.WriteLine("              \\\\��ӭ��������Ա////");
                    LogIn logInIt = AddInformation(name);
                    WriteLog(logInIt);
                    WorkerFunctions.FunctionChose(name);
                    //������������
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("              ��Կ����");
                    continue;
                }
            } while (true);
        }

        static LogIn AddInformation(string name)
        {
            LogIn logIn = new LogIn
            {
                Time = DateTime.Now,
                Operating = "LogIn",
                WorkerId = name
            };
            return logIn;
        }

        static string logFilePath = Path.Combine("..", "..", "..", "Data", "log", "login.json");
        static List<LogIn> GetLogsFile()
        {
            if (!File.Exists(logFilePath))
            {
                return new List<LogIn>(); // �ļ������ڣ����ؿ��б�
            }
            string json = File.ReadAllText(logFilePath);
            return JsonConvert.DeserializeObject<List<LogIn>>(json);
        }

        static void WriteLog(LogIn inLog)
        {
            List<LogIn> logs = GetLogsFile();
            logs.Add(inLog);
            //д��
            string init = JsonConvert.SerializeObject(logs, Formatting.Indented);
            File.WriteAllText(logFilePath, init, new UTF8Encoding(false));
            //Console.WriteLine("��־д��ɹ� �ǵ�ɾ��");
        }
    
        // public static void Changeddd()
        // {
        //     string originalPath = Path.Combine("..", "..", "..", "Data", "products.json");
        //     string newPath = Path.Combine("..", "..", "..", "Data", "newproducts.json");

        //     // ��ȡԭʼ��Ʒ����
        //     string originalFile = File.ReadAllText(originalPath, new UTF8Encoding(false));
        //     List<Product> products = JsonConvert.DeserializeObject<List<Product>>(originalFile);
            
        //     // �ڴ˴��Բ�Ʒ���ݽ����޸ģ������޸ĺ�����ݱ��浽�µĲ�Ʒ�б� newProducts ��
        //     List<NewProduct> newProducts = new List<NewProduct>();
        //     foreach (var product in products)
        //     {
        //         // �Բ�Ʒ���ݽ���ת�������޸ģ�����һ���µ� NewProduct ����
        //         NewProduct newProduct = new NewProduct
        //         {
        //             Id = product.Id,
        //             Name = product.Name,
        //             Production_date = product.Production_date,
        //             Expiration_date = product.Expiration_date,
        //             Category = product.Category,
        //             Purchase_price = product.Purchase_price,
        //             Selling_price = product.Selling_price,
        //             Gross_profit_per_unit = product.Gross_profit_per_unit,
        //             Total_purchase_quantity = product.Total_purchase_quantity,
        //             Total_sales_quantity = product.Total_sales_quantity,

        //             Remaining = product.Remaining,
        //             // ������۶�����
        //             Sales = product.Selling_price * product.Total_sales_quantity
        //         };
                
        //         // ���µĲ�Ʒ������ӵ� newProducts �б���
        //         newProducts.Add(newProduct);
        //     }

        //     // ���µĲ�Ʒ�б����л�Ϊ JSON �ַ�����������д�뵽�µ��ļ���
        //     string newProductsJson = JsonConvert.SerializeObject(newProducts, Formatting.Indented);
        //     File.WriteAllText(newPath, newProductsJson, new UTF8Encoding(false));
        // }

    }
    
}