// <copyright file="NotificationObject.cs" company="TestCompany">
// Copyright (c) TestCompany. All rights reserved.
// </copyright>

namespace SimpleToDo
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Provides property change notifications.
    /// </summary>
    internal class NotificationObject : INotifyPropertyChanged
    {
        /// <summary>
        /// Invoked after a property has changed its value;
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = "")
        {
            if (!object.Equals(storage, value))
            {
                storage = value;
                this.RaisePropertyChanged(propertyName);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event for the given <paramref name="propertyName"/>.
        /// </summary>
        /// <param name="propertyName">Identifies the property which has changed its value.</param>
        protected void RaisePropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}