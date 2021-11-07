using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace LegendCoffe
{
    public class OrderItem : BaseViewModel
    {
        public string ProductName
        {
            set; get;
        }
        public ICommand RemoveCMD { set; get; }

        public OrderItem(String detail, Action<OrderItem> remove)
        {
            List<string> output = new List<string>();
            Dictionary<string, int> orderDetail = JsonConvert.DeserializeObject<Dictionary<string, int>>(detail);
            foreach (var kv in orderDetail)
            {
                output.Add(string.Format("{0}: ({1})", kv.Key, kv.Value));
            }
            ProductName = string.Join(", ", output);
            RemoveCMD = new RelayCommand(() =>
            {
                remove(this);
            });
        }

    }
}
