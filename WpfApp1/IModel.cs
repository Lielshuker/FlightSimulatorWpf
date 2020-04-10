﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Microsoft.Maps.MapControl.WPF;

namespace FlightSimulatorWpf
{
    public interface IModel : INotifyPropertyChanged
    {
        double HeadingDeg { get; /*set;*/ }
        double VerticalSpeed { get; /*set;*/ }
        double GroundSpeed { get; /*set;*/ }
        double IndicatedSpeed { get; /*set;*/ }
        double GpsAltitude { get;/*set;*/ }
        double InternalRoll { get;/*set;*/ }
        double InternalPitch { get; /*set;*/ }
        double AltimeterAltitude { get; /*set;*/ }
        double Latitude { get; /**set;*/ }
        double Longitude { get; /*set;*/ }
        double Throttle { get; set; }
        double Aileron { get; set; }
        double Elevator { get; set; }
        double Rudder { get; set; }
        Location AirPlaneLocation { get; }
        void NotifyPropertyChanged(string PropertyName);

        void connect(string ip, string port);
        void connect();
        void disconnect();
        void start();

        void moveJoystick(double elevator, double rudder);
        void updateSliders(double throttle, double aileron);
    }
}