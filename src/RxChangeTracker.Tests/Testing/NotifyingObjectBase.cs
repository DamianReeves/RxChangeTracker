namespace RxChangeTracker.Testing {
    using System.ComponentModel;

    public class NotifyingObjectBase: INotifyPropertyChanged, INotifyPropertyChanging {
        public event PropertyChangedEventHandler PropertyChanged;
        public event PropertyChangingEventHandler PropertyChanging;

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e) {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null) handler(this, e);
        }

        protected virtual void OnPropertyChanging(PropertyChangingEventArgs e) {
            PropertyChangingEventHandler handler = this.PropertyChanging;
            if (handler != null) handler(this, e);
        }
    }
}