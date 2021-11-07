using PropertyChanged;
using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Windows;


namespace LegendCoffe
{
    [AddINotifyPropertyChangedInterface]
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };


        public Visibility Visibility = Visibility.Collapsed;

        public bool IsShow() => Visibility == Visibility.Visible;

        public void Show()
        {
            Visibility = Visibility.Visible;
        }

        public void Hide()
        {
            Visibility = Visibility.Collapsed;
        }


        public BaseViewModel()
        {

        }

        ///<summary>
        ///
        /// 
        ///</summary>
        public void OnPropertyChagned(string name)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }
}
