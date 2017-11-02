using System;
using System.Collections.Generic;
using System.Text;

namespace NullPlayer
{
    public class FrameEventArgs : EventArgs
    {
        public FrameEventArgs(IntPtr frame)
        {
            _frame = frame;
        }

        public IntPtr Frame
        {
            get
            {
                return _frame;
            }
        }

        private IntPtr _frame;
    }
}
