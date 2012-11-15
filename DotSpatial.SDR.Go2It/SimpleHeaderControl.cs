// -----------------------------------------------------------------------
// <copyright file="SimpleHeaderControl.cs" company="DotSpatial Team">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using DotSpatial.Controls.Header;

namespace DotSpatial.SDR.Go2It
{
    /// <summary>
    /// Creates a ToolStripContainer that hosts a MenuBarHeaderControl.
    /// </summary>
    [Export(typeof(IHeaderControl))]
    public class SimpleHeaderControl : GridHeaderControl, IPartImportsSatisfiedNotification
    {
        private ToolStripContainer _toolStripContainer1;
        private GridControlPanel _gridControlPanel1;

        [Import("Shell", typeof(ContainerControl))]
        private ContainerControl Shell { get; set; }

        #region IPartImportsSatisfiedNotification Members

        /// <summary>
        /// Called when a part's imports have been satisfied and it is safe to use. (Shell will have a value)
        /// </summary>
        public void OnImportsSatisfied()
        {
            this._toolStripContainer1 = new ToolStripContainer();
            this._toolStripContainer1.ContentPanel.SuspendLayout();
            this._toolStripContainer1.SuspendLayout();

            //this._toolStripContainer1.TopToolStripPanelVisible = false;
            //this._toolStripContainer1.LeftToolStripPanelVisible = false;
            //this._toolStripContainer1.RightToolStripPanelVisible = false;
            //this._toolStripContainer1.BottomToolStripPanelVisible = false;
            this._toolStripContainer1.Dock = DockStyle.Fill;
            this._toolStripContainer1.Name = "toolStripContainer1";

            // place all of the controls that were on the form originally inside of our content panel.
            while (Shell.Controls.Count > 0)
            {
                foreach (Control control in Shell.Controls)
                {
                    this._toolStripContainer1.ContentPanel.Controls.Add(control);
                }
            }

            Shell.Controls.Add(this._toolStripContainer1);

            this._toolStripContainer1.ContentPanel.ResumeLayout(false);
            this._toolStripContainer1.ResumeLayout(false);
            this._toolStripContainer1.PerformLayout();

            // create the table layout panel for buttons now
            this._gridControlPanel1 = new GridControlPanel();
            this._gridControlPanel1.Dock = DockStyle.Fill;
            this._gridControlPanel1.Name = "gridControlContainer1";
            this._gridControlPanel1.SuspendLayout();
            this._gridControlPanel1.TableLayoutPanel.SuspendLayout();

            Shell.Controls.Add(this._gridControlPanel1);

            var container = Shell.Controls.Find("vertSplitter", true)[0] as SplitContainer;
            if (container != null)
            {
                container.Panel1.Controls.Add(this._gridControlPanel1);   
            }
            else
            {
                Trace.WriteLine("SplitContainer was expected.");
            }

            this._gridControlPanel1.ResumeLayout(false);
            this._gridControlPanel1.TableLayoutPanel.ResumeLayout(false);
            this._gridControlPanel1.PerformLayout();
            this._gridControlPanel1.TableLayoutPanel.PerformLayout();

            Initialize(_toolStripContainer1, _gridControlPanel1);
        }

        #endregion
    }
}