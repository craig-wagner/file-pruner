#region using
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
#endregion using

namespace Wagner.FilePruner
{
    public sealed class DirectoriesCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new DirectoriesElement();
        }

        protected override object GetElementKey( ConfigurationElement element )
        {
            return ( (DirectoriesElement)element ).Name;
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMap; }
        }

        protected override string ElementName
        {
            get { return "directory"; }
        }
    }
}
