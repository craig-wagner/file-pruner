#region using
using System;
using System.Collections.Generic;
using System.Text;
#endregion using

namespace Wagner.FilePruner
{
    /// <summary>
    /// Identifies which date the process should use when checking to see if
    /// a file should be removed.
    /// </summary>
    public enum DateToUse
    {
        /// <summary>
        /// Use the LastWriteTimeUtc property of the FileInfo
        /// </summary>
        Write,
        /// <summary>
        /// Use the LastAccessTimeUtc property of the FileInfo
        /// </summary>
        Access,
        /// <summary>
        /// Use the CreationTimeUtc property of the FileInfo
        /// </summary>
        Create
    }
}
