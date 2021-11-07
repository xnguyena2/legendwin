using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace LegendCoffe
{
    public class Product : BaseViewModel
    {
        public string ProductName
        {
            set;get;
        }
        public ICommand RemoveCMD { set; get; }

        public Product(string name, Action<Product> remove)
        {
            ProductName = name;
            RemoveCMD = new RelayCommand(() =>
            {
                remove(this);
            });
        }
    }
}
