using Newtonsoft.Json;
using System.Text;
using System.Security.Cryptography;
// using System.Runtime.InteropServices;
// using System.Text.Unicode;


namespace Worker
{
    public class LogProduct  //��Ʒ������ʽ
    {
        public DateTime Time {get; set;}
        public string Id{get; set;}
        public int Nums{get; set;}
        public string WorkerId{get; set;}
        public string ConstomerId{get; set;}
    }
    public class LogAddProduct  //��������
    {
        public DateTime Intime {get; set;} //����ʱ��
        public string WorkerName {get; set;} //������Ա
        public string Id { get; set; }
        public string ProductName { get; set; }
        public DateTime Production_date { get; set; }
        public DateTime Expiration_date { get; set; }
        public double Purchase_price { get; set; }
        public double Selling_price { get; set; }
        public int quantity { get; set; }  //������
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
        public double Gross_profit_per_unit { get; set; } //����
        public int Total_purchase_quantity { get; set; }  //������
        public int Total_sales_quantity { get; set; }  //������
        public int Remaining { get; set; }  //ʣ������
    }
    
    static class WorkerFunctions
    {
        public static void FunctionChose(string operatingIdentity)
        {
            Console.WriteLine("��ѡ����Ҫִ�еĹ���");
            string chose;
            do
            {   
                Console.WriteLine("������������������������������������������������������������������������������������������������������������");
                Console.WriteLine("��        0.������һ��            �����ǣ�            ��");
                Console.WriteLine("��        1.����                {0}            ��",DateTime.Now.ToString("yyyy-MM-dd"));
                Console.WriteLine("��        2.����                 {0}             ��",DateTime.Now.ToString("HH:mm:ss"));
                Console.WriteLine("��        3.�°�               ף���������           ��");
                Console.WriteLine("������������������������������������������������������������������������������������������������������������");

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
                    Console.WriteLine("����Ƿ� ��������");
                }
            } while (true);

        }
        static Dictionary<string, int> bill= new Dictionary<string, int>() ;

        static string ProductsFilePath = Path.Combine("..", "..", "..", "Data", "Products.json");
        //��Ʒ����
        static string tmp_jsonDate = File.ReadAllText(ProductsFilePath ,new UTF8Encoding(false));
        static List<Product> have = JsonConvert.DeserializeObject<List<Product>>(tmp_jsonDate);
        
        static void AddBill(string operatingIdentity)
        {
            
            do
            {
                Console.WriteLine("����������: over  ����������: price");
                Console.Write("�������ƷID:");
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
                if (!int.TryParse(input,out productId)) //int���
                {
                    Console.Write("ID���Ϸ�:");
                    Console.Clear();
                    continue;
                }
                //��Ʒ���ڼ��
                if (InOrOut(input))
                {
                    if (bill.ContainsKey(input)) //����
                    {
                        bill[input]++;  
                        Console.WriteLine("�˵������");
                        Console.WriteLine("------------------------------------------------------");
                        continue;
                    }
                    else  
                    {
                        bill.Add(input,1); 
                        Console.WriteLine("�˵������");
                        Console.WriteLine("------------------------------------------------------");
                        continue;
                    }
                }
                else
                {
                    Console.WriteLine("��ǰ��ƷIdδ¼��,����ϵ������Ա//�˼���Ʒδ�����˵�");
                    Console.WriteLine("------------------------------------------------------");
                }
                
            } while (true);

        }

        //Vip�ͻ������ļ�·��
        static string CostomerFilePath = Path.Combine("..", "..", "..", "Data", "costomer.json");
        

        public static void TotalPrice(Dictionary<string, int> bill, string operatingIdentity)
        {
            double Discount = 1;  //�ۿ�
            string isVip = null;    
            do
            {
                Console.WriteLine("������VIP_ID: ,����������y,û��������n:");
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
                    Console.WriteLine("��������򲻴��� ��������:");
                }
    
            } while (true); 
            
            double sum = 0;
            Console.WriteLine("������������������������������������������������������������������������������������������������������������");
            Console.WriteLine();
            Console.WriteLine("           �����˵�");
            Console.WriteLine("--------------------------------");
            Console.WriteLine("ID      ����      ����      ����");

            string changeNumOfProduct = File.ReadAllText(ProductsFilePath ,new UTF8Encoding(false));
            List<Product> change = JsonConvert.DeserializeObject<List<Product>>(changeNumOfProduct);
            
            LogProduct writeLog = new LogProduct();
            foreach (var item in bill)
            {
                string productId = item.Key; // ��ѯ ID
                int quantity = item.Value;   // ��ȡ����

                
                Product product = change.Find(p => p.Id.Equals(productId, StringComparison.CurrentCultureIgnoreCase));

                if (product != null)
                {
                    product.Total_sales_quantity +=quantity; //�������
                    product.Remaining -= quantity;  //���ٿ��
                    double totalPriceForProduct = product.Selling_price * quantity;  //���㵥��
                    sum += totalPriceForProduct;  //�����ܼ�
                    // д����־
                    writeLog.Time = DateTime.Now;
                    writeLog.Id = product.Id;
                    writeLog.Nums = quantity;
                    writeLog.WorkerId = operatingIdentity;
                    writeLog.ConstomerId = isVip;

                    Log.WriteLog(writeLog);//  ��־д��

                    Console.Write($"{productId,-8}"); // ID �п��Ϊ 6�������
                    
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

                    Console.Write($"{product.Selling_price.ToString("C"),-10}"); // ���۸�ʽ��Ϊ���ң��п��Ϊ 8�������
                    Console.WriteLine($"{quantity,3}"); // �����п��Ϊ 6�������
                }
                
            }
            string updatedJson = JsonConvert.SerializeObject(change, Formatting.Indented);
            File.WriteAllText(ProductsFilePath, updatedJson, new UTF8Encoding(false));
            Console.WriteLine("--------------------------------");
            // ����
            if (Discount!=1)
            {
                Console.WriteLine("����VIP�˿�:");
                Console.WriteLine(string.Format("�����ۿ�:{0,23:P}",Discount));
            }
            AddScore(Encrypt.Md5(isVip),(int)sum);
            Console.WriteLine("Ӧ����{0,26:F2}", sum);
            Console.WriteLine("ʵ����{0,26:F2}",sum*Discount);
            Console.WriteLine("--------------------------------");
            Console.WriteLine();
            return;
        }
        
        //�����Ʒ
        public static void AddProduct(string operatingIdentity)
        {
            string input="y";
            DateTime outtime;
            double price;
            int num;
            List<Product> add = new List<Product>(); //��λ��!!!!!!!!!!!!!!!!!!!!!!!

            if (File.Exists(ProductsFilePath))
            {
                string jsonExisting = File.ReadAllText(ProductsFilePath ,new UTF8Encoding(false));
                add = JsonConvert.DeserializeObject<List<Product>>(jsonExisting);
            }
            do
            {
                Product addproduct = new Product();
                Console.WriteLine("�������Ʒ��Ϣ:");
                Console.Write("Id(��λ����):"); //���п���ż�� Ҳ���Բ��� �Ͼ�ʵ����������������ͬ��ŵĲ�Ʒ������
                while (true)
                {
                    addproduct.Id = Console.ReadLine() ?? "";
                    if (int.TryParse(addproduct.Id,out int tmp) && addproduct.Id.Length == 4)
                        break;
                    else
                        Console.Write("���벻�Ϸ� ��������Id:");
                    
                }
                Console.Write("�������Ʒ����:");
                addproduct.Name = Console.ReadLine() ?? "";

                while (true)
                {
                    Console.Write("�������Ʒ��������(yyyy-mm-dd):");
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
                            Console.WriteLine("��������ڱ������ڻ���ڵ�ǰϵͳ����,����������!");
                            continue;
                        }
                    }
                    else
                        Console.WriteLine("��������ڸ�ʽ����ȷ,����������!");
                }
                    
                while (true)
                {
                    Console.Write("���������ʱ�� (yyyy-MM-dd):");
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
                            Console.WriteLine("��������ڱ���������������,����������!");
                            continue;
                        }
                    }
                    else
                        Console.WriteLine("��������ڸ�ʽ����ȷ,����������!");
                }
                    
                Console.Write("�������Ʒ����: ");
                addproduct.Category = Console.ReadLine() ?? "";

                while (true)
                {
                    Console.Write("�������Ʒ����: ");
                    if (double.TryParse(Console.ReadLine(), out price))
                    {
                        addproduct.Purchase_price = price;
                        break;
                    }
                    else
                    {
                        Console.WriteLine("����Ƿ�!��������!");
                        continue;
                    }
                }
                    
                while (true)
                {
                    Console.Write("�������Ʒ�ۼ�: ");
                    if (double.TryParse(Console.ReadLine(), out price))
                    {
                        addproduct.Selling_price = price;
                        break;
                    }
                    else
                    {
                        Console.WriteLine("����Ƿ�!��������!");
                        continue;
                    }
                }
                
                while (true)
                {
                    Console.Write("�������Ʒ����:");
                    input = Console.ReadLine() ?? "";
                    if (int.TryParse(input,out num))
                    {
                        addproduct.Total_purchase_quantity += num;
                        break;
                    }
                    else
                    {
                        Console.WriteLine("����Ƿ�!��������!");
                        continue;
                    }
                }
                //��������
                addproduct.Gross_profit_per_unit = Math.Round(addproduct.Selling_price - addproduct.Purchase_price,2);
                add.Add(addproduct);
                //�����־
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


                Console.Write("�Ƿ����¼��(y/n): ");
                input = Console.ReadLine() ?? "";



            } while (input.Equals("y",StringComparison.CurrentCultureIgnoreCase));
            //д���ļ�
            string jsonIn = JsonConvert.SerializeObject(add, Formatting.Indented);

            File.WriteAllText(ProductsFilePath, jsonIn , new UTF8Encoding(false));


            Console.WriteLine("��Ʒ��ӳɹ�!");
        }

        //�ֵ����Ƿ����        
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

        //ȷ����ݺ��ۿ�
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
        
        //�쿨
        public static void CreatVipCostomer()
        {
            string input = null;
            //���л��ļ�����
            string CostomerFile = File.ReadAllText(CostomerFilePath ,new UTF8Encoding(false));
            List<Property> OriginalCostomer = JsonConvert.DeserializeObject<List<Property>>(CostomerFile);     
            //�������
            Property additem = new Property();
            do
            {
                Console.WriteLine("������8λ���ڵ����ִ���VIP�˻�,�˳�������break");
                Console.Write("�����봴���˻�ID:");

                input = Console.ReadLine() ?? "";
                if (input.Equals("break",StringComparison.CurrentCultureIgnoreCase))
                    return ;

                if (OriginalCostomer.Any(c => c.Md5 == Encrypt.Md5(input)))
                {
                    Console.WriteLine("��ǰID�ѱ�ʹ�� �����!");
                    Console.WriteLine("������������������������������������������������������������������������������������������������������������");
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
                    Console.WriteLine("��������˻�ID���Ϸ� ����������8λ���ڵ�����!");
                    Console.WriteLine("������������������������������������������������������������������������������������������������������������");
                    continue;
                }
            } while (true);

            string updatedJson = JsonConvert.SerializeObject(OriginalCostomer, Formatting.Indented);
            File.WriteAllText(CostomerFilePath, updatedJson , new UTF8Encoding(false));
            Console.WriteLine("�˻���ӳɹ�");
            
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
            //��һ��λ��Ϊ�� �ڶ���λ��Ϊ��
            //yyyy  yyyy-mm
            string[] fileName = new string[2];
            fileName[0] = DateTime.Now.ToString("yyyy"); //��
            fileName[1] = DateTime.Now.ToString("MM");  //��
            return fileName;
        }

        static string fileDate = Path.Combine("..", "..","..", "Data", "sellLogBuyDate");
        static bool SellDataLog(Dictionary<string,int> bill ,string Identity ,List<Product> hava, string constomerId)
        {
            // sellLog = new List<LogAddProduct>();
            string filePath = fileDate; 
            string[] dataCreat = NowData();

            if (!Directory.Exists(Path.Combine(fileDate, dataCreat[0])))// �����ļ���
            {
                Directory.CreateDirectory(Path.Combine(fileDate,dataCreat[0]));//�������ļ���
                filePath = Path.Combine(filePath,dataCreat[0],dataCreat[1] + ".json"); //ָ����json
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
                filePath = Path.Combine(fileDate,dataCreat[0],dataCreat[1] + ".json"); //ָ�����ļ�
                if (!Directory.Exists(Path.Combine(filePath))) // ����json
                {
                    Directory.CreateDirectory(filePath);// ����json
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
                    //д���ļ�
                }
                else// �����������ļ�
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
            //д��
            string init = JsonConvert.SerializeObject(logs, Formatting.Indented);
            File.WriteAllText(logFilePath, init ,new UTF8Encoding(false));
            //Console.WriteLine("��־д��ɹ� �ǵ�ɾ��");
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
            //д��
            string init = JsonConvert.SerializeObject(logs, Formatting.Indented);
            File.WriteAllText(logFilePath, init ,new UTF8Encoding(false));
            //Console.WriteLine("��־д��ɹ� �ǵ�ɾ��");
        }
    }

    class Encrypt //MD5����
    {
        public static string Md5(string original)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] hashBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(original));
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2")); // ʹ��Сдʮ�����Ʊ�ʾ
                }
                return sb.ToString();
            }
        }
    }
}