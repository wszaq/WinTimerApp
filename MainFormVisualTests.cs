using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Xunit;

namespace WindowsTimerApp.Tests;

public class MainFormVisualTests
{
    private readonly string _baseDir;
    private readonly string _baselinePath;
    private readonly string _currentPath;

    public MainFormVisualTests()
    {
        // Use a path relative to the project root for easier access
        _baseDir = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "test_artifacts/visual_regression"));
        _baselinePath = Path.Combine(_baseDir, "baseline/golden.png");
        _currentPath = Path.Combine(_baseDir, "current/latest.png");
    }

    [Fact]
    public void MainForm_VisualRegression_Check()
    {
        // Arrange
        var engine = new TimerEngine(10, new StateManager());
        var tray = new TrayController();
        var form = new MainForm(engine, tray);

        // Act: Capture the UI
        CaptureFormScreenshot(form, _currentPath);

        // Assert
        if (!File.Exists(_baselinePath))
        {
            // First run: Create the baseline from the current capture
            Directory.CreateDirectory(Path.GetDirectoryName(_baselinePath)!);
            File.Copy(_currentPath, _baselinePath, true);
        }
        else
        {
            // Subsequent runs: Compare current against baseline
            var difference = CalculatePixelDifference(_baselinePath, _currentPath);

            // Assert that the difference is below a certain threshold (e.g., 0.1%)
            Assert.True(difference < 0.001, $"Visual regression detected! Difference: {difference:P2}");
        }
    }

    private void CaptureFormScreenshot(Form form, string filePath)
    {
        // Ensure directory exists
        var dir = Path.GetDirectoryName(filePath);
        if (!string.IsNullOrEmpty(dir)) Directory.CreateDirectory(dir);

        // Use DrawToBitmap for WinForms capture without needing window focus/interaction
        using (var bitmap = new Bitmap(form.Width, form.Height))
        {
            form.DrawToBitmap(bitmap, new Rectangle(0, 0, form.Width, form.Height));
            bitmap.Save(filePath, System.Drawing.Imaging.ImageFormat.Png);
        }
    }

    private double CalculatePixelDifference(string baselinePath, string currentPath)
    {
        using (var bitmapBaseline = new Bitmap(baselinePath))
        using (var bitmapCurrent = new Bitmap(currentPath))
        {
            if (bitmapBaseline.Size != bitmapCurrent.Size)
            {
                // If dimensions changed, the difference is effectively 100%
                return 1.0;
            }

            int diffPixels = 0;
            int totalPixels = bitmapBaseline.Width * bitmapBaseline.Height;

            for (int x = 0; x < bitmapBaseline.Width; x++)
            {
                for (int y = 0; y < bitmapBaseline.Height; y++)
                {
                    Color pixelBaseline = bitmapBaseline.GetPixel(x, y);
                    Color pixelCurrent = bitmapCurrent.GetPixel(x, y);

                    if (pixelBaseline.R != pixelCurrent.R ||
                        pixelBaseline.G != pixelCurrent.G ||
                        pixelBaseline.B != pixelCurrent.B)
                    {
                        diffPixels++;
                    }
                }
            }

            return (double)diffPixels / totalPixels;
        }
    }
}
