using System;
using System.Collections.Generic;
using System.Text;

namespace PlayerControl
{
    public class ProgressEventArgs : EventArgs
    {
        public ProgressEventArgs(TimeSpan progress)
        {
            _progress = progress;
        }

        public TimeSpan Progress
        {
            get
            {
                return _progress;
            }
        }

        private TimeSpan _progress;
    }
}
