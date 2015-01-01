using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace XSYCollegeEnglishVocabulary.DataModel
{
    public class Vocabulary : INotifyPropertyChanged
    {
        public string Word { get; set; }
        public string Voice { get; set; }
        public ICollection<Definition> Definitions { get; set; }

        private bool _definitionVisible;
        public bool DefinitionVisible
        {
            get { return _definitionVisible; }
            set { this.SetProperty(ref this._definitionVisible, value); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] String propertyName = null)
        {
            if (object.Equals(storage, value))
            {
                return false;
            }
            storage = value;
            this.OnPropertyChanged(propertyName);
            return true;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var eventHandler = this.PropertyChanged;
            if (eventHandler != null)
            {
                eventHandler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
