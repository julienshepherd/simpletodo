// <copyright file="ToDo.cs" company="TestCompany">
// Copyright (c) TestCompany. All rights reserved.
// </copyright>

namespace SimpleToDo
{
    /// <summary>
    /// Represents one task in a to-do list.
    /// </summary>
    internal class ToDo
    {
        /// <summary>
        /// Gets or sets the content of the to-do.
        /// </summary>
        public string Details { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the to-do is complete.
        /// </summary>
        public bool IsDone { get; set; }
    }
}
