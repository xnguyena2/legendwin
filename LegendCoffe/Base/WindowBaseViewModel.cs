
using System;
using System.Windows;
using System.Windows.Input;

namespace LegendCoffe
{
    public class WindowBaseViewModel : BaseViewModel
    {

        private Window mWindow;
        private int mBorderThickness = 6;
        private int mOuterMarginDropShadown = 0;
        private int mOuterMarginDropShadownMaximize = 6;
        private int mWindowsRadius = 10;
        private int mTitelBarHeight = 52;
        private bool isNormal = true;

        public Thickness ResizeBorderThickness { get { return new Thickness(mBorderThickness); } }
        public Thickness OuterMarginDropShadown
        {
            get
            {
                return mWindow.WindowState == WindowState.Maximized ? new Thickness(mOuterMarginDropShadownMaximize) : new Thickness(mOuterMarginDropShadown);
            }
        }

        public CornerRadius WindownRadius
        {
            get
            {
                return new CornerRadius(mWindowsRadius);
            }
        }

        public int TitleBarHeight { get { return mTitelBarHeight; } }
        public GridLength TitleBarGridHeight { get { return new GridLength(mTitelBarHeight + mBorderThickness); } }



        public ICommand MinimazeCommand { get; set; }
        public ICommand MaximazeCommand { get; set; }
        public ICommand CloseCommand { get; set; }

        public WindowBaseViewModel(Window window)
        {
            mWindow = window;
            mWindow.StateChanged += MWindow_StateChanged;

            MinimazeCommand = new RelayCommand(() => { mWindow.WindowState = WindowState.Minimized; });
            MaximazeCommand = new RelayCommand(() =>
            {
                if (isNormal)
                {
                    MaximazeState();
                }
                else
                {
                    NormalState();
                }
            });
            CloseCommand = new RelayCommand(() =>
            {
                //window.Hide();
                closing_Window();
            });
        }

        void closing_Window()
        {
            Console.WriteLine("Closing");
            Logout();
            mWindow.Close();
        }

        public static void Logout()
        {
            Console.WriteLine("Closing");
        }


        private void MWindow_StateChanged(object sender, EventArgs e)
        {
            OnPropertyChagned(nameof(ResizeBorderThickness));
            OnPropertyChagned(nameof(OuterMarginDropShadown));
            OnPropertyChagned(nameof(ResizeBorderThickness));
        }

        private void MaximazeState()
        {
            mWindow.Width = System.Windows.SystemParameters.PrimaryScreenWidth;
            mWindow.Height = System.Windows.SystemParameters.PrimaryScreenHeight;
            mWindow.Left = 0;
            mWindow.Top = 0;
            mWindow.WindowState = WindowState.Normal;
            mWindow.ResizeMode = ResizeMode.NoResize;
            isNormal = false;
        }

        private void NormalState()
        {
            mWindow.Width = System.Windows.SystemParameters.PrimaryScreenWidth - 100;
            mWindow.Height = System.Windows.SystemParameters.PrimaryScreenHeight - 100;
            mWindow.Left = 50;
            mWindow.Top = 50;
            mWindow.WindowState = WindowState.Normal;
            mWindow.ResizeMode = ResizeMode.CanResize;
            isNormal = true;
        }
    }
}
