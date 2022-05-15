// <copyright file="AddToDoDialog.xaml.cs" company="TestCompany">
// Copyright (c) TestCompany. All rights reserved.
// </copyright>
// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace SimpleToDo
{
    using Windows.UI.Xaml.Controls;

    /// <summary>
    /// The modal view which prompts the user to add a new to-do.
    /// </summary>
    public sealed partial class AddToDoDialog : ContentDialog
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AddToDoDialog"/> class.
        /// </summary>
        public AddToDoDialog()
        {
            this.InitializeComponent();
        }
    }
}
