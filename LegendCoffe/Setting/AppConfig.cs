using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LegendCoffe
{
    class AppConfig
    {
        public static AppConfig CurrentConfig;

        public int tableNo = 0;

        public List<string> listProduct = new List<string>();

        public AppConfig(string tableNo, List<string> products)
        {
            this.tableNo = int.Parse(tableNo);
            this.listProduct = products;
        }

        public void SaveConfig()
        {
            string fileName = Path.Combine(Path.GetTempPath(), "config.json");
            CurrentConfig = this;

            System.IO.File.WriteAllText(fileName, JsonConvert.SerializeObject(this));
        }

        public static AppConfig LoadConfig()
        {
            string fileName = Path.Combine(Path.GetTempPath(), "config.json");
            if (!File.Exists(fileName))
                return new AppConfig("0", new List<string>());
            CurrentConfig = JsonConvert.DeserializeObject<AppConfig>(File.ReadAllText(fileName));
            return CurrentConfig;
        }
    }
}
