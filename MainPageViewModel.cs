// <copyright file="MainPageViewModel.cs" company="TestCompany">
// Copyright (c) TestCompany. All rights reserved.
// </copyright>

namespace SimpleToDo
{
    using System;
    using System.Threading.Tasks;

    using Windows.UI.Xaml.Controls;

    /// <summary>
    /// Contains the MainPage view logic.
    /// </summary>
    internal class MainPageViewModel
    {
        /// <summary>
        /// Shows a dialog prompting the user to add a new to-do.
        /// </summary>
        /// <returns>The asynchronous operation.</returns>
        public async Task ShowAddToDoDialogAsync()
        {
            var dialog = new ContentDialog()
            {
                CloseButtonText = "Add",
            };

            await dialog.ShowAsync().AsTask().ConfigureAwait(false);
        }
    }
}
