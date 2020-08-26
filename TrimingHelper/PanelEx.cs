using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace TrimmingHelper
{
    public class PanelEx : Panel
    {
        public PanelEx() : base()
        {
        }
        protected override void OnPaintBackground( PaintEventArgs e )
        {
            if( this.DesignMode == true )
            {
                base.OnPaintBackground( e );
                //using(SolidBrush brush = new SolidBrush(Color.Black)){
                //    e.Graphics.FillRectangle( brush, e.ClipRectangle );
                //}
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // PanelEx
            // 
            this.Scroll += new System.Windows.Forms.ScrollEventHandler(this.PanelEx_Scroll);
            this.ResumeLayout(false);

        }

        private void PanelEx_Scroll( object sender, ScrollEventArgs e )
        {

        }
    }
}
