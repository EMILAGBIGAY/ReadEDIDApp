using System.Diagnostics.CodeAnalysis;
using Microsoft.Win32;

class Program
{   
    [SuppressMessage("Microsoft.Usage", "CA1416:ThisCallSiteIsReachableOnAllPlatforms", Justification = "Intentionally targeting Windows platform.")]
    static void Main()
    {   
        //path to EDID.reg file
        string registryPath = @"SYSTEM\CurrentControlSet\Enum\DISPLAY\DELD08E\5&2291a7e&0&UID4352\Device Parameters";
        //string registryPath = @"SYSTEM\CurrentControlSet\Enum\DISPLAY\CMN1451\4&34803ba9&0&UID8388688\Device Parameters";
        string outputFilePath = "output.txt"; // File path for the output file

        try
        {
            // Open the registry key
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(registryPath))
            {
                // Check if the key exists
                if (key != null)
                {
                    // Export the key values to a .reg file
                    string[] valueNames = key.GetValueNames();
                    string regFileContent = "Windows Registry Editor Version 5.00\n\n";
                    regFileContent += $"[{key.Name}]\n";
                    foreach (string valueName in valueNames)
                    {
                        object value = key.GetValue(valueName);
                        if (value is byte[] byteValue)
                        {
                            string hexValue = BitConverter.ToString(byteValue).Replace("-", " ");
                            regFileContent += $"\"{valueName}\"=hex:{hexValue}\n";
                        }
                        else if (value != null)
                        {
                            regFileContent += $"\"{valueName}\"=\"{value}\"\n";
                        }
                    }

                    // Save the content to a file
                    Console.WriteLine(regFileContent);
                    File.WriteAllText(outputFilePath, regFileContent);

                    Console.WriteLine($"Content saved to {outputFilePath}");
                }
                else
                {
                    Console.WriteLine("Registry key not found.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred: " + ex.Message);
        }
    }
}
