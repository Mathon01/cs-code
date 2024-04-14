using Newtonsoft.Json;
using System.Text;
using System.Security.Cryptography;
// using AdminShow;
// using System.Reflection.Metadata;
// using System.Runtime.InteropServices;
// using System.Text.Unicode;


namespace Worker
{
    class LogProduct  //产品进出格式
    {
        public DateTime Time {get; set;}
        public string Id{get; set;}
        public int Nums{get; set;}
        public string WorkerId{get; set;}
        public string ConstomerId{get; set;}
    }
    class LogAddProduct  //log_进出货物
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
    class Property  //log_登录记录
    {
        public string Md5 {get; set;}
        public int Score {get; set;}
    }
    class Product  //产品格式
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
        public double Sales {get; set; } //销售额
        public int Remaining { get; set; }  //剩余数量
    }
    
    static class WorkerFunctions  //收银功能
    {
        public static void FunctionChose(string operatingIdentity)
        {
            Console.WriteLine("请选择您要执行的功能");
            string chose;
            do
            {   
                Console.WriteLine("┌──────────────────────────────────────────────────────┐");
                Console.WriteLine("│                您好!收银员：                         │");
                Console.WriteLine("│          0.返回上一级            现在是              │");
                Console.WriteLine("│          1.结账                {0}            │",DateTime.Now.ToString("yyyy-MM-dd"));
                Console.WriteLine("│          2.进货                 {0}             │",DateTime.Now.ToString("HH:mm:ss"));
                Console.WriteLine("│          3.下班               祝您工作愉快           │");
                Console.WriteLine("└──────────────────────────────────────────────────────┘");
                
                //chose = Console.ReadLine();
                ConsoleKeyInfo keyInfo = Console.ReadKey(); //调试时报错
                // if (chose.Equals("0",StringComparison.CurrentCultureIgnoreCase))
                // {
                //     //Console.Clear();
                //     return;
                // }
                // else if (chose.Equals("1",StringComparison.CurrentCultureIgnoreCase))
                // {
                //     //Console.Clear();
                //     AddBill(operatingIdentity);
                //     continue;
                // }
                // else if (chose.Equals("2",StringComparison.CurrentCultureIgnoreCase))
                // {
                //     //Console.Clear();
                //     AddProduct(operatingIdentity);
                //     continue;
                // }
                // else if (chose.Equals("3",StringComparison.CurrentCultureIgnoreCase))
                // {
                //     //Console.Clear();
                //     Environment.Exit(0);
                // }
                // else
                // {
                //     //Console.Clear();
                //     Console.WriteLine("输入非法 重新输入");
                // }
                //-------------------------------------------------------------------
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
                //----------------------------------------------------------------------
            } while (true);

        }
        
        static string ProductsFilePath = Path.Combine("..", "..", "..", "Data", "Products.json");
        //商品数据
        static string tmp_jsonDate = File.ReadAllText(ProductsFilePath ,new UTF8Encoding(false));
        public static List<Product> have = JsonConvert.DeserializeObject<List<Product>>(tmp_jsonDate);
        //记录账单
        static void AddBill(string operatingIdentity)
        {
            Dictionary<string, int> bill= new Dictionary<string, int>() ;
            do
            {
                Console.WriteLine();
                Console.WriteLine("-----------------------------------------------------");
                Console.WriteLine("              结束请输入: over    ");
                Console.WriteLine("              结账请输入: price");
                Console.WriteLine("              删除商品输入: del");
                Console.WriteLine("-----------------------------------------------------");
                Console.Write("请输入产品ID:");
                string input = Console.ReadLine() ?? "";
                
                if (input.Equals("over",StringComparison.CurrentCultureIgnoreCase))
                {
                    bill.Clear();
                    Console.Clear();
                    return;
                }
                else if (input.Equals("price",StringComparison.CurrentCultureIgnoreCase))
                {
                    TotalPrice(bill, operatingIdentity);
                    bill.Clear();
                    Console.Clear();
                    continue;
                }
                
                if (input.Equals("del", StringComparison.CurrentCultureIgnoreCase))
                {
                    Console.Clear();
                    bill = DelBill(bill);
                    ShowBill(bill);
                    continue;
                }

                int productId;
                if (!int.TryParse(input,out productId)) //int检测
                {
                    Console.Write("ID不合法:");
                    // //Console.Clear();
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
                    }
                    else  
                    {
                        bill.Add(input,1); 
                        Console.WriteLine("账单已添加");
                        Console.WriteLine("------------------------------------------------------");
                    }
                    //Console.Clear();
                }
                else
                {
                    Console.WriteLine("当前商品Id未录入,请联系工作人员//此件商品未计入账单");
                    Console.WriteLine("------------------------------------------------------");
                }
                // 账单展示
                Console.Clear();
                ShowBill(bill);

            } while (true);

        }
        // 显示账单
        static void ShowBill(Dictionary<string, int> bill)
        {
            Console.WriteLine("──────────────────────────────────────────────────────");
            Console.WriteLine();
            Console.WriteLine("                    您当前的账单如下");
            Console.WriteLine("           --------------------------------");
            Console.WriteLine("           编号    名称      单价      数量");
            foreach (var item in bill)
            {
                Product product = new Product();
                product = have.Find(p => p.Id.Equals(item.Key, StringComparison.CurrentCultureIgnoreCase));
                Console.Write($"           {product.Id,-8}"); // ID 列宽度为 6，左对齐
                
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

                Console.Write($"{product.Selling_price.ToString("C"),-9}"); // 单价格式化为货币，列宽度为 8，左对齐
                Console.WriteLine($"{item.Value,3}"); // 数量列宽度为 6，左对齐
            }
            Console.WriteLine("            --------------------------------");
        }
        // 账单退货
        static Dictionary<string,int> DelBill(Dictionary<string,int> bill)
        {
            string delId;
            do
            {
                // Console.WriteLine("编号     数量");
                // foreach (var item in bill)
                // {
                //     Console.WriteLine("{0}       {1}",item.Key,item.Value);
                // }
                // Console.WriteLine("------------------------------------------------------");
                ShowBill(bill);
                Console.WriteLine("结束退货请输入: 0");
                Console.Write("请输入您退掉的商品编号:");
                delId = Console.ReadLine() ?? "";
                if (delId.Equals("0", StringComparison.CurrentCultureIgnoreCase))
                {
                    Console.Clear();
                    return bill;
                }
                if (bill.TryGetValue(delId,out int num))
                {
                    if (num==1)
                    {
                        bill.Remove(delId);
                    }
                    else
                    {
                        bill[delId] = num - 1 ;
                    }
                }
                else
                {
                    Console.WriteLine("您输入的编号不存在,请重新输入!");
                }
                Console.Clear();
            } while (true);
        }
        //Vip客户数据文件路径
        static string CostomerFilePath = Path.Combine("..", "..", "..", "Data", "costomer.json");
        // 结算
        static void TotalPrice(Dictionary<string, int> bill, string operatingIdentity)
        {
            int costomerScore;
            double Discount = 1;  //折扣
            string isVip = null;    
            do
            {
                Console.WriteLine("请输入VIP_ID: ,办理请输入y,没有请输入n:");
                isVip = Console.ReadLine() ?? "";
                if (isVip.Equals("y",StringComparison.CurrentCultureIgnoreCase))
                {
                    Console.Clear();
                    CreatVipCostomer();
                    continue;
                }
                else if (isVip.Equals("n",StringComparison.CurrentCultureIgnoreCase))
                {
                    Console.Clear();
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
            Console.Clear();
            Console.WriteLine("──────────────────────────────────────────────────────");
            Console.WriteLine();
            Console.WriteLine("                      您的账单");
            Console.WriteLine("           --------------------------------");
            Console.WriteLine("           编号    名称      单价      数量");

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
                    product.Sales += sum;  //增加销售额
                    // 写进日志
                    writeLog.Time = DateTime.Now;
                    writeLog.Id = product.Id;
                    writeLog.Nums = quantity;
                    writeLog.WorkerId = operatingIdentity;
                    writeLog.ConstomerId = isVip;

                    Log.WriteLog(writeLog);//  日志写入
                    Console.Write($"           {productId,-8}"); // ID 列宽度为 6，左对齐
                    
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

                    Console.Write($"{product.Selling_price.ToString("C"),-9}"); // 单价格式化为货币，列宽度为 8，左对齐
                    Console.WriteLine($"{quantity,3}"); // 数量列宽度为 6，左对齐
                }
            }
            Log.SellDataLog(bill, operatingIdentity, have, isVip); //日期log

            string updatedJson = JsonConvert.SerializeObject(change, Formatting.Indented);
            File.WriteAllText(ProductsFilePath, updatedJson, new UTF8Encoding(false)); //更新产品信息表
            Console.WriteLine("           --------------------------------");
            // 结算
            costomerScore = AddScore(Encrypt.Md5(isVip),(int)sum);
            if (Discount!=1)
            {
                Console.WriteLine("           您好VIP顾客:");
                Console.WriteLine("           您目前的积分有:{0,17}",costomerScore);
                Console.WriteLine(string.Format("           您的折扣:{0,23:P}",Discount));
            }
            Console.WriteLine("           应付：{0,26:F2}", sum);
            Console.WriteLine("           实付：{0,26:F2}",sum*Discount);
            Console.WriteLine();
            Console.WriteLine("                      欢迎下次光临!");
            Console.WriteLine("                 {0}",DateTime.Now.ToString("yyyy-MM-dd  HH:mm:ss"));
            // Console.WriteLine("           {0}",DateTime.Now.ToString("HH:mm:ss"));
            Console.WriteLine("           --------------------------------");
            Console.WriteLine("-----------------------------------------------------");
            Console.WriteLine();
            Console.ReadLine();
            return;
        }           
        //添加商品
        static void AddProduct(string operatingIdentity)
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

        /// <summary>
        /// 货物中是否记录该商品存在    
        /// </summary>
        /// <param name="real"></param>
        /// <returns></returns>  
        static bool InOrOut(string real)
        {
            Product look = new Product();
            look = have.Find(p => p.Id.Equals(real, StringComparison.CurrentCultureIgnoreCase));
            // foreach (var item in have)
            // {
            //     if (item.Id.Equals(real,StringComparison.CurrentCultureIgnoreCase))
            //     {
            //         return true;
            //     }
            // }
            if (look != null && !string.IsNullOrEmpty(look.Id))
            {
                return true;
            }
            return false;
        }

        //确定身份和折扣
        static bool IsVip(string check , out double Discount)
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
        
        /// <summary>
        /// 创建会员账户
        /// </summary>
        static void CreatVipCostomer()
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
                    Console.Clear();
                    Console.WriteLine("当前ID已被使用 请更换!");
                    Console.WriteLine();
                    // Console.WriteLine("──────────────────────────────────────────────────────");
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
        
        /// <summary>
        /// 累计会员结算的积分
        /// </summary>
        /// <param name="md5"></param>
        /// <param name="num"></param>
        static int AddScore(string md5 , int num)
        {
            int costomerScore=0;
            string constomerFile = File.ReadAllText(CostomerFilePath , new UTF8Encoding(false));
            List<Property> properties = JsonConvert.DeserializeObject<List<Property>>(constomerFile);
            foreach (var item in properties)
            {
                if (item.Md5.Equals(md5,StringComparison.CurrentCultureIgnoreCase))
                {
                    item.Score += num;
                    costomerScore = item.Score;
                    break;
                }
            }
            string updatedJson = JsonConvert.SerializeObject(properties, Formatting.Indented);
            File.WriteAllText(CostomerFilePath, updatedJson, new UTF8Encoding(false));
            return costomerScore;
        }

    }

    class Log  //日志
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

        /// <summary>
        /// 根据年-月划分log数据
        /// </summary>
        /// <param name="bill"></param>
        /// <param name="Identity"></param>
        /// <param name="hava"></param>
        /// <param name="constomerId"></param>
        /// <returns></returns>
        public static bool SellDataLog(Dictionary<string,int> bill ,string Identity ,List<Product> hava, string constomerId)
        {
            // sellLog = new List<LogAddProduct>();
            string filePath = fileDate; 
            string[] dataCreat = NowData();

            if (!Directory.Exists(Path.Combine(fileDate, dataCreat[0])))// 无年文件夹
            {
                Directory.CreateDirectory(Path.Combine(fileDate,dataCreat[0]));//创建年文件夹
                filePath = Path.Combine(filePath,dataCreat[0],dataCreat[1] + ".json"); //指向月json
                
                File.WriteAllText(filePath,""); //写空文件
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
                if (!File.Exists(filePath)) // 无月json
                {
                    File.WriteAllText(filePath,""); // 创建json
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