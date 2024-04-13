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
                Console.WriteLine("请选择操作:(over.返回上一级)");
                Console.WriteLine("0.身份信息修改 1.商品信息修改 2.商品信息查看 log.产看日志");
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
                    Console.WriteLine("输入无效:");
                }
            } while (true);
        }
        
        
        //添加用户
        class ChangeDate
        {
            public static void IdentityChose()
            {
                string input;
                do
                {
                    Console.WriteLine("0.添加  1.删除  return,返回上一级");
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
                    Console.WriteLine("请选择添加身份类型: a.管理员  w.收银员 b.结束程序");
                    string input = Console.ReadLine() ?? "";
                    if (input.Equals("over",StringComparison.CurrentCultureIgnoreCase))
                    {
                        Chose();
                    }
                    if (input.Length != 1)
                    {
                        Console.WriteLine("无效选择 重新输入:");    
                        continue;
                    }
                    type = (char)input[0];
                    if (type == 'a')
                    {
                        Console.WriteLine("您选择了身份 [管理员] ");
                        AddInfo(type);
                    }
                    else if (type == 'w')
                    {
                        Console.Write("您选择了身份 [收银员] ");
                        AddInfo(type);
                    }
                    else if (type == 'b')
                    {
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
                Console.WriteLine("请输入名字和密码请以空格隔开:");
                string input = Console.ReadLine() ?? "";
                string[] parts = input.Split(' ');
                if (parts.Length != 2)
                {
                    Console.WriteLine("格式有误,重新输入:");
                    return; 
                }
                IdentityData addtmp = new IdentityData();
            
                addtmp.Identity = (IDtype=='a') ? "admin" : "worker" ;    
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
                    Console.Write("请输入删除用户的名称和密码: (over.终止删除)");
                    string delInput = Console.ReadLine();
                    if (delInput.Equals("over",StringComparison.CurrentCultureIgnoreCase))
                    {
                        return;
                    }
                    string[] delData = delInput.Split(' ');
                    if (delData.Length != 2)
                    {
                        Console.WriteLine("数据错误,请重新输入!");
                        continue;
                    }
                    var delItem = identityList.FirstOrDefault(x => x.Name == delData[0] && x.key == delData[1]);
                    if (delItem!=null)
                    {
                        identityList.Remove(delItem);
                        File.WriteAllText(IdentityFilePath, JsonConvert.SerializeObject(identityList,Formatting.Indented) ,new UTF8Encoding(false));
                        Console.WriteLine($"成功移除用户{delData[0]}");
                    }
                        
                    else
                        Console.WriteLine($"{delData[0]}用户不存在！");
                    Console.WriteLine("请选择接下来的操作: 0.继续删除用户 任意键:返回上一界面");
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

                        // 确保属性存在
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

                        // 将修改后的产品列表重新序列化为 JSON 字符串
                        string updatedProductsJson = JsonConvert.SerializeObject(products, Formatting.Indented);

                        // 将修改后的产品数据写入文件
                        File.WriteAllText(productsFilePath, updatedProductsJson, new UTF8Encoding(false));

                        Console.WriteLine("产品数据已更新并保存到文件。");
                    }
                } while (true);


            }

        }

    }

}