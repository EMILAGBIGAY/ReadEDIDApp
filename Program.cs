using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Collections.Generic;
using Windows.Devices.Display;
using Windows.Devices.Enumeration;


namespace EDID.Core.cs
{
    class Program
    {
        [SuppressMessage("Microsoft.Usage", "CA1416:ThisCallSiteIsReachableOnAllPlatforms", Justification = "Intentionally targeting Windows platform.")]
        static void Main(string[] args)
        {   
            string[] files = {"output1.txt", "output2.txt"};
            string[] EDIDInfo = {"EDIDInformation1.txt", "EDIDInformation2.txt"};

            var devices = new EnumerateDevices(DisplayMonitor.GetDeviceSelector());

            for(int i = 0; i < files.Length;i++){
                devices.EnumDisplay(files[i],i);

                Process processInstance = new Process();
                using (StreamWriter writer = new StreamWriter(EDIDInfo[i])){}
                processInstance.ParseEDID(files[i],EDIDInfo[i]);
            }
            

        }
        
        class EnumerateDevices
        {
            readonly List<DeviceInformation> deviceList = new List<DeviceInformation>();

            public EnumerateDevices(string selector) 
            {
                EnumDevices(selector);
            }

            
            private async void EnumDevices(string selector = null)
            {
                var devices = await DeviceInformation.FindAllAsync(selector);

                if (devices.Count > 0)
                    for (var i = 0; i < devices.Count; i++) 
                        deviceList.Add(devices[i]);
            }

            
            public async void EnumDisplay(string outputFilePath,int index)
            {   
                if (index != 0) Console.WriteLine();
                DisplayMonitor display = await DisplayMonitor.FromInterfaceIdAsync(deviceList[index].Id);
                byte[] EDID = display.GetDescriptor(DisplayMonitorDescriptorKind.Edid);

                string hexBuffer = BitConverter.ToString(EDID).Replace("-", " ");
                Console.Write(hexBuffer + "\n");
                using (StreamWriter writer = new StreamWriter(outputFilePath))
                {
                    writer.Write("\"EDID\"=hex:");
                    writer.WriteLine(hexBuffer + "\n");
                }
            }
        }

    }

}
