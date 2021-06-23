using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;

namespace Wagner.FilePruner
{
    /// <summary>
    /// This class traverses a directory structure and removes files that are
    /// older than a specified threshold.
    /// </summary>
    public class FilePruner
    {
        private TraceSwitch traceLevel;
        private DateTime nowUtc = DateTime.UtcNow;

        /// <summary>
        /// The entry point for the application.
        /// </summary>
        public static void Main()
        {
            FilePruner pruner = new FilePruner();
            pruner.PruneDirectories();
        }

        /// <summary>
        /// Loops over the directories from the configuration file and
        /// processes each one.
        /// </summary>
        private void PruneDirectories()
        {
            try
            {
                // Set up tracing
                Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));
                Trace.IndentSize = 2;
                traceLevel = new TraceSwitch("traceLevel", "Main routine trace switch");

                // Load the settings from the application configuration
                DirectoriesSection directories =
                    ConfigurationManager.GetSection("directories") as DirectoriesSection;

                // Loop over each directory from the configuration and process
                foreach (DirectoriesElement directory in directories.Directories)
                {
                    Trace.IndentLevel = 0;

                    // Let the user know what's going to happen with each
                    // directory node from the config file
                    Console.WriteLine("");
                    Console.WriteLine("Processing config item with options:");

                    Console.WriteLine("  Name: " + directory.Name);
                    Console.WriteLine("  IncludeSubdirectories: " + directory.Recursive.ToString());
                    Console.WriteLine("  RemoveEmptySubdirectories: " + directory.RemoveEmptySubdirectories.ToString());
                    Console.WriteLine("  SkipHiddenDirectories: " + directory.SkipHiddenDirectories.ToString());
                    Console.WriteLine("  SkipReadOnlyDirectories: " + directory.SkipReadOnlyDirectories.ToString());
                    Console.WriteLine("  SkipSystemDirectories: " + directory.SkipSystemDirectories.ToString());
                    Console.WriteLine("  SkipHiddenFiles: " + directory.SkipHiddenFiles.ToString());
                    Console.WriteLine("  SkipReadOnlyFiles: " + directory.SkipReadOnlyFiles.ToString());
                    Console.WriteLine("  SkipSystemFiles: " + directory.SkipSystemFiles.ToString());
                    Console.WriteLine("  DateToUse: " + directory.DateToUse.ToString());
                    Console.WriteLine("  AgeThreshold: " + directory.AgeThreshold.ToString());

                    DirectoryInfo dirInfo = new DirectoryInfo(directory.Name);

                    if (dirInfo.Exists)
                        ProcessDirectory(dirInfo, directory);
                    else
                        Trace.WriteLineIf(traceLevel.TraceError, "Directory does not exist: " + dirInfo.FullName);
                }
            }
            catch (ConfigurationException ex)
            {
                Trace.WriteLineIf(traceLevel.TraceError, "There is an error in the application configuration file.\r\n" + ex.Message);
            }
            catch (Exception ex)
            {
                Trace.WriteLineIf(traceLevel.TraceError, "Unexpected exception has occurred\r\n" + ex.Message);
            }
        }

        /// <summary>
        /// Process each directory for old files.
        /// </summary>
        /// <param name="dirInfo">
        /// DirectoryInfo object containing a reference to the directory to be
        /// processed.
        /// </param>
        /// <param name="directory">
        /// A directory structure providing details on how to process
        /// the directory.
        /// </param>
        private void ProcessDirectory(DirectoryInfo dirInfo, DirectoriesElement directory)
        {
            if ((((dirInfo.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly && directory.SkipReadOnlyDirectories) ||
                ((dirInfo.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden && directory.SkipHiddenDirectories) ||
                ((dirInfo.Attributes & FileAttributes.System) == FileAttributes.System && directory.SkipSystemDirectories)) &&
                directory.Name != dirInfo.FullName)
            {
            }
            else
            {
                Trace.Indent();

                Trace.WriteLineIf(traceLevel.TraceInfo, dirInfo.FullName);

                // We've taken care of all the subdirectories, now let's check out
                // the files in the directory
                ProcessFiles(dirInfo.GetFiles(), directory);

                // If we are supposed to traverse the directory structure then
                // grab all the subdirectories of dirInfo and recursively call
                // this method for each one
                if (directory.Recursive)
                {
                    DirectoryInfo[] subdirectories = dirInfo.GetDirectories();

                    foreach (DirectoryInfo subdirectory in subdirectories)
                        ProcessDirectory(subdirectory, directory);
                }

                // If we're supposed to remove empty directories check if this one
                // is empty. We will never delete the top-level directory.
                if (directory.RemoveEmptySubdirectories && directory.Name != dirInfo.FullName)
                {
                    dirInfo.Refresh();

                    // Get a list of objects in the directory...
                    FileSystemInfo[] items = dirInfo.GetFileSystemInfos();

                    // And see if there are any or not...
                    if (items.Length == 0)
                    {
                        try
                        {
                            // Before deleting, remove the ReadOnly attribute so
                            // we don't throw an exception
                            if ((dirInfo.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                                dirInfo.Attributes -= FileAttributes.ReadOnly;
                            dirInfo.Delete();

                            Trace.WriteLineIf(traceLevel.TraceVerbose, "Directory " + dirInfo.FullName + " was successfully deleted.");
                        }
                        catch (Exception ex)
                        {
                            Trace.WriteLineIf(traceLevel.TraceError, "Error: Could not delete " + dirInfo.FullName);
                            Trace.WriteLineIf(traceLevel.TraceError, ex.Message);
                        }
                    }
                }

                Trace.Unindent();
            }
        }

        /// <summary>
        /// Look at each file in a directory and determine if it needs to
        /// be deleted.
        /// </summary>
        /// <param name="files">
        /// An array of FileInfo objects, one for each file in a directory.
        /// </param>
        /// <param name="directory">
        /// A DirectoryToPrune structure containing settings for how to handle
        /// each file in the directory.
        /// </param>
        private void ProcessFiles(FileInfo[] files, DirectoriesElement directory)
        {
            Trace.Indent();

            TimeSpan age = new TimeSpan(0);

            foreach (FileInfo file in files)
            {
                Trace.WriteLineIf(traceLevel.TraceVerbose, file.Name);

                switch (directory.DateToUse)
                {
                    case DateToUse.Create:
                        age = nowUtc - file.CreationTimeUtc;
                        break;

                    case DateToUse.Access:
                        age = nowUtc - file.LastAccessTimeUtc;
                        break;

                    case DateToUse.Write:
                        age = nowUtc - file.LastWriteTimeUtc;
                        break;
                }

                if (age.TotalHours > directory.AgeThreshold)
                {
                    try
                    {
                        file.Refresh();

                        if (((file.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly && directory.SkipReadOnlyFiles) ||
                            ((file.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden && directory.SkipHiddenFiles) ||
                            ((file.Attributes & FileAttributes.System) == FileAttributes.System && directory.SkipSystemFiles))
                        {
                            // skip this file
                        }
                        else
                        {
                            // Before deleting, remove the ReadOnly attribute
                            // if it is present so we don't throw an exception
                            if ((file.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                                file.Attributes -= FileAttributes.ReadOnly;

                            file.Delete();

                            Trace.WriteLineIf(traceLevel.TraceVerbose, "File " + file.Name + " was successfully deleted.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Trace.WriteLineIf(traceLevel.TraceError, "Error: Could not delete " + file.Name);
                        Trace.WriteLineIf(traceLevel.TraceError, ex.Message);
                    }
                }
            }

            Trace.Unindent();
        }
    }
}