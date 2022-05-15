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

        /// <summary>
        /// Sets the value of a property and fires <see cref="PropertyChanged"/> if the value is different than the existing one.
        /// </summary>
        /// <typeparam name="T">The type of the property value to set.</typeparam>
        /// <param name="storage">The existing value of the property.</param>
        /// <param name="value">The new value of the property.</param>
        /// <param name="propertyName">The name of the property to set.</param>
        /// <returns><c>True</c> if the property has been changed; otherwise <c>false</c>.</returns>
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