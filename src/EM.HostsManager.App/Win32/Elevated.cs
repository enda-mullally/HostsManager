﻿namespace EM.HostsManager.App.Win32;

using Process=System.Diagnostics.Process;

public static class Elevated
{
    // Normal button
    private const int BcmFirst = 0x1600;
        
    // Elevated button
    private const int BcmSetShield = (BcmFirst + 0x000C); 

    public static bool IsElevated()
    {
        return new WindowsPrincipal(WindowsIdentity.GetCurrent())
            .IsInRole(WindowsBuiltInRole.Administrator);
    }
        
    public static void AddShieldToButton(Button b)
    {
        b.FlatStyle = FlatStyle.System;

        User32.SendMessage(b.Handle, BcmSetShield, 0, 0xFFFFFFFF);
    }

    public static void RestartElevated(string args = "/elevate")
    {
        var startInfo = new ProcessStartInfo
        {
            UseShellExecute = true,
            WorkingDirectory = Environment.CurrentDirectory,
            FileName = Application.ExecutablePath,
            Arguments = args,
            Verb = "runas"
        };

        if (startInfo
            .FileName
            .ToLowerInvariant()
            .EndsWith(".dll"))
        {
            startInfo.FileName = startInfo
                .FileName
                .ToLowerInvariant()
                .Replace(".dll", ".exe"); // .NET Core re-direct
        }

        try
        {
            if (Process.Start(startInfo) != null)
            {
                Application.Exit();
            }
        }
        catch
        {
            // ignore
        }
    }
}