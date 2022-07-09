using System;
using System.Net;
using System.IO;
using System.Net.NetworkInformation;
using System.Reflection;
using Microsoft.Win32;
using System.Diagnostics;
using System.Deployment.Application;
using System.Collections;
using System.Management;

namespace sistemozellikleri1
{
    public partial class Form1 : MetroFramework.Forms.MetroForm
    {
        public Form1()
        {
            InitializeComponent();
        }

        [Obsolete]
        private void Form1_Load(object sender, EventArgs e)
        {
            CPU();
            HDD();
            BuildTime.Text = BUILD_DATE();
            OperatingSystem.Text = OS();
            Raminfo.Text = RAM();
            Gpuinfo.Text = EKRANKART();
        }

        public class HardDrive
        {
            public string Model { get; set; }
            public string InterfaceType { get; set; }
            public string Caption { get; set; }
            public string SerialNo { get; set; }
        }
        private void HDD()
        {
            var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");
            long totalSize = 0;
            foreach (ManagementObject disk in searcher.Get())
            {
                foreach (DriveInfo drive in DriveInfo.GetDrives())
                {
                    if (drive.IsReady)// şeklinde IsReady olanları almalısın.
                        totalSize += drive.TotalSize / 1024 / 1024 / 1024;
                }
                Harddisk.Text = "Model: " + disk["Model"] + "\n InterfaceType: " + disk["InterfaceType"] + "\nBoyut: " + disk["Size"] + "\npart:" + disk["Partitions"] +"\nboyut" + totalSize.ToString() + " Gb";
            }

        }


        private string BUILD_DATE()
        {
            DateTime buildDate = new FileInfo(Assembly.GetExecutingAssembly().Location).LastWriteTime;
            return buildDate.ToString();
        }

        public void CPU()
        {
            CoreNumber.Text = System.Environment.ProcessorCount.ToString();
            RegistryKey Rkey1 = Registry.LocalMachine;
            Rkey1 = Rkey1.OpenSubKey("HARDWARE\\DESCRIPTION\\System\\CentralProcessor\\0");
            Cpuinfo.Text = (string)Rkey1.GetValue("ProcessorNameString").ToString();
        }

        private string OS()//İşletim Sistemi Özellikleri İçin
        {
            return System.Environment.OSVersion.ToString();
        }

        public static string RAM()
        {
            string ramSizeInfo = null;
            ManagementObjectSearcher ramSearcher = new ManagementObjectSearcher("Select * From Win32_ComputerSystem");

            foreach (ManagementObject mObject in ramSearcher.Get())
            {
                double Ram_Bytes = (Convert.ToDouble(mObject["TotalPhysicalMemory"]));
                double ramgb = Ram_Bytes / 1073741824;
                double ramSize = Math.Ceiling(ramgb);
                ramSizeInfo = ramSize.ToString() + " GB";
            }
            return ramSizeInfo;
        }

        public static string EKRANKART()
        {
            string videoControllerInfo = null;
            string name = null;
            string ram = null;
            string horizontalResolution = null;
            string verticalResolution = null;
            string deviceID = null;

            ManagementObjectSearcher vidSearcher = new ManagementObjectSearcher("Select * from Win32_VideoController Where availability='3'");

            foreach (ManagementObject mObject in vidSearcher.Get())
            {
                name = mObject["name"].ToString();
                ram = (Convert.ToDouble(mObject["AdapterRam"]) / 1073741824).ToString();
                deviceID = (string)mObject["DeviceID"];
                horizontalResolution = mObject["CurrentHorizontalResolution"].ToString();
                verticalResolution = mObject["CurrentVerticalResolution"].ToString();
            }
            videoControllerInfo = name + "\r\n Ram Miktarı : " + ram + " GB \r\n ID : " + deviceID + "\r\n Çözünürlük :" + horizontalResolution + " x " + verticalResolution;

            return videoControllerInfo;
        }

        private void lblMac_Click(object sender, EventArgs e)
        {

        }
    }
}
