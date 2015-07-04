#region using
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
#endregion using

namespace Wagner.FilePruner
{
    public sealed class DirectoriesSection : ConfigurationSection
    {
        public DirectoriesSection()
        {
        }

        [ConfigurationProperty( "", IsDefaultCollection = true )]
        public DirectoriesCollection Directories
        {
            get { return (DirectoriesCollection)this[""]; }
        }
    }
}
