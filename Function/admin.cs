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
            string judge ;
            do
            {
                Console.WriteLine("------------------------------------------------------");
                Console.WriteLine("请选择操作:(over.返回上一级)");
                Console.WriteLine("0.身份添加 1.身份删除 2.商品数据查看 log.产看日志");
                judge = Console.ReadLine() ?? "";
                if (judge.Equals("0",StringComparison.CurrentCultureIgnoreCase))
                {
                    AddIdentity();
                }
                else if (judge.Equals("1",StringComparison.CurrentCultureIgnoreCase))
                {
                    DelIdentity();
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
        static void AddIdentity()  
        {
            char type;
            while (true)
            {
                Console.WriteLine("------------------------------------------------------");
                Console.WriteLine("请选择添加身份类型: a.管理员  w.收银员 over.结束程序");
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

    }

}