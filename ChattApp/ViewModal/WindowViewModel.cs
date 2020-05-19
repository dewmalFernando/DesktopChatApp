using ChattApp.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ChattApp.ViewModal
{
    /// <summary>
    /// View Model for the custom flat Window
    /// </summary>
    class WindowViewModel : BaseViewModel
    {

        #region Private Member

        /// <summary>
        /// The window this view model controls
        /// </summary>
        private Window mWindow;

        /// <summary>
        /// The margin around the window to allow for a drop shadow
        /// </summary>
        private int mOuterMarginSize = 10;

        /// <summary>
        /// The radius of the edges of the window
        /// </summary>
        private int mWindowRadius = 10;

        /// <summary>
        /// The position of the window dock
        /// </summary>
        private WindowDockPosition mDockPosition;
        #endregion

        #region Public Properties



        /// <summary>
        /// The smallest width the window can go to 
        /// </summary>
        public double WindowMinimumWidth { get; set; } = 400;


        /// <summary>
        /// The smallest height the window can go to 
        /// </summary>
        public double WindowMinimumHeight { get; set; } = 400;

        public bool Borderless { get { return(mWindow.WindowState == WindowState.Maximized || mDockPosition != WindowDockPosition.Undocked); } }

        /// <summary>
        /// The size of the resize border around the window
        /// </summary>
        public int ResizeBorder { get { return Borderless ? 0 : 6; } }

        /// <summary>
        /// The size of the resize border around the window taking into account the outer margin
        /// </summary>
        public Thickness ResizeBorderThickness { get { return new Thickness(ResizeBorder + OuterMarginSize); } }

        /// <summary>
        /// The padding of the inner content fo the main window
        /// </summary>
        public Thickness InnerContentpadding { get; set; } = new Thickness(0);


        /// <summary>
        /// The margin around the window to allow for a drop shadow
        /// </summary>
        public int OuterMarginSize
        {
            get
            {
                return mWindow.WindowState == WindowState.Maximized ? 0 : mOuterMarginSize;
            }
            set
            {
                mOuterMarginSize = value;
            }
        }


        public Thickness OuterMarginSizeThikness { get { return new Thickness(OuterMarginSize ); } }
    

        /// <summary>
        /// The size of the resize border around the window
        /// </summary>
        public int WindowRadius
        {
            get
            {
                return mWindow.WindowState == WindowState.Maximized ? 0 : mWindowRadius;
            }
            set
            {
                mWindowRadius = value;
            }
        }

        public CornerRadius WindowCornerRadius { get { return new CornerRadius(WindowRadius); } }

        /// <summary>
        /// The header of the window
        /// </summary>
        public int TitleHeight { get; set; } = 42;

        public GridLength TitleHeightGridLength { get { return new GridLength(TitleHeight + ResizeBorder); } }

        /// <summary>
        /// The current page of the application
        /// </summary>
        public ApplicationPage CurrentPage { set; get; } = ApplicationPage.Login;

        #endregion

        #region Commands
        
        /// <summary>
        /// The command to minimize the window
        /// </summary>
        /// <param name="window"></param>
        public ICommand MinimizeCommand { get; set; }

        /// <summary>
        /// The command to maximize the window
        /// </summary>
        /// <param name="window"></param>
        public ICommand MaximizeCommand { get; set; }

        /// <summary>
        /// The command to close the windo 
        /// </summary>
        /// <param name="window"></param>
        public ICommand CloseCommand { get; set; }

        /// <summary>
        /// The command to show system menu
        /// </summary>
        /// <param name="window"></param>
        public ICommand MenuCommand { get; set; }
        #endregion

        #region Constructor

        public WindowViewModel(Window window)
        {
            mWindow = window;

            mWindow.StateChanged += (sender, e) =>
            {
                OnPropertyChanged(nameof(ResizeBorderThickness));
                OnPropertyChanged(nameof(OuterMarginSize));
                OnPropertyChanged(nameof(OuterMarginSizeThikness));
                OnPropertyChanged(nameof(WindowRadius));
                OnPropertyChanged(nameof(WindowCornerRadius));
            };


            MinimizeCommand = new RelayCommand(() => mWindow.WindowState = WindowState.Minimized);
            MaximizeCommand = new RelayCommand(() => mWindow.WindowState ^= WindowState.Maximized);
            CloseCommand = new RelayCommand(() => mWindow.Close());
            MenuCommand = new RelayCommand(() => SystemCommands.ShowSystemMenu(mWindow, GetMoustPosition()));

            //Fix window resizer issue
            var resizer = new WindowResizer(mWindow);
        }


        #endregion


        #region Private Helpers

        #region Solution 1: To get the current position of the mouse
        /*[DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]

        internal static extern bool GetCursorPos(ref Win32Point pt);

        [StructLayout(LayoutKind.Sequential)]
        internal struct Win32Point
        {
            public Int32 X;
            public Int32 Y;
        };*/

        /// <summary>
        /// Get the current mouse position
        /// </summary>
        /// <returns></returns>
        ///This Works fine
        /*private static Point GetMoustPosition()
        {
            Win32Point w32Mouse = new Win32Point();
            GetCursorPos(ref w32Mouse);
            return new Point(w32Mouse.X, w32Mouse.Y);
        }*/
        #endregion
        #region Solution 2: To get the current positoin of the mouse
        private Point GetMoustPosition()
        {
            // Position of the mouse ralative to the window
            var position = Mouse.GetPosition(mWindow);

            // Add the windoe position so its a "ToScreen"
            return new Point(position.X + mWindow.Left, position.Y + mWindow.Top);
        }
        #endregion
        #endregion

        
    }
}
