using Emgu.CV;
using Emgu.CV.CvEnum;
using System.Diagnostics;
using System.Drawing;
using System.Text;

namespace Melius_MM
{
	internal class Program
	{
		static void Main(string[] args)
		{
			string vidPath;
			if (args.Length != 0 && File.Exists(args[0] )&& args[0].ToLower().EndsWith(".mp4")) {
			
			vidPath = args[0];
			}
			else
			{

				Console.WriteLine("Hello, World!");
				Console.WriteLine("Naval mp4 soubor:");
				vidPath = Console.ReadLine().Replace("\"", "");
			}


           

			
			Size newSize = new Size(16 * 10, 9 * 10); 

			VideoCapture capture = new VideoCapture(vidPath);

			double fps = capture.Get(CapProp.Fps);
			Console.WriteLine("FPS: " + fps);

			int frameDelay = (int)(1000 / fps);

			

			Mat frame = new Mat();
			Stopwatch stopwatch = new Stopwatch();

			int frameCount = 0;

			while (true)
			{
			
				stopwatch.Restart();



				capture.Read(frame);

				if (frame.IsEmpty)
				{
					Console.WriteLine("Konec videa.");
					break;
				}
				if (frameCount % 1 == 0)//render pouze pro n-ty frame
				{

					Mat resizedFrame = new Mat();
					CvInvoke.Resize(frame, resizedFrame, newSize);

					Console.SetCursorPosition(0, 0);
					Console.CursorVisible = false;

					Bitmap bitmap = resizedFrame.ToBitmap();

					StringBuilder builder = new StringBuilder();
					for (int j = 0; j < bitmap.Height; j += 2)
					{
						for (int i = 0; i < bitmap.Width; i++)
						{
							Color color = bitmap.GetPixel(i, j);
							Color color1 = bitmap.GetPixel(i, j + 1);
							builder.Append($"\u001b[48;2;{color.R};{color.G};{color.B};38;2;{color1.R};{color1.G};{color1.B}m" + "▄");

						}
						builder.AppendLine();
					}
					Console.WriteLine(builder.ToString());
				}

				frameCount++;

			

				
				stopwatch.Stop();
				int timeElapsed = (int)stopwatch.ElapsedMilliseconds;
				if (timeElapsed < frameDelay)
				{
					Thread.Sleep(frameDelay - timeElapsed);
				}
			}

			capture.Dispose();
			

			Console.ResetColor();

            Console.WriteLine("Konec videa...");
        }
	}

}
