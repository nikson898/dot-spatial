using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DotSpatial.Controls.Header
{
    public partial class GridControlContainer : UserControl
    {
        #region ---------------- Class Variables

        // internal variables
        private TableLayoutPanel _tableLayoutPanel;
        private ToolStripContainer _toolStripContainer;

        #endregion

        #region ---------------- Public Properties

        public TableLayoutPanel TableLayoutPanel
        {
            get
            {
                return _tableLayoutPanel;
            }
            set
            {
                if (value == null) return;
                _tableLayoutPanel = value;
                _tableLayoutPanel = this.tableLayoutPanel1;
            }
        }

        public ToolStripContainer ToolStripContainer
        {
            get
            {
                return _toolStripContainer;
            }
            set
            {
                if (value == null) return;
                _toolStripContainer = value;
                _toolStripContainer = this.toolStripContainer1;
            }
        }

        #endregion

        #region ---------------- Public Methods

        /// <summary>
        /// This is the constructor, it makes a GridControlContainer
        /// </summary>
        public GridControlContainer()
        {
            InitializeComponent();
            _tableLayoutPanel = new TableLayoutPanel();
            _toolStripContainer = new ToolStripContainer();
        }

        #endregion
    }
}
