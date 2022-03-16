using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Plugin.Calendar.Controls;
using Xamarin.Plugin.Calendar.Models;

namespace Xamarin.Plugin.Calendar.Shared.Controls.ViewLayoutEngines
{
    internal abstract class ViewLayoutBase
    {
        protected const int _numberOfDaysInWeek = 7;

        protected ViewLayoutBase(CultureInfo culture)
        {
            Culture = culture;
        }

        public CultureInfo Culture { get; set; }

        protected DateTime GetFirstDateOfWeek(DateTime dateInWeek)
        {
            var difference = (7 + (dateInWeek.DayOfWeek - Culture.DateTimeFormat.FirstDayOfWeek)) % 7;
            return dateInWeek.AddDays(-1 * difference).Date;
        }

        protected Grid GenerateWeekLayout(
                List<DayView> dayViews,
                double daysTitleHeight,
                Color daysTitleColor,
                Style daysTitleLabelStyle,
                double dayViewSize,
                ICommand dayTappedCommand,
                PropertyChangedEventHandler dayModelPropertyChanged,
                int numberOfWeeks
            )
        {
            var grid = new Grid
            {
                ColumnSpacing = 0d,
                RowDefinitions = new RowDefinitionCollection()
                {
                    new RowDefinition(){
                        Height = 1,
                    },
                    new RowDefinition(){
                        Height = daysTitleHeight
                    },
                     new RowDefinition(){
                        Height = 1,
                    },
                }
            };


            BoxView boxView_top_of_weekdays = new BoxView()
            {
                HeightRequest = 1,
                BackgroundColor = Color.FromHex("#ebebeb"),
               
            };

            Grid.SetRow(boxView_top_of_weekdays, 0);
            Grid.SetColumnSpan(boxView_top_of_weekdays, 7);

            grid.Children.Add(boxView_top_of_weekdays);


            for (int i = 0; i < _numberOfDaysInWeek; i++)
            {
                var label = new Label()
                {
                    TextColor = daysTitleColor,
                    TextTransform = TextTransform.None,
                    Style = daysTitleLabelStyle,
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalOptions = LayoutOptions.Center,
                    
                };
                Grid.SetRow(label, 1);
                Grid.SetColumn(label, i);

                grid.Children.Add(label);
            }


            BoxView boxView_bottom_of_weekdays = new BoxView()
            {
                HeightRequest = 1,
                BackgroundColor = Color.FromHex("#ebebeb"),

            };

            Grid.SetRow(boxView_bottom_of_weekdays, 2);
            Grid.SetColumnSpan(boxView_bottom_of_weekdays, 7);

            grid.Children.Add(boxView_bottom_of_weekdays);

            dayViews.Clear();

            for (int i = 3; i <= numberOfWeeks +2; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition()
                {
                    Height = dayViewSize,
                });

                for (int ii = 0; ii < 7; ii++)
                {
                    var dayView = new DayView();
                    var dayModel = new DayModel();

                    dayView.BindingContext = dayModel;
                    dayModel.DayTappedCommand = dayTappedCommand;
                    dayModel.PropertyChanged += dayModelPropertyChanged;

                    Grid.SetRow(dayView, i);
                    Grid.SetColumn(dayView, ii);

                    dayViews.Add(dayView);
                    grid.Children.Add(dayView);
                }
            }

            return grid;
        }
    }
}
