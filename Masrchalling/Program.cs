using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using ImSh = SixLabors.ImageSharp;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

// my functions: 
// GradientPattern: Generates a smooth gradient from red to blue across the image.
// WavePattern: Creates a wave-like color pattern using sinusoidal variations.
// NoisePattern: Fills the image with random noise.
// InvertColors: Inverts the colors of the image.
// SepiaTones: Applies a sepia tone to the image.

namespace Frontend
{
    internal partial class Program
    {
        // importing C++ functions
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate bool TryReportCallback(float progress);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate MyColor GetColorCallback(float x, float y);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate MyColor ModifyColorCallback(float x, float y, MyColor original);

        //  MyColor
        [StructLayout(LayoutKind.Sequential)]
        public struct MyColor
        {
            public byte R;
            public byte G;
            public byte B;
            public byte A;
        }

        // Circle
        [StructLayout(LayoutKind.Sequential)]
        public struct Circle
        {
            public float X;
            public float Y;
            public float Radius;
        }

        // Import funkcji z biblioteki DLL
        private const string DllName = "ImageGenerator.dll";

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GenerateImage(
            [Out] MyColor[] texture, int textureWidth, int textureHeight,
            TryReportCallback tryReportCallback);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GenerateImage_Custom(
            [Out] MyColor[] texture, int textureWidth, int textureHeight,
            GetColorCallback getColor, TryReportCallback tryReportCallback);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ProcessPixels_Custom(
            [In, Out] MyColor[] texture, int textureWidth, int textureHeight,
            ModifyColorCallback modifyColor, TryReportCallback tryReportCallback);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void Blur(
            [In, Out] MyColor[] texture, int textureWidth, int textureHeight,
            int w, int h, TryReportCallback tryReportCallback);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void DrawCircles(
            [In, Out] MyColor[] texture, int textureWidth, int textureHeight,
            [In] Circle[] circles, int circleCount, TryReportCallback tryReportCallback);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ColorCorrection(
            [In, Out] MyColor[] texture, int textureWidth, int textureHeight,
            float red, float green, float blue, TryReportCallback tryReportCallback);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GammaCorrection(
            [In, Out] MyColor[] texture, int textureWidth, int textureHeight,
            float gamma, TryReportCallback tryReportCallback);

        static bool stopProcessing = false;
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the Image Generator");
            Console.WriteLine("Type 'Help' to see available commands and 'Exit' to quit");
            while (true)
            {
                Console.Write(">>> ");
                string input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                    continue;
                if (input.Equals("Exit", StringComparison.OrdinalIgnoreCase))
                    break;
                if (input.Equals("Help", StringComparison.OrdinalIgnoreCase))
                {
                    DisplayHelp();
                    continue;
                }
                try
                {
                    ExecuteCommandChain(input);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }

            static void DisplayHelp()
            {
                Console.WriteLine("Available commands:");
                Console.WriteLine("1) Generate n width height          - Generate n random images of size width x height.");
                Console.WriteLine("2) GradientPattern n width height   - Generate n images with a gradient pattern.");
                Console.WriteLine("3) WavePattern n width height freq  - Generate n images with sinusoidal wave patterns.");
                Console.WriteLine("4) NoisePattern n width height      - Generate n images filled with random noise.");
                Console.WriteLine("5) Blur w h                         - Apply a blur with width w and height h.");
                Console.WriteLine("6) InvertColors                     - Invert the colors of all images.");
                Console.WriteLine("7) SepiaTone                        - Apply a sepia tone to all images.");
                Console.WriteLine("8) Output filename_prefix           - Save images to disk with the given filename prefix.");
                Console.WriteLine("9) RandomCircles n r                - Add n random circles with radius r.");
                Console.WriteLine("10) Room x1 y1 x2 y2                - Draw a filled rectangle.");
                Console.WriteLine("11) ColorCorrection red green blue  - Adjust color channels.");
                Console.WriteLine("12) GammaCorrection gamma           - Apply gamma correction.");
                Console.WriteLine("13) Help                            - Display this help message.");
                Console.WriteLine("14) Exit                            - Quit the application.");
            }

            static void ExecuteCommandChain(string input)
            {
                var commands = input.Split('|', StringSplitOptions.RemoveEmptyEntries);
                List<Image<Rgba32>> images = new();
                int width = 0;
                int height = 0;

                foreach (var command in commands)
                {
                    var parts = command.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length == 0) throw new InvalidOperationException("Empty command detected");

                    string cmdName = parts[0];
                    string[] args = parts.Length > 1 ? parts[1..] : Array.Empty<string>();

                    switch (cmdName)
                    {
                        case "Generate":
                            (images, width, height) = GenerateImages(args);
                            break;
                        case "Blur":
                            EnsureImagesInitialized(images);
                            ApplyBlur(images, width, height, args);
                            break;
                        case "Output":
                            EnsureImagesInitialized(images);
                            SaveImages(images, args);
                            break;
                        case "RandomCircles":
                            EnsureImagesInitialized(images);
                            AddRandomCircles(images, width, height, args);
                            break;
                        case "Room":
                            EnsureImagesInitialized(images);
                            DrawRoom(images, width, height, args);
                            break;
                        case "ColorCorrection":
                            EnsureImagesInitialized(images);
                            ApplyColorCorrection(images, width, height, args);
                            break;
                        case "GammaCorrection":
                            EnsureImagesInitialized(images);
                            ApplyGammaCorrection(images, width, height, args);
                            break;
                        case "GradientPattern": 
                            (images, width, height) = GenerateGradientPattern(args);
                            break;
                        case "WavePattern": 
                            (images, width, height) = GenerateWavePattern(args);
                            break;
                        case "NoisePattern": 
                            (images, width, height) = GenerateNoisePattern(args);
                            break;
                        case "InvertColors": 
                            EnsureImagesInitialized(images);
                            ApplyInvertColors(images, width, height);
                            break;
                        case "SepiaTone": 
                            EnsureImagesInitialized(images);
                            ApplySepiaTone(images, width, height);
                            break;
                        default:
                            throw new InvalidOperationException($"Unknown command: {cmdName}");
                    }
                }
            }

            static void EnsureImagesInitialized(List<Image<Rgba32>> images)
            {
                if (images == null || images.Count == 0)
                    throw new InvalidOperationException("Start with the 'Generate' command.");
            }

            static (List<Image<Rgba32>>, int, int) GenerateImages(string[] args)
            {
                if (args.Length != 3) throw new ArgumentException("Generate requires 3 arguments: n width height");
                int n = int.Parse(args[0]);
                int width = int.Parse(args[1]);
                int height = int.Parse(args[2]);

                var images = new List<Image<Rgba32>>();
                for (int i = 0; i < n; i++)
                {
                    var texture = new MyColor[width * height];
                    TryReportCallback progressCallback = progress =>
                    {
                        Console.WriteLine($"Generating image {i + 1}: {progress * 100:F2}%");
                        return !stopProcessing;
                    };

                    // Generate the raw image data
                    GenerateImage(texture, width, height, progressCallback);

                    // Convert raw MyColor[] data to Image<Rgba32>
                    var image = new Image<Rgba32>(width, height);
                    image.DangerousTryGetSinglePixelMemory(out var pixelMemory);
                    var pixelSpan = pixelMemory.Span;

                    for (int j = 0; j < texture.Length; j++)
                    {
                        pixelSpan[j] = new Rgba32(texture[j].R, texture[j].G, texture[j].B, texture[j].A);
                    }

                    images.Add(image);
                }

                Console.WriteLine($"Generated {n} images of size {width}x{height}.");
                return (images, width, height);
            }

            static void ApplyBlur(List<Image<Rgba32>> images, int width, int height, string[] args)
            {
                if (args.Length != 2) throw new ArgumentException("Blur requires 2 arguments: w h.");
                int w = int.Parse(args[0]);
                int h = int.Parse(args[1]);

                for (int i = 0; i < images.Count; i++)
                {
                    TryReportCallback progressCallback = progress =>
                    {
                        Console.WriteLine($"Blurring image {i + 1}: {progress * 100:F2}%");
                        return !stopProcessing;
                    };

                    var image = images[i];
                    image.DangerousTryGetSinglePixelMemory(out var pixelMemory);
                    var pixelSpan = pixelMemory.Span;

                    // Convert Image<Rgba32> to MyColor[]
                    var rawData = new MyColor[width * height];
                    for (int j = 0; j < pixelSpan.Length; j++)
                    {
                        rawData[j] = new MyColor
                        {
                            R = pixelSpan[j].R,
                            G = pixelSpan[j].G,
                            B = pixelSpan[j].B,
                            A = pixelSpan[j].A
                        };
                    }

                    // Call the C++ backend Blur function
                    Blur(rawData, width, height, w, h, progressCallback);

                    // Convert MyColor[] back to Image<Rgba32>
                    for (int j = 0; j < rawData.Length; j++)
                    {
                        pixelSpan[j] = new Rgba32(rawData[j].R, rawData[j].G, rawData[j].B, rawData[j].A);
                    }
                }
            }

            static void SaveImages(List<ImSh.Image<ImSh.PixelFormats.Rgba32>> images, string[] args)
            {
                if (args.Length != 1) 
                    throw new ArgumentException("Output requires 1 argument: filename_prefix.");
                
                string prefix = args[0];

                for (int i = 0; i < images.Count; i++)
                {
                    string filename = $"{prefix}{i + 1}.jpeg";

                    using (var fs = new FileStream(filename, FileMode.Create, FileAccess.Write))
                    {
                        var encoder = new ImSh.Formats.Jpeg.JpegEncoder();
                        encoder.Encode(images[i], fs);
                    }

                    Console.WriteLine($"Saved image {i + 1} as {filename}.");
                }
            }

            static void AddRandomCircles(List<ImSh.Image<ImSh.PixelFormats.Rgba32>> images, int width, int height, string[] args)
            {
                if (args.Length != 2) 
                    throw new ArgumentException("RandomCircles requires 2 arguments: n r.");
                
                int n = int.Parse(args[0]);
                float r = float.Parse(args[1]);

                var circles = new Circle[n];
                var rand = new Random();

                // Tworzenie losowych kół
                for (int i = 0; i < n; i++)
                {
                    circles[i] = new Circle
                    {
                        X = (float)rand.NextDouble(), // Normalizowane współrzędne X
                        Y = (float)rand.NextDouble(), // --/-- Y
                        Radius = r                    
                    };
                }

                // Dodawanie kół do każdego obrazu
                for (int i = 0; i < images.Count; i++)
                {
                    var image = images[i];
                    image.DangerousTryGetSinglePixelMemory(out var pixelMemory);
                    var pixelSpan = pixelMemory.Span;

                    // Konwersja Image<Rgba32> -> MyColor[]
                    var rawData = new MyColor[width * height];
                    for (int j = 0; j < pixelSpan.Length; j++)
                    {
                        rawData[j] = new MyColor
                        {
                            R = pixelSpan[j].R,
                            G = pixelSpan[j].G,
                            B = pixelSpan[j].B,
                            A = pixelSpan[j].A
                        };
                    }

                    // Wywołanie DrawCircles z backendu
                    TryReportCallback progressCallback = progress =>
                    {
                        Console.WriteLine($"Adding circles to image {i + 1}: {progress * 100:F2}%");
                        return !stopProcessing;
                    };

                    DrawCircles(rawData, width, height, circles, n, progressCallback);

                    // Konwersja MyColor[] -> Image<Rgba32>
                    for (int j = 0; j < rawData.Length; j++)
                    {
                        pixelSpan[j] = new Rgba32(rawData[j].R, rawData[j].G, rawData[j].B, rawData[j].A);
                    }
                }
            }

            static void DrawRoom(List<ImSh.Image<ImSh.PixelFormats.Rgba32>> images, int width, int height, string[] args)
            {
                if (args.Length != 4) 
                    throw new ArgumentException("Room requires 4 arguments: x1 y1 x2 y2.");
                
                float x1 = float.Parse(args[0]);
                float y1 = float.Parse(args[1]);
                float x2 = float.Parse(args[2]);
                float y2 = float.Parse(args[3]);

                ModifyColorCallback modifyCallback = (x, y, color) =>
                {
                    if (x >= x1 && x <= x2 && y >= y1 && y <= y2)
                    {
                        return new MyColor { R = 255, G = 255, B = 255, A = 255 };
                    }
                    return color;
                };

                for (int i = 0; i < images.Count; i++)
                {
                    var image = images[i];
                    image.DangerousTryGetSinglePixelMemory(out var pixelMemory);
                    var pixelSpan = pixelMemory.Span;

                    var rawData = new MyColor[width * height];
                    for (int j = 0; j < pixelSpan.Length; j++)
                    {
                        rawData[j] = new MyColor
                        {
                            R = pixelSpan[j].R,
                            G = pixelSpan[j].G,
                            B = pixelSpan[j].B,
                            A = pixelSpan[j].A
                        };
                    }

                    TryReportCallback progressCallback = progress =>
                    {
                        Console.WriteLine($"Drawing room on image {i + 1}: {progress * 100:F2}%");
                        return !stopProcessing;
                    };

                    ProcessPixels_Custom(rawData, width, height, modifyCallback, progressCallback);

                    for (int j = 0; j < rawData.Length; j++)
                    {
                        pixelSpan[j] = new Rgba32(rawData[j].R, rawData[j].G, rawData[j].B, rawData[j].A);
                    }
                }
            }

            static void ApplyColorCorrection(List<ImSh.Image<ImSh.PixelFormats.Rgba32>> images, int width, int height, string[] args)
            {
                if (args.Length != 3) 
                    throw new ArgumentException("ColorCorrection requires 3 arguments: red green blue.");
                
                float red = float.Parse(args[0]);
                float green = float.Parse(args[1]);
                float blue = float.Parse(args[2]);

                for (int i = 0; i < images.Count; i++)
                {
                    var image = images[i];
                    image.DangerousTryGetSinglePixelMemory(out var pixelMemory);
                    var pixelSpan = pixelMemory.Span;

                    var rawData = new MyColor[width * height];
                    for (int j = 0; j < pixelSpan.Length; j++)
                    {
                        rawData[j] = new MyColor
                        {
                            R = pixelSpan[j].R,
                            G = pixelSpan[j].G,
                            B = pixelSpan[j].B,
                            A = pixelSpan[j].A
                        };
                    }

                    TryReportCallback progressCallback = progress =>
                    {
                        Console.WriteLine($"Applying color correction to image {i + 1}: {progress * 100:F2}%");
                        return !stopProcessing;
                    };

                    ColorCorrection(rawData, width, height, red, green, blue, progressCallback);

                    for (int j = 0; j < rawData.Length; j++)
                    {
                        pixelSpan[j] = new Rgba32(rawData[j].R, rawData[j].G, rawData[j].B, rawData[j].A);
                    }
                }
            }

            static void ApplyGammaCorrection(List<ImSh.Image<ImSh.PixelFormats.Rgba32>> images, int width, int height, string[] args)
            {
                if (args.Length != 1) 
                    throw new ArgumentException("GammaCorrection requires 1 argument: gamma.");
                
                float gamma = float.Parse(args[0]);

                for (int i = 0; i < images.Count; i++)
                {
                    var image = images[i];
                    image.DangerousTryGetSinglePixelMemory(out var pixelMemory);
                    var pixelSpan = pixelMemory.Span;

                    // Konwersja Image<Rgba32> -> MyColor[]
                    var rawData = new MyColor[width * height];
                    for (int j = 0; j < pixelSpan.Length; j++)
                    {
                        rawData[j] = new MyColor
                        {
                            R = pixelSpan[j].R,
                            G = pixelSpan[j].G,
                            B = pixelSpan[j].B,
                            A = pixelSpan[j].A
                        };
                    }

                    // Wywołanie funkcji GammaCorrection z backendu
                    TryReportCallback progressCallback = progress =>
                    {
                        Console.WriteLine($"Applying gamma correction to image {i + 1}: {progress * 100:F2}%");
                        return !stopProcessing;
                    };
                    GammaCorrection(rawData, width, height, gamma, progressCallback);

                    // Konwersja MyColor[] -> Image<Rgba32>
                    for (int j = 0; j < rawData.Length; j++)
                    {
                        pixelSpan[j] = new Rgba32(rawData[j].R, rawData[j].G, rawData[j].B, rawData[j].A);
                    }
                }
            }

                // Generates a smooth gradient from red to blue across the image.
            static (List<ImSh.Image<ImSh.PixelFormats.Rgba32>>, int, int) GenerateGradientPattern(string[] args)
            {
                if (args.Length != 3)
                    throw new ArgumentException("GradientPattern requires 3 arguments: n width height.");

                int n = int.Parse(args[0]);
                int width = int.Parse(args[1]);
                int height = int.Parse(args[2]);

                var images = new List<ImSh.Image<ImSh.PixelFormats.Rgba32>>();

                for (int i = 0; i < n; i++)
                {
                    var image = new ImSh.Image<ImSh.PixelFormats.Rgba32>(width, height);
                    image.DangerousTryGetSinglePixelMemory(out var pixelMemory);
                    var pixelSpan = pixelMemory.Span;

                    // Konwersja Span<Rgba32> na MyColor[]
                    var rawData = new MyColor[width * height];
                    for (int j = 0; j < pixelSpan.Length; j++)
                    {
                        rawData[j] = new MyColor
                        {
                            R = pixelSpan[j].R,
                            G = pixelSpan[j].G,
                            B = pixelSpan[j].B,
                            A = pixelSpan[j].A
                        };
                    }

                    // Wywołanie funkcji backendowej
                    TryReportCallback progressCallback = progress =>
                    {
                        Console.WriteLine($"Generating gradient pattern {i + 1}: {progress * 100:F2}%");
                        return !stopProcessing;
                    };

                    GenerateImage_Custom(rawData, width, height, (x, y) =>
                    {
                        byte r = (byte)(x * 255);
                        byte b = (byte)(y * 255);
                        return new MyColor { R = r, G = 0, B = b, A = 255 };
                    }, progressCallback);

                    // Konwersja MyColor[] z powrotem na Span<Rgba32>
                    for (int j = 0; j < rawData.Length; j++)
                    {
                        pixelSpan[j] = new Rgba32(rawData[j].R, rawData[j].G, rawData[j].B, rawData[j].A);
                    }

                    images.Add(image);
                }

                return (images, width, height);
            }

                // Creates a wave-like color pattern using sinusoidal variations.
            static (List<ImSh.Image<ImSh.PixelFormats.Rgba32>>, int, int) GenerateWavePattern(string[] args)
            {
                if (args.Length != 4)
                    throw new ArgumentException("WavePattern requires 4 arguments: n width height freq.");
                
                int n = int.Parse(args[0]);
                int width = int.Parse(args[1]);
                int height = int.Parse(args[2]);
                float frequency = float.Parse(args[3]);

                var images = new List<ImSh.Image<ImSh.PixelFormats.Rgba32>>();

                for (int i = 0; i < n; i++)
                {
                    var image = new ImSh.Image<ImSh.PixelFormats.Rgba32>(width, height);
                    image.DangerousTryGetSinglePixelMemory(out var pixelMemory);
                    var pixelSpan = pixelMemory.Span;

                    // Konwersja Span<Rgba32> na MyColor[]
                    var rawData = new MyColor[width * height];
                    for (int j = 0; j < pixelSpan.Length; j++)
                    {
                        rawData[j] = new MyColor
                        {
                            R = pixelSpan[j].R,
                            G = pixelSpan[j].G,
                            B = pixelSpan[j].B,
                            A = pixelSpan[j].A
                        };
                    }

                    // Wywołanie funkcji backendowej
                    TryReportCallback progressCallback = progress =>
                    {
                        Console.WriteLine($"Generating wave pattern {i + 1}: {progress * 100:F2}%");
                        return !stopProcessing;
                    };

                    GenerateImage_Custom(rawData, width, height, (x, y) =>
                    {
                        byte r = (byte)(Math.Sin(x * frequency * Math.PI) * 127 + 128);
                        byte g = (byte)(Math.Cos(y * frequency * Math.PI) * 127 + 128);
                        return new MyColor { R = r, G = g, B = 0, A = 255 };
                    }, progressCallback);

                    // Konwersja MyColor[] z powrotem na Span<Rgba32>
                    for (int j = 0; j < rawData.Length; j++)
                    {
                        pixelSpan[j] = new Rgba32(rawData[j].R, rawData[j].G, rawData[j].B, rawData[j].A);
                    }

                    images.Add(image);
                }

                return (images, width, height);
            }

                // Fills the image with random noise.
            static (List<ImSh.Image<ImSh.PixelFormats.Rgba32>>, int, int) GenerateNoisePattern(string[] args)
            {
                if (args.Length != 3)
                    throw new ArgumentException("NoisePattern requires 3 arguments: n width height.");
                
                int n = int.Parse(args[0]);
                int width = int.Parse(args[1]);
                int height = int.Parse(args[2]);

                var rand = new Random();
                var images = new List<ImSh.Image<ImSh.PixelFormats.Rgba32>>();

                for (int i = 0; i < n; i++)
                {
                    var image = new ImSh.Image<ImSh.PixelFormats.Rgba32>(width, height);
                    image.DangerousTryGetSinglePixelMemory(out var pixelMemory);
                    var pixelSpan = pixelMemory.Span;

                    // Konwersja Span<Rgba32> na MyColor[]
                    var rawData = new MyColor[width * height];
                    for (int j = 0; j < pixelSpan.Length; j++)
                    {
                        rawData[j] = new MyColor
                        {
                            R = pixelSpan[j].R,
                            G = pixelSpan[j].G,
                            B = pixelSpan[j].B,
                            A = pixelSpan[j].A
                        };
                    }

                    // Wywołanie funkcji backendowej
                    TryReportCallback progressCallback = progress =>
                    {
                        Console.WriteLine($"Generating noise pattern {i + 1}: {progress * 100:F2}%");
                        return !stopProcessing;
                    };

                    GenerateImage_Custom(rawData, width, height, (x, y) =>
                    {
                        byte intensity = (byte)rand.Next(256); // Generowanie losowej jasności
                        return new MyColor { R = intensity, G = intensity, B = intensity, A = 255 };
                    }, progressCallback);

                    // Konwersja MyColor[] z powrotem na Span<Rgba32>
                    for (int j = 0; j < rawData.Length; j++)
                    {
                        pixelSpan[j] = new Rgba32(rawData[j].R, rawData[j].G, rawData[j].B, rawData[j].A);
                    }

                    images.Add(image);
                }

                return (images, width, height);
            }

                // Inverts the colors of the image.
            static void ApplyInvertColors(List<ImSh.Image<ImSh.PixelFormats.Rgba32>> images, int width, int height)
            {
                for (int i = 0; i < images.Count; i++)
                {
                    var image = images[i];
                    image.DangerousTryGetSinglePixelMemory(out var pixelMemory);
                    var pixelSpan = pixelMemory.Span;

                    // Konwersja na MyColor[]
                    var rawData = new MyColor[width * height];
                    for (int j = 0; j < pixelSpan.Length; j++)
                    {
                        rawData[j] = new MyColor
                        {
                            R = pixelSpan[j].R,
                            G = pixelSpan[j].G,
                            B = pixelSpan[j].B,
                            A = pixelSpan[j].A
                        };
                    }

                    TryReportCallback progressCallback = progress =>
                    {
                        Console.WriteLine($"Inverting colors for image {i + 1}: {progress * 100:F2}%");
                        return !stopProcessing;
                    };

                    ProcessPixels_Custom(rawData, width, height, (x, y, color) =>
                    {
                        return new MyColor
                        {
                            R = (byte)(255 - color.R),
                            G = (byte)(255 - color.G),
                            B = (byte)(255 - color.B),
                            A = color.A
                        };
                    }, progressCallback);

                    // Konwersja z powrotem na Rgba32
                    for (int j = 0; j < rawData.Length; j++)
                    {
                        pixelSpan[j] = new Rgba32(rawData[j].R, rawData[j].G, rawData[j].B, rawData[j].A);
                    }
                }
            }
            
            static void ApplySepiaTone(List<ImSh.Image<ImSh.PixelFormats.Rgba32>> images, int width, int height)
            {
                for (int i = 0; i < images.Count; i++)
                {
                    var image = images[i];
                    image.DangerousTryGetSinglePixelMemory(out var pixelMemory);
                    var pixelSpan = pixelMemory.Span;

                    // Konwersja na MyColor[]
                    var rawData = new MyColor[width * height];
                    for (int j = 0; j < pixelSpan.Length; j++)
                    {
                        rawData[j] = new MyColor
                        {
                            R = pixelSpan[j].R,
                            G = pixelSpan[j].G,
                            B = pixelSpan[j].B,
                            A = pixelSpan[j].A
                        };
                    }

                    TryReportCallback progressCallback = progress =>
                    {
                        Console.WriteLine($"Applying sepia tone to image {i + 1}: {progress * 100:F2}%");
                        return !stopProcessing;
                    };

                    ProcessPixels_Custom(rawData, width, height, (x, y, color) =>
                    {
                        byte tr = (byte)Math.Min(255, (0.393 * color.R + 0.769 * color.G + 0.189 * color.B));
                        byte tg = (byte)Math.Min(255, (0.349 * color.R + 0.686 * color.G + 0.168 * color.B));
                        byte tb = (byte)Math.Min(255, (0.272 * color.R + 0.534 * color.G + 0.131 * color.B));
                        return new MyColor { R = tr, G = tg, B = tb, A = color.A };
                    }, progressCallback);

                    // Konwersja z powrotem na Rgba32
                    for (int j = 0; j < rawData.Length; j++)
                    {
                        pixelSpan[j] = new Rgba32(rawData[j].R, rawData[j].G, rawData[j].B, rawData[j].A);
                    }
                }
            }
        }
    }
}
