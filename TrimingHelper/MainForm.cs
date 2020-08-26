using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Threading;
using System.Threading.Tasks;

using TrimmingHelper.stamp;

namespace TrimmingHelper
{

    delegate void SimpleDelegate();

    public partial class frmMain : Form
    {
        // バックバッファ
        private Bitmap m_bmp = null;
        private Bitmap m_backBuffer = null;

        private int m_mouseDownX = 0;
        private int m_mouseDownY = 0;
        
        private Task task;
        
        private CancellationTokenSource ctsrc = null;
        private CancellationToken ct;
        private Rectangle m_trimingRect = new Rectangle();

        private StampView m_stanpView = new StampView();


        // スタンプ一覧
        private Dictionary<String, StampItem> m_stamps = new Dictionary<string, StampItem>();

        private List<StampItem> m_drawStamps = new List<StampItem>();


        // 高速化用
        private byte[] bitmapBuff = null;
        private int bitmapStride;

        public frmMain()
        {
            InitializeComponent();
            this.Controls.Add(m_stanpView);
        }

        private void frmMain_Load( object sender, EventArgs e )
        {
            this.DoubleBuffered = true;
            m_cmbMode.SelectedIndex = 0;
            updateImgInfo();
        }

        private void m_btnPaste_Click( object sender, EventArgs e )
        {
            IDataObject clip = Clipboard.GetDataObject();
            if( clip.GetDataPresent( typeof( Bitmap ) ) == false )
            {
                return;
            }
            m_bmp = clip.GetData( typeof( Bitmap ) ) as Bitmap;
            if( m_bmp == null )
            {
                return;
            }

            if( m_chkClearSelect.Checked == true )
            {
                m_trimingRect = new Rectangle( 0, 0, 0, 0 );
            }

            m_pnlTriming.AutoScrollMinSize = new Size( m_bmp.Width, m_bmp.Height );
            m_pnlTriming.HorizontalScroll.Value = 0;
            m_pnlTriming.VerticalScroll.Value = 0;
            getBitmapBuff(m_bmp);

            updateImgInfo();

            if (m_cbAutoSend.Checked == true)
            {
                // 範囲指定があるかどうかは先の処理でチェックしてた
                m_btnTrim_Click(sender, e);
            }
        }

        private void m_btnTrim_Click( object sender, EventArgs e )
        {
            try
            {
				if (m_trimingRect == new Rectangle(0, 0, 0, 0))
				{
					return;
				}

                using( Bitmap trimBmp = new Bitmap( m_trimingRect.Width, m_trimingRect.Height ) )
                using (Graphics graph = Graphics.FromImage(trimBmp))
                {
                    graph.DrawImage( m_bmp, 0, 0, m_trimingRect, GraphicsUnit.Pixel );

                    // うまく投下されないのでコメントアウト
                    if (m_cbLeftTopTransparent.Checked == true)
                    {
                        // 左上の色で透過
                        Color color;
                        if (m_cbUseClasssic.Checked == true)
                        {
                            color = m_bmp.GetPixel(0, 0);
                        }
                        else
                        {
                            color = this.GetPixel(0, 0);
                        }
                        trimBmp.MakeTransparent(color);
 
                        // BITMAPだとクリップボードに張り付けると透過情報がなくなる
                        // PNG形式なら問題なくできる
                        // https://tomovertex.at.webry.info/201009/article_2.html
                        using (MemoryStream ms = new MemoryStream())
                        {
                            trimBmp.Save(ms, ImageFormat.Png);
                            DataObject obj = new DataObject();
                            obj.SetData("PNG", false, ms);
                            Clipboard.SetDataObject(obj, true);
                        }
                    }
                    else
                    {
                        Clipboard.SetDataObject(trimBmp, true);
                    }
                }
            }
            catch( Exception ex )
            {
                MessageBox.Show( this, ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error );
            }
        }

        private void m_pnlTriming_Paint( object sender, PaintEventArgs e )
        {
            if( this.DesignMode == true )
            {
                return;
            }
            if( m_backBuffer != null )
            {
                e.Graphics.DrawImage( m_backBuffer, 0, 0 );
            }
            else
            {
                using( var brush = new SolidBrush( Color.Black ) )
                {
                    e.Graphics.FillRectangle( brush, m_pnlTriming.Bounds );
                }
            }
        }


        private void frmMain_Resize( object sender, EventArgs e )
        {
            updateImgInfo();
        }

        private void updateImgInfo()
        {
            try
            {
                if( task != null )
                {
                    return;
                }
                if( m_bmp == null )
                {
                    m_pnlTriming.HorizontalScroll.Minimum = 0;
                    m_pnlTriming.VerticalScroll.Minimum = 0;
                }
                else
                {
                    int width = m_bmp.Width;
                    int height = m_bmp.Height;
                    if( m_pnlTriming.Width <= width )
                    {
                        m_pnlTriming.HorizontalScroll.Minimum = width;
                    }
                    else
                    {
                        m_pnlTriming.HorizontalScroll.Minimum = 0;
                    }
                    if( m_pnlTriming.Height <= height )
                    {
                        m_pnlTriming.VerticalScroll.Minimum = height;
                    }
                    else
                    {
                        m_pnlTriming.VerticalScroll.Minimum = 0;
                    }
                    //m_pnlTriming.AutoScrollMinSize = m_pnlTriming.Size;
                }
                if( m_backBuffer != null )
                {
                    m_backBuffer.Dispose();
                    m_backBuffer = null;
                }
                if( m_pnlTriming.Width != 0 && m_pnlTriming.Height != 0 )
                {
                    m_backBuffer = new Bitmap( m_pnlTriming.Width, m_pnlTriming.Height );
                }

                // スタンプの描画


                updateBackBuffer();
            }
            catch( Exception e )
            {
                System.Console.WriteLine( e.ToString() );
            }
        }

        private void updateBackBuffer()
        {
            if( task != null )
            {
                return;
            }
            if( m_backBuffer == null )
            {
                return;
            }
            using( SolidBrush bkBrush = new SolidBrush( Color.Black ))
            using( SolidBrush filterBrush = new SolidBrush( Color.FromArgb(32, Color.Black) ) )
            using(Font filterFont = new Font( this.Font.FontFamily, this.Font.Size * 4, FontStyle.Bold, Font.Unit))
            using( Graphics bkBuff = Graphics.FromImage( m_backBuffer ) )
            {
                // 背景クリア
                bkBuff.FillRectangle( bkBrush, 0, 0, m_pnlTriming.Width, m_pnlTriming.Height );

                // キャプチャ画像を描画
                if( m_bmp != null )
                {
                    //int x = m_scBarH.Value;
                    //int y = m_scBarV.Value;
                    int x = m_pnlTriming.HorizontalScroll.Value;
                    int y = m_pnlTriming.VerticalScroll.Value;

                    bkBuff.DrawImage( m_bmp, 0 - x, 0 - y );
                }
                bkBuff.FillRectangle( filterBrush, 0, 0, m_pnlTriming.Width, m_pnlTriming.Height );

                // Stampを描画
                if (m_drawStamps != null)
                {
                    foreach (StampItem item in m_drawStamps)
                    {
                        item.onPaint(bkBuff);
                    }
                }
                
                // Trimingの文字を描画
                SizeF size = bkBuff.MeasureString( "Triming", filterFont );
                float strX = ( m_pnlTriming.Width - size.Width ) / 2;
                float strY = ( m_pnlTriming.Height - size.Height ) / 2;
                DrawStringWithEdge( bkBuff, "Triming", (int)strX, (int)strY, filterFont, Color.FromArgb( 2, Color.White ), Color.FromArgb( 48, Color.Black ) );

                // トリミングの枠を描画
                if( m_trimingRect.IsEmpty == false )
                {
                    //int x = m_scBarH.Value;
                    //int y = m_scBarV.Value;
                    int x = m_pnlTriming.HorizontalScroll.Value;
                    int y = m_pnlTriming.VerticalScroll.Value;
                    Rectangle trimRect = m_trimingRect;
                    trimRect.Y = trimRect.Y - y;
                    trimRect.X = trimRect.X - x;
                    using( SolidBrush brush = new SolidBrush( Color.FromArgb( 128, Color.Yellow ) ) )
                    using( Pen pen = new Pen( brush, 4 ) )
                    {
                        pen.DashStyle = DashStyle.Dash;
                        bkBuff.DrawRectangle( pen, trimRect );
                    }

                    string textLT = string.Format( "{0},{1}", m_trimingRect.Left, m_trimingRect.Top );
                    string textRB = string.Format( "{0},{1}", m_trimingRect.Right, m_trimingRect.Bottom );

                    DrawStringWithEdge( bkBuff, textLT, trimRect.Left, trimRect.Top, this.Font, Color.White, Color.Black );
                    DrawStringWithEdge( bkBuff, textRB, trimRect.Right, trimRect.Bottom, this.Font, Color.White, Color.Black );
                }
            }
            this.Invoke( (SimpleDelegate)delegate()
            {
                m_pnlTriming.Invalidate();
                m_pnlTriming.Update();
            } );
        }

        private void DrawStringWithEdge(Graphics graph, string text, int x, int y, Font font, Color edgeColor, Color textColor)
        {
            using( SolidBrush txtBkBrush = new SolidBrush( edgeColor ) )
            using( SolidBrush txtFrBrush = new SolidBrush( textColor ) )
            {
                graph.DrawString( text, font, txtBkBrush, new Point( x - 1, y - 1 ) );
                graph.DrawString( text, font, txtBkBrush, new Point( x - 1, y ) );
                graph.DrawString( text, font, txtBkBrush, new Point( x - 1, y + 1 ) );

                graph.DrawString( text, font, txtBkBrush, new Point( x, y - 1 ) );
                graph.DrawString( text, font, txtBkBrush, new Point( x, y - 0 ) );
                graph.DrawString( text, font, txtBkBrush, new Point( x, y + 1 ) );

                graph.DrawString( text, font, txtBkBrush, new Point( x + 1, y - 1 ) );
                graph.DrawString( text, font, txtBkBrush, new Point( x + 1, y ) );
                graph.DrawString( text, font, txtBkBrush, new Point( x + 1, y + 1 ) );

                graph.DrawString( text, font, txtFrBrush, new Point( x, y ) );
            }
        }

        private void m_scBarV_Scroll( object sender, ScrollEventArgs e )
        {
            updateBackBuffer();
        }

        private void m_scBarH_Scroll( object sender, ScrollEventArgs e )
        {
            updateBackBuffer();
        }

        private void m_pnlTriming_Click( object sender, EventArgs e )
        {
            this.Focus();
        }

        private void m_pnlTriming_MouseDown( object sender, MouseEventArgs e )
        {
            this.Focus();
            //Console.WriteLine("MouseDown");
            m_mouseDownX = e.X + m_pnlTriming.HorizontalScroll.Value;
            m_mouseDownY = e.Y + m_pnlTriming.VerticalScroll.Value;

            // スタンプをクリックしたか調べる
            bool inStamp = false;
            if (m_drawStamps != null)
            {
                Point pos = new Point(e.X, e.Y);
                foreach (StampItem stamp in m_drawStamps)
                {
                    if (stamp.HitTest(pos))
                    {
                        break;
                    }
                }
            }
            if (false == inStamp)
            {
                m_updateRectagle(m_mouseDownX + 1, m_mouseDownY + 1);
            }
		}

        private void m_pnlTriming_MouseMove( object sender, MouseEventArgs e )
        {
            //Console.WriteLine("MouseMove");

            if( (e.Button & MouseButtons.Left) != MouseButtons.Left )
            {
/*
                if( task == null )
                {
                    var toHeight = -1;
                    if( e.Y <= 8 )
                    {
                        toHeight = 76;
                    }
                    else
                    {
                        toHeight = 8;
                    }

                    if( m_pnlFunc.Height == toHeight )
                    {
                        return;
                    }

                    task = new Task( delegate()
                    {
                        if( m_pnlFunc.Height < toHeight )
                        {
                            for( var h = m_pnlFunc.Height; h < toHeight; h++ )
                            {
                                this.Invoke( (SimpleDelegate)delegate()
                                {
                                    m_pnlFunc.Height++;
                                } );
//                                System.Threading.Thread.Sleep( 3 );
                            }
                        }
                        else
                        {
                            for( var h = m_pnlFunc.Height; h> toHeight; h-- )
                            {
                                this.Invoke( (SimpleDelegate)delegate()
                                {
                                    m_pnlFunc.Height--;
                                } );
                            }
                        }
                    } );
                    task.ContinueWith( (Action<Task>)delegate( Task obj )
                    {
                        task = null;
                        updateImgInfo();
                    } );

                    task.Start();
                }
*/
                showRGB(e.X, e.Y);
                return;
            }
			//m_trimingRect.Width = e.X - m_trimingRect.Left + x;
			//m_trimingRect.Height = e.Y - m_trimingRect.Top + y;
            
            //Console.WriteLine(String.Format("X {0} / Y {1}", e.X, e.Y));




            // 自動でスクロールさせるかどうかの判断
            if( m_pnlTriming.HorizontalScroll.Enabled == true )
            {
                if( e.X <= 16 && m_pnlTriming.HorizontalScroll.Value > 1 )
                {
                    // 自動スクロール
                    if( m_pnlTriming.HorizontalScroll.Value - m_pnlTriming.HorizontalScroll.SmallChange <= 0 )
                    {
                        m_pnlTriming.HorizontalScroll.Value = 0;
                    }
                    else
                    {
                        int zx = this.Left - e.X;
                        //zx = (int)Math.Round((float)(zx / 100.0f), 0);
                        if (zx < m_pnlTriming.HorizontalScroll.SmallChange)
                        {
                            zx = m_pnlTriming.HorizontalScroll.SmallChange;
                        }
                        //Console.WriteLine(String.Format("zx {0}", zx));
                        //m_pnlTriming.HorizontalScroll.Value -= m_pnlTriming.HorizontalScroll.SmallChange;
                        if (m_pnlTriming.HorizontalScroll.Value - zx < m_pnlTriming.HorizontalScroll.Minimum)
                        {
                            m_pnlTriming.HorizontalScroll.Value = m_pnlTriming.HorizontalScroll.Minimum;
                        }
                        else
                        {
                            m_pnlTriming.HorizontalScroll.Value -= zx;
                        }
                    }
                }
                else if( e.X >= m_pnlTriming.Width - 16 && m_pnlTriming.HorizontalScroll.Value < m_pnlTriming.HorizontalScroll.Maximum )
                {
                    // 自動スクロール
                    if( m_pnlTriming.HorizontalScroll.Value + m_pnlTriming.HorizontalScroll.SmallChange >= m_pnlTriming.HorizontalScroll.Maximum )
                    {
                        m_pnlTriming.HorizontalScroll.Value = m_pnlTriming.HorizontalScroll.Maximum;
                    }
                    else
                    {
                        int zx = e.X - (this.Left + this.Width);
                        //zx = (int)Math.Round((float)(zx / 100.0f), 0);
                        if (zx < m_pnlTriming.HorizontalScroll.SmallChange)
                        {
                            zx = m_pnlTriming.HorizontalScroll.SmallChange;
                        }
                        //Console.WriteLine(String.Format("zx {0}", zx));
                        //m_pnlTriming.HorizontalScroll.Value += m_pnlTriming.HorizontalScroll.SmallChange;
                        if (m_pnlTriming.HorizontalScroll.Value + zx > m_pnlTriming.HorizontalScroll.Maximum)
                        {
                            m_pnlTriming.HorizontalScroll.Value = m_pnlTriming.HorizontalScroll.Maximum;
                        }
                        else
                        {
                            m_pnlTriming.HorizontalScroll.Value += zx;
                        }
                    }
                }
            }

            if( m_pnlTriming.VerticalScroll.Enabled == true )
            {
                if( e.Y <= 16 && m_pnlTriming.VerticalScroll.Value > 1 )
                {
                    // 自動スクロール
                    if( m_pnlTriming.VerticalScroll.Value - m_pnlTriming.VerticalScroll.SmallChange <= 0 )
                    {
                        m_pnlTriming.VerticalScroll.Value = 0;
                    }
                    else
                    {
                        int zy = this.Top - e.Y;
                        //zy = (int)Math.Round((float)(zy / 100.0f), 0);
                        if (zy < m_pnlTriming.VerticalScroll.SmallChange)
                        {
                            zy = m_pnlTriming.VerticalScroll.SmallChange;
                        }
                        //Console.WriteLine(String.Format("zy {0}", zy));

                        // マウスの位置でスクロール量を変えたい
                        //m_pnlTriming.VerticalScroll.Value -= m_pnlTriming.VerticalScroll.SmallChange;
                        if (m_pnlTriming.VerticalScroll.Value - zy < m_pnlTriming.VerticalScroll.Minimum)
                        {
                            m_pnlTriming.VerticalScroll.Value = m_pnlTriming.VerticalScroll.Minimum;
                        }
                        else
                        {
                            m_pnlTriming.VerticalScroll.Value -= zy;
                        }
                    }
                }
                else if( e.Y >= m_pnlTriming.Height - 16 && m_pnlTriming.VerticalScroll.Value < m_pnlTriming.VerticalScroll.Maximum )
                {
                    // 自動スクロール
                    if( m_pnlTriming.VerticalScroll.Value + m_pnlTriming.VerticalScroll.SmallChange >= m_pnlTriming.VerticalScroll.Maximum )
                    {
                        m_pnlTriming.VerticalScroll.Value = m_pnlTriming.VerticalScroll.Maximum;
                    }
                    else
                    {
                        int zy = e.Y - (this.Top + this.Height);
                        //zy = (int)Math.Round((float)(zy / 100.0f), 0);
                        if (zy < m_pnlTriming.HorizontalScroll.SmallChange)
                        {
                            zy = m_pnlTriming.HorizontalScroll.SmallChange;
                        }
                        //m_pnlTriming.VerticalScroll.Value += m_pnlTriming.VerticalScroll.SmallChange;
                        if (m_pnlTriming.VerticalScroll.Value + zy > m_pnlTriming.VerticalScroll.Maximum)
                        {
                            m_pnlTriming.VerticalScroll.Value = m_pnlTriming.VerticalScroll.Maximum;
                        }else{
                            m_pnlTriming.VerticalScroll.Value += zy;
                        }
                    }
                }
            }
            
			m_updateRectagle(e.X, e.Y);

            showRGB(e.X, e.Y);
		}

        /// <summary>
        /// RGB表示
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void showRGB(int x, int y){
               Color ckColor = Color.Black;
            int mouseStopX = x + m_pnlTriming.HorizontalScroll.Value;
            int mouseStopY = y + m_pnlTriming.VerticalScroll.Value;
            if (m_bmp != null && 0 <= mouseStopX && mouseStopX < m_bmp.Width && 0 <= mouseStopY && mouseStopY < m_bmp.Height)
            {
                Color cl1 = m_bmp.GetPixel(mouseStopX, mouseStopY);
                Color cl2 = this.GetPixel(mouseStopX, mouseStopY);

                //Console.WriteLine("[C]{0},{1},{2} : [N]{3},{4},{5}", 
                //    cl1.R, cl1.G, cl1.B, 
                //    cl2.R, cl2.G, cl2.B);

                if (cl1.R != cl2.R || cl1.G != cl2.G || cl1.B != cl2.B)
                {
                    // ないとは思うけど、GetPixelの戻り値が違う場合にこっそり気付けるように
                    m_stRGB.Text = String.Format("RGB! {0,3},{1,3},{2,3}({0:X2}{1:X2}{2:X2})", cl2.R, cl2.G, cl2.B);
                }
                else
                {
                    m_stRGB.Text = String.Format("RGB  {0,3},{1,3},{2,3}({0:X2}{1:X2}{2:X2})", cl2.R, cl2.G, cl2.B);
                }
                ckColor = cl1;
            }
            else
            {
                m_stRGB.Text = "RGB  ---,---,---(------)";
            }
            m_stColor.ForeColor = ckColor;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void m_pnlTriming_MouseUp( object sender, MouseEventArgs e )
        {
            //Console.WriteLine("MouseUp");
            //int x = m_scBarH.Value;
			//int y = m_scBarV.Value;
			//m_trimingRect.Width = e.X - m_trimingRect.Left + x;
			//m_trimingRect.Height = e.Y - m_trimingRect.Top + y;
			m_updateRectagle(e.X, e.Y);
//            trimImg( true );
            trimImg( false );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void m_pnlTriming_Scroll( object sender, ScrollEventArgs e )
        {
            updateBackBuffer();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void m_pnlTriming_MouseHover( object sender, EventArgs e )
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click( object sender, EventArgs e )
        {
            trimImg(false);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="update"></param>
		private void m_updateRectagle(int x, int y, bool update = true)
		{
            int mouseStopX = x + m_pnlTriming.HorizontalScroll.Value;
            int mouseStopY = y + m_pnlTriming.VerticalScroll.Value;
            int topLeftX = m_mouseDownX;
			int topLeftY = m_mouseDownY;
			int rightBottomX = mouseStopX;
			int rightBottomY = mouseStopY;
			if (mouseStopX < m_mouseDownX)
			{
				topLeftX = mouseStopX;
				rightBottomX = m_mouseDownX;
			}
			if (mouseStopY < m_mouseDownY)
			{
				topLeftY = mouseStopY;
				rightBottomY = m_mouseDownY;
			}
			if (topLeftX < 0)
			{
				topLeftX = 0;
			}
			if (topLeftY < 0)
			{
				topLeftY = 0;
			}
			if (m_bmp != null)
			{
				if (topLeftX > m_bmp.Width)
				{
					topLeftX = m_bmp.Width;
				}
				if (topLeftY > m_bmp.Height)
				{
					topLeftY = m_bmp.Height;
				}
				if (rightBottomX > m_bmp.Width)
				{
					rightBottomX = m_bmp.Width;
				}
				if (rightBottomY > m_bmp.Height)
				{
					rightBottomY = m_bmp.Height;
				}
			}
			m_trimingRect = new Rectangle(topLeftX, topLeftY, rightBottomX - topLeftX, rightBottomY - topLeftY);

            if (update)
            {
                updateBackBuffer();
            }
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="withAnime"></param>
		private void trimImg(bool withAnime)
        {
            if( m_bmp == null )
            {
                return;
            }

            if( m_trimingRect.Left < 0 )
            {
                m_trimingRect.X = 0;
            }
            if( m_trimingRect.Top < 0 )
            {
                m_trimingRect.Y = 0;
            }
            if( m_trimingRect.Right >= m_bmp.Width )
            {
                m_trimingRect.Width = m_trimingRect.Width - ( m_trimingRect.Right - m_bmp.Width ) - 1;
            }
            if( m_trimingRect.Bottom >= m_bmp.Height )
            {
                m_trimingRect.Height = m_trimingRect.Height - ( m_trimingRect.Bottom - m_bmp.Height ) - 1;
            }

            Rectangle startRect = m_trimingRect;

//            getBitmapBuff(m_backBuffer);
//            getBitmapBuff(m_bmp);

            switch( m_cmbMode.SelectedIndex )
            {
                case 0:
                    // トリミングの自動計算

                    // +---A---+ 
                    // |       |
                    // B       D
                    // |       |
                    // +---C---+

                    Rectangle trimRect = new Rectangle();
                    int threshold = 0;
                    if( int.TryParse( m_txtOffset.Text, out threshold ) == true )
                    {
                        try
                        {
                            // Aのチェック
                            trimRect.Y = getY( true );
                            // Bのチェック
                            trimRect.X = getX( true );
                            // Cのチェック
                            trimRect.Height = getY( false ) - trimRect.Y;
                            // Dのチェック
                            trimRect.Width = getX( false ) - trimRect.X;

                            m_trimingRect = trimRect;
                        }
                        catch( Exception ex )
                        {
                            m_trimingRect = new Rectangle();
                        }
                    }
                    else
                    {
                        m_trimingRect = new Rectangle();
                    }
                    break;

                case 1:
                    // 範囲をそのまま利用する
                    break;
            }

            if( withAnime == true )
            {
                Rectangle endRect = m_trimingRect;

                m_trimingRect = startRect;
                var task = new Task( (Action)delegate()
                {
                    var tval = endRect.Top - startRect.Top;
                    var lval = endRect.Left - startRect.Left;
                    var bval = endRect.Bottom - startRect.Bottom;
                    var rval = endRect.Right - startRect.Right;

                    var tstep = (double)tval / 10.0;
                    var lstep = (double)lval / 10.0;
                    var bstep = (double)bval / 10.0;
                    var rstep = (double)rval / 10.0;

                    var top = (double)startRect.Top;
                    var bottom = (double)startRect.Bottom;
                    var left = (double)startRect.Left;
                    var right = (double)startRect.Right;

                    for( int i = 0; i <= 10; i++ )
                    {
                        top += tstep;
                        bottom += bstep;
                        left += lstep;
                        right += rstep;
                        m_trimingRect = new Rectangle((int)left, (int)top, (int)(right - left), (int)(bottom - top));
                        updateBackBuffer();
                        //Thread.Sleep( 1 );
                    }
                    m_trimingRect = endRect;
                    updateBackBuffer();

                } );
                task.Start();
            }
            else
            {
                updateBackBuffer();
            }
        }

        /// <summary>
        /// 以下のサイトを参考に高速化
        /// https://qiita.com/Nuits/items/4a2fbc0f4e8583bd5531
        /// </summary>
        /// <param name="src"></param>
        private void getBitmapBuff(Bitmap src)
        {
            using(Bitmap tmp = new Bitmap(src.Width, src.Height))
            using(Graphics g = Graphics.FromImage(tmp))
            {
                g.DrawImage(src, 0, 0);
                // Bitmapをロックし、BitmapDataを取得する
                Rectangle srcRect = new Rectangle(0, 0, tmp.Width, tmp.Height);
                BitmapData srcBitmapData = tmp.LockBits(srcRect, ImageLockMode.ReadOnly, src.PixelFormat);

                // 変換対象のカラー画像の情報をバイト列へ書き出す
                byte[] srcPixels = new byte[srcBitmapData.Stride * src.Height];
                bitmapStride = srcBitmapData.Stride;
                System.Runtime.InteropServices.Marshal.Copy(srcBitmapData.Scan0, srcPixels, 0, srcPixels.Length);
                bitmapBuff = srcPixels;
                tmp.UnlockBits(srcBitmapData);
            }

        }

        /// <summary>
        /// 以下のサイトを参考に高速化
        /// https://qiita.com/Nuits/items/4a2fbc0f4e8583bd5531
        /// Xの計算で、上記サイトでは3をかけているが、色の情報は、4byteなので4をかけるようにしている
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private Color GetPixel(int x, int y)
        {
            if (bitmapBuff == null)
            {
                return Color.Black;
            }

            int arY = bitmapStride * y;
            int arX = 4 * x;
//            int arX = 3 * x;
            byte b = bitmapBuff[arX + arY + 0];
            byte g = bitmapBuff[arX + arY + 1];
            byte r = bitmapBuff[arX + arY + 2];
            return Color.FromArgb(r, g, b);
        }

        private int getY(bool isTop)
        {
            int sy = m_trimingRect.Top;
            int sx = m_trimingRect.Left;
            int ex = m_trimingRect.Right;
            int ey = m_trimingRect.Bottom;

            if( sy < 0 )
            {
                sy = 0;
            }
            if( sx < 0 )
            {
                sx = 0;
            }
            if( ex >= m_bmp.Width )
            {
                ex = m_bmp.Width - 1;
            }
            if( ey >= m_bmp.Height )
            {
                ey = m_bmp.Height - 1;
            }


            int offset = m_trimingRect.Height / 2;
            int loopOffset = 0;
            int threshold;
            if( int.TryParse( m_txtOffset.Text, out threshold ) == false )
            {
                return 0;
            }
            if( isTop == true )
            {
                sy = m_trimingRect.Top;
                ey = m_trimingRect.Top + offset;
                loopOffset = 1;
            }
            else
            {
                sy = m_trimingRect.Bottom;
                ey = m_trimingRect.Bottom - offset;
                loopOffset = -1;
            }

            int[] chkCnt = new int[Math.Abs(ey - sy)];
            for( int idx = 0; idx < offset; idx++ )
            {
                chkCnt[idx] = 0;
            }

            // AのY座標を決定する
            for( int x = sx; x < ex; x++ )
            {
                int preGs = -1;
                for( int y = sy; y != ey; y += loopOffset )
                {
                    Color color;
                    if (m_cbUseClasssic.Checked == true)
                    {
                        color = m_bmp.GetPixel(x, y);
                    }
                    else
                    {
                        color = this.GetPixel(x, y);
                    }

                    int r = color.R;
                    int g = color.G;
                    int b = color.B;
                    int max = Max( new int[] { r, g, b } );
                    int min = Min( new int[] { r, g, b } );
                    int gs = ( max + min ) / 2;
                    if( preGs == -1 )
                    {
                    }
                    else
                    {
                        if( Math.Abs( preGs - gs ) > threshold )
                        {
                            chkCnt[Math.Abs(y - sy)] += 1;
                        }
                    }
                    preGs = gs;
                }
            }
            int maxIdx = -1;
            int si = 0;
            int ei = 0;
            if( isTop )
            {
                si = 0;
                ei = offset;
            }
            else
            {
                si = offset - 1;
                ei = 0;
            }
            for( int i = si; i != ei; i += loopOffset )
            {
                if( maxIdx == -1 )
                {
                    maxIdx = i;
                }
                else
                {
                    if( chkCnt[i] > chkCnt[maxIdx] )
                    {
                        maxIdx = i;
                    }
                }
            }
            if( isTop == true )
            {
                return maxIdx + sy;
            }
            else
            {
                return sy - maxIdx;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isLeft"></param>
        /// <returns></returns>
        private int getX(bool isLeft)
        {
            int sy = m_trimingRect.Top;
            int sx = m_trimingRect.Left;
            int ex = m_trimingRect.Right;
            int ey = m_trimingRect.Bottom;
            int offset = m_trimingRect.Width / 2;

            int threshold;
            if( int.TryParse( m_txtOffset.Text, out threshold ) == false )
            {
                return 0;
            }
            int loopOffset = 0;
            if( isLeft == true )
            {
                sx = m_trimingRect.Left;
                ex = m_trimingRect.Left + offset;
                loopOffset = 1;
            }
            else
            {
                sx = m_trimingRect.Right;
                ex = m_trimingRect.Right - offset;
                loopOffset = -1;
            }

            int[] chkCnt = new int[offset];
            for( int idx = 0; idx < offset; idx++ )
            {
                chkCnt[idx] = 0;
            }

            for( int y = sy; y < ey; y++ )
            {
                int preGs = -1;
                for( int x = sx; x != ex; x += loopOffset )
                {
                    //System.Console.WriteLine( string.Format( "{0} / {1}", x, Math.Abs( x - sx ) ) );
                    Color color;
                    if (m_cbUseClasssic.Checked == true)
                    {
                        color = m_bmp.GetPixel(x, y);
                    }
                    else
                    {
                        color = this.GetPixel(x, y);
                    }

                    int r = color.R;
                    int g = color.G;
                    int b = color.B;
                    int max = Max( new int[] { r, g, b } );
                    int min = Min( new int[] { r, g, b } );
                    int gs = ( max + min ) / 2;
                    if( preGs == -1 )
                    {
                    }
                    else
                    {
                        if( Math.Abs( preGs - gs ) > threshold )
                        {
                            chkCnt[Math.Abs(x - sx)] += 1;
                        }
                    }
                    preGs = gs;
                }
            }
            int maxIdx = -1;
            int si = 0;
            int ei = 0;
            if( isLeft )
            {
                si = 0;
                ei = offset;
            }
            else
            {
                si = offset - 1;
                ei = 0;
//                si = 0;
//                ei = offset;
            }
            for( int i = si; i != ei; i += loopOffset )
            {
                if( maxIdx == -1 )
                {
                    maxIdx = i;
                }
                else
                {
                    if( chkCnt[i] > chkCnt[maxIdx] )
                    {
                        maxIdx = i;
                    }
                }
            }
            if( isLeft == true )
            {
                return maxIdx + sx;
            }
            else
            {
                return sx - maxIdx;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        private static int Max( int[] val )
        {
            Array.Sort<int>( val );
            return val[val.Length - 1];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        private static int Min( int[] val )
        {
            Array.Sort<int>( val );
            return val[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void m_btnStamp_Click( object sender, EventArgs e )
        {
            // スタンプ表示表
            //MessageBox.Show( this, "...coming soon...", "Information", MessageBoxButtons.OK );
            //ドロップダウンで表示
            m_stanpView.Left = m_btnStamp.Left;
            m_stanpView.Top = m_btnStamp.Top + m_btnStamp.Height;
            m_stanpView.Width = m_btnStamp.Width * 10;
            m_stanpView.Height = m_btnStamp.Height * 10;
            m_stanpView.Show();
            m_stanpView.BringToFront();
            m_stanpView.Focus();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void m_btnSave_Click( object sender, EventArgs e )
        {
            try
            {
                if( m_trimingRect == new Rectangle( 0, 0, 0, 0 ) )
                {
                    MessageBox.Show( this, "範囲を指定してください", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error );
                    return;
                }
                var result = m_dlgSaveFile.ShowDialog();
                if( result != DialogResult.OK )
                {
                    return;
                }

                var saveFileName = m_dlgSaveFile.FileName;
                using( Bitmap trimBmp = new Bitmap( m_trimingRect.Width, m_trimingRect.Height ) )
                using( Graphics graph = Graphics.FromImage( trimBmp ) )
                {
                    graph.DrawImage( m_bmp, 0, 0, m_trimingRect, GraphicsUnit.Pixel );
                    if( m_cbLeftTopTransparent.Checked == true )
                    {
                        // 左上の色で透過
                        Color color;
                        if (m_cbUseClasssic.Checked == true)
                        {
                            color = m_bmp.GetPixel(0, 0);
                        }
                        else
                        {
                            color = this.GetPixel(0, 0);
                        }
                        trimBmp.MakeTransparent(color);

                    }
                    // 保存
                    trimBmp.Save( saveFileName );
                    MessageBox.Show( this, "保存しました", "情報", MessageBoxButtons.OK, MessageBoxIcon.Information );
                }
            }
            catch( Exception ex )
            {
                MessageBox.Show( this, ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error );
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void m_btnPasteProcess_Click( object sender, EventArgs e )
        {
            var frm = new ProcessForm();
            var result = frm.ShowDialog( this );
            if( result != DialogResult.OK )
            {
                return;
            }
            if( m_chkClearSelect.Checked == true )
            {
                m_trimingRect = new Rectangle( 0, 0, 0, 0 );
            }
            if( m_bmp != null )
            {
                m_bmp.Dispose();
            }
            m_bmp = frm.previewImage.Clone() as Bitmap;
            m_pnlTriming.AutoScrollMinSize = new Size( m_bmp.Width, m_bmp.Height );
            m_pnlTriming.HorizontalScroll.Value = 0;
            m_pnlTriming.VerticalScroll.Value = 0;

            updateImgInfo();

            if (m_cbAutoSend.Checked == true)
            {
                // 範囲指定があるかどうかは先の処理でチェックしてた
                m_btnTrim_Click(sender, e);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void m_btnStartTimer_Click( object sender, EventArgs e )
        {
            if( m_cmbTimer.SelectedItem == null )
            {
                MessageBox.Show( this, "時間を設定してください", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error );
                return;
            }
            int time;
            if( int.TryParse( m_cmbTimer.SelectedItem.ToString(), out time ) == false )
            {
                return;
            }
            m_grpFrom.Enabled = false;
            m_grpMode.Enabled = false;
            m_btnStamp.Enabled = false;
            m_btnTrim.Enabled = false;
            m_btnSave.Enabled = false;
            m_cmbTimer.Enabled = false;
            m_btnStartTimer.Enabled = false;
            m_btnStopTimer.Enabled = true;
            var timerTask = new Task( (Action)delegate()
            {
                int cnt = 0;
                while( true )
                {
                    ct.ThrowIfCancellationRequested();   
                    Thread.Sleep( 1000 );
                    cnt++;
                    if( cnt == time )
                    {
                        break;
                    }
                }
            } );

            timerTask.ContinueWith( (Action<Task>)delegate( Task task )
            {
                
                this.Invoke( (SimpleDelegate)delegate()
                {
                    if( task.IsCompleted == true && task.IsFaulted == false && task.IsCanceled == false )
                    {
                        int scWidth = 0;
                        int scHeight = 0;
                        foreach( Screen screen in Screen.AllScreens )
                        {
                            scWidth += screen.Bounds.Width;
                            if( scHeight < screen.Bounds.Height )
                            {
                                scHeight = screen.Bounds.Height;
                            }
                        }
                        m_bmp = new Bitmap( scWidth, scHeight );
                        if( m_bmp != null )
                        {
                            Graphics g = Graphics.FromImage( m_bmp );
                            //画面全体をコピーする
                            g.CopyFromScreen( new Point( 0, 0 ), new Point( 0, 0 ), m_bmp.Size );
                            //解放
                            g.Dispose();

                            if( m_chkClearSelect.Checked == true )
                            {
                                m_trimingRect = new Rectangle( 0, 0, 0, 0 );
                            }

                            m_pnlTriming.AutoScrollMinSize = new Size( m_bmp.Width, m_bmp.Height );
                            m_pnlTriming.HorizontalScroll.Value = 0;
                            m_pnlTriming.VerticalScroll.Value = 0;
                            updateImgInfo();
                        }
                    }

                    m_grpFrom.Enabled = true;
                    m_grpMode.Enabled = true;
                    m_btnStamp.Enabled = true;
                    m_btnTrim.Enabled = true;
                    m_btnSave.Enabled = true;
                    m_cmbTimer.Enabled = true;
                    m_btnStartTimer.Enabled = true;
                    m_btnStopTimer.Enabled = false;
                    ctsrc.Dispose();
                    ctsrc = null;
                } );
            } );
            
            
            ctsrc = new CancellationTokenSource();
            ct = ctsrc.Token;
            timerTask.Start();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void m_btnStopTimer_Click( object sender, EventArgs e )
        {
            if( ctsrc == null )
            {
                return;
            }
            ctsrc.Cancel();
        }

        /// <summary>
        /// s
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void m_btnAbout_Click(object sender, EventArgs e)
        {
            // バージョン情報表示
            About dlg = new About();
            dlg.ShowDialog(this);
        }
    }
}

