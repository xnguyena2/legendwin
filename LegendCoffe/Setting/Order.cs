using System;
using System.Collections.Generic;
using System.Text;

namespace LegendCoffe
{
    public class Order
    {
        public string tableName { set; get; }

        public Dictionary<string, int> listProduct { set; get; }
    }
}
