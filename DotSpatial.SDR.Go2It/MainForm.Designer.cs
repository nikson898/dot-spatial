namespace DotSpatial.SDR.Go2It
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.vertSplitter = new System.Windows.Forms.SplitContainer();
            this.horzSplitter = new System.Windows.Forms.SplitContainer();
            this.map = new DotSpatial.Controls.Map();
            this.appManager = new DotSpatial.Controls.AppManager();
            ((System.ComponentModel.ISupportInitialize)(this.vertSplitter)).BeginInit();
            this.vertSplitter.Panel2.SuspendLayout();
            this.vertSplitter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.horzSplitter)).BeginInit();
            this.horzSplitter.Panel2.SuspendLayout();
            this.horzSplitter.SuspendLayout();
            this.SuspendLayout();
            // 
            // vertSplitter
            // 
            this.vertSplitter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vertSplitter.Location = new System.Drawing.Point(0, 0);
            this.vertSplitter.Name = "vertSplitter";
            // 
            // vertSplitter.Panel2
            // 
            this.vertSplitter.Panel2.Controls.Add(this.horzSplitter);
            this.vertSplitter.Size = new System.Drawing.Size(825, 571);
            this.vertSplitter.SplitterDistance = 215;
            this.vertSplitter.TabIndex = 0;
            // 
            // horzSplitter
            // 
            this.horzSplitter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.horzSplitter.Location = new System.Drawing.Point(0, 0);
            this.horzSplitter.Name = "horzSplitter";
            this.horzSplitter.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // horzSplitter.Panel2
            // 
            this.horzSplitter.Panel2.Controls.Add(this.map);
            this.horzSplitter.Size = new System.Drawing.Size(606, 571);
            this.horzSplitter.SplitterDistance = 96;
            this.horzSplitter.TabIndex = 0;
            // 
            // map
            // 
            this.map.AllowDrop = true;
            this.map.BackColor = System.Drawing.Color.White;
            this.map.CollectAfterDraw = false;
            this.map.CollisionDetection = false;
            this.map.Dock = System.Windows.Forms.DockStyle.Fill;
            this.map.ExtendBuffer = false;
            this.map.FunctionMode = DotSpatial.Controls.FunctionMode.None;
            this.map.IsBusy = false;
            this.map.Legend = null;
            this.map.Location = new System.Drawing.Point(0, 0);
            this.map.Name = "map";
            this.map.ProgressHandler = null;
            this.map.ProjectionModeDefine = DotSpatial.Controls.ActionMode.Prompt;
            this.map.ProjectionModeReproject = DotSpatial.Controls.ActionMode.Prompt;
            this.map.RedrawLayersWhileResizing = false;
            this.map.SelectionEnabled = true;
            this.map.Size = new System.Drawing.Size(606, 471);
            this.map.TabIndex = 0;
            // 
            // appManager
            // 
            this.appManager.CompositionContainer = null;
            this.appManager.Directories = ((System.Collections.Generic.List<string>)(resources.GetObject("appManager.Directories")));
            this.appManager.DockManager = null;
            this.appManager.HeaderControl = null;
            this.appManager.Legend = null;
            this.appManager.Map = this.map;
            this.appManager.ProgressHandler = null;
            this.appManager.ShowExtensionsDialog = DotSpatial.Controls.ShowExtensionsDialog.Default;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(825, 571);
            this.Controls.Add(this.vertSplitter);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.vertSplitter.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.vertSplitter)).EndInit();
            this.vertSplitter.ResumeLayout(false);
            this.horzSplitter.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.horzSplitter)).EndInit();
            this.horzSplitter.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.AppManager appManager;
        private Controls.Map map;
        private System.Windows.Forms.SplitContainer vertSplitter;
        private System.Windows.Forms.SplitContainer horzSplitter;
    }
}

