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
        public double Gross_profit_per_unit { get; set; } //ë��
        public int Total_purchase_quantity { get; set; }  //�ܹ�����
        public int Total_sales_quantity { get; set; }  //������
        public int Remaining { get; set; }  //���
    }
    public class ShowProduct
    {
        public static void Chose()
        {
            do
            {
                Console.WriteLine("------------------------------------------------------");
                Console.WriteLine("��ѡ������ʽ:");
                Console.WriteLine("0.Id 1.Name 2.�������� 3.�ۼ� 4.ë��");
                Console.WriteLine("5.������ 6.���� 7.��� log.��־�鿴 over.������һ���˵�");
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
                    Console.WriteLine("�Ƿ�����");
                    //Console.WriteLine("--------------------------------------");
                }


            
            } while (true);
        }

    }

    public class AllSort
    {
        static string ProductsFilePath = Path.Combine("..", "..", "..", "Data", "Products.json");        
        //������� 
        public static void SortId()
        {
            string jsonContent = File.ReadAllText(ProductsFilePath);
            List<Product> SortList = JsonConvert.DeserializeObject<List<Product>>(jsonContent);
            List<Product> sortedProducts = SortList.OrderByDescending(p => p.Id).ToList();
            foreach (var product in sortedProducts)
            {
                Console.WriteLine($"Id:{product.Id.PadRight(6)} Name:{product.Name.PadRight(6)} ��������:{product.Expiration_date} �ۼ�:{product.Selling_price:F2}");
                Console.WriteLine($"ë��:{product.Gross_profit_per_unit:F2}      �ܽ�����:{product.Total_purchase_quantity:F0}      ������:{product.Total_sales_quantity:F0}     ���:{product.Remaining:F0}");
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
                Console.WriteLine($"Id:{product.Id.PadRight(6)} Name:{product.Name.PadRight(6)} ��������:{product.Expiration_date} �ۼ�:{product.Selling_price:F2}");
                Console.WriteLine($"ë��:{product.Gross_profit_per_unit:F2}      �ܽ�����:{product.Total_purchase_quantity:F0}      ������:{product.Total_sales_quantity:F0}     ���:{product.Remaining:F0}");
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
                Console.WriteLine($"Id:{product.Id.PadRight(6)} Name:{product.Name.PadRight(6)} ��������:{product.Expiration_date} �ۼ�:{product.Selling_price:F2}");
                Console.WriteLine($"ë��:{product.Gross_profit_per_unit:F2}      �ܽ�����:{product.Total_purchase_quantity:F0}      ������:{product.Total_sales_quantity:F0}     ���:{product.Remaining:F0}");
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
                Console.WriteLine($"Id:{product.Id.PadRight(6)} Name:{product.Name.PadRight(6)} ��������:{product.Expiration_date} �ۼ�:{product.Selling_price:F2}");
                Console.WriteLine($"ë��:{product.Gross_profit_per_unit:F2}      �ܽ�����:{product.Total_purchase_quantity:F0}      ������:{product.Total_sales_quantity:F0}     ���:{product.Remaining:F0}");
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
                Console.WriteLine($"Id:{product.Id.PadRight(6)} Name:{product.Name.PadRight(6)} ��������:{product.Expiration_date} �ۼ�:{product.Selling_price:F2}");
                Console.WriteLine($"ë��:{product.Gross_profit_per_unit:F2}      �ܽ�����:{product.Total_purchase_quantity:F0}      ������:{product.Total_sales_quantity:F0}     ���:{product.Remaining:F0}");
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
                Console.WriteLine($"Id:{product.Id.PadRight(6)} Name:{product.Name.PadRight(6)} ��������:{product.Expiration_date} �ۼ�:{product.Selling_price:F2}");
                Console.WriteLine($"ë��:{product.Gross_profit_per_unit:F2}      �ܽ�����:{product.Total_purchase_quantity:F0}      ������:{product.Total_sales_quantity:F0}     ���:{product.Remaining:F0}");
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
                Console.WriteLine($"Id:{product.Id.PadRight(6)} Name:{product.Name.PadRight(6)} ��������:{product.Expiration_date} �ۼ�:{product.Selling_price:F2}");
                Console.WriteLine($"ë��:{product.Gross_profit_per_unit:F2}      �ܽ�����:{product.Total_purchase_quantity:F0}      ������:{product.Total_sales_quantity:F0}     ���:{product.Remaining:F0}");
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
                Console.WriteLine($"Id:{product.Id.PadRight(6)} Name:{product.Name.PadRight(6)} ��������:{product.Expiration_date} �ۼ�:{product.Selling_price:F2}");
                Console.WriteLine($"ë��:{product.Gross_profit_per_unit:F2}      �ܽ�����:{product.Total_purchase_quantity:F0}      ������:{product.Total_sales_quantity:F0}     ���:{product.Remaining:F0}");
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
                Console.WriteLine("����������Ҫ�鿴����־:  (over.�˳�)");
                Console.WriteLine("0.��Ʒ������¼");
                Console.WriteLine("1.������Ա��¼��¼");
                Console.WriteLine("2.��Ʒ���ۼ�¼");
                Console.WriteLine("3.�鿴��ʱ���Ϊ�ֽ��������־");
                input = Console.ReadLine() ?? "" ;
                if (input.Equals("over", StringComparison.CurrentCultureIgnoreCase))    return;
                //path = null;  ��ʼ��������Ҫ��
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
                    Console.WriteLine("�������ֵ������ ����������!!!");
                    Console.WriteLine("������������������������������������������������������������������������������������������������������������");  //��׼����
                    continue;
                }
                
                    
                // �����ļ���ַ�����ú����������ļ����Ͷ�ȡ��Ӧ���б�
                List<object> listData = ReadJsonFile(path);
                Console.WriteLine("------------------------------------------------------");
                // ��ӡ�б�����
                foreach (var item in listData)
                {
                    if (item is LogProduct)
                    {
                        LogProduct product = (LogProduct)item;
                        Console.WriteLine($"Time:{product.Time}");
                        Console.WriteLine($"������ԱID:{product.WorkerId}");
                        Console.WriteLine($"Id:{product.Id}, ����:{product.Nums}");
                        Console.WriteLine($"�ͻ�����:{product.ConstomerId}");
                        Console.WriteLine("������������������������������������������������������������������������������������������������������������");
                    }
                    else if (item is LogAddProduct)
                    {
                        LogAddProduct addProduct = (LogAddProduct)item;
                        Console.WriteLine($"��Ʒ�����־: ���ʱ��:{addProduct.Intime},");
                        Console.WriteLine($"������ԱID:{addProduct.WorkerName}");
                        Console.WriteLine($"Id:{addProduct.Id} Ʒ��:{addProduct.ProductName}");
                        Console.WriteLine($"�������ڣ�{addProduct.Production_date}    �������ڣ�{addProduct.Expiration_date}");
                        Console.WriteLine($"����:{addProduct.Purchase_price}   �ۼ�:{addProduct.Selling_price}");
                        Console.WriteLine($"����:{addProduct.quantity}");
                        Console.WriteLine("������������������������������������������������������������������������������������������������������������");
                    }
                    else if (item is LogIn)
                    {
                        LogIn login = (LogIn)item;
                        Console.WriteLine($"ʱ��:{login.Time,-17}    ������ԱID:{login.WorkerId,-7}    ����:{login.Operating}");
                    }
                }
            
            } while (true);
        }

        static void SellData()
        {
            do
            {
                int i=0;
                string path = Path.Combine("..", "..", "..", "Data", "sellLogBuyDate"); //�ļ��е�ַ
                string[] yearPath = Directory.GetFileSystemEntries(path);
                foreach (string item in yearPath)
                {
                    Console.WriteLine(i+1 + ": " + yearPath[i].Substring(yearPath[i].LastIndexOf('\\')) + "��");
                    i++;
                }
                
                Console.WriteLine("�������ڷ�����־�鿴: over");
                Console.Write("�������Ӧ�ı�Ų鿴:");
                string input = Console.ReadLine() ?? "";
                int num;
                // ConsoleKeyInfo keyInfo = Console.ReadKey(); //����ʱ����
                
                if (input.Equals("over", StringComparison.InvariantCultureIgnoreCase))
                {
                    return;
                }
                if (!int.TryParse(input,out num))
                {
                    Console.WriteLine("��������ȷ��ţ�");
                    continue;
                }
                else if (num <= 0 || num > i)
                {
                    Console.WriteLine("��������ȷ��ţ�");
                    continue;
                }
                i=0;
                
                //����ȷ����json�ļ�
                path = Path.Combine(path, yearPath[num-1].Substring(yearPath[num-1].LastIndexOf('\\') + 1 ));
                string[] monthPath = Directory.GetFileSystemEntries(path);
                foreach (string item in monthPath)
                {
                    Console.WriteLine(i+1 + ": " + monthPath[i].Substring(monthPath[i].LastIndexOf('\\') - 4));
                    i++;
                }
                num=0;
                Console.WriteLine("����[��]����ѡ��: return");
                Console.Write("�������Ӧ�ı�Ų鿴:");
                input = Console.ReadLine() ?? "";
                if (input.Equals("return"))
                {
                    continue;
                }
                if (!int.TryParse(input,out num))
                {
                    Console.WriteLine("��������ȷ��ţ�");
                    continue;
                }
                else if (num <= 0 || num > i)
                {
                    Console.WriteLine("��������ȷ��ţ�");
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
            var sortedPairs = log.OrderByDescending(p => p.Value); //����
            // �������Ľ��ת��Ϊ�µ� Dictionary<string, int>
            Dictionary<string, int> sortedDictionary = sortedPairs.ToDictionary(p => p.Key, p => p.Value); //ת��
            Worker.Product product = new Worker.Product();
            Console.WriteLine("���     ����     ����     ����    ���۶�");
            foreach (var item in sortedDictionary)
            {
                product = WorkerFunctions.have.Find(p => p.Id.Equals(item.Key, StringComparison.CurrentCultureIgnoreCase));
                
            }




        }








        // �����ļ���ַ��ȡ JSON �ļ���������Ӧ���б�����
        public static List<object> ReadJsonFile(string filePath)
        {
            // �����ļ���չ���ж��ļ�����
            string extension = Path.GetExtension(filePath);
            
            // ��ʼ��һ���յ��б����
            List<object> result = new List<object>();

            // �����ļ����Ͷ�ȡ��Ӧ�� JSON �ļ�
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