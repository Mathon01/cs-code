using Newtonsoft.Json;
using System.Text;
using System.Security.Cryptography;
// using System.Runtime.InteropServices;
// using System.Text.Unicode;


namespace Worker
{
    public class LogProduct  //产品进出格式
    {
        public DateTime Time {get; set;}
        public string Id{get; set;}
        public int Nums{get; set;}
        public string WorkerId{get; set;}
        public string ConstomerId{get; set;}
    }
    public class LogAddProduct  //进出货物
    {
        public DateTime Intime {get; set;} //进货时间
        public string WorkerName {get; set;} //操作人员
        public string Id { get; set; }
        public string ProductName { get; set; }
        public DateTime Production_date { get; set; }
        public DateTime Expiration_date { get; set; }
        public double Purchase_price { get; set; }
        public double Selling_price { get; set; }
        public int quantity { get; set; }  //进货量
    }
    public class Property
    {
        public string Md5 {get; set;}
        public int Score {get; set;}
    }
    public class Product
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime Production_date { get; set; }
        public DateTime Expiration_date { get; set; }
        public string Category { get; set; }
        public double Purchase_price { get; set; }
        public double Selling_price { get; set; }
        public double Gross_profit_per_unit { get; set; } //利润
        public int Total_purchase_quantity { get; set; }  //进货量
        public int Total_sales_quantity { get; set; }  //总销量
        public int Remaining { get; set; }  //剩余数量
    }
    
    static class WorkerFunctions
    {
        public static void FunctionChose(string operatingIdentity)
        {
            Console.WriteLine("请选择您要执行的功能");
            string chose;
            do
            {   
                Console.WriteLine("┌────────────────────────────────────────────────────┐");
                Console.WriteLine("│        0.返回上一级            现在是：            │");
                Console.WriteLine("│        1.结账                {0}            │",DateTime.Now.ToString("yyyy-MM-dd"));
                Console.WriteLine("│        2.进货                 {0}             │",DateTime.Now.ToString("HH:mm:ss"));
                Console.WriteLine("│        3.下班               祝您工作愉快           │");
                Console.WriteLine("└────────────────────────────────────────────────────┘");

                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                if (keyInfo.Key == ConsoleKey.D0)
                {
                    Console.Clear();
                    return;
                }
                else if (keyInfo.Key == ConsoleKey.D1)
                {
                    Console.Clear();
                    AddBill(operatingIdentity);
                    continue;
                }
                else if (keyInfo.Key == ConsoleKey.D2)
                {
                    Console.Clear();
                    AddProduct(operatingIdentity);
                    continue;
                }
                else if (keyInfo.Key == ConsoleKey.D3)
                {
                    Console.Clear();
                    Environment.Exit(0);
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("输入非法 重新输入");
                }
            } while (true);

        }
        static Dictionary<string, int> bill= new Dictionary<string, int>() ;

        static string ProductsFilePath = Path.Combine("..", "..", "..", "Data", "Products.json");
        //商品数据
        static string tmp_jsonDate = File.ReadAllText(ProductsFilePath ,new UTF8Encoding(false));
        static List<Product> have = JsonConvert.DeserializeObject<List<Product>>(tmp_jsonDate);
        
        static void AddBill(string operatingIdentity)
        {
            
            do
            {
                Console.WriteLine("结束请输入: over  结账请输入: price");
                Console.Write("请输入产品ID:");
                string input = Console.ReadLine() ?? "";
                
                if (input.Equals("over",StringComparison.CurrentCultureIgnoreCase))
                {
                    bill.Clear();
                    Console.Clear();
                    return;
                }
                if (input.Equals("price",StringComparison.CurrentCultureIgnoreCase))
                {
                    TotalPrice(bill, operatingIdentity);
                    bill.Clear();
                    Console.Clear();
                    continue;
                }
                int productId;
                if (!int.TryParse(input,out productId)) //int检测
                {
                    Console.Write("ID不合法:");
                    Console.Clear();
                    continue;
                }
                //商品存在检测
                if (InOrOut(input))
                {
                    if (bill.ContainsKey(input)) //存在
                    {
                        bill[input]++;  
                        Console.WriteLine("账单已添加");
                        Console.WriteLine("------------------------------------------------------");
                        continue;
                    }
                    else  
                    {
                        bill.Add(input,1); 
                        Console.WriteLine("账单已添加");
                        Console.WriteLine("------------------------------------------------------");
                        continue;
                    }
                }
                else
                {
                    Console.WriteLine("当前商品Id未录入,请联系工作人员//此件商品未计入账单");
                    Console.WriteLine("------------------------------------------------------");
                }
                
            } while (true);

        }

        //Vip客户数据文件路径
        static string CostomerFilePath = Path.Combine("..", "..", "..", "Data", "costomer.json");
        

        public static void TotalPrice(Dictionary<string, int> bill, string operatingIdentity)
        {
            double Discount = 1;  //折扣
            string isVip = null;    
            do
            {
                Console.WriteLine("请输入VIP_ID: ,办理请输入y,没有请输入n:");
                isVip = Console.ReadLine() ?? "";
                if (isVip.Equals("y",StringComparison.CurrentCultureIgnoreCase))
                {
                    CreatVipCostomer();
                    continue;
                }
                else if (isVip.Equals("n",StringComparison.CurrentCultureIgnoreCase))
                {
                    break;
                }
                else if (IsVip(isVip ,out Discount))
                {
                    break;
                }
                else 
                {
                    Console.WriteLine("输入有误或不存在 重新输入:");
                }
    
            } while (true); 
            
            double sum = 0;
            Console.WriteLine("──────────────────────────────────────────────────────");
            Console.WriteLine();
            Console.WriteLine("           您的账单");
            Console.WriteLine("--------------------------------");
            Console.WriteLine("ID      名称      单价      数量");

            string changeNumOfProduct = File.ReadAllText(ProductsFilePath ,new UTF8Encoding(false));
            List<Product> change = JsonConvert.DeserializeObject<List<Product>>(changeNumOfProduct);
            
            LogProduct writeLog = new LogProduct();
            foreach (var item in bill)
            {
                string productId = item.Key; // 查询 ID
                int quantity = item.Value;   // 获取数量

                
                Product product = change.Find(p => p.Id.Equals(productId, StringComparison.CurrentCultureIgnoreCase));

                if (product != null)
                {
                    product.Total_sales_quantity +=quantity; //添加销量
                    product.Remaining -= quantity;  //减少库存
                    double totalPriceForProduct = product.Selling_price * quantity;  //计算单价
                    sum += totalPriceForProduct;  //计算总价
                    // 写进日志
                    writeLog.Time = DateTime.Now;
                    writeLog.Id = product.Id;
                    writeLog.Nums = quantity;
                    writeLog.WorkerId = operatingIdentity;
                    writeLog.ConstomerId = isVip;

                    Log.WriteLog(writeLog);//  日志写入

                    Console.Write($"{productId,-8}"); // ID 列宽度为 6，左对齐
                    
                    if (product.Name.Length==2)
                        Console.Write($"{product.Name,-8}"); 
                    else if (product.Name.Length==3)
                        Console.Write($"{product.Name,-7}"); 
                    else if (product.Name.Length==4)
                        Console.Write($"{product.Name,-6}"); 
                    else if (product.Name.Length==5)
                        Console.Write($"{product.Name,-5}");
                    else if (product.Name.Length==6)
                        Console.Write($"{product.Name,-4}");

                    Console.Write($"{product.Selling_price.ToString("C"),-10}"); // 单价格式化为货币，列宽度为 8，左对齐
                    Console.WriteLine($"{quantity,3}"); // 数量列宽度为 6，左对齐
                }
                
            }
            string updatedJson = JsonConvert.SerializeObject(change, Formatting.Indented);
            File.WriteAllText(ProductsFilePath, updatedJson, new UTF8Encoding(false));
            Console.WriteLine("--------------------------------");
            // 结算
            if (Discount!=1)
            {
                Console.WriteLine("您好VIP顾客:");
                Console.WriteLine(string.Format("您的折扣:{0,23:P}",Discount));
            }
            AddScore(Encrypt.Md5(isVip),(int)sum);
            Console.WriteLine("应付：{0,26:F2}", sum);
            Console.WriteLine("实付：{0,26:F2}",sum*Discount);
            Console.WriteLine("--------------------------------");
            Console.WriteLine();
            return;
        }
        
        //添加商品
        public static void AddProduct(string operatingIdentity)
        {
            string input="y";
            DateTime outtime;
            double price;
            int num;
            List<Product> add = new List<Product>(); //换位置!!!!!!!!!!!!!!!!!!!!!!!

            if (File.Exists(ProductsFilePath))
            {
                string jsonExisting = File.ReadAllText(ProductsFilePath ,new UTF8Encoding(false));
                add = JsonConvert.DeserializeObject<List<Product>>(jsonExisting);
            }
            do
            {
                Product addproduct = new Product();
                Console.WriteLine("请输入产品信息:");
                Console.Write("Id(四位长度):"); //已有库存编号检测 也可以不加 毕竟实际生活里有两个相同编号的产品不可能
                while (true)
                {
                    addproduct.Id = Console.ReadLine() ?? "";
                    if (int.TryParse(addproduct.Id,out int tmp) && addproduct.Id.Length == 4)
                        break;
                    else
                        Console.Write("输入不合法 重新输入Id:");
                    
                }
                Console.Write("请输入产品名称:");
                addproduct.Name = Console.ReadLine() ?? "";

                while (true)
                {
                    Console.Write("请输入产品生产日期(yyyy-mm-dd):");
                    input = Console.ReadLine() ?? "";
                    if (DateTime.TryParse(input,out outtime))
                    {
                        if (DateTime.Compare(outtime,DateTime.Now)<=0)
                        {
                            addproduct.Production_date = outtime;
                            break;
                        }
                        else
                        {
                            Console.WriteLine("输入的日期必须早于或等于当前系统日期,请重新输入!");
                            continue;
                        }
                    }
                    else
                        Console.WriteLine("输入的日期格式不正确,请重新输入!");
                }
                    
                while (true)
                {
                    Console.Write("请输入过期时间 (yyyy-MM-dd):");
                    input = Console.ReadLine() ?? "";
                    if (DateTime.TryParse(input,out outtime))
                    {
                        if (DateTime.Compare(outtime,addproduct.Production_date) > 0)
                        {
                            addproduct.Production_date = outtime;
                            break;
                        }
                        else
                        {
                            Console.WriteLine("输入的日期必须晚于生产日期,请重新输入!");
                            continue;
                        }
                    }
                    else
                        Console.WriteLine("输入的日期格式不正确,请重新输入!");
                }
                    
                Console.Write("请输入产品种类: ");
                addproduct.Category = Console.ReadLine() ?? "";

                while (true)
                {
                    Console.Write("请输入产品进价: ");
                    if (double.TryParse(Console.ReadLine(), out price))
                    {
                        addproduct.Purchase_price = price;
                        break;
                    }
                    else
                    {
                        Console.WriteLine("输入非法!重新输入!");
                        continue;
                    }
                }
                    
                while (true)
                {
                    Console.Write("请输入产品售价: ");
                    if (double.TryParse(Console.ReadLine(), out price))
                    {
                        addproduct.Selling_price = price;
                        break;
                    }
                    else
                    {
                        Console.WriteLine("输入非法!重新输入!");
                        continue;
                    }
                }
                
                while (true)
                {
                    Console.Write("请输入产品数量:");
                    input = Console.ReadLine() ?? "";
                    if (int.TryParse(input,out num))
                    {
                        addproduct.Total_purchase_quantity += num;
                        break;
                    }
                    else
                    {
                        Console.WriteLine("输入非法!重新输入!");
                        continue;
                    }
                }
                //计算利润
                addproduct.Gross_profit_per_unit = Math.Round(addproduct.Selling_price - addproduct.Purchase_price,2);
                add.Add(addproduct);
                //添加日志
                LogAddProduct addLog = new LogAddProduct
                {
                    Intime = DateTime.Now,
                    WorkerName = operatingIdentity,
                    Id = addproduct.Id,
                    ProductName = addproduct.Name,
                    Production_date = addproduct.Production_date,
                    Expiration_date = addproduct.Expiration_date,
                    Purchase_price = addproduct.Purchase_price,
                    Selling_price = addproduct.Selling_price,
                    quantity = num
                };
                Log.WriteProductLog(addLog);


                Console.Write("是否继续录入(y/n): ");
                input = Console.ReadLine() ?? "";



            } while (input.Equals("y",StringComparison.CurrentCultureIgnoreCase));
            //写入文件
            string jsonIn = JsonConvert.SerializeObject(add, Formatting.Indented);

            File.WriteAllText(ProductsFilePath, jsonIn , new UTF8Encoding(false));


            Console.WriteLine("商品添加成功!");
        }

        //字典中是否存在        
        static bool InOrOut(string real)
        {
            foreach (var item in have)
            {
                if (item.Id.Equals(real,StringComparison.CurrentCultureIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        //确定身份和折扣
        public static bool IsVip(string check , out double Discount)
        {
            string CostomerFileContent = File.ReadAllText(CostomerFilePath ,new UTF8Encoding(false));
            List<Property> checkList = JsonConvert.DeserializeObject<List<Property>>(CostomerFileContent);
            Discount = 1;
            foreach (var item in checkList)
            {
                if (item.Md5.Equals(Encrypt.Md5(check),StringComparison.CurrentCultureIgnoreCase))
                {
                    if (item.Score<=500)
                    {
                        Discount = 0.95;
                        return true;
                    }
                    else if (item.Score <= 800)
                    {
                        Discount = 0.9;
                        return true;
                    }
                    else if (item.Score<=1000)
                    {
                        Discount = 0.87;
                        return true;
                    }
                    else
                    {
                        Discount = 0.8;
                        return true;
                    }
                }
            }
            
            return false;
        }
        
        //办卡
        public static void CreatVipCostomer()
        {
            string input = null;
            //序列化文件数据
            string CostomerFile = File.ReadAllText(CostomerFilePath ,new UTF8Encoding(false));
            List<Property> OriginalCostomer = JsonConvert.DeserializeObject<List<Property>>(CostomerFile);     
            //添加数据
            Property additem = new Property();
            do
            {
                Console.WriteLine("请输入8位以内的数字创建VIP账户,退出请输入break");
                Console.Write("请输入创建账户ID:");

                input = Console.ReadLine() ?? "";
                if (input.Equals("break",StringComparison.CurrentCultureIgnoreCase))
                    return ;

                if (OriginalCostomer.Any(c => c.Md5 == Encrypt.Md5(input)))
                {
                    Console.WriteLine("当前ID已被使用 请更换!");
                    Console.WriteLine("──────────────────────────────────────────────────────");
                    continue;
                }

                if(int.TryParse(input, out int accountId) && input.Length<9)
                {
                    additem.Md5 = Encrypt.Md5(input);
                    additem.Score = 0;
                    OriginalCostomer.Add(additem);
                    break;
                }
                else
                {
                    Console.WriteLine("您输入的账户ID不合法 请重新输入8位以内的数字!");
                    Console.WriteLine("──────────────────────────────────────────────────────");
                    continue;
                }
            } while (true);

            string updatedJson = JsonConvert.SerializeObject(OriginalCostomer, Formatting.Indented);
            File.WriteAllText(CostomerFilePath, updatedJson , new UTF8Encoding(false));
            Console.WriteLine("账户添加成功");
            
        }
        
        static void AddScore(string md5 , int num)
        {
            string constomerFile = File.ReadAllText(CostomerFilePath , new UTF8Encoding(false));
            List<Property> properties = JsonConvert.DeserializeObject<List<Property>>(constomerFile);
            foreach (var item in properties)
            {
                if (item.Md5.Equals(md5,StringComparison.CurrentCultureIgnoreCase))
                {
                    item.Score += num;
                    break;
                }
            }
            string updatedJson = JsonConvert.SerializeObject(properties, Formatting.Indented);
            File.WriteAllText(CostomerFilePath, updatedJson, new UTF8Encoding(false));
        }

    }

    public class Log
    {
        static string[] NowData()
        {
            //第一个位置为年 第二个位置为月
            //yyyy  yyyy-mm
            string[] fileName = new string[2];
            fileName[0] = DateTime.Now.ToString("yyyy"); //年
            fileName[1] = DateTime.Now.ToString("MM");  //月
            return fileName;
        }

        static string fileDate = Path.Combine("..", "..","..", "Data", "sellLogBuyDate");
        static bool SellDataLog(Dictionary<string,int> bill ,string Identity ,List<Product> hava, string constomerId)
        {
            // sellLog = new List<LogAddProduct>();
            string filePath = fileDate; 
            string[] dataCreat = NowData();

            if (!Directory.Exists(Path.Combine(fileDate, dataCreat[0])))// 无年文件夹
            {
                Directory.CreateDirectory(Path.Combine(fileDate,dataCreat[0]));//创建年文件夹
                filePath = Path.Combine(filePath,dataCreat[0],dataCreat[1] + ".json"); //指向月json
                List<LogProduct> addLog = new List<LogProduct>();
                Product lookFor = new Product();
                foreach (var item in bill)
                    {
                        LogProduct add = new LogProduct();
                        lookFor = hava.Find(p=>p.Id.Equals(item.Key,StringComparison.CurrentCultureIgnoreCase));
                        add.Time = DateTime.Now;
                        add.Id = item.Key;
                        add.Nums = item.Value;
                        add.WorkerId = Identity;
                        add.ConstomerId = constomerId;
                        addLog.Add(add);
                    }
                    string jsonIn = JsonConvert.SerializeObject(addLog, Formatting.Indented);
                    File.WriteAllText(filePath, jsonIn, new UTF8Encoding(false));
            }
            else
            {
                filePath = Path.Combine(fileDate,dataCreat[0],dataCreat[1] + ".json"); //指向月文件
                if (!Directory.Exists(Path.Combine(filePath))) // 无月json
                {
                    Directory.CreateDirectory(filePath);// 创建json
                    List<LogProduct> addLog = new List<LogProduct>();
                    Product lookFor = new Product();
                    foreach (var item in bill)
                    {
                        LogProduct add = new LogProduct();
                        lookFor = hava.Find(p=>p.Id.Equals(item.Key,StringComparison.CurrentCultureIgnoreCase));
                        add.Time = DateTime.Now;
                        add.Id = item.Key;
                        add.Nums = item.Value;
                        add.WorkerId = Identity;
                        add.ConstomerId = constomerId;
                        addLog.Add(add);
                    }
                    string jsonIn = JsonConvert.SerializeObject(addLog, Formatting.Indented);
                    File.WriteAllText(filePath, jsonIn, new UTF8Encoding(false));
                    //写入文件
                }
                else// 有年有月有文件
                {
                    string originalLog = File.ReadAllText(filePath ,new UTF8Encoding(false));
                    List<LogProduct> newLog = JsonConvert.DeserializeObject<List<LogProduct>>(originalLog);
                    List<LogProduct> addLog = new List<LogProduct>();
                    Product lookFor = new Product();
                    foreach (var item in bill)
                    {
                        LogProduct add = new LogProduct();
                        lookFor = hava.Find(p=>p.Id.Equals(item.Key,StringComparison.CurrentCultureIgnoreCase));
                        add.Time = DateTime.Now;
                        add.Id = item.Key;
                        add.Nums = item.Value;
                        add.WorkerId = Identity;
                        add.ConstomerId = constomerId;
                        addLog.Add(add);
                    }
                    newLog.AddRange(addLog);

                    string jsonIn = JsonConvert.SerializeObject(newLog, Formatting.Indented);
                    File.WriteAllText(filePath, jsonIn, new UTF8Encoding(false));
                }
            }
            

            return true;
        }

        static List<LogProduct> GetLogsFile(out string logFilePathReturn)
        {
            string logFilePath = Path.Combine("..", "..", "..", "Data", "log", "productslog.json");
            logFilePathReturn = logFilePath;
            string json = File.ReadAllText(logFilePath);
            return JsonConvert.DeserializeObject<List<LogProduct>>(json);
        }

        public static void WriteLog(LogProduct inLog)
        {
            string logFilePath;
            List<LogProduct> logs = GetLogsFile(out logFilePath);
            logs.Add(inLog);
            //写入
            string init = JsonConvert.SerializeObject(logs, Formatting.Indented);
            File.WriteAllText(logFilePath, init ,new UTF8Encoding(false));
            //Console.WriteLine("日志写入成功 记得删除");
        }

        static List<LogAddProduct> GetLogsProductFile(out string logFilePathReturn)
        {
            string logFilePath = Path.Combine("..", "..", "..", "Data", "log", "addproducts.json");
            logFilePathReturn = logFilePath;
            string json = File.ReadAllText(logFilePath);
            return JsonConvert.DeserializeObject<List<LogAddProduct>>(json);
        }

        public static void WriteProductLog(LogAddProduct inLog)
        {
            string logFilePath;
            List<LogAddProduct> logs = GetLogsProductFile(out logFilePath);
            logs.Add(inLog);
            //写入
            string init = JsonConvert.SerializeObject(logs, Formatting.Indented);
            File.WriteAllText(logFilePath, init ,new UTF8Encoding(false));
            //Console.WriteLine("日志写入成功 记得删除");
        }
    }

    class Encrypt //MD5加密
    {
        public static string Md5(string original)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] hashBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(original));
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2")); // 使用小写十六进制表示
                }
                return sb.ToString();
            }
        }
    }
}