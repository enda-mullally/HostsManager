//
// Copyright © 2009-2024 Enda Mullally.
//

namespace EM.HostsManager.Win32;

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
            .EndsWith(".dll", StringComparison.CurrentCultureIgnoreCase))
        {
            startInfo.FileName = startInfo
                .FileName
                .Replace(".dll", ".exe", StringComparison.InvariantCultureIgnoreCase); // .NET Core re-direct
        }

        try
        {
            if (System.Diagnostics.Process.Start(startInfo) != null)
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