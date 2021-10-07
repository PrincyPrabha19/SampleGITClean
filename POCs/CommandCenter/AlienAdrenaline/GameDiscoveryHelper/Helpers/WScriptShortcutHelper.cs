using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using AlienLabs.GameDiscoveryHelper.Classes;
using AlienLabs.WindowsIconHelper;
using IWshRuntimeLibrary;

namespace AlienLabs.GameDiscoveryHelper.Helpers
{
    public class WScriptShortcutHelper
    {
        public static WScriptShortcutData GetShortcutInfo(string path)
        {
            if (!System.IO.File.Exists(path))
                return null;

            try
            {
                var shell = new WshShell();
                var link = (IWshShortcut)shell.CreateShortcut(path);
                if (link != null)
                    return new WScriptShortcutData()
                               {
                                   TargetPath = link.TargetPath,
                                   WorkingDirectory = link.WorkingDirectory
                               };
            }
            catch (Exception e)
            {               
            }

            return null;            
        }

        public static bool CreateShortcutx(
            string shortcutName, string shortcutDescription, string shortcutPath, string shortcutTargetPath, string shortcutArguments, string gameIconPath)
        {
            bool created = false;

            try
            {
                Icon originalIcon = null;
                try
                {
                    originalIcon = IconHelper.ExtractBestFitIcon(gameIconPath, 0, new Size(48, 48));
                }
                catch (Exception)
                {
                }

                string iconLocation = string.Empty;
                if (originalIcon != null)
                {
                    iconLocation = Path.GetTempFileName();
                    using (var fs = new FileStream(iconLocation, FileMode.Append, FileAccess.Write))
                    {
                        originalIcon.Save(fs);
                    }
                }

                var wshShell = new WshShell();
                var shortcut = (IWshRuntimeLibrary.IWshShortcut)wshShell.CreateShortcut(shortcutPath);
                shortcut.Description = shortcutDescription;                    
                shortcut.TargetPath = shortcutTargetPath;                    
                shortcut.Arguments = String.Format("\"{0}\"", shortcutArguments);
                if (!String.IsNullOrEmpty(iconLocation))
                    shortcut.IconLocation = iconLocation;
                shortcut.Save();

                created = true;
            }
            catch (Exception e)
            {
            }

            return created;
        }

        public static bool CreateShortcut(
            string shortcutName, string shortcutDescription, string shortcutPath, string shortcutTargetPath, string shortcutArguments, string gameIconPath)
        {
            bool created = false;

            try
            {
                Icon originalIcon = null;
                try
                {
                    originalIcon = IconHelper.ExtractBestFitIcon(gameIconPath, 0, new Size(48, 48));
                }
                catch (Exception)
                {
                }

                string iconLocation = string.Empty;
                if (originalIcon != null)
                {
                    iconLocation = Path.GetTempFileName();
                    using (var fs = new FileStream(iconLocation, FileMode.Append, FileAccess.Write))
                    {
                        originalIcon.Save(fs);
                    }
                }

                var link = (IShellLink)new ShellLink();
                link.SetDescription(shortcutDescription);
                link.SetPath(shortcutTargetPath);
                link.SetArguments(String.Format("\"{0}\"", shortcutArguments));
                link.SetIconLocation(iconLocation, 0);

                var file = (IPersistFile)link;
                file.Save(shortcutPath, false);

                created = true;
            }
            catch (Exception e)
            {
            }

            return created;
        }

        [ComImport]
        [Guid("00021401-0000-0000-C000-000000000046")]
        internal class ShellLink
        {
        }

        [ComImport]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [Guid("000214F9-0000-0000-C000-000000000046")]
        internal interface IShellLink
        {
            void GetPath([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszFile, int cchMaxPath, out IntPtr pfd, int fFlags);
            void GetIDList(out IntPtr ppidl);
            void SetIDList(IntPtr pidl);
            void GetDescription([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszName, int cchMaxName);
            void SetDescription([MarshalAs(UnmanagedType.LPWStr)] string pszName);
            void GetWorkingDirectory([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszDir, int cchMaxPath);
            void SetWorkingDirectory([MarshalAs(UnmanagedType.LPWStr)] string pszDir);
            void GetArguments([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszArgs, int cchMaxPath);
            void SetArguments([MarshalAs(UnmanagedType.LPWStr)] string pszArgs);
            void GetHotkey(out short pwHotkey);
            void SetHotkey(short wHotkey);
            void GetShowCmd(out int piShowCmd);
            void SetShowCmd(int iShowCmd);
            void GetIconLocation([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszIconPath, int cchIconPath, out int piIcon);
            void SetIconLocation([MarshalAs(UnmanagedType.LPWStr)] string pszIconPath, int iIcon);
            void SetRelativePath([MarshalAs(UnmanagedType.LPWStr)] string pszPathRel, int dwReserved);
            void Resolve(IntPtr hwnd, int fFlags);
            void SetPath([MarshalAs(UnmanagedType.LPWStr)] string pszFile);
        }
    }
}
