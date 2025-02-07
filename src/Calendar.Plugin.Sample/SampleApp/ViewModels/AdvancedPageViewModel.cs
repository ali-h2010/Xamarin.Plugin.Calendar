﻿using SampleApp.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Plugin.Calendar.Enums;
using Xamarin.Plugin.Calendar.Models;

namespace SampleApp.ViewModels
{
    public class AdvancedPageViewModel : BasePageViewModel
    {
        public ICommand DayTappedCommand => new Command<DateTime>(async (date) => await DayTapped(date));
        public ICommand SwipeLeftCommand => new Command(() => ChangeShownUnit(1));
        public ICommand SwipeRightCommand => new Command(() => ChangeShownUnit(-1));
        public ICommand SwipeUpCommand => new Command(() => { ShownDate = DateTime.Today; });

        public ICommand EventSelectedCommand => new Command(async (item) => await ExecuteEventSelectedCommand(item));

        public AdvancedPageViewModel() : base()
        {
            //Device.BeginInvokeOnMainThread(async () => await App.Current.MainPage.DisplayAlert("Info", "Loading events with delay, and changeing current view.", "Ok"));

            Culture = CultureInfo.CreateSpecificCulture("en-GB");
            // testing all kinds of adding events
            // when initializing collection

            Events = new EventCollection();
            Events = new EventCollection
            {
                [DateTime.Now.AddDays(3)] = new List<AdvancedEventModel>(GenerateEvents(10, "Cool")),
                [DateTime.Now.AddDays(-3)] = new List<AdvancedEventModel>(GenerateEvents(10, "Cool")),
                [DateTime.Now.AddDays(-6)] = new DayEventCollection<AdvancedEventModel>(Color.FromHex("#009fdf"), Color.FromHex("#009fdf"))
                {
                    new AdvancedEventModel { Name = "Cool event1", Description = "This is Cool event1's description!", Starting= new DateTime() },
                    new AdvancedEventModel { Name = "Cool event2", Description = "This is Cool event2's description!", Starting= new DateTime() }
                }
            };

            ////Adding a day with a different dot color
            //Events.Add(DateTime.Now.AddDays(-2), new DayEventCollection<AdvancedEventModel>(GenerateEvents(10, "Cool")) { EventIndicatorColor = Color.Blue, EventIndicatorSelectedColor = Color.Blue });
            //Events.Add(DateTime.Now.AddDays(-4), new DayEventCollection<AdvancedEventModel>(GenerateEvents(10, "Cool")) { EventIndicatorColor = Color.Green, EventIndicatorSelectedColor = Color.White });
            //Events.Add(DateTime.Now.AddDays(-5), new DayEventCollection<AdvancedEventModel>(GenerateEvents(10, "Cool")) { EventIndicatorColor = Color.Orange, EventIndicatorSelectedColor = Color.Orange });

            //// with add method
            //Events.Add(DateTime.Now.AddDays(-1), new List<AdvancedEventModel>(GenerateEvents(5, "Cool")));

            // with indexer
            //Events[DateTime.Now] = new List<AdvancedEventModel>(GenerateEvents(2, "Boring"));

            // ShownDate = ShownDate.AddMonths(1);

            // Task.Delay(5000).ContinueWith(_ =>
            //{
            //    // indexer - update later
            //    Events[DateTime.Now] = new ObservableCollection<AdvancedEventModel>(GenerateEvents(10, "Cool"));

            //    // add later
            //    Events.Add(DateTime.Now.AddDays(3), new List<AdvancedEventModel>(GenerateEvents(5, "Cool")));

            //    // indexer later
            //    Events[DateTime.Now.AddDays(10)] = new List<AdvancedEventModel>(GenerateEvents(10, "Boring"));

            //    // add later
            //    Events.Add(DateTime.Now.AddDays(15), new List<AdvancedEventModel>(GenerateEvents(10, "Cool")));

            //    Task.Delay(3000).ContinueWith(t =>
            //    {
            //        ShownDate = ShownDate.AddMonths(-2);

            //        // get observable collection later
            //        var todayEvents = Events[DateTime.Now] as ObservableCollection<AdvancedEventModel>;

            //        // insert/add items to observable collection
            //        todayEvents.Insert(0, new AdvancedEventModel { Name = "Cool event insert", Description = "This is Cool event's description!", Starting = new DateTime() });
            //        todayEvents.Add(new AdvancedEventModel { Name = "Cool event add", Description = "This is Cool event's description!", Starting = new DateTime() });
            //    }, TaskScheduler.FromCurrentSynchronizationContext());
            //}, TaskScheduler.FromCurrentSynchronizationContext());

            SelectedDate = DateTime.Today;//.AddDays(10);
        }

        private IEnumerable<AdvancedEventModel> GenerateEvents(int count, string name)
        {
            return Enumerable.Range(1, count).Select(x => new AdvancedEventModel
            {
                Name = $"{name} event{x}",
                Description = $"This is {name} event{x}'s description!",
                Starting = new DateTime(2000, 1, 1, (x * 2) % 24, (x * 3) % 60, 0)
            });
        }

        public EventCollection Events { get; }

        private DateTime _shownDate = DateTime.Today;

        public DateTime ShownDate
        {
            get => _shownDate;
            set => SetProperty(ref _shownDate, value);
        }

        private WeekLayout _calendarLayout = WeekLayout.Month;

        public WeekLayout CalendarLayout
        {
            get => _calendarLayout;
            set => SetProperty(ref _calendarLayout, value);
        }

        private DateTime? _selectedDate = DateTime.Today;

        public DateTime? SelectedDate
        {
            get => _selectedDate;
            set => SetProperty(ref _selectedDate, value);
        }

        private CultureInfo _culture = CultureInfo.InvariantCulture;

        public CultureInfo Culture
        {
            get => _culture;
            set => SetProperty(ref _culture, value);
        }

        private static async Task DayTapped(DateTime date)
        {
            //var message = $"Received tap event from date: {date}";
            //await App.Current.MainPage.DisplayAlert("DayTapped", message, "Ok");
        }

        private async Task ExecuteEventSelectedCommand(object item)
        {
            if (item is AdvancedEventModel eventModel)
            {
                var title = $"Selected: {eventModel.Name}";
                var message = $"Starts: {eventModel.Starting:HH:mm}{Environment.NewLine}Details: {eventModel.Description}";
                await App.Current.MainPage.DisplayAlert(title, message, "Ok");
            }
        }

        private void ChangeShownUnit(int amountToAdd)
        {
            switch (CalendarLayout)
            {
                case WeekLayout.Week:
                case WeekLayout.TwoWeek:
                    ChangeShownWeek(amountToAdd);
                    break;

                case WeekLayout.Month:
                default:
                    ChangeShownMonth(amountToAdd);
                    break;
            }
        }

        private void ChangeShownMonth(int monthsToAdd)
        {
            ShownDate.AddMonths(monthsToAdd);
        }

        private void ChangeShownWeek(int weeksToAdd)
        {
            ShownDate.AddDays(weeksToAdd * 7);
        }
    }
}
