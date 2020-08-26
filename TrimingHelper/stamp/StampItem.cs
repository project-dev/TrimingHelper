using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;


namespace TrimmingHelper.stamp
{
    public abstract class StampItem
    {

        /// <summary>
        /// テキストの入力の可否
        /// </summary>
        public bool IsTextInput
        {
            get;
            set;
        }

        public Point Position
        {
            get;
            set;
        }

        public Size Size
        {
            get;
            set;
        }

        /// <summary>
        /// 指定された位置がこのオブジェクト内にいるかどうかを判定します。
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public bool HitTest(Point position)
        {
            int mySX = this.Position.X;
            int mySY = this.Position.Y;
            int myEX = this.Position.X + this.Size.Width;
            int myEY = this.Position.Y + this.Size.Height;
            int hitX = position.X;
            int hitY = position.Y;

            if(mySX >= hitX && hitX <= myEX
            && mySY >= hitY && hitY <= myEY)
            {
                return true;
            }
            return false;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="g"></param>
        public abstract void onPaint(Graphics g);
    }
}
