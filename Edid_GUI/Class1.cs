using System.Diagnostics.CodeAnalysis;
using Windows.Devices.Display;
using Windows.Devices.Enumeration;


namespace Edid_GUI
{   static class fileGlobal
    {
        public static List<string> files = new List<string>();
        public static List<string> EDIDInfo = new List<string>();
    }
    public class Reader
    {
        [SuppressMessage("Microsoft.Usage", "CA1416:ThisCallSiteIsReachableOnAllPlatforms", Justification = "Intentionally targeting Windows platform.")]
        
        public void Runner()
        {
            var devices = new EnumerateDevices(DisplayMonitor.GetDeviceSelector());

            

            for (int i = 1; i <= devices.numDevices(); i++)
            {
                fileGlobal.files.Add("output" + i + ".txt");
                fileGlobal.EDIDInfo.Add("EDIDInformation" + i + ".txt");
            }

            for (int i = 0; i < fileGlobal.files.Count; i++)
            {
                devices.EnumDisplay(fileGlobal.files[i], i);

                process processInstance = new process();
                using (StreamWriter writer = new StreamWriter(fileGlobal.EDIDInfo[i])) { }
                processInstance.ParseEDID(fileGlobal.files[i], fileGlobal.EDIDInfo[i]);
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
                using (StreamWriter writer = new StreamWriter(outputFilePath))
                {
                    writer.WriteLine(hexBuffer + "\n");
                }
            }
        }

    }

}

