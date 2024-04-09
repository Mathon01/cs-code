using Newtonsoft.Json;

namespace Judge
{
    class IdentityData
    {
        public string Identity { get; set; }
        public string Name { get; set; }
        public string key { get; set; }
    }
    class IdentityJudge
    {
        static string IdentityFilePath = Path.Combine("..", "..", "..", "Data", "Identity.json");
        //static string IdentityFilePath = "C:\\Users\\Matho\\Desktop\\sup\\projecttry\\Data\\Identity.json" ;
        public static List<IdentityData> AllMembers = JsonConvert.DeserializeObject<List<IdentityData>>(File.ReadAllText(IdentityFilePath));
        
        //
        public static string IsWorkerOrAdmin(string name ,string key)
        {
            foreach (IdentityData member in AllMembers)
            {
                if (member.Name==name && member.key==key)
                {
                    return member.Identity;
                }
            }
            return "worng";
        }

    }
}