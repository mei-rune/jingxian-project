using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace jingxian.ui.dialogs
{
    using Empinia.Core.Runtime;
    using jingxian.ui.controls;

    public partial class ArchivingSettingPanel : UserControl
    {
        //private Dictionary<string, ArchivingActionConfiguration> m_configurationsById;

        public ArchivingSettingPanel()
        {
            InitializeComponent();
        }

        public void Initialize(IExtensionRegistry extensionRegistry)
        {
            //m_configurationsById = new Dictionary<string, ArchivingActionConfiguration>();
            ConfigurationSupplier<ArchivingActionConfiguration> supplier =
                new ConfigurationSupplier<ArchivingActionConfiguration>(extensionRegistry);
            IDictionary<string, ArchivingActionConfiguration> configurations =
                supplier.Fetch(jingxian.ui.Constants.ArchivingActionPointId);

            foreach (ArchivingActionConfiguration cfg in configurations.Values)
            {
                //m_configurationsById.Add(cfg.Id, cfg);
                this.m_MessageAction.Items.Add(cfg);
            }
        }

        public string Time
        {
            get{ return m_MessageTime.Text; }
            set 
            {
                if (null == value)
                    return;
                if (m_MessageTime.Items.Contains(value))
                    m_MessageTime.SelectedItem = value;
            }
        }

        public string Action
        {
            get 
            {
                ArchivingActionConfiguration cfg = m_MessageAction.SelectedItem as ArchivingActionConfiguration;
                return (null == cfg) ? null : cfg.Id;
            }
            set
            {
                if (null == value)
                    return;
                foreach (ArchivingActionConfiguration cfg in m_MessageAction.Items)
                {
                    if (cfg.Id == value)
                    {
                        m_MessageAction.SelectedItem = cfg;
                        break;
                    }
                }
            }
        }

    }
}

