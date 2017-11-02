using System;
using System.Collections.Generic;
using System.Text;

namespace DirectShowNETCF
{
    public class FrameEventArgs : EventArgs
    {
        public FrameEventArgs(IntPtr frame, int width, int height)
        {
            _frame = frame;
            height_ = height;
            width_ = width;
        }

        public IntPtr Frame
        {
            get
            {
                return _frame;
            }
        }

        public int Height
        {
            get
            {
                return height_;
            }
        }

        public int Width
        {
            get
            {
                return width_;
            }
        }

        private IntPtr _frame;
        private int width_;
        private int height_;
    }
}
