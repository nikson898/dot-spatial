// -----------------------------------------------------------------------
// <copyright file="Go2ItHeaderControl.cs" company="Spatial Data Research">
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
    /// Creates a ToolStripContainer that hosts a GridHeaderControl.
    /// </summary>
    [Export(typeof(IHeaderControl))]
    public class SimpleHeaderControl : GridHeaderControl, IPartImportsSatisfiedNotification
    {
        private ToolStripContainer _toolStripContainer1;
        private TableLayoutPanel _tableLayoutPanel1;

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
            this._toolStripContainer1.TopToolStripPanelVisible = false;
            this._toolStripContainer1.LeftToolStripPanelVisible = false;
            this._toolStripContainer1.RightToolStripPanelVisible = false;
            this._toolStripContainer1.BottomToolStripPanelVisible = false;
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
            this._tableLayoutPanel1 = new TableLayoutPanel();
            this._tableLayoutPanel1.SuspendLayout();
            this._tableLayoutPanel1.AutoSize = true;
            this._tableLayoutPanel1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this._tableLayoutPanel1.ColumnCount = 3;
            this._tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
            this._tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
            this._tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
            this._tableLayoutPanel1.GrowStyle = TableLayoutPanelGrowStyle.AddRows;
            this._tableLayoutPanel1.Dock = DockStyle.Left;
            this._tableLayoutPanel1.Name = "gridControlContainer1";
            // this._tableLayoutPanel1.BackColor = Color.Firebrick;
            this._tableLayoutPanel1.Location = new Point(0, 0);
            this._tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.AutoSize, 100F));

            Shell.Controls.Add(this._tableLayoutPanel1);

            var container = Shell.Controls.Find("vertSplitter", true)[0] as SplitContainer;
            if (container != null)
            {
                container.Panel1.Controls.Add(this._tableLayoutPanel1);   
            }
            else
            {
                Trace.WriteLine("SplitContainer was expected.");
            }

            this._tableLayoutPanel1.ResumeLayout(false);
            this._tableLayoutPanel1.PerformLayout();

            Initialize(_toolStripContainer1, _tableLayoutPanel1);
        }

        #endregion
    }
}