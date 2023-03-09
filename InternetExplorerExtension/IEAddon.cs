using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using IEExt.Properties;
using Microsoft.Win32;
using mshtml;
using SHDocVw;

namespace IEExt
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    [Guid("D40C654D-7C51-4EB3-95B2-1E23905C2A2D")]
    [ProgId("MyBHO.ActiveXInjector")]
    public class ActiveXInjectorBHO : IObjectWithSite
    {
        IWebBrowser2 browser;
        private object site;

        #region ActiveX Injector
        void OnDocumentComplete(object pDisp, ref object URL)
        {
            try
            {
                var document2 = browser.Document as IHTMLDocument2;
                var window = document2.parentWindow;
                window.execScript(Resources.emulator);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion
        #region Load and Save Data
        public static string RegData = "Software\\ActiveXInjector";

        [DllImport("ieframe.dll")]
        public static extern int IEGetWriteableHKCU(ref IntPtr phKey);

        #endregion

        [Guid("6D5140C1-7436-11CE-8034-00AA006009FA")]
        [InterfaceType(1)]
        public interface IServiceProvider
        {
            int QueryService(ref Guid guidService, ref Guid riid, out IntPtr ppvObject);
        }

        #region Implementation of IObjectWithSite
        int IObjectWithSite.SetSite(object site)
        {
            this.site = site;

            if (site != null)
            {
                var serviceProv = (IServiceProvider)this.site;
                var guidIWebBrowserApp = Marshal.GenerateGuidForType(typeof(IWebBrowserApp)); // new Guid("0002DF05-0000-0000-C000-000000000046");
                var guidIWebBrowser2 = Marshal.GenerateGuidForType(typeof(IWebBrowser2)); // new Guid("D30C1661-CDAF-11D0-8A3E-00C04FC9E26E");
                IntPtr intPtr;
                serviceProv.QueryService(ref guidIWebBrowserApp, ref guidIWebBrowser2, out intPtr);

                browser = (IWebBrowser2)Marshal.GetObjectForIUnknown(intPtr);

                ((DWebBrowserEvents2_Event)browser).DocumentComplete +=
                    new DWebBrowserEvents2_DocumentCompleteEventHandler(this.OnDocumentComplete);
            }
            else
            {
                ((DWebBrowserEvents2_Event)browser).DocumentComplete -=
                    new DWebBrowserEvents2_DocumentCompleteEventHandler(this.OnDocumentComplete);
                browser = null;
            }
            return 0;
        }
        int IObjectWithSite.GetSite(ref Guid guid, out IntPtr ppvSite)
        {
            IntPtr punk = Marshal.GetIUnknownForObject(browser);
            int hr = Marshal.QueryInterface(punk, ref guid, out ppvSite);
            Marshal.Release(punk);
            return hr;
        }
        #endregion

        #region Registering with regasm
        public static string RegBHO = "Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\Browser Helper Objects";
        public static string RegCmd = "Software\\Microsoft\\Internet Explorer\\Extensions";

        [ComRegisterFunction]
        public static void RegisterBHO(Type type)
        {
            string guid = type.GUID.ToString("B");

            // BHO
            {
                RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(RegBHO, true) ?? Registry.LocalMachine.CreateSubKey(RegBHO);
                RegistryKey key = registryKey.OpenSubKey(guid) ?? registryKey.CreateSubKey(guid);
                key.SetValue("Alright", 1);
                registryKey.Close();
                key.Close();
            }

        }

        [ComUnregisterFunction]
        public static void UnregisterBHO(Type type)
        {
            string guid = type.GUID.ToString("B");
            // BHO
            {
                RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(RegBHO, true);
                registryKey?.DeleteSubKey(guid, false);
            }
            // Command
            {
                RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(RegCmd, true);
                registryKey?.DeleteSubKey(guid, false);
            }
        }
        #endregion
    }
}