using System;
using System.IO;
using System.Windows;
using JPMorrow.Tools.Diagnostics;

namespace JPMorrow.UI.ViewModels
{
	public partial class ParentViewModel
    {
        /// <summary>
        /// prompt for save and exit
        /// </summary>
        public void MasterClose(Window window)
        {
            try
            {
                window.Close();
            }
            catch(Exception ex)
            {
                debugger.show(err:ex.ToString());
            }
        }
	}
}