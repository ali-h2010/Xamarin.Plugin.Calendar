using System.Globalization;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SampleApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdvancedPage : ContentPage
    {
        public AdvancedPage()
        {
            InitializeComponent();

            var test = AdvanceCalendar;

            CultureInfo current = CultureInfo.CurrentCulture;
            CultureInfo clone = (CultureInfo)current.Clone();

            string[] CustomDayNames = { "Sun", "Mon", "Tue", "Wed", "Thur", "Fri", "Sat" };

            clone.DateTimeFormat.AbbreviatedDayNames = CustomDayNames;

            clone.DateTimeFormat.FirstDayOfWeek = System.DayOfWeek.Sunday;

            AdvanceCalendar.Culture = clone;//new System.Globalization.CultureInfo("ar-SY");
        }
    }
}
