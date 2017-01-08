using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mengsk.AutoTest
{
    public class AutoTestManager
    {
        private static readonly AutoTestManager instance = new AutoTestManager();

        public static AutoTestManager Instance { get { return instance; } }
        private Task autoTestTask = null;
        private CancellationTokenSource cancellationSource = null;

        public bool IsRunningTest { get; private set; }


        public event EventHandler<AutoTestStartEventArgs> TestStarting;

        public event EventHandler<AutoTestEndEventArgs> TestEnding;

        public void StartAutoTest(AutoTestStartParameter parameter)
        {
            cancellationSource = CancellationTokenSource.CreateLinkedTokenSource(new CancellationToken(false));
            this.autoTestTask = new Task(() => this.AutoTest(parameter), cancellationSource.Token);
            this.autoTestTask.Start();
        }

        public void StopAutoTest()
        {
            if (this.IsRunningTest == false)
            {
                throw new InvalidOperationException("Auto test is not running");
            }

            this.cancellationSource.Cancel(true);
        }

        public void AutoTest(AutoTestStartParameter parameter)
        {
            try
            {
            }
            finally
            {
                this.IsRunningTest = false;
                this.autoTestTask = null;
                this.cancellationSource = null;
            }
        }
    }
}
