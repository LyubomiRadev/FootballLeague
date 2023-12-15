using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;

using System.Collections.Generic;

using System;
using System.Reflection.Metadata;

namespace FootballLeaguesStandingsTableProject.Views
{
	public partial class ChartUserControlView : UserControl
	{
		#region Dependency Properties

		#region SectionsNumber

		public static readonly StyledProperty<FootballTeamInfoModel> SectionsNumberProperty =
			AvaloniaProperty.Register<ChartUserControlView, FootballTeamInfoModel>(nameof(SectionsNumber), null);

		public FootballTeamInfoModel SectionsNumber
		{
			get => GetValue(SectionsNumberProperty);
			set => SetValue(SectionsNumberProperty, value);
		}

		#endregion

		#endregion

		private List<(GradientBrush Gradient, string Info)> sections = new List<(GradientBrush Gradient, string Info)>();

		private TextBlock tooltipTextBlock;

		// Tooltip for displaying information
		private ToolTip tooltip = new ToolTip();
		public ChartUserControlView()
		{
			InitializeComponent();
		}

		protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
		{
			base.OnPropertyChanged(change);

			if (change.NewValue is FootballTeamInfoModel)
			{
				this.SectionsNumber = change.NewValue as FootballTeamInfoModel;

				this.InitializeComponent();
			}
		}

		private void InitializeComponent()
		{
			AvaloniaXamlLoader.Load(this);

			if (this.SectionsNumber != null)
			{
				sections = new List<(GradientBrush, string)> { };

				DrawCircularChart();

			}
		}
		private void DrawCircularChart()
		{
			var canvas = this.FindControl<Canvas>("canvas");

			var center = new Point(200, 200);
			var radius = 150;
			var totalGames = this.SectionsNumber.GamesData.GamesLost + this.SectionsNumber.GamesData.GamesDrawn + this.SectionsNumber.GamesData.GamesWon;

			double lostAngle = 360.0 * this.SectionsNumber.GamesData.GamesLost / totalGames;
			double drawnAngle = 360.0 * this.SectionsNumber.GamesData.GamesDrawn / totalGames;
			double wonAngle = 360.0 * this.SectionsNumber.GamesData.GamesWon / totalGames;


			if (this.SectionsNumber.GamesData.GamesLost > 0)
			{
				var lostPath = CreatePieSlice(center, radius, 0, lostAngle, CreateGradientBrush(Colors.Red, Colors.Orange));
				lostPath.PointerEntered += (sender, e) => OnSliceHoverEnter(lostPath, $"Games lost: {this.SectionsNumber.GamesData.GamesLost}");
				lostPath.PointerExited += (sender, e) => OnSliceHoverLeave(lostPath);
				canvas.Children.Add(lostPath);
			}

			if (this.SectionsNumber.GamesData.GamesDrawn > 0)
			{
				var drawnPath = CreatePieSlice(center, radius, lostAngle, drawnAngle, CreateGradientBrush(Colors.Gray, Colors.DarkGray));
				drawnPath.PointerEntered += (sender, e) => OnSliceHoverEnter(drawnPath, $"Games drawn: {this.SectionsNumber.GamesData.GamesDrawn}");
				drawnPath.PointerExited += (sender, e) => OnSliceHoverLeave(drawnPath);
				canvas.Children.Add(drawnPath);
			}

			if (this.SectionsNumber.GamesData.GamesWon > 0)
			{
				var wonPath = CreatePieSlice(center, radius, lostAngle + drawnAngle, wonAngle, CreateGradientBrush(Colors.Green, Colors.Yellow));
				wonPath.PointerEntered += (sender, e) => OnSliceHoverEnter(wonPath, $"Games won: {this.SectionsNumber.GamesData.GamesWon}");
				wonPath.PointerExited += (sender, e) => OnSliceHoverLeave(wonPath);
				canvas.Children.Add(wonPath);
			}

			// Create a TextBlock for tooltips
			tooltipTextBlock = new TextBlock
			{
				FontSize = 12,
				Foreground = Brushes.Black,
			};

			// Add the TextBlock to the canvas
			canvas.Children.Add(tooltipTextBlock);

			var centralCircleGeometry = new EllipseGeometry(new Rect(center.X - 75, center.Y - 75, 150, 150));
			var centralCirclePath = new Avalonia.Controls.Shapes.Path
			{
				Data = centralCircleGeometry,
				Fill = new SolidColorBrush(Colors.White)
			};

			canvas.Children.Add(centralCirclePath);
		}



		private Avalonia.Controls.Shapes.Path CreatePieSlice(Point center, double radius, double startAngle, double sweepAngle, GradientBrush fillBrush)
		{
			var pathGeometry = new PathGeometry();
			var figure = new PathFigure
			{
				IsClosed = true,
				StartPoint = center,
			};

			var radiansPerDegree = Math.PI / 180.0;
			var startPoint = new Point(center.X + radius * Math.Cos(startAngle * radiansPerDegree), center.Y + radius * Math.Sin(startAngle * radiansPerDegree));

			figure.Segments.Add(new LineSegment { Point = startPoint });
			figure.Segments.Add(new ArcSegment
			{
				Size = new Size(radius, radius),
				Point = new Point(center.X + radius * Math.Cos((startAngle + sweepAngle) * radiansPerDegree), center.Y + radius * Math.Sin((startAngle + sweepAngle) * radiansPerDegree)),
				IsLargeArc = sweepAngle > 180,
				SweepDirection = SweepDirection.Clockwise,
			});

			pathGeometry.Figures.Add(figure);

			var path = new Avalonia.Controls.Shapes.Path
			{
				Data = pathGeometry,
				Fill = fillBrush,
				Opacity = 1.0,
			};

			return path;
		}

		private LinearGradientBrush CreateGradientBrush(Color color1, Color color2)
		{
			var gradientStops = new GradientStops
			{
				new GradientStop(color1, 0.0),
				new GradientStop(color2, 1.0),
			};

			return new LinearGradientBrush
			{
				GradientStops = gradientStops,
				StartPoint = new RelativePoint(0, 0, RelativeUnit.Relative),
				EndPoint = new RelativePoint(1, 0, RelativeUnit.Relative),
				SpreadMethod = GradientSpreadMethod.Pad,
			};
		}

		private void OnSliceHoverEnter(Avalonia.Controls.Shapes.Path path, string content)
		{
			path.Opacity = 0.7;
			// Show a tooltip for the hovered slice
			if (tooltipTextBlock != null)
			{
				tooltipTextBlock.Text = content;
			}
		}

		private void OnSliceHoverLeave(Avalonia.Controls.Shapes.Path path)
		{
			path.Opacity = 1.0;
			// Remove the tooltip when leaving the slice
			// Show a tooltip for the hovered slice
			if (tooltipTextBlock != null)
			{
				tooltipTextBlock.Text = string.Empty;
			}
		}
	}
}
