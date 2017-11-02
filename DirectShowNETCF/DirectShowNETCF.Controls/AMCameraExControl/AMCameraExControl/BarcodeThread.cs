using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace AMCameraExControl
{
    public class BarcodeThread
    {
        private int width_;
        private int height_;
        private Thread thread_;
        private object locker_;

        public event EventHandler<BarcodeEventArgs> Decoded;

        private BarcodeThread()
        {
            width_ = 0;
            height_ = 0;
            locker_ = new object();
        }

        private static BarcodeThread instance_;

        public static BarcodeThread Instance
        {
            get
            {
                if (instance_ == null)
                {
                    instance_ = new BarcodeThread();
                }

                return instance_;
            }
        }

        public void Initialize(int width, int height)
        {
            width_ = width;
            height_ = height;
        }

        public void Start()
        {
            thread_ = new Thread(ThreadProc);
            thread_.IsBackground = true;
            thread_.Priority = ThreadPriority.BelowNormal;
            thread_.Start();
        }

        public void Stop()
        {
            try
            {
                thread_.Join(5000);
            }
            catch (ArgumentOutOfRangeException)
            {
            }
            catch (ThreadStateException)
            {
            }

            thread_ = null;
        }

        public bool TryCheck(IntPtr ptr)
        {
            bool isEntered = Monitor.TryEnter(locker_);

            if (isEntered)
            {
                Monitor.Exit(locker_);
                return true;
            }

            return false;
        }

        public void Check(IntPtr ptr)
        {
            Monitor.Enter(locker_);



            Monitor.Exit(locker_);
        }

        private void ThreadProc()
        {
            Monitor.Enter(locker_);

            //do decode

            Monitor.Exit(locker_);
        }
    }

    public class BarcodeEventArgs : EventArgs
    {
        private string code_;

        public BarcodeEventArgs(string code)
        {
            code_ = code;
        }

        public string Code
        {
            get
            {
                return code_;
            }
        }

        public bool IsRecognized
        {
            get
            {
                return code_ != "None";
            }
        }
    }
}
