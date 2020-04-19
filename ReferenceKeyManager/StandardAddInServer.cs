using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Inventor;
using Microsoft.Win32;

namespace ReferenceKeyManager
{
    public class WindowWrapper : IWin32Window
    {
        private IntPtr _hwnd;
        public WindowWrapper(int hwnd)
        {
            _hwnd = new IntPtr(hwnd);
        }
        public IntPtr Handle => _hwnd;
    }

    /// <summary>
    /// This is the primary AddIn Server class that implements the ApplicationAddInServer interface
    /// that all Inventor AddIns are required to implement. The communication between Inventor and
    /// the AddIn is via the methods on this interface.
    /// </summary>
    [GuidAttribute("3562a43d-704b-4386-9325-26702b3b56fa")]
    public class StandardAddInServer : Inventor.ApplicationAddInServer
    {

        // Inventor application object.
        private Inventor.Application m_inventorApplication;
        private Inventor.ButtonDefinition m_buttonDefinition;
        //private string m_guid = "";

        public StandardAddInServer()
        {
        }

        #region ApplicationAddInServer Members

        public void Activate(Inventor.ApplicationAddInSite addInSiteObject, bool firstTime)
        {
            m_inventorApplication = addInSiteObject.Application;

            m_buttonDefinition = m_inventorApplication.CommandManager.ControlDefinitions.AddButtonDefinition(
                "Reference Key Manager", "Adam.ReferenceKeyManager", CommandTypesEnum.kNonShapeEditCmdType);
            m_buttonDefinition.AutoAddToGUI();
            m_buttonDefinition.OnExecute += M_buttonDefinition_OnExecute;
        }

        private void M_buttonDefinition_OnExecute(NameValueMap Context)
        {
            MainForm form = new MainForm(m_inventorApplication);
            form.Show(new WindowWrapper(m_inventorApplication.MainFrameHWND));
        }
    

        public void Deactivate()
        {
            m_buttonDefinition.OnExecute -= M_buttonDefinition_OnExecute;

            m_buttonDefinition = null;
            m_inventorApplication = null;

            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        public void ExecuteCommand(int commandID)
        {
            // Note:this method is now obsolete, you should use the 
            // ControlDefinition functionality for implementing commands.
        }

        public object Automation
        {
            // This property is provided to allow the AddIn to expose an API 
            // of its own to other programs. Typically, this  would be done by
            // implementing the AddIn's API interface in a class and returning 
            // that class object through this property.

            get
            {
                // TODO: Add ApplicationAddInServer.Automation getter implementation
                return null;
            }
        }

        #endregion

    }
}
