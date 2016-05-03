using System;
using System.IO;
using System.Threading;

namespace SystemWatcher
{
    class Program
    {
            static void Main(string[] args)
            {
                // enable watching
                DirectoryWatcher(@"C:\Windows");

                // again, stupid code to just keep the function alive
                while (true) { Thread.Sleep(1000); }
            }


            public static void DirectoryWatcher(string directoryToWatch)
            {
                // check incoming arguments
                if (string.IsNullOrWhiteSpace(directoryToWatch))
                    throw new ArgumentNullException("directoryToWatch");

                // create a new FileSystemWatcher
                FileSystemWatcher w = new FileSystemWatcher();

                // set the directory to watch
                w.Path = directoryToWatch;

                //this is the heart - setup multiple filters
                // to watch various types of changes to watch
                w.NotifyFilter = NotifyFilters.Size |
                                    NotifyFilters.FileName |
                                    NotifyFilters.DirectoryName |
                                    NotifyFilters.CreationTime;

                // setup which file types do we want to monitor
                w.Filter = "*.*";

                // setup event handlers to watch for changes
                w.Changed += watcher_Change;
                w.Created += watcher_Change;
                w.Deleted += watcher_Change;
                w.Renamed += new RenamedEventHandler(watcher_Renamed);

                // just some debugging
                Console.WriteLine(
                "Manipulate files in {0} to see activity...", directoryToWatch);

                // enable watching by allowing events to be raised
                w.EnableRaisingEvents = true;
            }

            static void watcher_Change(object sender, FileSystemEventArgs e)
            {
                Console.WriteLine("{0} changed ({1})", e.Name, e.ChangeType);
            }

            static void watcher_Renamed(object sender, RenamedEventArgs e)
            {
                Console.WriteLine("{0} renamed to {1}", e.OldName, e.Name);
            }
        }
    }
