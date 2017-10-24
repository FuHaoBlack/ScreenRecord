using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.Video.VFW;
using System.Drawing;

namespace ScreenRecord
{
    class AccordModel
    {

        static AVIWriter avi = null;
        public static void WriteAvi(string path, int width, int height, VideoType videoType = VideoType.Default)
        {
            avi = new AVIWriter();
            if (videoType != VideoType.Default)
            {
                avi.Codec = videoType.ToString();
            }
            avi.FrameRate = 10;
            avi.Open(path, width, height);
        }

        public static void AddBmpInAvi(Image bmp)
        {
            using (Bitmap b = ScreenModel.GetPicThumbnail(bmp, 80).ToBitmap())
            {
                b.SetPixel(0, 0, Color.Red);
                avi.AddFrame(b);
            }
        }

        public static void OverAvi()
        {
            avi.Close();
            avi.Dispose();
            avi = null;
        }
    }

    public enum VideoType
    {
        Default,
        MPEG4
    }
}
