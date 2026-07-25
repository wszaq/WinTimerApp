using System;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsTimerApp;

public class MainForm : Form
{
    private readonly TimerEngine _engine;
    private readonly TrayController _trayController;
    private readonly Label _timeLabel;
    private readonly Button _startButton;
    private readonly Button _pauseButton;
    private readonly Button _resetButton;

    private Point _dragStart = Point.Empty;

    public MainForm(TimerEngine engine, TrayController trayController)
    {
        _engine = engine;
        _trayController = trayController;

        this.Text = "Windows Timer";
        this.FormBorderStyle = FormBorderStyle.None;
        this.TopMost = true;
        this.Size = new Size(400, 300);
        this.StartPosition = FormStartPosition.CenterScreen;
        this.BackColor = Color.FromArgb(45, 45, 48);

        _timeLabel = new Label
        {
            Dock = DockStyle.Fill,
            ForeColor = Color.White,
            TextAlign = ContentAlignment.MiddleCenter,
            Font = new Font("Segoe UI", 48, FontStyle.Bold)
        };

        _startButton = new Button { Text = "Start", Dock = DockStyle.Bottom, Height = 60, FlatStyle = FlatStyle.Flat, ForeColor = Color.White };
        _pauseButton = new Button { Text = "Pause", Dock = DockStyle.Bottom, Height = 60, FlatStyle = FlatStyle.Flat, ForeColor = Color.White };
        _resetButton = new Button { Text = "Reset", Dock = DockStyle.Bottom, Height = 60, FlatStyle = FlatStyle.Flat, ForeColor = Color.White };

        _startButton.Click += (s, e) => { _engine.Start(); UpdateDisplay(); };
        _pauseButton.Click += (s, e) => { _engine.Pause(); UpdateDisplay(); };
        _resetButton.Click += (s, e) => { _engine.Reset(10); UpdateDisplay(); };

        this.Controls.Add(_timeLabel);
        this.Controls.Add(_startButton);
        this.Controls.Add(_pauseButton);
        this.Controls.Add(_resetButton);

        // Dragging implementation
        this.MouseDown += (s, e) => {
            if (e.Button == MouseButtons.Left)
            {
                _dragStart = e.Location;
            }
        };
        this.MouseMove += (s, e) => {
            if (e.Button == MouseButtons.Left && _dragStart != Point.Empty)
            {
                this.Location = new Point(this.Location.X + e.X - _dragStart.X, this.Location.Y + e.Y - _dragStart.Y);
            }
        };
        this.MouseUp += (s, e) => { _dragStart = Point.Empty; };

        // Wiring up engine events
        _engine.OnTick += UpdateDisplay;
        _engine.OnTimerFinished += OnFinished;

        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        string text = $"{_engine.RemainingSeconds}s";
        _timeLabel.Text = text;
        _trayController.UpdateText($"Timer: {text}");
        System.Diagnostics.Debug.WriteLine($"UI Update: {text}");
    }

    private void OnFinished()
    {
        _timeLabel.ForeColor = Color.Red;
        _trayController.UpdateText("TIME IS UP!");
        MessageBox.Show("Time is up!", "Timer", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }
}
