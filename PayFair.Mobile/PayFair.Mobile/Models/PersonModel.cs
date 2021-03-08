using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PayFair.Mobile.Models
{
    public class PersonModel : ViewModels.BaseViewModel
    {
        public delegate Task PersonButtonClickDel(PersonModel person);
        public PersonButtonClickDel OnPositiveButtonClick;
        public PersonButtonClickDel OnNegativeButtonClick;

        private bool isCheckded;

        public int UserId { get; set; }
        public string Name { get; set; }

        public bool IsChecked
        {
            get => isCheckded;
            set => SetProperty(ref isCheckded, value);
        }

        public Command PositiveCommand { get; }
        public Command NegativeCommand { get; }

        public PersonModel()
        {
            
        }
        public PersonModel(PersonButtonClickDel positive, PersonButtonClickDel negative)
        {
            PositiveCommand = new Command(async () => await positive(this), () => !IsBusy);
            NegativeCommand = new Command(async () => await negative(this), () => !IsBusy);
        }
    }
}
