using Microsoft.Win32;
using System.Diagnostics.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

class Program
{   
    [SuppressMessage("Microsoft.Usage", "CA1416:ThisCallSiteIsReachableOnAllPlatforms", Justification = "Intentionally targeting Windows platform.")]
    static void Main()
    {
        try
        {
            string regContent = FindEDIDRegistryValue();

            if (string.IsNullOrEmpty(regContent))
                Console.WriteLine("EDID.reg value not found in the registry.");
            else
                Console.WriteLine(regContent);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }

    static string FindEDIDRegistryValue()
    {
        string monitorKeyPath = @"SYSTEM\CurrentControlSet\Enum\DISPLAY";
        string registryValueName = "EDID";
        string edidValue = DFSRegistrySearch(monitorKeyPath, registryValueName);

        if (string.IsNullOrEmpty(edidValue))
            return null;

        StringBuilder regContent = new StringBuilder();

        regContent.AppendLine("Windows Registry Editor Version 5.00");
        regContent.AppendLine();
        regContent.AppendLine("[HKEY_CURRENT_USER\\EDID]");
        regContent.AppendLine("\"EDID\"=\"" + edidValue + "\"");

        regContent.AppendLine();
        regContent.AppendLine("; Found EDID.reg value in the registry.");

        return regContent.ToString();
    }

    static string DFSRegistrySearch(string keyPath, string valueName)
    {
        Stack<string> stack = new Stack<string>();
        stack.Push(keyPath);

        while (stack.Count > 0)
        {
            string currentKeyPath = stack.Pop();

            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(currentKeyPath))
            {
                if (key != null)
                {
                    string[] valueNames = key.GetValueNames();
                    foreach (string name in valueNames)
                    {
                        if (name == valueName)
                            return key.GetValue(valueName, null) as string;
                    }

                    string[] subKeys = key.GetSubKeyNames();
                    foreach (string subKey in subKeys)
                    {
                        string subKeyPath = $"{currentKeyPath}\\{subKey}";
                        stack.Push(subKeyPath);
                    }
                }
            }
        }

        return null;
    }
}
