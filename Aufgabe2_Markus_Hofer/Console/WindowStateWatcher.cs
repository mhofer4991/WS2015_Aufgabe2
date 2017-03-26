//-----------------------------------------------------------------------
// <copyright file="WindowStateWatcher.cs" company="Markus Hofer">
//     Copyright (c) Markus Hofer
// </copyright>
// <summary>This class monitors the console window and informs about changes of it's state.</summary>
//-----------------------------------------------------------------------
namespace Aufgabe2_Markus_Hofer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// This class monitors the console window and informs about changes of it's state.
    /// </summary>
    public class WindowStateWatcher
    {
        /// <summary> Gets the default interval of the watcher. </summary>
        private const int DefaultInterval = 100;

        /// <summary> The current interval of the watcher. </summary>
        private int interval;

        /// <summary> Is used for monitoring the state of the console window in the background. </summary>
        private Thread monitorThread;

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowStateWatcher"/> class.
        /// </summary>
        public WindowStateWatcher()
        {
            this.interval = DefaultInterval;
        }

        /// <summary>
        /// Delegate for event OnWindowSizeChanged.
        /// </summary>
        /// <param name="oldWindowWidth">The old window width.</param>
        /// <param name="oldWindowHeight">The old window height.</param>
        public delegate void WindowSizeChanged(int oldWindowWidth, int oldWindowHeight);

        /// <summary>
        /// Gets called when the console window has been resized.
        /// </summary>
        public event WindowSizeChanged OnWindowSizeChanged;

        /// <summary> Gets a value indicating whether the watcher is currently watching or not. </summary>
        /// <value> A value indicating whether the watcher is currently watching or not. </value>
        public bool IsWatching { get; private set; }

        /// <summary>
        /// Sets the interval of the watcher.
        /// </summary>
        /// <param name="interval">The new interval which will be applied.</param>
        public void SetInterval(int interval)
        {
            this.interval = interval;
        }

        /// <summary>
        /// Starts the watcher if he is currently not running.
        /// </summary>
        public void Start()
        {
            if (!this.IsWatching)
            {
                this.monitorThread = new Thread(new ThreadStart(this.MonitorWindow));
                this.monitorThread.IsBackground = true;

                this.IsWatching = true;
                this.monitorThread.Start();
            }
        }

        /// <summary>
        /// Stops the watcher if he is currently running.
        /// </summary>
        public void Stop()
        {
            if (this.IsWatching)
            {
                this.IsWatching = false;
                this.monitorThread.Abort();
            }
        }

        /// <summary>
        /// Monitors the console window and informs about changes of it's state.
        /// </summary>
        private void MonitorWindow()
        {
            int oldWindowWidth = Console.WindowWidth;
            int oldWindowHeight = Console.WindowHeight;

            while (this.IsWatching)
            {
                if (Console.WindowWidth != oldWindowWidth ||
                    Console.WindowHeight != oldWindowHeight)
                {
                    if (this.OnWindowSizeChanged != null)
                    {
                        this.OnWindowSizeChanged(oldWindowWidth, oldWindowHeight);

                        oldWindowWidth = Console.WindowWidth;
                        oldWindowHeight = Console.WindowHeight;
                    }
                }
                else
                {
                    Thread.Sleep(this.interval);
                }
            }
        }
    }
}
