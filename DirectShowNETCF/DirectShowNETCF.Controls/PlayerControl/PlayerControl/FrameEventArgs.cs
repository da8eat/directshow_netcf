using System;
using System.Drawing;
using System.Collections.Generic;
using System.Text;

namespace PlayerControl
{
    public class FrameEventArgs : EventArgs
    {
        public FrameEventArgs(Bitmap frame)
        {
            _frame = frame;
        }

        public Bitmap Frame
        {
            get
            {
                return _frame;
            }
        }

        private Bitmap _frame;
    }
}
