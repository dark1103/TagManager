using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.Windows.Forms;

namespace TagManager
{
    class ContextMenuElement
    {
        public static void AddFolderContextItem(string fileType, string name, string lable, string iconPath = "")
        {
            RegistryKey key = Registry.ClassesRoot.OpenSubKey("Directory").OpenSubKey("Background").OpenSubKey("shell", true).CreateSubKey(name);

            key.SetValue("", lable);
            RegistryKey commandkey = key.CreateSubKey("command");
            commandkey.SetValue("", Application.ExecutablePath + " %V");
            if(!String.IsNullOrEmpty(iconPath)) key.SetValue("Icon", iconPath);
        }

        public static void AddFileContentItem(string fileType, string name,string lable,string iconPath = "")
        {
            RegistryKey key = Registry.ClassesRoot.OpenSubKey(fileType).OpenSubKey("shell", true).CreateSubKey(name);
            key.SetValue("", lable);
            RegistryKey commandkey = key.CreateSubKey("command");
            commandkey.SetValue("", Application.ExecutablePath + " %1");
            if (!String.IsNullOrEmpty(iconPath)) key.SetValue("Icon", iconPath);
        }
        public static void RemoveFolderContextItem(string fileType, string name)
        {
            Registry.ClassesRoot.OpenSubKey("Directory").OpenSubKey("Background").OpenSubKey("shell", true).DeleteSubKeyTree(name);
        }
        public static void RemoveItemContextItem(string fileType, string name)
        {
            Registry.ClassesRoot.OpenSubKey(fileType).OpenSubKey("shell", true).DeleteSubKeyTree(name);
        }
    }
}
