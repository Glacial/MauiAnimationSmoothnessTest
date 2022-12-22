using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace MauiAnimationRateTest2;

public partial class MainPage : ContentPage
{
	int count = 0;

    private Stopwatch _frameStopwatch = new Stopwatch();

    private long _lastProgressEventTicks = 0;
    private readonly List<float> _lastHundredProgressEventTimeValues = new(100);
    private float _lastHundredProgressEventTimeAverage = 0f;
    private float _lastHundredProgressEventTimeHigh = 0f;
    private float _lastHundredProgressEventTimeLow = 0f;


    //Dummy property for animation timing purposes
    public BindableProperty ProgressFilledProperty =
        BindableProperty.Create(nameof(ProgressFilled), typeof(float), typeof(ProgressBar), 0f);

    public float ProgressFilled
    {
        get => (float)GetValue(ProgressFilledProperty);
        set => SetValue(ProgressFilledProperty, value);
    }


    //Property for animation stats
    public BindableProperty AnimationDeltaStringProperty =
        BindableProperty.Create(nameof(AnimationDeltaString), typeof(string), typeof(MainPage), "");

    public string AnimationDeltaString
    {
        get => (string)GetValue(AnimationDeltaStringProperty);
        set => SetValue(AnimationDeltaStringProperty, value);
    }



	public MainPage()
	{
		InitializeComponent();
        this.BindingContext = this;
    }


	private void OnCounterClicked(object sender, EventArgs e)
	{
		count++;

		if (count == 1)
			CounterBtn.Text = $"Clicked {count} time";
		else
			CounterBtn.Text = $"Clicked {count} times";

		SemanticScreenReader.Announce(CounterBtn.Text);
	}


	private void ContentPage_Loaded(object sender, EventArgs e)
    {
        _frameStopwatch.Start();
        Animation progressAnimation = new Microsoft.Maui.Controls.Animation(v =>
        {
            ProgressFilled = (float)v;
            AnimatingBorder.TranslationX += 2;
            if (AnimatingBorder.TranslationX > this.Width)
                AnimatingBorder.TranslationX = 0;
            AnimatingBorder2.TranslationX += 4;
            if (AnimatingBorder2.TranslationX > this.Width)
                AnimatingBorder2.TranslationX = 0;
        }, 0, 1, easing: Easing.Linear);
        progressAnimation.Commit(this, "ProgressAnimationName", length: 10000, finished: (l, c) => progressAnimation = null, repeat: () => IsLoaded);
    }

    protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        if (propertyName == nameof(ProgressFilled))
        {
            var ticks = _frameStopwatch.ElapsedTicks;

            var elapsed = ticks - _lastProgressEventTicks;
            _lastHundredProgressEventTimeValues.Add((((float)elapsed) / Stopwatch.Frequency) * 1000);
            if (_lastHundredProgressEventTimeValues.Count > 100)
                _lastHundredProgressEventTimeValues.RemoveAt(0);
            _lastHundredProgressEventTimeAverage = _lastHundredProgressEventTimeValues.Average();
            _lastHundredProgressEventTimeLow = _lastHundredProgressEventTimeValues.Min();
            _lastHundredProgressEventTimeHigh = _lastHundredProgressEventTimeValues.Max();
            _lastProgressEventTicks = ticks;

            AnimationDeltaString = $"{_lastHundredProgressEventTimeAverage:##0.00}, Lo:{_lastHundredProgressEventTimeLow:##0.00}. Hi:{_lastHundredProgressEventTimeHigh:##0.00}";
        }

        base.OnPropertyChanged(propertyName);
    }

}

