﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
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

namespace FlightSimulatorWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();


            joystickView.DataContext = (Application.Current as App).Simulator_vm.vm_joystick;
            dashBoardView.DataContext = (Application.Current as App).Simulator_vm.vm_dashBoard;
            mapView.DataContext = (Application.Current as App).Simulator_vm.vm_map;
            messagesView.DataContext = (Application.Current as App).Simulator_vm.vm_message;

            /*
                        model.Connect(ip, port);
                        model.Start();*/
        }

    }
}