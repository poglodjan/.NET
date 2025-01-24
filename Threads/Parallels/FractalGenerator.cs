using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Lab11_task3;

public class FractalGenerator
{
    public static Color GetColor(int iterations)
    {
        if (iterations == MandelbrotSet.MaxIterations)
            return Color.Black;

        byte c = (byte)(255 - (iterations * 255 / MandelbrotSet.MaxIterations));
        return Color.FromRgb(c, c, c);
    }

    public static void GenerateFractalSingleThread(Image<Rgba32> image)
    {
        int width = image.Width;
        int height = image.Height;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                double a = (x - width / 2.0) * 4.0 / width;
                double b = (y - height / 2.0) * 4.0 / height;
                int iterations = MandelbrotSet.Calculate(a, b);
                image[x, y] = GetColor(iterations);
            }
        }
    }

    public static void GenerateFractalMultiThread(Image<Rgba32> image)
    {
        int width = image.Width;
        int height = image.Height;

        int numThreads = Environment.ProcessorCount;
        int rowsPerThread = height / numThreads;

        Thread[] threads = new Thread[numThreads];

        for (int t = 0; t < numThreads; t++)
        {
            int startRow = t * rowsPerThread;
            int endRow = (t == numThreads - 1) ? height : (t + 1) * rowsPerThread;

            threads[t] = new Thread(() =>
            {
                for (int y = startRow; y < endRow; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        double a = (x - width / 2.0) * 4.0 / width;
                        double b = (y - height / 2.0) * 4.0 / height;
                        int iterations = MandelbrotSet.Calculate(a, b);
                        image[x, y] = GetColor(iterations);
                    }
                }
            });

            threads[t].Start();
        }

        foreach (var thread in threads)
        {
            thread.Join();
        }
    }

    public static void GenerateFractalWithTasks(Image<Rgba32> image)
    {
        int width = image.Width;
        int height = image.Height;

        int numTasks = Environment.ProcessorCount;
        int rowsPerTask = height / numTasks;

        Task[] tasks = new Task[numTasks];

        for (int t = 0; t < numTasks; t++)
        {
            int startRow = t * rowsPerTask;
            int endRow = (t == numTasks - 1) ? height : (t + 1) * rowsPerTask;

            tasks[t] = Task.Run(() =>
            {
                for (int y = startRow; y < endRow; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        double a = (x - width / 2.0) * 4.0 / width;
                        double b = (y - height / 2.0) * 4.0 / height;
                        int iterations = MandelbrotSet.Calculate(a, b);
                        image[x, y] = GetColor(iterations);
                    }
                }
            });
        }

        Task.WhenAll(tasks).Wait();
    }

    public static void GenerateFractalParallel(Image<Rgba32> image)
    {
        // TODO: Use Parallel class to calculate pixel values
        throw new NotImplementedException();
    }

}