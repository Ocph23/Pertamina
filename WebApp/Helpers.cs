using System;
using System.IO;
using System.Collections.Generic;
using System.Drawing;

namespace WebApp
{
    public class Helpers
    {

        public static string ImagePath => Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/bukti/images/");
        public static string VideoPath => Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/bukti/videos/");
        public static string ThumbPath => Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/bukti/thumbs/");
        public static string ProfilePath => Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/profiles/");
        public static string LogoPath => Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/logo/");


        public static byte[] CreateThumb(byte[] byteArray)
        {
            try
            {
                System.Drawing.Image imThumbnailImage;
                System.Drawing.Image OriginalImage;
                MemoryStream ms = new MemoryStream();

                // Stream / Write Image to Memory Stream from the Byte Array.
                ms.Write(byteArray, 0, byteArray.Length);

                OriginalImage = System.Drawing.Image.FromStream(ms);

                // Shrink the Original Image to a thumbnail size.
                imThumbnailImage = OriginalImage.GetThumbnailImage(100, 100, new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallBack), IntPtr.Zero);

                // Save Thumbnail to Memory Stream for Conversion to Byte Array.
                MemoryStream myMS = new MemoryStream();
                imThumbnailImage.Save(myMS, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] test_imge = myMS.ToArray();
                return test_imge;
            }
            catch (System.Exception)
            {
                return byteArray;
            }
        }
        public static byte[] CreateThumbVideo(byte[] byteArray)
        {
            try
            {

                System.Drawing.Image imThumbnailImage;
                System.Drawing.Image OriginalImage;
                MemoryStream ms = new MemoryStream();

                // Stream / Write Image to Memory Stream from the Byte Array.
                ms.Write(byteArray, 0, byteArray.Length);

                OriginalImage = System.Drawing.Image.FromStream(ms);

                // Shrink the Original Image to a thumbnail size.
                imThumbnailImage = OriginalImage.GetThumbnailImage(75, 75, new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallBack), IntPtr.Zero);

                // Save Thumbnail to Memory Stream for Conversion to Byte Array.
                MemoryStream myMS = new MemoryStream();
                imThumbnailImage.Save(myMS, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] test_imge = myMS.ToArray();
                return test_imge;
            }
            catch (System.Exception)
            {
                return byteArray;
            }
        }

        private static bool ThumbnailCallBack()
        {
            return false;
        }

        internal static string GetPath(string fileType)
        {
            if (fileType.Contains("video"))
                return VideoPath;
            return ImagePath;

        }

        internal static string CreateFileName(string fileType)
        {
            Guid guid = Guid.NewGuid();
            if (fileType.Contains("image"))
                return guid.ToString() + ".png";

            return guid.ToString() + ".mp4";


        }



    }


    public interface IImageResizer
    {
        void Resize(string source, string destination, int height);
    }

}