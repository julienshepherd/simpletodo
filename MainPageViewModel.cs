// <copyright file="MainPageViewModel.cs" company="TestCompany">
// Copyright (c) TestCompany. All rights reserved.
// </copyright>

namespace SimpleToDo
{
    using System;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;

    using Realms;

    using Windows.ApplicationModel;
    using Windows.UI.Core;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;

    /// <summary>
    /// Contains the MainPage view logic.
    /// </summary>
    internal class MainPageViewModel : NotificationObject
    {
        private Realm realm;
        private Transaction editTransaction;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainPageViewModel"/> class.
        /// </summary>
        public MainPageViewModel()
        {
            Application.Current.EnteredBackground += (s, e) => this.SaveEdits();
            Application.Current.LeavingBackground += (s, e) => this.EnsureEditTransaction();

            this.realm = Realm.GetInstance();

            foreach (var todo in this.realm.All<ToDo>())
            {
                this.ToDos.Add(todo);
            }

            this.editTransaction = this.realm.BeginWrite();
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
                var toDo = new ToDo()
                {
                    Details = dialog.ViewModel.ToDoText,
                };

                this.realm.Write(() =>
                {
                    this.realm.Add(toDo);
                });

                this.ToDos.Insert(0, toDo);

                this.RaisePropertyChanged(nameof(this.HasToDos));
                this.RaisePropertyChanged(nameof(this.IsToDoListEmpty));
            }

            // Start a new transaction to allow changes in the done state of to-dos.
            this.editTransaction = this.realm.BeginWrite();
        }
        private void EnsureEditTransaction()
        {
            if (this.editTransaction is null)
            {
                this.editTransaction = this.realm.BeginWrite();
            }
        }

        private void SaveEdits()
        {
            this.editTransaction.Commit();
        }
    }
}
