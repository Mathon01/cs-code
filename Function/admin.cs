using System.Reflection;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using AdminShow;
using mainProcess;
using Newtonsoft.Json;
using Worker;

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
            string judge ;
            do
            {
                Console.WriteLine("------------------------------------------------------");
                Console.WriteLine("��ѡ�����:(over.������һ��)");
                Console.WriteLine("0.�����Ϣ�޸� 1.��Ʒ��Ϣ�޸� 2.��Ʒ��Ϣ�鿴 log.������־");
                judge = Console.ReadLine() ?? "";
                if (judge.Equals("0",StringComparison.CurrentCultureIgnoreCase))
                {
                    ChangeDate.IdentityChose();
                }
                else if (judge.Equals("1",StringComparison.CurrentCultureIgnoreCase))
                {
                    ChangeDate.ProductsData();
                }
                else if (judge.Equals("2",StringComparison.CurrentCultureIgnoreCase))
                {
                    ShowProduct.Chose();
                }
                else if (judge.Equals("over",StringComparison.CurrentCultureIgnoreCase))
                {
                    Program.Chose();
                }
                else if (judge.Equals("log",StringComparison.CurrentCultureIgnoreCase))
                {
                    LogShow.LogChose();
                }   
                else
                {
                    Console.WriteLine("������Ч:");
                }
            } while (true);
        }
        
        
        //����û�
        class ChangeDate
        {
            public static void IdentityChose()
            {
                string input;
                do
                {
                    Console.WriteLine("0.���  1.ɾ��  return,������һ��");
                    input = Console.ReadLine() ?? "";
                    if (input.Equals("return", StringComparison.InvariantCultureIgnoreCase))
                    {
                        return;
                    }
                    if (input.Equals("0", StringComparison.CurrentCultureIgnoreCase))
                    {
                        AddIdentity();
                    }
                    else if (input.Equals("1", StringComparison.CurrentCultureIgnoreCase))
                    {
                        DelIdentity();
                    }
                    else
                    {
                        //Console.Clear();
                        continue;
                    }
                } while (true);
            }
            static void AddIdentity()  
            {
                char type;
                while (true)
                {
                    Console.WriteLine("------------------------------------------------------");
                    Console.WriteLine("��ѡ������������: a.����Ա  w.����Ա b.��������");
                    string input = Console.ReadLine() ?? "";
                    if (input.Equals("over",StringComparison.CurrentCultureIgnoreCase))
                    {
                        Chose();
                    }
                    if (input.Length != 1)
                    {
                        Console.WriteLine("��Чѡ�� ��������:");    
                        continue;
                    }
                    type = (char)input[0];
                    if (type == 'a')
                    {
                        Console.WriteLine("��ѡ������� [����Ա] ");
                        AddInfo(type);
                    }
                    else if (type == 'w')
                    {
                        Console.Write("��ѡ������� [����Ա] ");
                        AddInfo(type);
                    }
                    else if (type == 'b')
                    {
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
                Console.WriteLine("���������ֺ��������Կո����:");
                string input = Console.ReadLine() ?? "";
                string[] parts = input.Split(' ');
                if (parts.Length != 2)
                {
                    Console.WriteLine("��ʽ����,��������:");
                    return; 
                }
                IdentityData addtmp = new IdentityData();
            
                addtmp.Identity = (IDtype=='a') ? "admin" : "worker" ;    
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
                    Console.Write("������ɾ���û������ƺ�����: (over.��ֹɾ��)");
                    string delInput = Console.ReadLine();
                    if (delInput.Equals("over",StringComparison.CurrentCultureIgnoreCase))
                    {
                        return;
                    }
                    string[] delData = delInput.Split(' ');
                    if (delData.Length != 2)
                    {
                        Console.WriteLine("���ݴ���,����������!");
                        continue;
                    }
                    var delItem = identityList.FirstOrDefault(x => x.Name == delData[0] && x.key == delData[1]);
                    if (delItem!=null)
                    {
                        identityList.Remove(delItem);
                        File.WriteAllText(IdentityFilePath, JsonConvert.SerializeObject(identityList,Formatting.Indented) ,new UTF8Encoding(false));
                        Console.WriteLine($"�ɹ��Ƴ��û�{delData[0]}");
                    }
                        
                    else
                        Console.WriteLine($"{delData[0]}�û������ڣ�");
                    Console.WriteLine("��ѡ��������Ĳ���: 0.����ɾ���û� �����:������һ����");
                    delInput = Console.ReadLine();
                    if (delInput.Equals("0",StringComparison.CurrentCultureIgnoreCase))
                        continue;
                    else
                        Chose();
                    
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

                        // ȷ�����Դ���
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

                        // ���޸ĺ�Ĳ�Ʒ�б��������л�Ϊ JSON �ַ���
                        string updatedProductsJson = JsonConvert.SerializeObject(products, Formatting.Indented);

                        // ���޸ĺ�Ĳ�Ʒ����д���ļ�
                        File.WriteAllText(productsFilePath, updatedProductsJson, new UTF8Encoding(false));

                        Console.WriteLine("��Ʒ�����Ѹ��²����浽�ļ���");
                    }
                } while (true);


            }

        }

    }

}