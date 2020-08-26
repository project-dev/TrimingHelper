using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TrimmingHelper
{
    public partial class StampView : UserControl
    {

        public int SelectImageIndex
        {
            get
            {
                if (0 == m_lstStamp.SelectedItems.Count)
                {
                    return -1;
                }

                ListViewItem item = m_lstStamp.SelectedItems[0];
                return item.ImageIndex;
            }
        }

        public Image SelectImage
        {
            get
            {
                if (0 == m_lstStamp.SelectedItems.Count)
                {
                    return null;
                }

                ListViewItem item = m_lstStamp.SelectedItems[0];
                return m_snapList.Images[item.ImageIndex];
            }
        }

        public ImageList StampImageList
        {
            get
            {
                return this.m_snapList;
            }
        }


        public StampView()
        {
            InitializeComponent();
        }

        private void StampView_Load(object sender, EventArgs e)
        {
            //m_snapList.Images.Add();
        }

        private void m_lstStamp_Leave(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void m_lstStamp_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Hide();
            }

        }

        private void m_lstStamp_MouseLeave(object sender, EventArgs e)
        {
            this.Hide();

        }

        private void m_lstStamp_DoubleClick(object sender, EventArgs e)
        {
            if (0 == m_lstStamp.SelectedItems.Count)
            {
                return;
            }

            ListViewItem item = m_lstStamp.SelectedItems[0];
            int imgIdx = item.ImageIndex;
            Image stamp = m_snapList.Images[imgIdx];
        }
    }
}
