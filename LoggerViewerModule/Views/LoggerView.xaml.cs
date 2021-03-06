﻿using LoggerViewerModule.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LoggerViewerModule.Views
{
    /// <summary>
    /// Логика взаимодействия для LoggerView.xaml
    /// </summary>
    public partial class LoggerView : UserControl
    {
        public LoggerView(LoggerViewModel vm)
        {
            DataContext = vm;
            InitializeComponent();

            // Scroll to end on new log entry
            LogTextBox.TextChanged += (o, e) =>
            {
                (o as TextBox)?.ScrollToEnd();
            };
        }
    }
}
