using System;
using System.Drawing;
using System.IO;
using System.Linq;

namespace ImageProcessingApp
{
    class Program
    {
        static void Main(string[] args)
        {

            Action<Bitmap> displayDelegate = (bitmap) =>
            {
                Console.WriteLine("Відображення зображення");
                bitmap.Save("processed_image.jpg");
                bitmap.Dispose();
            };

            Func<Bitmap, Bitmap>[] imageProcessingDelegates =
            {
                (bitmap) =>
                {
                    Console.WriteLine("Обертання зображення на 90 градусів");
                    bitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    return bitmap;
                },
                (bitmap) =>
                {
                    Console.WriteLine("Збільшення розміру зображення в два рази");
                    Bitmap newBitmap = new Bitmap(bitmap.Width * 2, bitmap.Height * 2);
                    using (Graphics graphics = Graphics.FromImage(newBitmap))
                    {
                        graphics.DrawImage(bitmap, new Rectangle(0, 0, newBitmap.Width, newBitmap.Height), new Rectangle(0, 0, bitmap.Width, bitmap.Height), GraphicsUnit.Pixel);
                    }
                    return newBitmap;
                }
            };

            string[] imageFilePaths = Directory.GetFiles("images/", "*.jpg");

            foreach (string imageFilePath in imageFilePaths)
            {
                Bitmap bitmap = new Bitmap(imageFilePath);
                foreach (Func<Bitmap, Bitmap> processingDelegate in imageProcessingDelegates)
                {
                    bitmap = processingDelegate(bitmap);
                }
                displayDelegate(bitmap);
            }
        }
    }
}
