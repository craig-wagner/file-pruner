#region using
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
#endregion using

namespace Wagner.FilePruner
{
    /// <summary>
    /// This structure is intended to hold one instance of a directory to prune
    /// from the application configuration file. See the app.config file for a
    /// description of each entry.
    /// </summary>
    public sealed class DirectoriesElement : ConfigurationElement
    {
        /// <summary>
        /// Gets or sets the name of the root directory to purge of old files.
        /// </summary>
        /// <value>
        /// The root directory can be identified by a UNC or a drive-letter 
        /// path.
        /// </value>
        [ConfigurationProperty( "name", IsKey = true, IsRequired = true )]
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }

        /// <summary>
        /// Gets or sets a flag indicating whether the process should traverse
        /// subdirectories of the root directory.
        /// </summary>
        /// <value>
        /// true if the subdirectories of the root directory should be cleaned 
        /// as well; false if only the root directory should be cleaned.
        /// </value>
        [ConfigurationProperty( "recursive" )]
        public bool Recursive
        {
            get { return (bool)this["recursive"]; }
            set { this["recursive"] = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether empty directories should be 
        /// removed.
        /// </summary>
        /// <value>
        /// true if empty directories should be removed; false if empty 
        /// directories should be left alone.
        /// </value>
        [ConfigurationProperty( "removeEmptySubdirectories" )]
        public bool RemoveEmptySubdirectories
        {
            get { return (bool)this["removeEmptySubdirectories"]; }
            set { this["removeEmptySubdirectories"] = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether hidden directories
        /// should be processed or skipped.
        /// </summary>
        /// <value>
        /// true if hidden directories should be skipped; false if hidden
        /// directories should be processed.
        /// </value>
        [ConfigurationProperty( "skipHiddenDirectories" )]
        public bool SkipHiddenDirectories
        {
            get { return (bool)this["skipHiddenDirectories"]; }
            set { this["skipHiddenDirectories"] = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether read-only directories
        /// should be processed or skipped.
        /// </summary>
        /// <value>
        /// true if read-only directories should be skipped; false if read-only
        /// directories should be processed.
        /// </value>
        [ConfigurationProperty( "skipReadOnlyDirectories" )]
        public bool SkipReadOnlyDirectories
        {
            get { return (bool)this["skipReadOnlyDirectories"]; }
            set { this["skipReadOnlyDirectories"] = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether system directories
        /// should be processed or skipped.
        /// </summary>
        /// <value>
        /// true if system directories should be skipped; false if system
        /// directories should be processed.
        /// </value>
        [ConfigurationProperty( "skipSystemDirectories" )]
        public bool SkipSystemDirectories
        {
            get { return (bool)this["skipSystemDirectories"]; }
            set { this["skipSystemDirectories"] = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether hidden files should be 
        /// processed or skipped.
        /// </summary>
        /// <value>
        /// true if hidden files should be skipped; false if hidden files 
        /// should be processed.
        /// </value>
        [ConfigurationProperty( "skipHiddenFiles" )]
        public bool SkipHiddenFiles
        {
            get { return (bool)this["skipHiddenFiles"]; }
            set { this["skipHiddenFiles"] = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether read-only files should be 
        /// processed or skipped.
        /// </summary>
        /// <value>
        /// true if read-only files should be skipped; false if read-only files 
        /// should be processed.
        /// </value>
        [ConfigurationProperty( "skipReadOnlyFiles" )]
        public bool SkipReadOnlyFiles
        {
            get { return (bool)this["skipReadOnlyFiles"]; }
            set { this["skipReadOnlyFiles"] = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether system files should be 
        /// processed or skipped.
        /// </summary>
        /// <value>
        /// true if system files should be skipped; false if system files 
        /// should be processed.
        /// </value>
        [ConfigurationProperty( "skipSystemFiles" )]
        public bool SkipSystemFiles
        {
            get { return (bool)this["skipSystemFiles"]; }
            set { this["skipSystemFiles"] = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating which file time to use when 
        /// determining whether or not to delete the file.
        /// </summary>
        /// <value>
        /// The value of this property is a member of the DateToUse 
        /// enumeration.
        /// </value>
        [ConfigurationProperty( "dateToUse" )]
        public DateToUse DateToUse
        {
            get { return (DateToUse)this["dateToUse"]; }
            set { this["dateToUse"] = value; }
        }

        /// <summary>
        /// Gets or sets the value indicating how old something should be 
        /// before it is removed.
        /// </summary>
        /// <value>
        /// The age threshold is indicated in hours. Any file with a creation
        /// date older than the threshold (when compared to the current date
        /// and time) will be removed.
        /// </value>
        [ConfigurationProperty( "ageThreshold", IsRequired = true )]
        public int AgeThreshold
        {
            get { return (int)this["ageThreshold"]; }
            set { this["ageThreshold"] = value; }
        }
    }
}
