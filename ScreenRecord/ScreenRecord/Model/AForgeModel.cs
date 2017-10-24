using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AForge.Video;
using AForge.Video.DirectShow;
using AForge.Controls;
using AForge.Video.FFMPEG;
using System.Drawing;

namespace ScreenRecord
{
    public class AForgeModel
    {
        /// <summary>
        /// 获取摄像头
        /// </summary>
        private FilterInfoCollection videoDevices;

        /// <summary>
        /// 视频控件
        /// </summary>
        private VideoSourcePlayer videoSourcePlayer;

        /// <summary>
        /// 视频流操作
        /// </summary>
        static private VideoFileWriter writer;
        //Extrenal 要添加到对应Debug目录下
        public static void WriteAvi(string path, int width, int height, VideoCodec videoType = VideoCodec.Default)
        {
            writer = new VideoFileWriter();
            writer.Open(path, width, height, 10, videoType);
        }

        public static void AddBmpInAvi(Image bmp)
        {
            using (Bitmap b = ScreenModel.GetPicThumbnail(bmp, 100).ToBitmap())
            {
                writer.WriteVideoFrame(b);
            }
        }

        public static void OverAvi()
        {
            writer.Close();
            writer.Dispose();
            writer.Close();
        }
    }
}
