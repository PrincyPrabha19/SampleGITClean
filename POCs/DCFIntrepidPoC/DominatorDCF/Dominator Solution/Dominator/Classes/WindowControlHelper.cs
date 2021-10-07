using System;
using Microsoft.Win32;

namespace Dominator.Classes
{
    public interface IWindowControlReader
    {
        string GetWindowState();
        bool GetWindowLocation(out int left, out int top, out int width, out int height);
    }

    public interface IWindowControlWriter
    {
        void SetWindowState(string state);
        void SetWindowLocation(int left, int top, int width, int height);
    }

    public class WindowControlHelper : IWindowControlReader, IWindowControlWriter
    {
        private const string CUSTOM_PATH = @"SOFTWARE\Alienware\OC Controls";
        private const string WINDOW_STATE = "WindowState";
        private const string WINDOW_LEFT = "WindowLeft";
        private const string WINDOW_TOP = "WindowTop";
        private const string WINDOW_WIDTH = "WindowWidth";
        private const string WINDOW_HEIGHT = "WindowHeight";

        public string GetWindowState()
        {
            try
            {
                using (var hklm = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64))
                using (var key = hklm.OpenSubKey(CUSTOM_PATH, false))
                {
                    return key?.GetValue(WINDOW_STATE)?.ToString();
                }
            }
            catch (Exception)
            {
            }

            return string.Empty;
        }

        public void SetWindowState(string state)
        {
            try
            {
                using (var hklm = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64))
                using (var key = hklm.CreateSubKey(CUSTOM_PATH))
                {
                    key?.SetValue(WINDOW_STATE, state);
                }
            }
            catch (Exception)
            {
            }
        }

        public bool GetWindowLocation(out int left, out int top, out int width, out int height)
        {
            left = top = width = height = 0;

            try
            {
                using (var hklm = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64))
                using (var key = hklm.OpenSubKey(CUSTOM_PATH, false))
                {
                    var _left = key?.GetValue(WINDOW_LEFT);
                    var _top = key?.GetValue(WINDOW_TOP);
                    var _width = key?.GetValue(WINDOW_WIDTH);
                    var _height = key?.GetValue(WINDOW_HEIGHT);

                    if (_left != null && _top != null && _width != null && _height != null)
                    {
                        left = Convert.ToInt32(_left);
                        top = Convert.ToInt32(_top);
                        width = Convert.ToInt32(_width);
                        height = Convert.ToInt32(_height);
                        return true;
                    }

                }
            }
            catch (Exception)
            {
            }

            return false;
        }

        public void SetWindowLocation(int left, int top, int width, int height)
        {
            try
            {
                using (var hklm = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64))
                using (var key = hklm.CreateSubKey(CUSTOM_PATH))
                {
                    key?.SetValue(WINDOW_LEFT, left);
                    key?.SetValue(WINDOW_TOP, top);
                    key?.SetValue(WINDOW_WIDTH, width);
                    key?.SetValue(WINDOW_HEIGHT, height);

                }
            }
            catch (Exception)
            {
            }
        }
    }
}