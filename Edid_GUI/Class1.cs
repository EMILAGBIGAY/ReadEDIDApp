using System.ComponentModel;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Forms.VisualStyles;
using Windows.Devices.Display;
using Windows.Devices.Enumeration;
using static System.ComponentModel.Design.ObjectSelectorEditor;

namespace Edid_GUI
{
    public class Reader
    {
        [SuppressMessage("Microsoft.Usage", "CA1416:ThisCallSiteIsReachableOnAllPlatforms", Justification = "Intentionally targeting Windows platform.")]
        
        public void Runner()
        {
            var devices = new EnumerateDevices(DisplayMonitor.GetDeviceSelector());

            List<string> files = new List<string>();
            List<string> EDIDInfo = new List<string>();

            for (int i = 1; i <= devices.numDevices(); i++)
            {
                files.Add("output" + i + ".txt");
                EDIDInfo.Add("EDIDInformation" + i + ".txt");
            }

            for (int i = 0; i < files.Count; i++)
            {
                devices.EnumDisplay(files[i], i);

                process processInstance = new process();
                using (StreamWriter writer = new StreamWriter(EDIDInfo[i])) { }
                processInstance.ParseEDID(files[i], EDIDInfo[i]);
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

            public int numDevices()
            {
                return deviceList.Count;
            }

            public async void EnumDisplay(string outputFilePath, int index)
            {
                if (index != 0) Console.WriteLine();
                DisplayMonitor display = await DisplayMonitor.FromInterfaceIdAsync(deviceList[index].Id);
                byte[] EDID = display.GetDescriptor(DisplayMonitorDescriptorKind.Edid);

                string hexBuffer = BitConverter.ToString(EDID).Replace("-", " ");
                //Console.Write(hexBuffer + "\n");
                using (StreamWriter writer = new StreamWriter(outputFilePath))
                {
                    writer.Write("\"EDID\"=hex:");
                    writer.WriteLine(hexBuffer + "\n");
                }
            }
        }

    }

}

