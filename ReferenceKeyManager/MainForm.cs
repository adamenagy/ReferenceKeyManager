using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Inventor;
using Newtonsoft.Json.Linq;
using ReferenceKeyManager.Properties;

namespace ReferenceKeyManager
{
    public partial class MainForm : Form
    {
        Inventor.Application m_app;
        Inventor.ApplicationEvents m_events;
        HashSet<string> m_docsWithContexts;

        const string kDefault = "Default";
        const string kDefault0 = kDefault + " [0]";
        const string kContext = "context";
        const string kReference = "reference";

        public MainForm(Inventor.Application app)
        {
            InitializeComponent();

            imageList.Images.Add(kContext, Resources.FolderOpened_16x);
            imageList.Images.Add(kReference, Resources.Key_16x);

            m_app = app;
            m_docsWithContexts = new HashSet<string>();

            ReferenceKeysTreeView.AfterSelect += ReferenceKeysTreeView_AfterSelect;

            m_events = m_app.ApplicationEvents;
            m_events.OnActivateDocument += M_events_OnActivateDocument;
            m_events.OnDeactivateDocument += M_events_OnDeactivateDocument;

            LoadData();
        }

        private void M_events_OnDeactivateDocument(_Document DocumentObject, EventTimingEnum BeforeOrAfter, NameValueMap Context, out HandlingCodeEnum HandlingCode)
        {
            if (BeforeOrAfter == EventTimingEnum.kBefore && this.Visible)
            {
                try
                {
                    SaveData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Could not save data");
                }
            }
            
            HandlingCode = HandlingCodeEnum.kEventHandled;
        }

        private void M_events_OnActivateDocument(_Document DocumentObject, EventTimingEnum BeforeOrAfter, NameValueMap Context, out HandlingCodeEnum HandlingCode)
        {
            if (BeforeOrAfter == EventTimingEnum.kAfter && this.Visible)
            {
                try
                {
                    LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Could not load data");
                }
            }
              
            HandlingCode = HandlingCodeEnum.kEventHandled;
        }

        private string GetDataFilePath()
        {
            string dllFolder = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string name = m_app.ActiveDocument.DisplayName;
            return System.IO.Path.Combine(dllFolder, $"{name}.json");
        }

        private void LoadData()
        {
            ReferenceKeysTreeView.Nodes.Clear();

            bool loaded = m_docsWithContexts.Contains(m_app.ActiveDocument.InternalName);

            string jsonPath = GetDataFilePath();
            if (System.IO.File.Exists(jsonPath))
            {
                JObject root = JObject.Parse(System.IO.File.ReadAllText(jsonPath, Encoding.UTF8));
                JObject contexts = root.GetValue("contexts") as JObject;
                foreach (JProperty context in contexts.Children())
                {
                    JObject data = context.Value as JObject;
                    int i = data.GetValue("index").Value<int>();

                    if (context.Name != kDefault && !loaded)
                    {
                        i = LoadContext(context.Name);
                    }

                    TreeNode node = ReferenceKeysTreeView.Nodes.Add(context.Name, $"{context.Name} [{i}]");
                    node.ImageKey = kContext;
                    node.SelectedImageKey = kContext;

                    JArray keys = data.GetValue("keys") as JArray;
                    foreach (JValue key in keys)
                    {
                        TreeNode subNode = node.Nodes.Add(key.Value as string, key.Value as string);
                        subNode.ImageKey = kReference;
                        subNode.SelectedImageKey = kReference;
                    }
                }
            }

            EnsureDefaultContext();

            ReferenceKeysTreeView.ExpandAll();

            CreateReferenceKeyButton.Enabled = false;
            SelectButton.Enabled = false;

            if (!loaded)
            {
                m_docsWithContexts.Add(m_app.ActiveDocument.InternalName);
            }
        }
   
        private void SaveData()
        {
            JObject root = new JObject();
            JObject contexts = new JObject();
            foreach (TreeNode node in ReferenceKeysTreeView.Nodes)
            {
                JObject context = new JObject();
                JArray keys = new JArray();
                foreach (TreeNode subNode in node.Nodes)
                {
                    keys.Add(subNode.Text);
                }

                int index = GetContextIndex(node.Text);
                context.Add("index", index);
                context.Add("keys", keys);
                contexts.Add(GetContextString(node.Text), context);
            }

            root.Add("contexts", contexts);

            string json = root.ToString();

            string jsonPath = GetDataFilePath();
            System.IO.File.WriteAllText(jsonPath, json, Encoding.UTF8);
        }

        private void EnsureDefaultContext()
        {
            if (!ReferenceKeysTreeView.Nodes.ContainsKey(kDefault))
            {
                ReferenceKeysTreeView.Nodes.Insert(0, kDefault, kDefault0).EnsureVisible();
            }
        }

        private void GetContextAndKey(TreeNode node, out string context, out string key)
        {
            TreeNode contextNode = node;
            key = null;
            if (node.Parent != null)
            {
                contextNode = node.Parent;
                key = node.Text;
            }

            context = contextNode.Text;
        }

        private int GetContextIndex(string context)
        {
            string id = context.Split(new char[] { '[', ']' })[1];
            return int.Parse(id);
        }

        private string GetContextString(string context)
        {
            return context.Split(new char[] { ' ' })[0];
        }

        private byte [] GetContextBytes(string context) 
        {
            byte[] bytes = new byte[] { };
            context = GetContextString(context);

            Inventor.ReferenceKeyManager rkm = m_app.ActiveDocument.ReferenceKeyManager;
            rkm.StringToKey(context, bytes);

            return bytes;
        }

        private void ReferenceKeysTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            string context, key;
            GetContextAndKey(e.Node, out context, out key);

            SelectButton.Enabled = (key != null);
            CreateReferenceKeyButton.Enabled = (key == null);
        }

        private void SelectButton_Click(object sender, EventArgs e)
        {
            Inventor.ReferenceKeyManager rkm = m_app.ActiveDocument.ReferenceKeyManager;

            string context, key;
            GetContextAndKey(ReferenceKeysTreeView.SelectedNode, out context, out key);
            int i = GetContextIndex(context);

            try 
            {
                byte[] bytes = new byte[] { };
                rkm.StringToKey(key, ref bytes);
                object matchType;
                dynamic result = rkm.BindKeyToObject(ref bytes, i, out matchType);
                if (result.GetType() == Type.GetType("Inventor.ObjectCollection"))
                {
                    m_app.ActiveDocument.SelectSet.SelectMultiple(result);
                }
                else
                {
                    m_app.ActiveDocument.SelectSet.Select(result);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Could not bind key to object");
            }
        }


        private int LoadContext(string context)
        {
            Inventor.ReferenceKeyManager rkm = m_app.ActiveDocument.ReferenceKeyManager;

            byte[] bytes = new byte[] { };
            rkm.StringToKey(context, ref bytes);

            return rkm.LoadContextFromArray(ref bytes);
        }

        private int CreateContext(out string context)
        {
            Inventor.ReferenceKeyManager rkm = m_app.ActiveDocument.ReferenceKeyManager;

            int index = rkm.CreateKeyContext();

            byte[] bytes = new byte[] { };
            rkm.SaveContextToArray(index, ref bytes);

            context = rkm.KeyToString(bytes);

            return index;
        }

        private void CreateContextButton_Click(object sender, EventArgs e)
        {
            string context;
            int i = CreateContext(out context);

            if (ReferenceKeysTreeView.Nodes.ContainsKey(context))
            {
                TreeNode existingNode = ReferenceKeysTreeView.Nodes.Find(context, false)[0];
                ReferenceKeysTreeView.SelectedNode = existingNode;
                
                Inventor.ReferenceKeyManager rkm = m_app.ActiveDocument.ReferenceKeyManager;
                rkm.ReleaseKeyContext(i);

                MessageBox.Show("A suitable context already exists and is now highlighted in the Browser", "Duplicate context");

                return;
            }

            TreeNode node = ReferenceKeysTreeView.Nodes.Add(context, $"{context} [{i}]");
            node.ImageKey = kContext;
            node.SelectedImageKey = kContext;
            node.EnsureVisible();
        }

        private void CreateReferenceKeyButton_Click(object sender, EventArgs e)
        {
            SelectSet ss = m_app.ActiveDocument.SelectSet;
            if (ss.Count != 1)
            {
                MessageBox.Show("Please select a single object before using this command", "No single object selected");
                return;
            }

            try
            {
                string context, key;
                GetContextAndKey(ReferenceKeysTreeView.SelectedNode, out context, out key);
                int i = GetContextIndex(context);

                byte[] bytes = new byte[] { };
                ss[1].GetReferenceKey(ref bytes, i);

                Inventor.ReferenceKeyManager rkm = m_app.ActiveDocument.ReferenceKeyManager;
                key = rkm.KeyToString(bytes);

                if (ReferenceKeysTreeView.SelectedNode.Nodes.ContainsKey(key))
                {
                    TreeNode existingNode = ReferenceKeysTreeView.SelectedNode.Nodes.Find(context, false)[0];
                    ReferenceKeysTreeView.SelectedNode = existingNode;

                    MessageBox.Show("Such key already exists and is now highlighted in the Browser", "Duplicate key");

                    return;
                }

                TreeNode node = ReferenceKeysTreeView.SelectedNode.Nodes.Add(key, key);
                node.ImageKey = kReference;
                node.SelectedImageKey = kReference;
                node.EnsureVisible();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Could not create key");
            }
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            TreeNode node = ReferenceKeysTreeView.SelectedNode;
            if (node != null)
            {
                // if it's a context
                if (node.Parent == null)
                {
                    int index = GetContextIndex(node.Text);
                    if (index > 0)
                    {
                        Inventor.ReferenceKeyManager rkm = m_app.ActiveDocument.ReferenceKeyManager;
                        rkm.ReleaseKeyContext(index);
                    }
                }

                node.Remove();

                EnsureDefaultContext();
            }
        }
    }
}
