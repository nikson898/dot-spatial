﻿// -----------------------------------------------------------------------
// <copyright file="TabbedDocking.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace DotSpatial.SDR.Go2It
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using DotSpatial.Controls.Docking;
    using System.Windows.Forms;
    using System.ComponentModel.Composition;
    using System.Diagnostics;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class TabbedDocking : IDockManager, IPartImportsSatisfiedNotification
    {
        private System.Windows.Forms.TabControl uxTabControl;

        [Import("Shell", typeof(ContainerControl))]
        private ContainerControl Shell { get; set; }

        public TabbedDocking()
        {

        }

        public event EventHandler<DockablePanelEventArgs> ActivePanelChanged;

        public event EventHandler<DockablePanelEventArgs> PanelClosed;

        public event EventHandler<DockablePanelEventArgs> PanelAdded;

        public event EventHandler<DockablePanelEventArgs> PanelRemoved;

        #region OnActivePanelChanged
        /// <summary>
        /// Triggers the ActivePanelChanged event.
        /// </summary>
        public virtual void OnActivePanelChanged(DockablePanelEventArgs ea)
        {
            if (ActivePanelChanged != null)
                ActivePanelChanged(null/*this*/, ea);
        }
        #endregion

        #region OnPanelClosed
        /// <summary>
        /// Triggers the PanelClosed event.
        /// </summary>
        public virtual void OnPanelClosed(DockablePanelEventArgs ea)
        {
            if (PanelClosed != null)
                PanelClosed(null/*this*/, ea);
        }
        #endregion

        #region OnPanelAdded
        /// <summary>
        /// Triggers the PanelAdded event.
        /// </summary>
        public virtual void OnPanelAdded(DockablePanelEventArgs ea)
        {
            if (PanelAdded != null)
                PanelAdded(null/*this*/, ea);
        }
        #endregion

        #region OnPanelRemoved
        /// <summary>
        /// Triggers the PanelRemoved event.
        /// </summary>
        public virtual void OnPanelRemoved(DockablePanelEventArgs ea)
        {
            if (PanelRemoved != null)
                PanelRemoved(null/*this*/, ea);
        }
        #endregion

        public void Add(DockablePanel panel)
        {
            System.Windows.Forms.TabPage tabPage = new TabPage();

            tabPage.Controls.Add(panel.InnerControl);
            panel.InnerControl.Dock = DockStyle.Fill;
            tabPage.Name = panel.Key;
            tabPage.Text = panel.Caption;

            this.uxTabControl.Controls.Add(tabPage);
        }

        public void Remove(string key)
        {
            if (uxTabControl.Controls.ContainsKey(key))
                uxTabControl.Controls.RemoveByKey(key);
        }

        public void ResetLayout()
        {

        }

        public void SelectPanel(string key)
        {

        }

        public void OnImportsSatisfied()
        {
            uxTabControl = new TabControl();
            this.uxTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uxTabControl.Location = new System.Drawing.Point(0, 0);
            this.uxTabControl.Name = "uxTabControl";
            this.uxTabControl.SelectedIndex = 0;
            this.uxTabControl.Size = new System.Drawing.Size(227, 506);
            this.uxTabControl.TabIndex = 0;

            var container = Shell.Controls.Find("horzSplitter", true)[0] as SplitContainer;

            if (container != null)
                container.Panel1.Controls.Add(this.uxTabControl);
            else
            {
                Trace.WriteLine("SplitContainer was expected.");
            }
        }

        public void HidePanel(string key)
        {

        }
    }
}
