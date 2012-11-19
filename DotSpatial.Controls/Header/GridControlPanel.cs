using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DotSpatial.Controls.Header
{
    public partial class _gridControlPanel : UserControl
    {
        #region ---------------- Class Variables

        private Dictionary<int, ToolStrip> _controlOrder = new Dictionary<int, ToolStrip>();
        private SplitContainer _splitContainer;

        #endregion

        #region ---------------- Properties

        //public TableLayoutPanel TableLayoutPanel
        //{
        //    get
        //    {
        //        return _tableLayoutPanel;
        //    }
        //    set
        //    {
        //        if (value == null) return;
        //        _tableLayoutPanel = value;
        //    }
        //}

        //public SplitContainer SplitContainer
        //{
        //    get
        //    {
        //        return _splitContainer;
        //    }
        //    set
        //    {
        //        if (value == null) return;
        //        _splitContainer = value;
        //        _splitContainer.SplitterMoved += SplitterMoved;
        //        _splitContainer.SplitterMoving += SplitterMoving;
        //    }
        //}

        #endregion

        #region ---------------- Constructors

        public _gridControlPanel()
        {
            InitializeComponent();
            _controlOrder = new Dictionary<int, ToolStrip>();
        }

        #endregion 

        #region ---------------- Public Methods

        public void AddControl(ToolStrip ctrl, int pos)
        {
            // anchor the control to top left corners
            ctrl.Anchor = (AnchorStyles.Top | AnchorStyles.Left);
            // if there are  no controls yet, make sure the parent sets the split container and events
            if (this._tableLayoutPanel.Controls.Count == 0)
            {
                _splitContainer = (SplitContainer)this.Parent.Controls.Owner.Parent;
            }
            if (_controlOrder.ContainsKey(pos))
            {
                // this is a dupe key, append this control to the end instead
                this.AddControl(ctrl);   
            }
            // add it to the sort tracking dict
            _controlOrder.Add(pos, ctrl);

            int row = 1;
            // find the proper row to place this control
            if (pos > _tableLayoutPanel.ColumnCount)
            {
                // find the proper row to place this control
                double drow = pos/_tableLayoutPanel.ColumnCount;
                row = (int)Math.Ceiling(drow);
            }
            var pos_sub = (row - 1) * _tableLayoutPanel.ColumnCount;
            var column = pos - pos_sub;

            // add the control to the table now
            this._tableLayoutPanel.Controls.Add(ctrl, column - 1, row - 1);

            // MessageBox.Show(this._tableLayoutPanel.Controls.Count.ToString());
            // _splitContainer.SplitterDistance = _tableLayoutPanel.Width;
        }

        public void AddControl(ToolStrip ctrl)
        {
            // determine the last slot and give this ctrl it 
            int sortOrder = 0;
            var list = _controlOrder.Keys.ToList();
            if (list.Count > 0)
            {
                list.Sort();
                sortOrder = list[list.Count] + 1;
            }
            // heads up seven up....  pass it on
            this.AddControl(ctrl, sortOrder);            
        }

        #endregion

        #region ------------------- Event Handlers

        private void SplitterMoved(object sender, EventArgs e)
        {
            var d = "lsidkahgl";
        }

        private void SplitterMoving(object sender, EventArgs e)
        {
            var f = "5239879";
        }

        #endregion
    }
}
