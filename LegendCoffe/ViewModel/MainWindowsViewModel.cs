using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace LegendCoffe
{
    public class MainWindowsViewModel : WindowBaseViewModel
    {

        public ObservableCollection<Product> ListProduct
        {
            set;
            get;
        } = new ObservableCollection<Product>();

        public ObservableCollection<OrderItem> ListOrder
        {
            set;
            get;
        } = new ObservableCollection<OrderItem>();

        public string ipLabel
        {
            set { }
            get
            {
                return "IP: " + GetLocalIPAddress();
            }
        }

        public string tableNo
        {
            set;
            get;
        }

        public string newProduct { set; get; }


        public ICommand CloseNotificationCMD { set; get; }
        public ICommand AddCMD { set; get; }

        public ICommand SaveSettingCMD { set; get; }


        public MainWindowsViewModel(Window window) : base(window)
        {
            AppConfig config = AppConfig.LoadConfig();
            tableNo = config.tableNo.ToString();
            ObservableCollection<Product> ps = new ObservableCollection<Product>();

            foreach (string p in config.listProduct)
            {
                ps.Add(new Product(p, pp =>
                {
                    ListProduct.Remove(pp);
                }));
            }

            ListProduct = ps;

            CloseNotificationCMD = new RelayCommand(() =>
            {
            });

            SaveSettingCMD = new RelayCommand(() =>
            {
                List<string> ps = new List<string>();
                foreach (Product p in ListProduct)
                {
                    ps.Add(p.ProductName);
                }
                new AppConfig(tableNo, ps).SaveConfig();
            });

            AddCMD = new RelayCommand(() =>
            {
                Product p = new Product(newProduct, p =>
                {
                    ListProduct.Remove(p);
                });
                ListProduct.Add(p);
                newProduct = "";
            });

            new MySocket(msg =>
            {
                switch (msg)
                {
                    case "PING":
                        return JsonConvert.SerializeObject(AppConfig.CurrentConfig);
                    default:
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            ListOrder.Add(new OrderItem(msg, p =>
                            {
                                ListOrder.Remove(p);
                            }));
                        });
                        return "SUCCESS";
                }
            }).StartServer();
        }

        public string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());

            List<string> listIP = new List<string>();
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    listIP.Add(ip.ToString());
                }
            }
            return string.Join(", ", listIP);
        }
    }
}
