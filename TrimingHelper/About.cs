using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TrimmingHelper
{
    public partial class About : Form
    {
        public About()
        {
            InitializeComponent();
        }

        private void About_Load(object sender, EventArgs e)
        {
            m_lbVersion.Text = Application.ProductVersion;

            StringBuilder version = new StringBuilder();
            version.AppendLine("1.0.0.8 ドラッグ中のスクロールの移動量をマウスの位置で変わるように変更");
            version.AppendLine("        画像の自動判定を高速化");
            version.AppendLine("        マウスカーソルの場所のRGBを表示");
            version.AppendLine("1.0.0.7 透過情報を保持たままクリップボードへ張り付けられるようにした。");
            version.AppendLine("        範囲選択があると自動でクリップボードへ転送できるオプション追加。");
            m_txtVersion.Text = version.ToString();
        }

        private void m_btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
