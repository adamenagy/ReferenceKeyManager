using System;
using System.Runtime.InteropServices;

namespace ReferenceKeyManager
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

            m_events.OnActivateDocument -= M_events_OnActivateDocument;
            m_events.OnDeactivateDocument -= M_events_OnDeactivateDocument;
            m_app = null;
            m_events = null;

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.ReferenceKeysTreeView = new System.Windows.Forms.TreeView();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.CreateContextButton = new System.Windows.Forms.Button();
            this.SelectButton = new System.Windows.Forms.Button();
            this.CreateReferenceKeyButton = new System.Windows.Forms.Button();
            this.DeleteButton = new System.Windows.Forms.Button();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ReferenceKeysTreeView
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.ReferenceKeysTreeView, 2);
            this.ReferenceKeysTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ReferenceKeysTreeView.ImageIndex = 0;
            this.ReferenceKeysTreeView.ImageList = this.imageList;
            this.ReferenceKeysTreeView.Location = new System.Drawing.Point(3, 35);
            this.ReferenceKeysTreeView.Name = "ReferenceKeysTreeView";
            this.ReferenceKeysTreeView.SelectedImageIndex = 0;
            this.ReferenceKeysTreeView.ShowRootLines = false;
            this.ReferenceKeysTreeView.Size = new System.Drawing.Size(298, 103);
            this.ReferenceKeysTreeView.TabIndex = 7;
            this.ReferenceKeysTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.ReferenceKeysTreeView_AfterSelect);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 130F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.CreateContextButton, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.SelectButton, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.ReferenceKeysTreeView, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.CreateReferenceKeyButton, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.DeleteButton, 1, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(5, 5);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(304, 173);
            this.tableLayoutPanel1.TabIndex = 8;
            // 
            // toolTip
            // 
            this.toolTip.ToolTipTitle = "Info";
            // 
            // CreateContextButton
            // 
            this.CreateContextButton.Image = global::ReferenceKeyManager.Properties.Resources.Add_16x;
            this.CreateContextButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.CreateContextButton.Location = new System.Drawing.Point(3, 3);
            this.CreateContextButton.Name = "CreateContextButton";
            this.CreateContextButton.Size = new System.Drawing.Size(124, 26);
            this.CreateContextButton.TabIndex = 1;
            this.CreateContextButton.Text = "Create Key Context";
            this.CreateContextButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolTip.SetToolTip(this.CreateContextButton, "Creates a Key Context");
            this.CreateContextButton.UseVisualStyleBackColor = true;
            this.CreateContextButton.Click += new System.EventHandler(this.CreateContextButton_Click);
            // 
            // SelectButton
            // 
            this.SelectButton.Image = global::ReferenceKeyManager.Properties.Resources.FindInFile_16x;
            this.SelectButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.SelectButton.Location = new System.Drawing.Point(3, 144);
            this.SelectButton.Name = "SelectButton";
            this.SelectButton.Size = new System.Drawing.Size(124, 26);
            this.SelectButton.TabIndex = 0;
            this.SelectButton.Text = "Select Object";
            this.SelectButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolTip.SetToolTip(this.SelectButton, "Selects the object based on the higlighted Reference Key");
            this.SelectButton.UseVisualStyleBackColor = true;
            this.SelectButton.Click += new System.EventHandler(this.SelectButton_Click);
            // 
            // CreateReferenceKeyButton
            // 
            this.CreateReferenceKeyButton.Image = global::ReferenceKeyManager.Properties.Resources.Add_16x;
            this.CreateReferenceKeyButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.CreateReferenceKeyButton.Location = new System.Drawing.Point(133, 3);
            this.CreateReferenceKeyButton.Name = "CreateReferenceKeyButton";
            this.CreateReferenceKeyButton.Size = new System.Drawing.Size(138, 26);
            this.CreateReferenceKeyButton.TabIndex = 6;
            this.CreateReferenceKeyButton.Text = "Get Reference Key";
            this.CreateReferenceKeyButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolTip.SetToolTip(this.CreateReferenceKeyButton, "Gets Reference Key for the currently selected object");
            this.CreateReferenceKeyButton.UseVisualStyleBackColor = true;
            this.CreateReferenceKeyButton.Click += new System.EventHandler(this.CreateReferenceKeyButton_Click);
            // 
            // DeleteButton
            // 
            this.DeleteButton.Image = global::ReferenceKeyManager.Properties.Resources.Trash_16x;
            this.DeleteButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.DeleteButton.Location = new System.Drawing.Point(133, 144);
            this.DeleteButton.Name = "DeleteButton";
            this.DeleteButton.Size = new System.Drawing.Size(138, 26);
            this.DeleteButton.TabIndex = 8;
            this.DeleteButton.Text = "Delete Browser Node";
            this.DeleteButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolTip.SetToolTip(this.DeleteButton, "Deletes the currently selected node in the Key Context/Reference Key browser");
            this.DeleteButton.UseVisualStyleBackColor = true;
            this.DeleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // imageList
            // 
            this.imageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(314, 183);
            this.Controls.Add(this.tableLayoutPanel1);
            this.MinimumSize = new System.Drawing.Size(330, 200);
            this.Name = "MainForm";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.Text = "Reference Key Manager";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button SelectButton;
        private System.Windows.Forms.Button CreateContextButton;
        private System.Windows.Forms.Button CreateReferenceKeyButton;
        private System.Windows.Forms.TreeView ReferenceKeysTreeView;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button DeleteButton;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.ImageList imageList;
    }
}