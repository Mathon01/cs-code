using Newtonsoft.Json;
using Admin;
using Worker;
using mainProcess;
using System.Text;

namespace AdminShow
{
    public class Product
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime Production_date { get; set; }
        public DateTime Expiration_date { get; set; }
        public string Category { get; set; }
        public double Purchase_price { get; set; }
        public double Selling_price { get; set; }
        public double Gross_profit_per_unit { get; set; } //毛利
        public int Total_purchase_quantity { get; set; }  //总购买量
        public int Total_sales_quantity { get; set; }  //总销量
        public int Remaining { get; set; }  //库存
    }
    public class ShowProduct
    {
        public static void Chose()
        {
            do
            {
                Console.WriteLine("------------------------------------------------------");
                Console.WriteLine("请选择排序方式:");
                Console.WriteLine("0.Id 1.Name 2.过期日期 3.售价 4.毛利");
                Console.WriteLine("5.进货量 6.销量 7.库存 log.日志查看 over.返回上一级菜单");
                string input = Console.ReadLine() ?? "";
                if (input.Equals("0",StringComparison.CurrentCultureIgnoreCase))
                {
                    AllSort.SortId();
                }
                else if (input.Equals("1",StringComparison.CurrentCultureIgnoreCase))
                {
                    AllSort.SortName();
                }
                else if (input.Equals("2",StringComparison.CurrentCultureIgnoreCase))
                {
                    AllSort.SortExpiration_date();
                }
                else if (input.Equals("3",StringComparison.CurrentCultureIgnoreCase))
                {
                    AllSort.SortSelling_price();
                }
                else if (input.Equals("4",StringComparison.CurrentCultureIgnoreCase))
                {
                    AllSort.SortGross_profit_per_unit();
                }
                else if (input.Equals("5",StringComparison.CurrentCultureIgnoreCase))
                {
                    AllSort.SortTotal_purchase_quantity();
                }
                else if (input.Equals("6",StringComparison.CurrentCultureIgnoreCase))
                {
                    AllSort.SortTotal_sales_quantity();
                }
                else if (input.Equals("7",StringComparison.CurrentCultureIgnoreCase))
                {
                    AllSort.SortRemaining();
                }
                else if (input.Equals("log", StringComparison.CurrentCultureIgnoreCase))
                {
                    LogShow.LogChose();
                }
                
                else if (input.Equals("over",StringComparison.CurrentCultureIgnoreCase))
                {
                    Functions.Chose();
                }
                else
                {
                    Console.WriteLine("非法输入");
                    //Console.WriteLine("--------------------------------------");
                }


            
            } while (true);
        }

    }

    public class AllSort
    {
        static string ProductsFilePath = Path.Combine("..", "..", "..", "Data", "Products.json");        
        //排序混乱 
        public static void SortId()
        {
            string jsonContent = File.ReadAllText(ProductsFilePath);
            List<Product> SortList = JsonConvert.DeserializeObject<List<Product>>(jsonContent);
            List<Product> sortedProducts = SortList.OrderByDescending(p => p.Id).ToList();
            foreach (var product in sortedProducts)
            {
                Console.WriteLine($"Id:{product.Id.PadRight(6)} Name:{product.Name.PadRight(6)} 过期日期:{product.Expiration_date} 售价:{product.Selling_price:F2}");
                Console.WriteLine($"毛利:{product.Gross_profit_per_unit:F2}      总进货量:{product.Total_purchase_quantity:F0}      总销量:{product.Total_sales_quantity:F0}     库存:{product.Remaining:F0}");
                Console.WriteLine("------------------------------------------------------------------");
            }
        }

        public static void SortName()
        {
            string jsonContent = File.ReadAllText(ProductsFilePath);
            List<Product> SortList = JsonConvert.DeserializeObject<List<Product>>(jsonContent);
            List<Product> sortedProducts = SortList.OrderByDescending(p => p.Name).ToList();
            foreach (var product in sortedProducts)
            {
                Console.WriteLine($"Id:{product.Id.PadRight(6)} Name:{product.Name.PadRight(6)} 过期日期:{product.Expiration_date} 售价:{product.Selling_price:F2}");
                Console.WriteLine($"毛利:{product.Gross_profit_per_unit:F2}      总进货量:{product.Total_purchase_quantity:F0}      总销量:{product.Total_sales_quantity:F0}     库存:{product.Remaining:F0}");
                Console.WriteLine("------------------------------------------------------------------");
            }
        }

        public static void SortExpiration_date()
        {
            string jsonContent = File.ReadAllText(ProductsFilePath);
            List<Product> SortList = JsonConvert.DeserializeObject<List<Product>>(jsonContent);
            List<Product> sortedProducts = SortList.OrderByDescending(p => p.Expiration_date).ToList();
            foreach (var product in sortedProducts)
            {
                Console.WriteLine($"Id:{product.Id.PadRight(6)} Name:{product.Name.PadRight(6)} 过期日期:{product.Expiration_date} 售价:{product.Selling_price:F2}");
                Console.WriteLine($"毛利:{product.Gross_profit_per_unit:F2}      总进货量:{product.Total_purchase_quantity:F0}      总销量:{product.Total_sales_quantity:F0}     库存:{product.Remaining:F0}");
                Console.WriteLine("------------------------------------------------------------------");
            }        
        }

        public static void SortSelling_price()
        {
            string jsonContent = File.ReadAllText(ProductsFilePath);
            List<Product> SortList = JsonConvert.DeserializeObject<List<Product>>(jsonContent);
            List<Product> sortedProducts = SortList.OrderByDescending(p => p.Selling_price).ToList();
            foreach (var product in sortedProducts)
            {
                Console.WriteLine($"Id:{product.Id.PadRight(6)} Name:{product.Name.PadRight(6)} 过期日期:{product.Expiration_date} 售价:{product.Selling_price:F2}");
                Console.WriteLine($"毛利:{product.Gross_profit_per_unit:F2}      总进货量:{product.Total_purchase_quantity:F0}      总销量:{product.Total_sales_quantity:F0}     库存:{product.Remaining:F0}");
                Console.WriteLine("------------------------------------------------------------------");
            }  
        }

        public static void SortGross_profit_per_unit()
        {
            string jsonContent = File.ReadAllText(ProductsFilePath);
            List<Product> SortList = JsonConvert.DeserializeObject<List<Product>>(jsonContent);
            List<Product> sortedProducts = SortList.OrderByDescending(p => p.Gross_profit_per_unit).ToList();
            foreach (var product in sortedProducts)
            {
                Console.WriteLine($"Id:{product.Id.PadRight(6)} Name:{product.Name.PadRight(6)} 过期日期:{product.Expiration_date} 售价:{product.Selling_price:F2}");
                Console.WriteLine($"毛利:{product.Gross_profit_per_unit:F2}      总进货量:{product.Total_purchase_quantity:F0}      总销量:{product.Total_sales_quantity:F0}     库存:{product.Remaining:F0}");
                Console.WriteLine("------------------------------------------------------------------");
            }  
        }

        public static void SortTotal_purchase_quantity()
        {
            string jsonContent = File.ReadAllText(ProductsFilePath);
            List<Product> SortList = JsonConvert.DeserializeObject<List<Product>>(jsonContent);
            List<Product> sortedProducts = SortList.OrderByDescending(p => p.Total_purchase_quantity).ToList();
            foreach (var product in sortedProducts)
            {
                Console.WriteLine($"Id:{product.Id.PadRight(6)} Name:{product.Name.PadRight(6)} 过期日期:{product.Expiration_date} 售价:{product.Selling_price:F2}");
                Console.WriteLine($"毛利:{product.Gross_profit_per_unit:F2}      总进货量:{product.Total_purchase_quantity:F0}      总销量:{product.Total_sales_quantity:F0}     库存:{product.Remaining:F0}");
                Console.WriteLine("------------------------------------------------------------------");
            }  
        }

        public static void SortTotal_sales_quantity()
        {
            string jsonContent = File.ReadAllText(ProductsFilePath);
            List<Product> SortList = JsonConvert.DeserializeObject<List<Product>>(jsonContent);
            List<Product> sortedProducts = SortList.OrderByDescending(p => p.Total_sales_quantity).ToList();
            foreach (var product in sortedProducts)
            {
                Console.WriteLine($"Id:{product.Id.PadRight(6)} Name:{product.Name.PadRight(6)} 过期日期:{product.Expiration_date} 售价:{product.Selling_price:F2}");
                Console.WriteLine($"毛利:{product.Gross_profit_per_unit:F2}      总进货量:{product.Total_purchase_quantity:F0}      总销量:{product.Total_sales_quantity:F0}     库存:{product.Remaining:F0}");
                Console.WriteLine("------------------------------------------------------------------");
            }  
        }

        public static void SortRemaining()
        {
            string jsonContent = File.ReadAllText(ProductsFilePath);
            List<Product> SortList = JsonConvert.DeserializeObject<List<Product>>(jsonContent);
            List<Product> sortedProducts = SortList.OrderByDescending(p => p.Remaining).ToList();
            foreach (var product in sortedProducts)
            {
                Console.WriteLine($"Id:{product.Id.PadRight(6)} Name:{product.Name.PadRight(6)} 过期日期:{product.Expiration_date} 售价:{product.Selling_price:F2}");
                Console.WriteLine($"毛利:{product.Gross_profit_per_unit:F2}      总进货量:{product.Total_purchase_quantity:F0}      总销量:{product.Total_sales_quantity:F0}     库存:{product.Remaining:F0}");
                Console.WriteLine("------------------------------------------------------------------");
            }  
        }
    }

    public class LogShow
    {
        static string addProductsLogPath = Path.Combine("..", "..", "..", "Data", "log", "addproducts.json");
        static string logInLogPath = Path.Combine("..", "..", "..", "Data", "log", "login.json");
        static string logProductsLog = Path.Combine("..", "..", "..", "Data", "log", "productslog.json");

        public static void LogChose()
        {
            string input;
            string path;
            do
            {
                Console.WriteLine("------------------------------------------------------");
                Console.WriteLine("请输入您需要查看的日志:  (over.退出)");
                Console.WriteLine("0.商品进货记录");
                Console.WriteLine("1.工作人员登录记录");
                Console.WriteLine("2.商品销售记录");
                Console.WriteLine("3.查看以时间段为分界的销售日志");
                input = Console.ReadLine() ?? "" ;
                if (input.Equals("over", StringComparison.CurrentCultureIgnoreCase))    return;
                //path = null;  初始化？不需要？
                if (input.Equals("0",StringComparison.CurrentCultureIgnoreCase)) path = addProductsLogPath;
                else if (input.Equals("1", StringComparison.CurrentCultureIgnoreCase)) path = logInLogPath;
                else if (input.Equals("2",StringComparison.CurrentCultureIgnoreCase)) path = logProductsLog; 
                else if (input.Equals("3", StringComparison.CurrentCultureIgnoreCase))
                {
                    SellData();
                    continue;
                }
                else
                {
                    Console.WriteLine("您输入的值不存在 请重新输入!!!");
                    Console.WriteLine("──────────────────────────────────────────────────────");  //标准长度
                    continue;
                }
                
                    
                // 传入文件地址，调用函数并根据文件类型读取对应的列表
                List<object> listData = ReadJsonFile(path);
                Console.WriteLine("------------------------------------------------------");
                // 打印列表内容
                foreach (var item in listData)
                {
                    if (item is LogProduct)
                    {
                        LogProduct product = (LogProduct)item;
                        Console.WriteLine($"Time:{product.Time}");
                        Console.WriteLine($"工作人员ID:{product.WorkerId}");
                        Console.WriteLine($"Id:{product.Id}, 数量:{product.Nums}");
                        Console.WriteLine($"客户卡号:{product.ConstomerId}");
                        Console.WriteLine("──────────────────────────────────────────────────────");
                    }
                    else if (item is LogAddProduct)
                    {
                        LogAddProduct addProduct = (LogAddProduct)item;
                        Console.WriteLine($"物品添加日志: 入库时间:{addProduct.Intime},");
                        Console.WriteLine($"工作人员ID:{addProduct.WorkerName}");
                        Console.WriteLine($"Id:{addProduct.Id} 品名:{addProduct.ProductName}");
                        Console.WriteLine($"生产日期：{addProduct.Production_date}    过期日期：{addProduct.Expiration_date}");
                        Console.WriteLine($"进价:{addProduct.Purchase_price}   售价:{addProduct.Selling_price}");
                        Console.WriteLine($"数量:{addProduct.quantity}");
                        Console.WriteLine("──────────────────────────────────────────────────────");
                    }
                    else if (item is LogIn)
                    {
                        LogIn login = (LogIn)item;
                        Console.WriteLine($"时间:{login.Time,-17}    工作人员ID:{login.WorkerId,-7}    操作:{login.Operating}");
                    }
                }
            
            } while (true);
        }

        static void SellData()
        {
            do
            {
                int i=0;
                string path = Path.Combine("..", "..", "..", "Data", "sellLogBuyDate"); //文件夹地址
                string[] yearPath = Directory.GetFileSystemEntries(path);
                foreach (string item in yearPath)
                {
                    Console.WriteLine(i+1 + ": " + yearPath[i].Substring(yearPath[i].LastIndexOf('\\')) + "年");
                    i++;
                }
                
                Console.WriteLine("结束日期分类日志查看: over");
                Console.Write("请输入对应的编号查看:");
                string input = Console.ReadLine() ?? "";
                int num;
                // ConsoleKeyInfo keyInfo = Console.ReadKey(); //调试时报错
                
                if (input.Equals("over", StringComparison.InvariantCultureIgnoreCase))
                {
                    return;
                }
                if (!int.TryParse(input,out num))
                {
                    Console.WriteLine("请输入正确编号！");
                    continue;
                }
                else if (num <= 0 || num > i)
                {
                    Console.WriteLine("请输入正确编号！");
                    continue;
                }
                i=0;
                
                //以下确定月json文件
                path = Path.Combine(path, yearPath[num-1].Substring(yearPath[num-1].LastIndexOf('\\') + 1 ));
                string[] monthPath = Directory.GetFileSystemEntries(path);
                foreach (string item in monthPath)
                {
                    Console.WriteLine(i+1 + ": " + monthPath[i].Substring(monthPath[i].LastIndexOf('\\') - 4));
                    i++;
                }
                num=0;
                Console.WriteLine("返回[年]日期选择: return");
                Console.Write("请输入对应的编号查看:");
                input = Console.ReadLine() ?? "";
                if (input.Equals("return"))
                {
                    continue;
                }
                if (!int.TryParse(input,out num))
                {
                    Console.WriteLine("请输入正确编号！");
                    continue;
                }
                else if (num <= 0 || num > i)
                {
                    Console.WriteLine("请输入正确编号！");
                    continue;
                }
                path = Path.Combine(path, monthPath[num-1].Substring(monthPath[num-1].LastIndexOf('\\') + 1));
                ShowDateLog(path);

            } while (true);
        }

        static void ShowDateLog (string path)
        {
            string jsonFile = File.ReadAllText(path, new UTF8Encoding(false));
            Dictionary<string, int> log = new Dictionary<string, int>();
            List<LogProduct> logs = JsonConvert.DeserializeObject<List<LogProduct>>(jsonFile);

            foreach (LogProduct item in logs)
            {
                if (!log.ContainsKey(item.Id))
                    log[item.Id] = 1;
                
                else
                    log[item.Id]++ ;
            }
            //WorkerFunctions.have;
            var sortedPairs = log.OrderByDescending(p => p.Value); //排序
            // 将排序后的结果转换为新的 Dictionary<string, int>
            Dictionary<string, int> sortedDictionary = sortedPairs.ToDictionary(p => p.Key, p => p.Value); //转化
            Worker.Product product = new Worker.Product();
            Console.WriteLine("编号     名称     单价     销量    销售额");
            foreach (var item in sortedDictionary)
            {
                product = WorkerFunctions.have.Find(p => p.Id.Equals(item.Key, StringComparison.CurrentCultureIgnoreCase));
                
            }




        }








        // 根据文件地址读取 JSON 文件并返回相应的列表类型
        public static List<object> ReadJsonFile(string filePath)
        {
            // 根据文件扩展名判断文件类型
            string extension = Path.GetExtension(filePath);
            
            // 初始化一个空的列表对象
            List<object> result = new List<object>();

            // 根据文件类型读取对应的 JSON 文件
            if (extension.Equals(".json", StringComparison.OrdinalIgnoreCase))
            {
                if (filePath.Contains("productslog"))
                {
                    List<LogProduct> logProducts = JsonConvert.DeserializeObject<List<LogProduct>>(File.ReadAllText(filePath));
                    result = logProducts.Cast<object>().ToList();
                }
                else if (filePath.Contains("addproducts"))
                {
                    List<LogAddProduct> logAddProducts = JsonConvert.DeserializeObject<List<LogAddProduct>>(File.ReadAllText(filePath));
                    result = logAddProducts.Cast<object>().ToList();
                }
                else if (filePath.Contains("login"))
                {
                    List<LogIn> logAddProducts = JsonConvert.DeserializeObject<List<LogIn>>(File.ReadAllText(filePath));
                    result = logAddProducts.Cast<object>().ToList();
                }
            }
            else
            {
                Console.WriteLine("Unsupported file format");
            }

            return result;
        }
    



    }
}