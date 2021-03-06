using System;
using System.Collections.Generic;

namespace SharpCifs.Util.Sharpen
{
    public class Thread : IRunnable
	{
		private static ThreadGroup _defaultGroup = new ThreadGroup ();
		private bool _interrupted;
		private IRunnable _runnable;
		private ThreadGroup _tgroup;
		private System.Threading.Tasks.Task _thread;
		
		[ThreadStatic]
		private static Thread _wrapperThread;

		public Thread () : this(null, null, null)
		{
		}

		public Thread (string name) : this (null, null, name)
		{
		}

		public Thread (ThreadGroup grp, string name) : this (null, grp, name)
		{
		}

		public Thread (IRunnable runnable): this (runnable, null, null)
		{
		}
		
		Thread (IRunnable runnable, ThreadGroup grp, string name)
		{
			_thread = new System.Threading.Tasks.Task(InternalRun);
			
			this._runnable = runnable ?? this;
			_tgroup = grp ?? _defaultGroup;
			_tgroup.Add (this);
		}

		public static Thread CurrentThread ()
		{
            if (_wrapperThread == null) {
				_wrapperThread = new Thread ();
			}
			return _wrapperThread;
		}

		public ThreadGroup GetThreadGroup ()
		{
			return _tgroup;
		}

		private void InternalRun ()
		{
			_wrapperThread = this;
			try {
				_runnable.Run ();
			} catch (Exception exception) {
				LogStream.GetInstance().WriteLine (exception);
			} finally {
				_tgroup.Remove (this);
			}
		}

        //public static void Yield ()
        //{
        //}

        //public void Interrupt ()
        //{
        //	lock (_thread) {
        //		_interrupted = true;
        //		//thread.Interrupt ();

        //              _thread.Abort();
        //	}
        //}

        //public static bool Interrupted ()
        //{
        //	if (Thread._wrapperThread == null) {
        //		return false;
        //	}
        //	Thread wrapperThread = Thread._wrapperThread;
        //	lock (wrapperThread) {
        //		bool interrupted = Thread._wrapperThread._interrupted;
        //		Thread._wrapperThread._interrupted = false;
        //		return interrupted;
        //	}
        //}

        //public void Join ()
        //{
        //	_thread.Join ();
        //}

        //public void Join (long timeout)
        //{
        //	_thread.Join ((int)timeout);
        //}

        //public void Abort ()
        //{
        //	_thread.Abort ();
        //}

        public virtual void Run ()
		{
		}

		public void SetDaemon (bool daemon)
		{
			//_thread.IsBackground = daemon;
		}

		public static void Sleep (long milis)
		{
            System.Threading.Tasks.Task.Delay((int)milis).Wait();
			//System.Threading.Thread.Sleep ((int)milis);
		}

		public void Start ()
		{
			_thread.Start ();
		}	
	}

	public class ThreadGroup
	{
		private List<Thread> _threads = new List<Thread> ();
		
		public ThreadGroup()
		{
		}
		
		public ThreadGroup (string name)
		{
		}

		internal void Add (Thread t)
		{
			lock (_threads) {
				_threads.Add (t);
			}
		}
		
		internal void Remove (Thread t)
		{
			lock (_threads) {
				_threads.Remove (t);
			}
		}

		public int Enumerate (Thread[] array)
		{
			lock (_threads) {
				int count = Math.Min (array.Length, _threads.Count);
				_threads.CopyTo (0, array, 0, count);
				return count;
			}
		}
	}
}
