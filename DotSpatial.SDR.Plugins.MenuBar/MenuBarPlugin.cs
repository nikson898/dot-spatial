// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MenuBarPlugin.cs" company="">
//
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using DotSpatial.Controls;
using DotSpatial.Controls.Header;
using DotSpatial.Data;
using DotSpatial.SDR.Plugins.MenuBar.Properties;
using Msg = DotSpatial.SDR.Plugins.MenuBar.MessageStrings;
using DotSpatial.Topology;

namespace DotSpatial.SDR.Plugins.MenuBar
{
    public class MenuBarPlugin : Extension
    {
        #region Constants and Fields

        private const string FileMenuKey = HeaderControl.ApplicationMenuKey;

        #endregion

        #region Public Methods

        public override void Activate()
        {
            IHeaderControl header = App.HeaderControl;
            header.Add(new RootItem(FileMenuKey, MessageStrings.File) { SortOrder = -20 });
            header.Add(new SimpleActionItem(FileMenuKey, Msg.File_Open, OpenProject_Click) { GroupCaption = HeaderControl.ApplicationMenuKey, SortOrder = 10, SmallImage = Resources.folder_16x16, LargeImage = Resources.folder_32x32 });
            header.Add(new SimpleActionItem(FileMenuKey, Msg.File_Print, PrintLayout_Click) { GroupCaption = HeaderControl.ApplicationMenuKey, SortOrder = 40, SmallImage = Resources.printer_16x16, LargeImage = Resources.printer_32x32 });
            header.Add(new SimpleActionItem(FileMenuKey, Msg.File_Reset_Layout, ResetLayout_Click) { GroupCaption = HeaderControl.ApplicationMenuKey, SortOrder = 200, SmallImage = Resources.layout_delete_16x16, LargeImage = Resources.layout_delete_32x32 });
            header.Add(new SimpleActionItem(FileMenuKey, Msg.File_Exit, Exit_Click) { GroupCaption = HeaderControl.ApplicationMenuKey, SortOrder = 5000, });
            base.Activate();
        }

        public override void Deactivate()
        {
            App.HeaderControl.RemoveAll();
            base.Deactivate();
        }

        #endregion

        #region Methods

        private void Exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void OpenProject_Click(object sender, EventArgs e)
        {
            using (var dlg = new OpenFileDialog())
            {
                dlg.Filter = App.SerializationManager.OpenDialogFilterText;
                if (dlg.ShowDialog() != DialogResult.OK)
                    return;
                try
                {
                    //use the AppManager.SerializationManager to open the project
                    App.SerializationManager.OpenProject(dlg.FileName);
                    App.Map.Invalidate();
                }
                catch (IOException)
                {
                    MessageBox.Show(String.Format(Resources.CouldNotOpenTheSpecifiedMapFile, dlg.FileName), Resources.Error,
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (XmlException)
                {
                    MessageBox.Show(String.Format(Resources.FailedToReadTheSpecifiedMapFile, dlg.FileName), Resources.Error,
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (ArgumentException)
                {
                    MessageBox.Show(String.Format(Resources.FailedToReadAPortionOfTheSpecifiedMapFile, dlg.FileName), Resources.Error,
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the PrintLayout control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void PrintLayout_Click(object sender, EventArgs e)
        {
            using (LayoutForm layout = new LayoutForm())
            {
                layout.MapControl = App.Map as Map;
                layout.ShowDialog();
            }
        }

        private void ResetLayout_Click(object sender, EventArgs e)
        {
            App.DockManager.ResetLayout();
        }
        #endregion
    }
}