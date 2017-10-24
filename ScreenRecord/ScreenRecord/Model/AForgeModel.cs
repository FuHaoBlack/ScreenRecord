using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AForge.Video;

namespace ScreenRecord
{
    public class AForgeModel
    {
        public static void WriteAvi()
        {
            MJPEGStream stream = new MJPEGStream();
            //JPEGStream stream = new JPEGStream();
            stream.NewFrame += Stream_NewFrame;
            stream.Start();
        }

        private static void Stream_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
        }
    }
}
