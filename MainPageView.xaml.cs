// <copyright file="MainPageView.xaml.cs" company="TestCompany">
// Copyright (c) TestCompany. All rights reserved.
// </copyright>

namespace SimpleToDo
{
    using System;
    using System.ComponentModel;
    using System.Threading.Tasks;

    using Windows.UI.Xaml.Controls;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainPage"/> class.
        /// </summary>
        public MainPage()
        {
            this.InitializeComponent();

            this.viewModel.PropertyChanged += this.OnViewModelPropertyChanged;
        }

        private async void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(MainPageViewModel.IsSearching))
            {
                // Delay necessary for the UI thread to pick up the Focus call.
                await Task.Delay(10);

                this.SearchTextBox.Focus(Windows.UI.Xaml.FocusState.Programmatic);
            }
        }
    }
}
