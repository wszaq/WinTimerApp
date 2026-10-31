using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Xunit;

namespace WindowsTimerApp.Tests;

public class MainWindowLayoutTests
{
    [Fact]
    public void TimerActionsFitInWindow() => MainWindowTests.OnUiThread(() =>
    {
        var window = new MainWindow(new TimerEngine(3600), () => { });
        window.Show();
        void CheckLayout()
        {
            window.UpdateLayout();
            foreach (var item in new FrameworkElement[] { window.Countdown, window.Minutes, window.Primary, window.CancelTimer })
            {
                if (!item.IsVisible) continue;
                var origin = item.TranslatePoint(new Point(), window);
                Assert.True(item.ActualWidth > 0 && item.ActualHeight > 0);
                Assert.InRange(origin.X + item.ActualWidth, 0, window.ActualWidth);
                Assert.InRange(origin.Y + item.ActualHeight, 0, window.ActualHeight);
            }
        }
        CheckLayout();
        var content = (FrameworkElement)window.Content;
        var bitmap = new RenderTargetBitmap((int)Math.Ceiling(content.ActualWidth), (int)Math.Ceiling(content.ActualHeight), 96, 96, PixelFormats.Pbgra32);
        bitmap.Render(content);
        var encoder = new PngBitmapEncoder();
        encoder.Frames.Add(BitmapFrame.Create(bitmap));
        var directory = Environment.GetEnvironmentVariable("TIMER_SCREENSHOT_DIR");
        if (directory != null)
        {
            Directory.CreateDirectory(directory);
            using var file = File.Create(Path.Combine(directory, "shutdown-timer.png"));
            encoder.Save(file);
        }
        MainWindowTests.Click(window.Primary);
        Assert.True(window.CancelTimer.IsVisible);
        CheckLayout();
        window.Close();
    });
}
