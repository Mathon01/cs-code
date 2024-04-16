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
    //     public double Gross_profit_per_unit { get; set; } //利润
    //     public int Total_purchase_quantity { get; set; }  //进货量
    //     public int Total_sales_quantity { get; set; }  //总销量
    //     public int Remaining { get; set; }  //剩余数量
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
    //     public double Gross_profit_per_unit { get; set; } //利润
    //     public int Total_purchase_quantity { get; set; }  //进货量
    //     public int Total_sales_quantity { get; set; }  //总销量
    //     public double Sales {get; set; } //销售额
    //     public int Remaining { get; set; }  //剩余数量
    // }

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
            //Changeddd();
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
                    //Console.WriteLine("程序结束");
                    Environment.Exit(0);
                    //return;  //结束程序
                }
                if (IdentityJudge.IsWorkerOrAdmin(name,Key) == "wrong")
                {
                    Console.Clear();
                    Console.WriteLine("认证错误");
                    continue;
                }
                else if (IdentityJudge.IsWorkerOrAdmin(name,Key) == "admin")
                {
                    Console.Clear();
                    Console.WriteLine("              \\\\欢迎回来管理员////");
                    LogIn logInIt = AddInformation(name);
                    WriteLog(logInIt);
                    Functions.Chose();
                    //进入管理后台
                }
                else if (IdentityJudge.IsWorkerOrAdmin(name,Key) == "worker")
                {
                    Console.Clear();
                    Console.WriteLine("              \\\\欢迎回来收银员////");
                    LogIn logInIt = AddInformation(name);
                    WriteLog(logInIt);
                    WorkerFunctions.FunctionChose(name);
                    //进入收银工作
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("              密钥错误");
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
                return new List<LogIn>(); // 文件不存在，返回空列表
            }
            string json = File.ReadAllText(logFilePath);
            return JsonConvert.DeserializeObject<List<LogIn>>(json);
        }

        static void WriteLog(LogIn inLog)
        {
            List<LogIn> logs = GetLogsFile();
            logs.Add(inLog);
            //写入
            string init = JsonConvert.SerializeObject(logs, Formatting.Indented);
            File.WriteAllText(logFilePath, init, new UTF8Encoding(false));
            //Console.WriteLine("日志写入成功 记得删除");
        }
    
        // public static void Changeddd()
        // {
        //     string originalPath = Path.Combine("..", "..", "..", "Data", "products.json");
        //     string newPath = Path.Combine("..", "..", "..", "Data", "newproducts.json");

        //     // 读取原始产品数据
        //     string originalFile = File.ReadAllText(originalPath, new UTF8Encoding(false));
        //     List<Product> products = JsonConvert.DeserializeObject<List<Product>>(originalFile);
            
        //     // 在此处对产品数据进行修改，并将修改后的数据保存到新的产品列表 newProducts 中
        //     List<NewProduct> newProducts = new List<NewProduct>();
        //     foreach (var product in products)
        //     {
        //         // 对产品数据进行转换或者修改，创建一个新的 NewProduct 对象
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
        //             // 添加销售额属性
        //             Sales = product.Selling_price * product.Total_sales_quantity
        //         };
                
        //         // 将新的产品对象添加到 newProducts 列表中
        //         newProducts.Add(newProduct);
        //     }

        //     // 将新的产品列表序列化为 JSON 字符串，并将其写入到新的文件中
        //     string newProductsJson = JsonConvert.SerializeObject(newProducts, Formatting.Indented);
        //     File.WriteAllText(newPath, newProductsJson, new UTF8Encoding(false));
        // }

    }
    
}