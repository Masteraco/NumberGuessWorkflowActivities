using System;
using System.Linq;
using System.Activities;
using System.Activities.Statements;
using NumberGuessWorkflowActivities;
using System.Threading;
using System.Collections.Generic;

namespace NumberGuessWorkflowHost
{

    class Program
    {
        static void Main(string[] args)
        {
            Activity workflow1 = new SequentialNumberGuessWorkflow();
            AutoResetEvent syncEvent = new AutoResetEvent(false);
            AutoResetEvent idleEvent = new AutoResetEvent(false);
            var inputs = new Dictionary<string, object>() { { "MaxNumber", 100 } };
            WorkflowApplication wfApp = new WorkflowApplication(workflow1, inputs)
            {
                Completed = delegate (WorkflowApplicationCompletedEventArgs e)
                {
                    syncEvent.Set();
                },

                Aborted = delegate (WorkflowApplicationAbortedEventArgs e)
                    {
                        Console.WriteLine(e.Reason);
                        syncEvent.Set();
                    },

                OnUnhandledException = delegate (WorkflowApplicationUnhandledExceptionEventArgs e)
                    {
                        Console.WriteLine(e.UnhandledException.ToString());
                        return UnhandledExceptionAction.Terminate;
                    },
                Idle = delegate (WorkflowApplicationIdleEventArgs e)
                {
                    idleEvent.Set();
                }
            };

            wfApp.Run();

            // Loop until the workflow completes.
            WaitHandle[] handles = new WaitHandle[] { syncEvent, idleEvent };
            while (WaitHandle.WaitAny(handles) != 0)
            {
                // Gather the user input and resume the bookmark.
                bool validEntry = false;
                while (!validEntry)
                {
                    if (!Int32.TryParse(Console.ReadLine(), out int Guess))
                    {
                        Console.WriteLine("Please enter an integer.");
                    }
                    else
                    {
                        validEntry = true;
                        wfApp.ResumeBookmark("EnterGuess", Guess);
                    }
                }
            }
        }
    }
}
