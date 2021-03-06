﻿namespace HelloDirectInk
{
  using System.Collections.Generic;
  using System.Linq;
  using Windows.UI.Xaml.Controls;

  public sealed partial class MainPage : Page
  {
    void AddStrokeEventHandlers()
    {
      this.inkPresenter.StrokesCollected += (s, e) =>
      {
        this.UpdateStrokesView();
      };
      this.inkPresenter.StrokesErased += (s, e) =>
      {
        this.UpdateStrokesView();
      };
      this.inkPresenter.StrokeInput.StrokeStarted += (s, e) =>
      {
        this.txtStroking.Text = "stroke in progress";
      };
      this.inkPresenter.StrokeInput.StrokeEnded += (s, e) =>
      {
        this.txtStroking.Text = string.Empty;
      };
    }
    void UpdateStrokesView()
    {
      this.txtStrokeCount.Text = 
        $"{this.inkStrokeContainer.GetStrokes().Count} strokes";

      this.lstStrokes.ItemsSource = this.BuildStrokeDescriptions();
      this.HighlightSelectedStrokes();
      this.DrawRecognisedStrokes();
    }
    IReadOnlyList<string> BuildStrokeDescriptions()
    {
      return (this.inkStrokeContainer.GetStrokes().Select(
        (stroke, i) =>
        {
          var points = stroke.GetInkPoints().Count;
          var segments = stroke.GetRenderingSegments().Count;
          var attr = stroke.DrawingAttributes;

          return (
            $"stroke {i}: [{stroke.BoundingRect}], [{points}] points, [{segments}] segments, " +
            $"recognised? [{stroke.Recognized}], selected? [{stroke.Selected}], " +
            $"color [{attr.Color}], pen tip [{attr.PenTip}], fit curve? [{attr.FitToCurve}], " +
            $"ignore pressure? [{attr.IgnorePressure}]");
        }
      ).ToList().AsReadOnly());
    }
  }
}
