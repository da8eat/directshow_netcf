/*
 * In Progress
 * */
using System;
using System.Collections.Generic;
using System.Text;

namespace DirectShowNETCF
{
    /*
    public enum FLVFrameType
    {
        Video,
        Audio
    }

    public interface FLVStream
    {
        bool isFlv();
        bool isAudioExist();
        bool isVideoExist();
        bool isEof();
        string[] GetDescription();
        AMMediaType getVideoType();
        AMMediaType getAudioType();
        long getDuration();
        IntPtr getNextFrame(out FLVFrameType type);
        bool setPosition(long position);
    }

    public class FLVPlayer
    {
        private FLVStream stream_;
        private IBaseFilter videoSource = null;
        private IBaseFilter audioSource = null;

        public FLVPlayer(FLVStream stream)
        {
            stream_ = stream;
        }

        public bool init()
        {
            if (!stream_.isFlv())
            {
                return false;
            }

            if (!stream_.isVideoExist() && !stream_.isAudioExist())
            {
                return false;
            }

            if (stream_.isVideoExist())
            {
                AMMediaType mt = stream_.getVideoType();
                //set video type tu push source
            }

            if (stream_.isAudioExist())
            {
                AMMediaType mt = stream_.getAudioType();
                //set audio mediatype
            }

            //get first audio and video streams and push it to sourcse filter then 
            //we can connect it to decoder and rendere!
        }

        public void play(IntPtr handle)
        {
        }

        public void play()
        {
        }

        public void pause()
        {
        }

        public void stop()
        {
        }

        private bool getVideo()
        {
            return false;
        }

        private bool getAudio()
        {
            return false;
        }
    }*/
}
