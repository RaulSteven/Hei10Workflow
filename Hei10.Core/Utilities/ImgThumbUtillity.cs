using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Kaliko.ImageLibrary;
using Kaliko.ImageLibrary.Scaling;

namespace Hei10.Core.Utilities
{
    public enum WaterMarkingPosition
    {
        LeftTop,
        LeftBottom,
        RightTop,
        RightBottom,
        Center
    }
    public class ImgThumbUtillity
    {


        //private string 
        public long QualityLevel { get; set; }

        /// <summary>
        /// 缩略图的截图模式是：当原图宽高比按照缩略图时缩放
        /// </summary>
        /// <param name="originalImagePath"></param>
        /// <param name="thumbnailPath"></param>
        /// <param name="extName"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="scale">Crop:取中间部分;Fit:按比例缩放，自动调整尺寸;Pad:按比例缩放，保持尺寸，不足部分填白</param>
        /// <param name="position">水印位置</param>
        /// <param name="waterMarkingPath">水印图片路劲</param>
        public void MakeThumbnail(string originalImagePath, string thumbnailPath, string extName, int width, int height, ThumbnailMethod scale = ThumbnailMethod.Fit, WaterMarkingPosition? position = null, string waterMarkingPath = "")
        {
            if (!File.Exists(originalImagePath))
            {
                return;
            }
            var image = new KalikoImage(originalImagePath) { BackgroundColor = Color.White };
            var format = GetImageFormat(extName);
            ScalingBase scalingBase = null;
            switch (scale)
            {
                case ThumbnailMethod.Crop:
                    scalingBase = new CropScaling(width, height);
                    break;
                case ThumbnailMethod.Fit:
                    scalingBase = new FitScaling(width, height);
                    break;
                case ThumbnailMethod.Pad:
                    scalingBase = new PadScaling(width, height);
                    break;
                default:
                    scalingBase = new CropScaling(width, height);
                    break;
            }
            var imageThumb = image.Scale(scalingBase);
            AddWaterMarking(position, waterMarkingPath, imageThumb);
            imageThumb.SaveImage(thumbnailPath, format);
            image.Dispose();
            imageThumb.Dispose();
        }

        private void AddWaterMarking(WaterMarkingPosition? position, string waterMarkingPath, KalikoImage imageThumb)
        {
            if (!position.HasValue || string.IsNullOrEmpty(waterMarkingPath)) return;
            //需要打上水印
            var waterMarkingImgage = new KalikoImage(waterMarkingPath);

            var waterMarkingWidth = (int) (imageThumb.Width/4.8);
            var waterMarkingHeight = (int) (waterMarkingImgage.Height*waterMarkingWidth/waterMarkingImgage.Width);
            var waterScalingBase = new FitScaling(waterMarkingWidth, waterMarkingHeight);
            var waterThumb = waterMarkingImgage.Scale(waterScalingBase);

            //计算距离
            var waterX = 0;
            var waterY = 0;
            const int edgeWidth = 40;
            switch (position.Value)
            {
                case WaterMarkingPosition.Center:
                    waterX = (imageThumb.Width - waterThumb.Width)/2;
                    waterY = (imageThumb.Height - waterThumb.Height)/2;
                    break;
                case WaterMarkingPosition.LeftBottom:
                    waterX = edgeWidth;
                    waterY = imageThumb.Height - waterThumb.Height - edgeWidth;
                    break;
                case WaterMarkingPosition.LeftTop:
                    waterX = edgeWidth;
                    waterY = edgeWidth;
                    break;
                case WaterMarkingPosition.RightBottom:
                    waterX = imageThumb.Width - waterThumb.Width - edgeWidth;
                    waterY = imageThumb.Height - waterThumb.Height - edgeWidth;
                    break;
                case WaterMarkingPosition.RightTop:
                    waterX = imageThumb.Width - waterThumb.Width - edgeWidth;
                    waterY = edgeWidth;
                    break;
            }
            imageThumb.BlitImage(waterThumb, waterX, waterY);
        }

        public ImageFormat GetImageFormat(string extName)
        {
            ImageFormat format = null;
            switch (extName.ToLower())
            {
                case ".jpg":
                case ".jpeg":
                    format = ImageFormat.Jpeg;
                    break;
                case ".png":
                    format = ImageFormat.Png;
                    break;
                case ".gif":
                    format = ImageFormat.Gif;
                    break;
                case ".bmp":
                    format = ImageFormat.Bmp;
                    break;
                case ".tif":
                    format = ImageFormat.Tiff;
                    break;
                case ".ico":
                    format = ImageFormat.Icon;
                    break;
                default:
                    format = ImageFormat.Jpeg;
                    break;
            }
            return format;
        }

        private EncoderParameters GetParams()
        {
            EncoderParameters parameters = new EncoderParameters(1);
            Encoder quality = Encoder.Quality;
            EncoderParameter parameter = new EncoderParameter(quality, QualityLevel);
            parameters.Param[0] = parameter;
            return parameters;
        }

        public ImageCodecInfo GetInfo(string value)
        {
            ImageCodecInfo[] imgInfos = ImageCodecInfo.GetImageEncoders();
            for (int i = 0; i < imgInfos.Length; i++)
            {
                if (imgInfos[i].MimeType.Equals(value, StringComparison.OrdinalIgnoreCase))
                {
                    return imgInfos[i];
                }
            }
            return null;
        }

    }
}