using System;
using System.ComponentModel;

namespace WeifenLuo.WinFormsUI.Docking
{
    public partial class DockPanel
    {
        private DockPanelSkin m_dockPanelSkin = VS2012LightTheme.CreateVisualStudio2012Light();
        [LocalizedCategory("Category_Docking")]
        [LocalizedDescription("DockPanel_DockPanelSkin")]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)] // We can't customize this in designer yet
        //[Obsolete("Please use Theme instead.")]
        public DockPanelSkin Skin
        {
            get { return m_dockPanelSkin;  }
            set { m_dockPanelSkin = value; }
        }

        private ThemeBase m_dockPanelTheme = new VS2012LightTheme();
        [LocalizedCategory("Category_Docking")]
        [LocalizedDescription("DockPanel_DockPanelTheme")]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)] // We can't customize this in designer yet
        public ThemeBase Theme
        {
            get { return m_dockPanelTheme; }
            set
            {
                if (value == null)
                {

                    return;
                }

                if (m_dockPanelTheme.GetType() == value.GetType()) {
                    return;
                }

                m_dockPanelTheme = value;
                m_dockPanelTheme.Apply(this);
            }
        }
    }
}
