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
                Console.WriteLine("       请选择操作: ");
                Console.WriteLine("               0.身份信息修改    ");
                Console.WriteLine("               1.商品信息修改");               
                Console.WriteLine("               2.商品信息查看");
                Console.WriteLine("               3.查询商品");
                Console.WriteLine("               4.产看日志");
                Console.WriteLine("               o.返回上一级");
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
                    Console.WriteLine("重新输入! ");
                }
                Console.Clear();
            } while (true);
        }
        
        
        //添加用户
        class ChangeDate
        {
            public static void IdentityChose()
            {
                // string input;
                ConsoleKeyInfo keyInfo;
                do
                {
                    Console.WriteLine("------------------------------------------------------");
                    Console.WriteLine("               请选择对身份进行的操作");
                    Console.WriteLine("               0.添加  1.删除  o.返回上一级");
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
                    Console.WriteLine("               请选择添加身份类型:");
                    Console.WriteLine("               A.管理员");
                    Console.WriteLine("               W.收银员");
                    Console.WriteLine("               B.结束程序");
                    Console.WriteLine("------------------------------------------------------");

                    // string input = Console.ReadLine() ?? "";
                    keyInfo = Console.ReadKey();
                    if (keyInfo.Key == ConsoleKey.A)
                    {
                        Console.Clear();
                        Console.WriteLine("您选择了身份 [管理员] ");
                        AddInfo((char)keyInfo.Key);
                    }
                    else if (keyInfo.Key == ConsoleKey.W)
                    {
                        Console.Clear();
                        Console.WriteLine("您选择了身份 [收银员] ");
                        AddInfo((char)keyInfo.Key);
                    }
                    else if (keyInfo.Key == ConsoleKey.B)
                    {
                        Console.Clear();
                        break;
                    }
                    else
                    {
                        Console.WriteLine("无效选择 重新输入:");
                    }
                }
            }

            //添加用户信息
            static void AddInfo(char IDtype)
            {
                Console.WriteLine("------------------------------------------------------");
                Console.WriteLine("               请输入名字和密码请以空格隔开");
                Console.WriteLine("------------------------------------------------------");
                string input = Console.ReadLine() ?? "";
                string[] parts = input.Split(' ');
                if (parts.Length != 2)
                {
                    Console.Clear();
                    Console.WriteLine("格式有误,重新输入:");
                    return; 
                }
                IdentityData addtmp = new IdentityData();
                addtmp.Identity = (IDtype=='A') ? "admin" : "worker" ;    
                addtmp.Name = parts[0];
                addtmp.key = parts[1];    
                WriteIntoFile(addtmp);        
            }

            //添加到文件
            static void WriteIntoFile(IdentityData data)
            {
                string IdentityFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, IdentityFileName);
                List<IdentityData> identityList = JsonConvert.DeserializeObject<List<IdentityData>>(File.ReadAllText(IdentityFilePath,new UTF8Encoding(false)));
                // 将新的身份信息添加到现有的身份信息列表中
                identityList.Add(data);
                // 序列化
                string json = JsonConvert.SerializeObject(identityList, Formatting.Indented); 
                // 反序列化
                File.WriteAllText(IdentityFilePath, json ,new UTF8Encoding(false));
                Console.Clear();
                Console.WriteLine("\\\\添加成功////");
                Chose();
            }

            //删除用户
            static void DelIdentity()
            {
                string IdentityFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, IdentityFileName);

                // string jsonContent = File.ReadAllText(IdentityFilePath);  //序列化
                List<IdentityData> identityList = JsonConvert.DeserializeObject<List<IdentityData>>(File.ReadAllText(IdentityFilePath,new UTF8Encoding(false)));  //反序列化

                Console.WriteLine("------------------------------------------------------");
                do
                {
                    foreach (var item in identityList) //列出所有数据
                    {
                        Console.WriteLine($"Identiy:{item.Identity.PadRight(7)} Name:{item.Name.PadRight(10)} Key:{item.key.PadRight(6)}");
                    }
                    Console.WriteLine("请输入删除用户的名称和密码: (over.终止删除)");
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
                        Console.WriteLine("数据错误,请重新输入!");
                        continue;
                    }
                    var delItem = identityList.FirstOrDefault(x => x.Name == delData[0] && x.key == delData[1]);
                    if (delItem!=null)
                    {
                        Console.Clear();
                        identityList.Remove(delItem);
                        File.WriteAllText(IdentityFilePath, JsonConvert.SerializeObject(identityList,Formatting.Indented) ,new UTF8Encoding(false));
                        Console.WriteLine($"成功移除用户{delData[0]}");
                    }
                        
                    else
                        Console.WriteLine($"{delData[0]}用户不存在！");
                    Console.WriteLine("请选择接下来的操作: 0.继续删除用户 任意键:返回上一界面");
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
                    Console.WriteLine($"Id:{item.Id} 品名:{item.Name}");
                    Console.WriteLine($"生产日期：{item.Production_date}    过期日期：{item.Expiration_date}");
                    Console.WriteLine($"进价:{item.Purchase_price}   售价:{item.Selling_price}");
                    Console.WriteLine($"数量:{item.Remaining}");
                    Console.WriteLine("──────────────────────────────────────────────────────");
                }
                string input;
                // string[] input = new string[];
                string modifiedContent;
                double newnum;
                do
                {
                    Console.WriteLine("over. 返回上一级");
                    Console.Write("请输入需要修改的产品编码:");
                    input = Console.ReadLine() ?? "";
                    Worker.Product product =products.Find(p => p.Id.Equals(input, StringComparison.CurrentCultureIgnoreCase));
                    if (input.Equals("over", StringComparison.CurrentCultureIgnoreCase))
                    {
                        Console.Clear();
                        return;
                    }
                    if (product == null)
                    {
                        Console.WriteLine("输入不存在 请重新输入!");
                        continue;
                    }
                    else
                    {
                        Console.WriteLine($"Id: {product.Id} 品名(Name):{product.Name}");
                        Console.WriteLine($"生产日期(Production_date): {product.Production_date}    过期日期(Expiration_date): {product.Expiration_date}");
                        Console.WriteLine($"进价(Purchase_price): {product.Purchase_price}   售价(Selling_price): {product.Selling_price}");
                        Console.WriteLine($"数量(Remaining): {product.Remaining}");
                        Console.WriteLine("──────────────────────────────────────────────────────");
                        Console.WriteLine("请输入：需要修改的条目及内容 使用空格隔开");
                        input = Console.ReadLine() ?? "";
                        string[] contentNum = input.Split(" ");
                        if (contentNum.Length!=2)
                        {
                            Console.WriteLine("输入不存在! 重新输入!");
                            continue;
                        }
                        // 获取用户输入的属性名称和新值
                        string propertyName = contentNum[0];
                        string newValue = contentNum[1];
                        // 使用反射获取产品类的属性
                        var property = typeof(Worker.Product).GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                        if (property == null)
                        {
                            Console.WriteLine($"属性 {propertyName} 不存在!");
                            continue;
                        }
                        // 将新值转换为与属性类型相匹配的类型
                        object convertedValue = Convert.ChangeType(newValue, property.PropertyType);

                        // 设置产品对象的属性值
                        property.SetValue(product, convertedValue);

                        // 输出修改后的产品信息
                        Console.WriteLine($"属性 {propertyName} 修改为 {newValue}。");

                        string updatedProductsJson = JsonConvert.SerializeObject(products, Formatting.Indented);
                        File.WriteAllText(productsFilePath, updatedProductsJson, new UTF8Encoding(false));

                        Console.WriteLine("产品数据已更新并保存到文件。");
                    }
                } while (true);


            }

        }

    }

}

