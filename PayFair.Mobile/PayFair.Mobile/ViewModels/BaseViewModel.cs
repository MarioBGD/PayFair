using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace PayFair.Mobile.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public enum ViewResult
        { Positive, Negative }

        public ViewResult Result { get; set; } = ViewResult.Positive;

        bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                SetProperty(ref isBusy, value);
                OnIsBusyChanged();
            }
        }

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        public virtual void OnIsBusyChanged()
        {

        }

        public delegate void OnPopedDel(object result);
        public OnPopedDel OnPoped { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged == null)
                return;

            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
