// <copyright file="MainPageViewModel.cs" company="TestCompany">
// Copyright (c) TestCompany. All rights reserved.
// </copyright>

namespace SimpleToDo
{
    using System;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;

    using Windows.UI.Core;
    using Windows.UI.Xaml.Controls;

    /// <summary>
    /// Contains the MainPage view logic.
    /// </summary>
    internal class MainPageViewModel : NotificationObject
    {
        private readonly CoreDispatcher dispatcher;

        /// <summary>
        /// Gets the collection of all to-dos ever added.
        /// </summary>
        public ObservableCollection<ToDo> ToDos { get; } = new ObservableCollection<ToDo>();

        /// <summary>
        /// Gets a value indicating whether there is at least one to-do in the list.
        /// </summary>
        public bool IsToDoListEmpty => this.ToDos.Count == 0;

        public bool HasToDos => this.ToDos.Count > 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainPageViewModel"/> class.
        /// </summary>
        public MainPageViewModel()
        {
            // View model is constructed by the view, so we're in the UI thread, we can grab the dispatcher.
            this.dispatcher = CoreWindow.GetForCurrentThread().Dispatcher;
        }

        /// <summary>
        /// Shows a dialog prompting the user to add a new to-do.
        /// </summary>
        /// <returns>The asynchronous operation.</returns>
        public async Task ShowAddToDoDialogAsync()
        {
            var dialog = new AddToDoDialog();

            var result = await dialog.ShowAsync().AsTask().ConfigureAwait(false);

            if (result is ContentDialogResult.Primary)
            {
                var toDo = new ToDo()
                {
                    Details = dialog.ViewModel.ToDoText,
                };

                await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => this.AddToDo(toDo))
                    .AsTask().ConfigureAwait(false);
            }
        }

        private void AddToDo(ToDo toDo)
        {
            this.ToDos.Add(toDo);

            this.RaisePropertyChanged(nameof(this.HasToDos));
            this.RaisePropertyChanged(nameof(this.IsToDoListEmpty));
        }
    }
}
