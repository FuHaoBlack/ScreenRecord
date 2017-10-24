using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScreenRecord
{
    public class ScreenModel
    {
        public static Screen[] GetAllScreen()
        {
            return Screen.AllScreens;
        }


        private static Image ScreenImage;
        /// <summary>
        /// 获取屏幕截图
        /// </summary>
        /// <param name="screen">屏幕</param>
        /// <returns></returns>
        public static Image GetScreenBitmap(Screen screen)
        {
            ScreenImage = new Bitmap(screen.WorkingArea.Width, screen.WorkingArea.Height);
            using (Graphics g = Graphics.FromImage(ScreenImage))
            {
                g.Clear(Color.WhiteSmoke);
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.CopyFromScreen(screen.WorkingArea.X, screen.WorkingArea.Y, 0, 0, new Size(screen.WorkingArea.Width, screen.WorkingArea.Height));
                using (MemoryStream ms = new MemoryStream())
                {
                    ScreenImage.Save(ms, ImageFormat.Bmp);
                    return ScreenImage;
                }
            }

        }

        /// <summary>
        /// 图片转byte
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static byte[] ImageToByte(Image image)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, image.RawFormat);
                return ms.GetBuffer();
            }
        }

        /// <summary>
        /// 压缩图片
        /// </summary>
        /// <param name="iSource">图片来源</param>
        /// <param name="flag">压缩质量</param>
        /// <returns></returns>
        public static Image GetPicThumbnail(Image iSource, int flag, bool isSave = false, string path = "")
        {
            ImageFormat tFormat = iSource.RawFormat;
            //以下代码为保存图片时，设置压缩质量    
            EncoderParameters ep = new EncoderParameters();
            long[] qy = new long[1];
            qy[0] = flag;//设置压缩的比例1-100    
            EncoderParameter eParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qy);
            ep.Param[0] = eParam;

            using (MemoryStream ms = new MemoryStream())
            {
                try
                {
                    ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();
                    ImageCodecInfo jpegICIinfo = null;
                    for (int x = 0; x < arrayICI.Length; x++)
                    {
                        if (arrayICI[x].FormatDescription.Equals("JPEG"))
                        {
                            jpegICIinfo = arrayICI[x];
                            break;
                        }
                    }
                    if (jpegICIinfo != null)
                    {
                        if (isSave)
                        {
                            iSource.Save(path + DateTime.Now.Ticks + ".jpg", jpegICIinfo, ep);
                        }
                        iSource.Save(ms, jpegICIinfo, ep);
                    }
                    else
                    {
                        if (isSave)
                        {
                            iSource.Save(path + DateTime.Now.Ticks + ".jpg", tFormat);
                        }
                        iSource.Save(ms, tFormat);
                    }
                    return Image.FromStream(ms);
                }
                catch
                {
                    return iSource;
                }
                finally
                {
                    iSource.Dispose();
                }
            }
        }

        /// <summary>
        /// 压缩图片并按比例缩小
        /// </summary>
        /// <param name="iSource">图片来源</param>
        /// <param name="flag">压缩比例</param>
        /// <param name="dHeight">高</param>
        /// <param name="dWidth">宽</param>
        /// <param name="isSave">是否保存</param>
        /// <param name="path">保存地址</param>
        /// <returns></returns>
        public static Image GetPicThumbnail(Image iSource, int flag, int dHeight, int dWidth, bool isSave = false, string path = "")
        {
            int sW = 0, sH = 0;

            //按比例缩放  
            Size tem_size = new Size(iSource.Width, iSource.Height);

            if (tem_size.Width > dHeight || tem_size.Width > dWidth)
            {
                if ((tem_size.Width * dHeight) > (tem_size.Width * dWidth))
                {
                    sW = dWidth;
                    sH = (dWidth * tem_size.Height) / tem_size.Width;
                }
                else
                {
                    sH = dHeight;
                    sW = (tem_size.Width * dHeight) / tem_size.Height;
                }
            }
            else
            {
                sW = tem_size.Width;
                sH = tem_size.Height;
            }

            Bitmap ob = new Bitmap(dWidth, dHeight);
            Graphics g = Graphics.FromImage(ob);

            g.Clear(Color.WhiteSmoke);
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            g.DrawImage(iSource, new Rectangle((dWidth - sW) / 2, (dHeight - sH) / 2, sW, sH), 0, 0, iSource.Width, iSource.Height, GraphicsUnit.Pixel);

            g.Dispose();

            ImageFormat tFormat = ob.RawFormat;
            //以下代码为保存图片时，设置压缩质量    
            EncoderParameters ep = new EncoderParameters();
            long[] qy = new long[1];
            qy[0] = flag;//设置压缩的比例1-100    
            EncoderParameter eParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qy);
            ep.Param[0] = eParam;

            using (MemoryStream ms = new MemoryStream())
            {
                try
                {
                    ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();
                    ImageCodecInfo jpegICIinfo = null;
                    for (int x = 0; x < arrayICI.Length; x++)
                    {
                        if (arrayICI[x].FormatDescription.Equals("JPEG"))
                        {
                            jpegICIinfo = arrayICI[x];
                            break;
                        }
                    }
                    if (jpegICIinfo != null)
                    {
                        if (isSave)
                        {
                            ob.Save(path + "/" + DateTime.Now.Ticks + ".jpg", jpegICIinfo, ep);
                        }
                        ob.Save(ms, jpegICIinfo, ep);
                    }
                    else
                    {
                        if (isSave)
                        {
                            ob.Save(path + "/" + DateTime.Now.Ticks + ".jpg", tFormat);
                        }
                        ob.Save(ms, tFormat);
                    }
                    return ob;
                }
                catch (Exception)
                {
                    return iSource;
                }
                finally
                {
                    iSource.Dispose();
                }
            }
        }

    }

    public static class Helper
    {

        public static Bitmap ToBitmap(this Image img)
        {
            return new Bitmap(img);
        }
    }
}
