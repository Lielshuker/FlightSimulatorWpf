﻿using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;


namespace FlightSimulatorWpf
{

	public class FlyData
	{
		public FlyData(String addres, String value)
		{
			this.addres = addres;
			this.value = value;
		}
		public String addres { get; set; }
		public String value { get; set; }
		public Mutex mutex = new Mutex();
	}
	class FlyModel : IModel
	{
		enum Error : int { notCanConnect, getErrFromServer, communicationSlowly, unepctedErr, communityProblemTryFix };
		enum variables : int { longitude, latitude, indicatedSpeed, gpsAltitude, internalRoll,
								internalPitch, altimeterAltitude, headingDeg, groundSpeed, verticalSpeed }
		class NotSuccessedTookWithServer : Exception { };
		private ITelnetClient telnetClient;
		private bool stop = false;
		private Thread updateThread;
		private Dictionary<string, FlyData> valueMap;
		public event PropertyChangedEventHandler PropertyChanged;
		private List<bool> errorMassage;
		private List<bool> errorVariables;
		//todo the problem: how i can update how value is err? and error limit
		public FlyModel(ITelnetClient telClient)
		{
			//read value
			valueMap = new Dictionary<string, FlyData>();
			valueMap.Add("longitude", new FlyData("/position/longitude-deg", "0"));
			valueMap.Add("latitude", new FlyData("/position/latitude-deg", "0"));
			valueMap.Add("indicatedSpeed", new FlyData("/instrumentation/airspeed-indicator/indicated-speed-kt", "0"));
			valueMap.Add("gpsAltitude", new FlyData("/instrumentation/gps/indicated-altitude-ft", "0"));
			valueMap.Add("internalRoll", new FlyData("/instrumentation/attitude-indicator/internal-roll-deg", "0"));
			valueMap.Add("internalPitch", new FlyData("/instrumentation/attitude-indicator/internal-pitch-deg", "0"));
			valueMap.Add("altimeterAltitude", new FlyData("/instrumentation/altimeter/indicated-altitude-ft", "0"));
			valueMap.Add("headingDeg", new FlyData("/instrumentation/heading-indicator/indicated-heading-deg", "0"));
			valueMap.Add("groundSpeed", new FlyData("/instrumentation/gps/indicated-ground-speed-kt", "0"));
			valueMap.Add("verticalSpeed", new FlyData("/instrumentation/gps/indicated-vertical-speed", "0"));
			// writh value 
			valueMap.Add("throttle", new FlyData("/controls/engines/current-engine/throttle", "0"));
			valueMap.Add("aileron", new FlyData("/controls/flight/aileron", "0"));
			valueMap.Add("elevator", new FlyData("/controls/flight/elevator", "0"));
			valueMap.Add("rudder", new FlyData("/controls/flight/rudder", "0"));
			this.errorMassage = new List<bool> { false, false, false, false, false}; 
			stop = false;
			this.telnetClient = telClient;
		}
		public void connect(string ip, string port) 
		{
			if(ip.Length == 0) {ip = ConfigurationManager.AppSettings["ip"].ToString(); }
			if(port.Length == 0) { port = ConfigurationManager.AppSettings["port"].ToString(); }
			this.telnetClient.connect(ip, port); 
		}
		public void connect() { 
			try { this.telnetClient.connect(); }
			catch {
				try { CommunityProblemTryFix = true; connect(); }
				catch (Exception) { NotCanConnect = true; /*check that this ok if not connect befor*/ disconnect(); }
			} 
		}
		public void disconnect()
		{
			stop = true;
			//todo not need stop the thread? 
			this.telnetClient.disconnect();
		}
		public void start()
		{
			this.updateThread = new Thread(delegate ()
			{
				while (!stop)
				{
					HeadingDeg = ReadFromTelnetClient("headingDeg");
					VerticalSpeed = ReadFromTelnetClient("verticalSpeed");
					GroundSpeed = ReadFromTelnetClient("groundSpeed");
					IndicatedSpeed = ReadFromTelnetClient("indicatedSpeed");
					GpsAltitude = ReadFromTelnetClient("gpsAltitude");
					InternalRoll = ReadFromTelnetClient("internalRoll");
					InternalPitch = ReadFromTelnetClient("internalPitch");
					AltimeterAltitude = ReadFromTelnetClient("altimeterAltitude");
					Latitude = ReadFromTelnetClient("latitude");
					Longitude = ReadFromTelnetClient("longitude");
					Thread.Sleep(250);
				}
			});
			this.updateThread.Start();
		}

		public Double ReadFromTelnetClient(String variable)
		{
			try
			{
				Double value =  Convert.ToDouble(this.telnetClient.read(this.valueMap[variable].addres));
				if (this.telnetClient.readTakeMoreTenSecond()) { CommunicationSlowly = true; }
				return value;
			}
			catch (notSuccessedSendTheMassage) { CommunityProblemTryFix = true; this.connect(); }
			catch (FormatException) { GetErrFromServer = true; }
			catch (Exception ) { UnepctedErr = true; }
			return Convert.ToDouble(this.valueMap[variable].value);
		}

		public void NotifyPropertyChanged(string PropertyName)
		{
			this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
		}
		public void moveJoystick(double elevator, double rudder)
		{
			Elevator = elevator;
			this.telnetClient.write(this.valueMap["elevator"].addres + " " + elevator);
			Rudder = rudder;
			this.telnetClient.write(this.valueMap["rudder"].addres + " " + rudder);
		}

		public void updateSliders(double throttle, double aileron)
		{
			Throttle = throttle;
			this.telnetClient.write(this.valueMap["throttle"].addres + " " + throttle);
			Aileron = aileron;
			this.telnetClient.write(this.valueMap["aileron"].addres + " " + aileron);
		}
		private void upDateSetProperty(String proparty, Double value)
		{
			this.valueMap[proparty].mutex.WaitOne();
			this.valueMap[proparty].value = value.ToString();
			if (proparty.Equals("latitude") || proparty.Equals("longitude")) { this.NotifyPropertyChanged("AirPlaneLocation"); }
			else { this.NotifyPropertyChanged(char.ToUpper(proparty[0]) + proparty.Substring(1)); }
			this.valueMap[proparty].mutex.ReleaseMutex();
		}
		public void KeepLimitAndUpdate(int[] limit, Double value, String name)
		{
			if (value < limit[0]) { upDateSetProperty(name, limit[0]); }
			else if (value > limit[1]) { upDateSetProperty(name, limit[1]); }
			else { upDateSetProperty(name, value); }
		}
		// Properties for binding with the viewModel
		//todo check limited of think
		public double HeadingDeg
		{
			get { return Double.Parse(this.valueMap["headingDeg"].value); }
			set { KeepLimitAndUpdate(new int[]{ 0, 360 }, value, "headingDeg"); }
		}
		public double VerticalSpeed
		{
			get { return Double.Parse(this.valueMap["verticalSpeed"].value); }
			set { KeepLimitAndUpdate(new int[] { -5000,721 }, value, "verticalSpeed"); }
		}
		public double GroundSpeed
		{
			get { return Double.Parse(this.valueMap["groundSpeed"].value); }
			set { KeepLimitAndUpdate(new int[] { -50, 302 }, value, "groundSpeed"); }
		}
		public double IndicatedSpeed
		{
			get { return Double.Parse(this.valueMap["indicatedSpeed"].value); }
			set { KeepLimitAndUpdate(new int[] { 0, 228 }, value, "indicatedSpeed"); }
		}
		public double GpsAltitude
		{
			get { return Double.Parse(this.valueMap["gpsAltitude"].value); }
			set { KeepLimitAndUpdate(new int[] { 0, 13500 }, value, "gpsAltitude"); }
		}
		public Location AirPlaneLocation
		{
			get { return new Location(this.Latitude, this.Longitude); }
		}
		public double InternalRoll
		{
			get { return Double.Parse(this.valueMap["internalRoll"].value); }
			set{ 
				//todo check this limit
				upDateSetProperty("internalRoll", value); }
		}
		public double InternalPitch
		{
			get { return Double.Parse(this.valueMap["internalPitch"].value); }
			set{ //todo check this limit
				upDateSetProperty("internalPitch", value); }
		}
		public double AltimeterAltitude
		{
			get { return Double.Parse(this.valueMap["altimeterAltitude"].value); }
			set { KeepLimitAndUpdate(new int[] { 0, 13500 }, value, "altimeterAltitude"); }
		}

		public double Latitude
		{
			get { return Double.Parse(this.valueMap["latitude"].value); }
			set { KeepLimitAndUpdate(new int[] { -90,90 }, value, "latitude"); }
		}

		public double Longitude
		{
			get { return Double.Parse(this.valueMap["longitude"].value); }
			set { KeepLimitAndUpdate(new int[] { -180, 180 }, value, "longitude"); }
		}
		
		public double Throttle
		{
			get { return Double.Parse(this.valueMap["throttle"].value); }
			set { KeepLimitAndUpdate(new int[] { 0, 1 }, value, "throttle"); }
		}
		public double Aileron
		{
			get { return Double.Parse(this.valueMap["aileron"].value); }
			set { KeepLimitAndUpdate(new int[] { -1, 1 }, value, "aileron"); }
		}
		public double Elevator
		{
			get { return Double.Parse(this.valueMap["elevator"].value); }
			set { KeepLimitAndUpdate(new int[] { -1, 1 }, value, "elevator"); }
		}
		public double Rudder
		{
			get { return Double.Parse(this.valueMap["rudder"].value); }
			set { KeepLimitAndUpdate(new int[] { -1, 1 }, value, "rudder");}
		}
		public bool NotCanConnect { get { return this.errorMassage[(int)Error.notCanConnect]; }
			set { this.errorMassage[(int)Error.notCanConnect] = value; } }
		public bool GetErrFromServer { get { return this.errorMassage[(int)Error.getErrFromServer]; }
			set { this.errorMassage[(int)Error.getErrFromServer] = value; } }
		public bool CommunicationSlowly { get { return this.errorMassage[(int)Error.communicationSlowly]; }
			set { this.errorMassage[(int)Error.communicationSlowly] = value; } }
		public bool UnepctedErr { get { return this.errorMassage[(int)Error.unepctedErr]; }
			set { this.errorMassage[(int)Error.unepctedErr] = value; }}
		public bool CommunityProblemTryFix
		{
			get { return this.errorMassage[(int)Error.communityProblemTryFix]; }
			set 
			{
				if (!this.errorMassage[(int)Error.communityProblemTryFix]) {
					this.errorMassage[(int)Error.communityProblemTryFix] = value;
				};
			}
		}
		

		/**
		 * 		public void UpdateThrottle() {  }
		public void UpdateAileron() { }
		public void UpdateElevator() {  }
		public void UpdateRudder() { }
		*/
	}
}
 