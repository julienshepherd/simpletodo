// <copyright file="MainPageViewModel.cs" company="TestCompany">
// Copyright (c) TestCompany. All rights reserved.
// </copyright>

namespace SimpleToDo
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;

    using Realms;

    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;

    /// <summary>
    /// Contains the MainPage view logic.
    /// </summary>
    internal class MainPageViewModel : NotificationObject
    {
        private readonly Realm realm;
        private Transaction editTransaction;
        private IDisposable toDosNotificationToken;
        private bool isAppInForeground;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainPageViewModel"/> class.
        /// </summary>
        public MainPageViewModel()
        {
            Application.Current.EnteredBackground += (s, e) => this.OnEnteredBackground();
            Application.Current.LeavingBackground += (s, e) => this.OnLeavingBackground();

            this.realm = Realm.GetInstance();

            foreach (var todo in this.realm.All<ToDo>())
            {
                this.ToDos.Insert(0, todo);
            }

            this.OnLeavingBackground();
        }

        /// <summary>
        /// Gets the collection of all to-dos ever added.
        /// </summary>
        public ObservableCollection<ToDo> ToDos { get; } = new ObservableCollection<ToDo>();

        /// <summary>
        /// Gets a value indicating whether there there are no to-dos.
        /// </summary>
        public bool IsToDoListEmpty => this.ToDos.Count == 0;

        /// <summary>
        /// Gets a value indicating whether there is at least one to-do.
        /// </summary>
        public bool HasToDos => this.ToDos.Count > 0;

        /// <summary>
        /// Shows a dialog prompting the user to add a new to-do.
        /// </summary>
        /// <returns>The asynchronous operation.</returns>
        public async Task ShowAddToDoDialogAsync()
        {
            var dialog = new AddToDoDialog();

            // Save any changes to the done state, prevent two transactions at the same time.
            this.editTransaction.Commit();

            var result = await dialog.ShowAsync();

            if (result is ContentDialogResult.Primary)
            {
                this.realm.Write(() =>
                {
                    var toDo = new ToDo()
                    {
                        Details = dialog.ViewModel.ToDoText,
                    };

                    this.realm.Add(toDo);
                });
            }

            // Start a new transaction to allow changes in the done state of to-dos.
            this.editTransaction = this.realm.BeginWrite();
        }

        /// <summary>
        /// Removes all checked to-dos from storage.
        /// </summary>
        public void RemoveDoneToDos()
        {
            this.editTransaction.Commit();

            var toDosToRemove = this.realm.All<ToDo>().Where(toDo => toDo.IsDone);
            this.realm.Write(() =>
            {
                foreach (var toDo in toDosToRemove)
                {
                    this.realm.Remove(toDo);
                }
            });

            this.editTransaction = this.realm.BeginWrite();
        }

        private void SaveEdits()
        {
            this.editTransaction.Commit();
        }

        private void OnEnteredBackground()
        {
            this.SaveEdits();
            this.toDosNotificationToken.Dispose();
            this.isAppInForeground = false;
        }

        private void OnLeavingBackground()
        {
            if (this.isAppInForeground)
            {
                return;
            }

            this.isAppInForeground = true;

            this.editTransaction = this.realm.BeginWrite();

            this.toDosNotificationToken = this.realm.All<ToDo>().SubscribeForNotifications(this.OnToDosChangedInStorage);
        }

        private void OnToDosChangedInStorage(IRealmCollection<ToDo> sender, ChangeSet changes, Exception error)
        {
            if (changes is null)
            {
                // First time notification
                return;
            }

            if (changes.DeletedIndices.Any())
            {
                var toDosToRemove = new List<ToDo>();
                var highestIndex = this.ToDos.Count - 1;

                foreach (var deletedIndex in changes.DeletedIndices)
                {
                    var reversedIndex = highestIndex - deletedIndex;
                    toDosToRemove.Add(this.ToDos.ElementAt(reversedIndex));
                }

                foreach (var toDoToRemove in toDosToRemove)
                {
                    this.ToDos.Remove(toDoToRemove);
                }
            }

            foreach (var addedIndex in changes.InsertedIndices)
            {
                this.ToDos.Insert(0, sender.ElementAt(addedIndex));
            }

            this.RaisePropertyChanged(nameof(this.HasToDos));
            this.RaisePropertyChanged(nameof(this.IsToDoListEmpty));
        }
    }
}
