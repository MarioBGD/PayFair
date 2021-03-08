using PayFair.Mobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PayFair.Mobile.Models
{
    public class GroupModel : BaseViewModel
    {
        public delegate Task GroupButtonClickDel(GroupModel person);
        public GroupButtonClickDel OnNegativeButtonClick;

        public int Id { get; set; }
        public string Name { get; set; }
        public string DefaultCurrency { get; set; }
        public DateTime CreatedDate { get; set; }

        public Command PositiveCommand { get; }
        public Command NegativeCommand { get; }

        public GroupModel()
        {

        }
        public GroupModel(GroupButtonClickDel positive, GroupButtonClickDel negative)
        {
            NegativeCommand = new Command(async () => await negative(this), () => !IsBusy);
            PositiveCommand = new Command(async () => await positive(this), () => !IsBusy);
        }
    }
}
