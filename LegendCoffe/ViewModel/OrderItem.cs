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
            Order orderDetail = JsonConvert.DeserializeObject<Order>(detail);
            foreach (var kv in orderDetail.listProduct)
            {
                output.Add(string.Format("{0}: ({1})", kv.Key, kv.Value));
            }
            ProductName = string.Format("{0}: {1}", orderDetail.tableName, string.Join(", ", output));
            RemoveCMD = new RelayCommand(() =>
            {
                remove(this);
            });
        }

    }
}
