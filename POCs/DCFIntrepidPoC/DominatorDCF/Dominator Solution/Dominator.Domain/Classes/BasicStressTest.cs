using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Dominator.Domain.Classes
{
    public class BasicStressTest : IBasicStressTest
    {
        public int Duration { get; }
        public int CPUUsage { get; set; }

        public event Action<int> ProgressChanged;
        public event Action Completed;

        private BackgroundWorker worker;
        private List<Thread> threads;
        private static readonly AutoResetEvent resetEvent = new AutoResetEvent(false);

        public BasicStressTest(int duration)
        {
            Duration = duration;
            CPUUsage = 100;
        }

        public void Start()
        {
            worker = new BackgroundWorker();
            worker.DoWork += worker_DoWork;
            worker.ProgressChanged += worker_ProgressChanged;
            //worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            worker.WorkerSupportsCancellation = true;
            worker.WorkerReportsProgress = true;                           
            worker.RunWorkerAsync();
        }

        public void Stop()
        {
            resetEvent.Set();            
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            keepRunningThreads = true;

            threads = new List<Thread>();
            for (int i = 0; i < Environment.ProcessorCount; i++)
            {
                Thread t = new Thread(new ParameterizedThreadStart(cpuKill));
                threads.Add(t);
                t.Start(CPUUsage);                
            }

            seconds = 0;
            System.Timers.Timer timer = new System.Timers.Timer(1000);
            timer.Elapsed += timer_Elapsed;
            timer.Start();
            
            if (!resetEvent.WaitOne(Duration*1000))
                ProgressChanged?.Invoke(100);

            timer.Stop();
            timer.Dispose();
            abortThreads();            

            Completed?.Invoke();
        }

        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ProgressChanged?.Invoke(e.ProgressPercentage);
        }

        //private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    abortThreads();
        //    Completed?.Invoke();
        //}

        private int seconds;
        private void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (!worker.IsBusy) return;
            if (++seconds < Duration)
            {
                int perc = seconds*100/Duration;
                worker.ReportProgress(perc);
            }
        }

        private void abortThreads()
        {
            keepRunningThreads = false;
            //if (threads == null) return;
            //foreach (var t in threads)
            //    if (t.IsAlive) t.Abort();
        }

        private bool keepRunningThreads;

        public void cpuKill(object cpuUsage)
        {
            Parallel.For(0, 1, new Action<int>((int i) =>
            {
                Stopwatch watch = new Stopwatch();
                watch.Start();
                while (keepRunningThreads)
                {
                    if (watch.ElapsedMilliseconds > (int)cpuUsage)
                    {
                        Thread.Sleep(100 - (int)cpuUsage);
                        //Debug.WriteLine(DateTime.Now.Ticks);
                        watch.Reset();
                        watch.Start();
                    }
                }
            }));
        }
    }
}
