using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Threading.Tasks;

namespace TrimmingHelper
{
    /// <summary>
    /// プロセス一覧からキャプチャする
    /// </summary>
    public partial class ProcessForm : Form
    {

        public Bitmap previewImage = null;
        private Task processTask = null;
        private CancellationTokenSource processTaskCancelToken = null;
        private bool isRefreshCancel = false;

        public ProcessForm()
        {
            InitializeComponent();
        }

        private void ProcessForm_Load( object sender, EventArgs e )
        {
            RefreshTree();
        }

        private void RefreshTree()
        {
            m_btnRefresh.Enabled = false;
            m_btnOK.Enabled = false;
            m_btnCancel.Enabled = false;
            m_treeProcess.Nodes.Clear();
            processTaskCancelToken = new CancellationTokenSource();

            processTask = new Task( (Action)delegate()
            {
                processTaskCancelToken.Token.ThrowIfCancellationRequested();

                var processes = Process.GetProcesses();
                foreach( var process in processes )
                {
                    var wnd = new WndNode();
                    wnd.PID = process.Id;
                    wnd.hWnd = process.MainWindowHandle;
                    wnd.Text = process.ProcessName;
                    this.Invoke( (SimpleDelegate)delegate()
                    {
                        m_stLabelState.Text = process.ProcessName;
                    } );
                    var result = Win32.EnumWindows( (Win32.EnumWindowsProc)delegate( IntPtr hwnd, IntPtr lParam )
                    {
                        RECT rect = new RECT();
                        Win32.GetWindowRect( hwnd, ref rect );
                        if( ( rect.right - rect.left ) <= 0 || ( rect.bottom - rect.top ) <= 0 )
                        {
                            return true;
                        }

                        if( Win32.IsWindowVisible( hwnd ) == false )
                        {
                            return true;
                        }

                        if( Win32.IsIconic( hwnd ) == true )
                        {
                            return true;
                        }

                        uint pid = 0;
                        Win32.GetWindowThreadProcessId( hwnd, out pid );
                        if( pid == (uint)lParam )
                        {
                            wnd.hWnd = hwnd;
                            return false;
                        }
                        return true;
                    }, new IntPtr( wnd.PID ) );

                    if( wnd.hWnd != IntPtr.Zero && Win32.IsWindowVisible(wnd.hWnd) == true )
                    {
                        // 子ウィンドウも対象する
                        ChildWindow( wnd );
                        this.Invoke( (SimpleDelegate)delegate()
                        {
                            m_treeProcess.Nodes.Add( wnd );
                        } );
                    }

                }
            }, processTaskCancelToken.Token );
            processTask.ContinueWith( (Action<Task>)delegate( Task t )
            {
                this.Invoke( (SimpleDelegate)delegate()
                {
                    m_btnRefresh.Enabled = true;
                    m_btnOK.Enabled = true;
                    m_btnCancel.Enabled = true;
                    if( t.IsCanceled == true )
                    {
                        m_stLabelState.Text = "キャンセルされました";
                    }
                    if( t.IsFaulted == true )
                    {
                        m_stLabelState.Text = "異常終了";
                    }
                    else
                    {
                        m_stLabelState.Text = "完了";
                    }
                } );
            } );
            processTask.Start();
        }

        private void ChildWindow( WndNode parent )
        {
            Win32.EnumChildWindows( parent.hWnd, (Win32.EnumWindowsProc)delegate( IntPtr hwnd, IntPtr lParam )
                            {
                                processTaskCancelToken.Token.ThrowIfCancellationRequested();

                                RECT rect = new RECT();
                                Win32.GetWindowRect( hwnd, ref rect );
                                if( (rect.right - rect.left) <= 0 || (rect.bottom - rect.top) <= 0 )
                                {
                                    return true;
                                }
                                if( Win32.IsWindowVisible( hwnd ) == false )
                                {
                                    return true;
                                }
                                if( Win32.IsIconic( hwnd ) == true )
                                {
                                    return false;
                                }

                                var text = new StringBuilder( 1024 );
                                var childWnd = new WndNode();
                                childWnd.PID = 0;
                                childWnd.hWnd = hwnd;
                                if( Win32.GetWindowText( hwnd, text, 1024 ) != 0 )
                                {
                                    childWnd.Text = text.ToString();
                                }
                                else if( Win32.GetClassName( hwnd, text, 1024 ) != 0 )
                                {
                                    childWnd.Text = text.ToString();
                                }
                                else
                                {
                                    childWnd.Text = "<unknow>";
                                }
                                if( Win32.IsWindowVisible( hwnd ) == true )
                                {
                                    this.Invoke( (SimpleDelegate)delegate()
                                    {
                                        parent.Nodes.Add( childWnd );
                                    } );
                                    ChildWindow( childWnd );
                                }
                                this.Invoke( (SimpleDelegate)delegate()
                                {
                                    m_stLabelState.Text = text.ToString();
                                } );

                                return true;
                            }, IntPtr.Zero );
        }


        bool EnumWndProc( IntPtr hwnd, IntPtr lParam )
        {
            return true;
        }

        private void m_btnRefresh_Click( object sender, EventArgs e )
        {
            isRefreshCancel = false;
            RefreshTree();
        }

        private void m_treeProcess_AfterSelect( object sender, TreeViewEventArgs e )
        {
            var node = e.Node as WndNode;

            UpdateImage(node);
        }

        private void UpdateImage(WndNode node)
        {
            if( node == null )
            {
                return;
            }
            if( previewImage != null )
            {
                previewImage.Dispose();
                previewImage = null;
            }
            previewImage = node.GetImage();
            m_pnlPreview.Refresh();
        }

        private void m_pnlPreview_Paint( object sender, PaintEventArgs e )
        {
            if( previewImage == null )
            {
                return;
            }
            e.Graphics.DrawImage( previewImage, 0, 0 );
        }

        private void m_btnOK_Click( object sender, EventArgs e )
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void m_btnCancel_Click( object sender, EventArgs e )
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void ProcessForm_Resize( object sender, EventArgs e )
        {
            var node = m_treeProcess.SelectedNode as WndNode;
            UpdateImage(node);

        }

        private void ProcessForm_FormClosing( object sender, FormClosingEventArgs e )
        {
            switch( processTask.Status )
            {
                case TaskStatus.Running:
                    processTaskCancelToken.Cancel();
                    break;
            }
        }

        private void m_btnStop_Click( object sender, EventArgs e )
        {
            switch( processTask.Status )
            {
                case TaskStatus.Running:
                    processTaskCancelToken.Cancel();
                    break;
            }
        }

    }

    class WndNode : TreeNode
    {

        public int PID
        {
            get;
            set;
        }
        public IntPtr hWnd
        {
            get;
            set;
        }

        public Bitmap GetImage()
        {
            IntPtr hSrcDc = IntPtr.Zero;
            IntPtr hDc = IntPtr.Zero;
            Graphics destGraphics = null;
            Graphics srcGraphics = null;
            Bitmap bmp = null;
            try
            {
                RECT rect = new RECT();
                RECT clrect = new RECT();
#if false
                hSrcDc = Win32.GetWindowDC( this.hWnd );
                srcGraphics = Graphics.FromHdc( hSrcDc );
                
                Win32.GetWindowRect( hWnd, ref rect );
                Win32.GetClientRect( hWnd, ref clrect );
                POINT point = new POINT();
                point.x = clrect.left;
                point.y = clrect.top;
                Win32.ClientToScreen( hWnd, ref point );
                int yOffset = point.y - rect.top;
                int xOffset = point.x - rect.left;

                var width = clrect.right - clrect.left;
                var height = clrect.bottom - clrect.top;
                bmp = new Bitmap( width, height );
                destGraphics = Graphics.FromImage( bmp );
                
                hDc = destGraphics.GetHdc();
                Win32.BitBlt( hDc, 0, 0, width, height, hSrcDc, xOffset, yOffset, Win32.TernaryRasterOperations.SRCCOPY );
#else
                Win32.GetWindowRect( hWnd, ref rect );
                var width = rect.right - rect.left;
                var height = rect.bottom - rect.top;
                bmp = new Bitmap( width, height );
                destGraphics = Graphics.FromImage( bmp );
                hDc = destGraphics.GetHdc();
                Win32.PrintWindow( this.hWnd, hDc, 0 );
#endif
            }
            catch( Exception ex )
            {
                if( bmp != null )
                {
                    bmp.Dispose();
                    bmp = null;
                }
            }
            finally
            {
                if( hDc != IntPtr.Zero )
                {
                    destGraphics.ReleaseHdc();
                }
                if( hSrcDc != IntPtr.Zero )
                {
                    Win32.DeleteDC( hSrcDc );
                }
                if( srcGraphics != null )
                {
                    srcGraphics.Dispose();
                    srcGraphics = null;
                }
            }
            return bmp;
        }
    }
}
