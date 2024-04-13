using System.Reflection;
using System.Text;
using AdminShow;
using mainProcess;
using Newtonsoft.Json;

namespace Admin
{
    class IdentityData
    {
        public string Identity { get; set; }
        public string Name { get; set; }
        public string key { get; set; }
    }
    static class Functions
    {
        
        static string IdentityFileName = Path.Combine("..", "..", "..", "Data", "Identity.json");
        public static void Chose()
        {
            ConsoleKeyInfo keyInfo;
            //string judge ;
            do
            {
                

                Console.WriteLine("------------------------------------------------------");
                Console.WriteLine("       ��ѡ�����: ");
                Console.WriteLine("               0.�����Ϣ�޸�    ");
                Console.WriteLine("               1.��Ʒ��Ϣ�޸�");               
                Console.WriteLine("               2.��Ʒ��Ϣ�鿴");
                Console.WriteLine("               3.��ѯ��Ʒ");
                Console.WriteLine("               4.������־");
                Console.WriteLine("               o.������һ��");
                Console.WriteLine("------------------------------------------------------");
                keyInfo = Console.ReadKey();
                

                if (keyInfo.Key == ConsoleKey.D0)
                {
                    Console.Clear();
                    ChangeDate.IdentityChose();
                }
                else if (keyInfo.Key == ConsoleKey.D1)
                {
                    Console.Clear();
                    ChangeDate.ProductsData();
                }
                else if (keyInfo.Key == ConsoleKey.D2)
                {
                    Console.Clear();
                    ShowProduct.Chose();
                }
                else if (keyInfo.Key == ConsoleKey.D3)
                {
                    Console.Clear();
                    ShowProduct.Findproduct();
                }
                else if (keyInfo.Key == ConsoleKey.D4)
                {
                    Console.Clear();
                    LogShow.LogChose();
                }
                else if (keyInfo.Key == ConsoleKey.O)
                {
                    Console.Clear();
                    Program.Chose();
                }
                else
                {
                    Console.WriteLine("��������! ");
                }
                Console.Clear();
            } while (true);
        }
        
        
        //����û�
        class ChangeDate
        {
            public static void IdentityChose()
            {
                // string input;
                ConsoleKeyInfo keyInfo;
                do
                {
                    Console.WriteLine("------------------------------------------------------");
                    Console.WriteLine("               ��ѡ�����ݽ��еĲ���");
                    Console.WriteLine("               0.���  1.ɾ��  o.������һ��");
                    Console.WriteLine("------------------------------------------------------");
                    //input = Console.ReadLine() ?? "";
                    keyInfo = Console.ReadKey();
                    // if (input.Equals("2", StringComparison.InvariantCultureIgnoreCase))
                    // {
                    //     return;
                    // }
                    // if (input.Equals("0", StringComparison.CurrentCultureIgnoreCase))
                    // {
                    //     AddIdentity();
                    // }
                    // else if (input.Equals("1", StringComparison.CurrentCultureIgnoreCase))
                    // {
                    //     DelIdentity();
                    // }
                    // else
                    // {
                    //     //Console.Clear();
                    //     continue;
                    // }
                    if (keyInfo.Key == ConsoleKey.D0)
                    {
                        Console.Clear();
                        AddIdentity();
                    }
                    else if (keyInfo.Key == ConsoleKey.D1)
                    {
                        Console.Clear();
                        DelIdentity();
                    }
                    else if (keyInfo.Key == ConsoleKey.O)
                    {
                        Console.Clear();
                        return;
                    }
                    else
                    {
                        Console.Clear();
                        continue;
                    }

                } while (true);
            }
            static void AddIdentity()  
            {
                ConsoleKeyInfo keyInfo;
                char type;
                while (true)
                {
                    Console.WriteLine("------------------------------------------------------");
                    Console.WriteLine("               ��ѡ������������:");
                    Console.WriteLine("               A.����Ա");
                    Console.WriteLine("               W.����Ա");
                    Console.WriteLine("               B.��������");
                    Console.WriteLine("------------------------------------------------------");

                    // string input = Console.ReadLine() ?? "";
                    keyInfo = Console.ReadKey();
                    if (keyInfo.Key == ConsoleKey.A)
                    {
                        Console.Clear();
                        Console.WriteLine("��ѡ������� [����Ա] ");
                        AddInfo((char)keyInfo.Key);
                    }
                    else if (keyInfo.Key == ConsoleKey.W)
                    {
                        Console.Clear();
                        Console.WriteLine("��ѡ������� [����Ա] ");
                        AddInfo((char)keyInfo.Key);
                    }
                    else if (keyInfo.Key == ConsoleKey.B)
                    {
                        Console.Clear();
                        break;
                    }
                    else
                    {
                        Console.WriteLine("��Чѡ�� ��������:");
                    }
                }
            }

            //����û���Ϣ
            static void AddInfo(char IDtype)
            {
                Console.WriteLine("------------------------------------------------------");
                Console.WriteLine("               ���������ֺ��������Կո����");
                Console.WriteLine("------------------------------------------------------");
                string input = Console.ReadLine() ?? "";
                string[] parts = input.Split(' ');
                if (parts.Length != 2)
                {
                    Console.Clear();
                    Console.WriteLine("��ʽ����,��������:");
                    return; 
                }
                IdentityData addtmp = new IdentityData();
                addtmp.Identity = (IDtype=='A') ? "admin" : "worker" ;    
                addtmp.Name = parts[0];
                addtmp.key = parts[1];    
                WriteIntoFile(addtmp);        
            }

            //��ӵ��ļ�
            static void WriteIntoFile(IdentityData data)
            {
                string IdentityFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, IdentityFileName);
                List<IdentityData> identityList = JsonConvert.DeserializeObject<List<IdentityData>>(File.ReadAllText(IdentityFilePath,new UTF8Encoding(false)));
                // ���µ������Ϣ��ӵ����е������Ϣ�б���
                identityList.Add(data);
                // ���л�
                string json = JsonConvert.SerializeObject(identityList, Formatting.Indented); 
                // �����л�
                File.WriteAllText(IdentityFilePath, json ,new UTF8Encoding(false));
                Console.Clear();
                Console.WriteLine("\\\\��ӳɹ�////");
                Chose();
            }

            //ɾ���û�
            static void DelIdentity()
            {
                string IdentityFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, IdentityFileName);

                // string jsonContent = File.ReadAllText(IdentityFilePath);  //���л�
                List<IdentityData> identityList = JsonConvert.DeserializeObject<List<IdentityData>>(File.ReadAllText(IdentityFilePath,new UTF8Encoding(false)));  //�����л�

                Console.WriteLine("------------------------------------------------------");
                do
                {
                    foreach (var item in identityList) //�г���������
                    {
                        Console.WriteLine($"Identiy:{item.Identity.PadRight(7)} Name:{item.Name.PadRight(10)} Key:{item.key.PadRight(6)}");
                    }
                    Console.WriteLine("������ɾ���û������ƺ�����: (over.��ֹɾ��)");
                    string delInput = Console.ReadLine();
                    if (delInput.Equals("over",StringComparison.CurrentCultureIgnoreCase))
                    {
                        Console.Clear();
                        return;
                    }
                    string[] delData = delInput.Split(' ');
                    if (delData.Length != 2)
                    {
                        Console.Clear();
                        Console.WriteLine("���ݴ���,����������!");
                        continue;
                    }
                    var delItem = identityList.FirstOrDefault(x => x.Name == delData[0] && x.key == delData[1]);
                    if (delItem!=null)
                    {
                        Console.Clear();
                        identityList.Remove(delItem);
                        File.WriteAllText(IdentityFilePath, JsonConvert.SerializeObject(identityList,Formatting.Indented) ,new UTF8Encoding(false));
                        Console.WriteLine($"�ɹ��Ƴ��û�{delData[0]}");
                    }
                        
                    else
                        Console.WriteLine($"{delData[0]}�û������ڣ�");
                    Console.WriteLine("��ѡ��������Ĳ���: 0.����ɾ���û� �����:������һ����");
                    delInput = Console.ReadLine();
                    if (delInput.Equals("0",StringComparison.CurrentCultureIgnoreCase))
                    {
                        Console.Clear();
                        continue;
                    }
                    else
                        return;
                    
                } while (true);
            }
            
            public static void ProductsData()
            {
                string productsFilePath = Path.Combine("..", "..", "..", "Data", "products.json");
                string productsFileString = File.ReadAllText(productsFilePath, new UTF8Encoding(false));
                List<Worker.Product> products = JsonConvert.DeserializeObject<List<Worker.Product>>(productsFileString);
                foreach (Worker.Product item in products)
                {
                    Console.WriteLine($"Id:{item.Id} Ʒ��:{item.Name}");
                    Console.WriteLine($"�������ڣ�{item.Production_date}    �������ڣ�{item.Expiration_date}");
                    Console.WriteLine($"����:{item.Purchase_price}   �ۼ�:{item.Selling_price}");
                    Console.WriteLine($"����:{item.Remaining}");
                    Console.WriteLine("������������������������������������������������������������������������������������������������������������");
                }
                string input;
                // string[] input = new string[];
                string modifiedContent;
                double newnum;
                do
                {
                    Console.WriteLine("over. ������һ��");
                    Console.Write("��������Ҫ�޸ĵĲ�Ʒ����:");
                    input = Console.ReadLine() ?? "";
                    Worker.Product product =products.Find(p => p.Id.Equals(input, StringComparison.CurrentCultureIgnoreCase));
                    if (input.Equals("over", StringComparison.CurrentCultureIgnoreCase))
                    {
                        Console.Clear();
                        return;
                    }
                    if (product == null)
                    {
                        Console.WriteLine("���벻���� ����������!");
                        continue;
                    }
                    else
                    {
                        Console.WriteLine($"Id: {product.Id} Ʒ��(Name):{product.Name}");
                        Console.WriteLine($"��������(Production_date): {product.Production_date}    ��������(Expiration_date): {product.Expiration_date}");
                        Console.WriteLine($"����(Purchase_price): {product.Purchase_price}   �ۼ�(Selling_price): {product.Selling_price}");
                        Console.WriteLine($"����(Remaining): {product.Remaining}");
                        Console.WriteLine("������������������������������������������������������������������������������������������������������������");
                        Console.WriteLine("�����룺��Ҫ�޸ĵ���Ŀ������ ʹ�ÿո����");
                        input = Console.ReadLine() ?? "";
                        string[] contentNum = input.Split(" ");
                        if (contentNum.Length!=2)
                        {
                            Console.WriteLine("���벻����! ��������!");
                            continue;
                        }
                        // ��ȡ�û�������������ƺ���ֵ
                        string propertyName = contentNum[0];
                        string newValue = contentNum[1];
                        // ʹ�÷����ȡ��Ʒ�������
                        var property = typeof(Worker.Product).GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                        if (property == null)
                        {
                            Console.WriteLine($"���� {propertyName} ������!");
                            continue;
                        }
                        // ����ֵת��Ϊ������������ƥ�������
                        object convertedValue = Convert.ChangeType(newValue, property.PropertyType);

                        // ���ò�Ʒ���������ֵ
                        property.SetValue(product, convertedValue);

                        // ����޸ĺ�Ĳ�Ʒ��Ϣ
                        Console.WriteLine($"���� {propertyName} �޸�Ϊ {newValue}��");

                        string updatedProductsJson = JsonConvert.SerializeObject(products, Formatting.Indented);
                        File.WriteAllText(productsFilePath, updatedProductsJson, new UTF8Encoding(false));

                        Console.WriteLine("��Ʒ�����Ѹ��²����浽�ļ���");
                    }
                } while (true);


            }

        }

    }

}

