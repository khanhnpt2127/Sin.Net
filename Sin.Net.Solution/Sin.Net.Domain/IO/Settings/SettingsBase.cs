﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Sin.Net.Domain.IO.Settings
{
    /// <summary>
    /// The BaseSettings are for deriving settings for different IO functionality. The implementation follows in the persistence layer.
    /// </summary>
    public abstract class SettingsBase
    {
        /// <summary>
        /// Gets or sets the Name-property
        /// In case of file access it provides the file name
        /// </summary>
        public string Name { get; set; }
    }
}
