using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;

namespace ThreadingApp.ViewModel
{
    public class TaskViewModel : BindableBase
    {

        private static Queue<Action> MyQueue = new Queue<Action>();
        private static EventWaitHandle newtaskavailable = new AutoResetEvent(false);
        public List<Action> TaskList = new List<Action>();
        private static readonly object queuelock = new object();
        private bool FinishedTask1;
        private bool FinishedTask2;
        private bool FinishedTask3;

        private bool btnTask1;

        public bool BtnTask1
        {
            get { return btnTask1; }
            set
            {
                btnTask1 = value;
                RaisePropertyChanged();
            }
        }

        private bool btnTask2;

        public bool BtnTask2
        {
            get { return btnTask2; }
            set
            {
                btnTask2 = value;
                RaisePropertyChanged();
            }
        }

        private bool btnTask3;

        public bool BtnTask3
        {
            get { return btnTask3; }
            set
            {
                btnTask3 = value;
                RaisePropertyChanged();
            }
        }

        private double _currentProgress1;
        public double CurrentProgress1
        {
            get { return _currentProgress1; }
            set
            {
                SetProperty(ref _currentProgress1, value);
            }
        }
        private double _currentProgress2;
        public double CurrentProgress2
        {
            get { return _currentProgress2; }
            set
            {
                SetProperty(ref _currentProgress2, value);
            }
        }

        private double _currentProgress3;
        public double CurrentProgress3
        {
            get { return _currentProgress3; }
            set
            {
                SetProperty(ref _currentProgress3, value);
            }
        }

        private string _taskNameList;
        public string TaskNameList
        {
            get { return _taskNameList; }
            set { SetProperty(ref _taskNameList, value); RaisePropertyChanged(); }
        }

        private string _taskNameList1;

        public string TaskNameList1
        {
            get { return _taskNameList1; }
            set { SetProperty(ref _taskNameList1, value); RaisePropertyChanged(); }
        }
        public ICommand ButtonTask1Command { get; set; }
        public ICommand ButtonTask2Command { get; set; }
        public ICommand ButtonTask3Command { get; set; }
        public ICommand StartTaskCommand { get; set; }
        public ICommand AddTaskCommand { get; set; }
        public ICommand ClearTaskCommand { get; set; }
        public TaskViewModel()
        {
            ButtonTask1Command = new DelegateCommand(RunTask1);
            ButtonTask2Command = new DelegateCommand(RunTask2);
            ButtonTask3Command = new DelegateCommand(RunTask3);
            StartTaskCommand = new DelegateCommand(ExecuteStartTask);
            AddTaskCommand = new DelegateCommand(ExecuteAddTask);
            ClearTaskCommand = new DelegateCommand(ExecuteClearTask);            
        }

        public void RunTask1()
        {
            this.TaskList.Add(Task1);
            this.TaskNameList += Environment.NewLine;
            this.TaskNameList += "Task 1 Added.";
        }
        public void RunTask2()
        {
            this.TaskNameList += Environment.NewLine;
            this.TaskNameList += "Task 2 Added.";
            this.TaskList.Add(Task2);
        }
        public void RunTask3()
        {
           this.TaskNameList += Environment.NewLine;
           this.TaskNameList += "Task 3 Added.";
           this.TaskList.Add(Task3);
        }

        public void ExecuteAddTask()
        {
            foreach (var item in TaskList)
            {
                MyQueue.Enqueue(item);
            }
            ExecuteTasks();
        }
        private void ExecuteStartTask()
        {
            MyQueue.Enqueue(Task1);
            MyQueue.Enqueue(Task2);
            MyQueue.Enqueue(Task3);
            ExecuteTasks();
        }

        private void ExecuteClearTask()
        {
            this.CurrentProgress1 = CurrentProgress2 = CurrentProgress3 = 0;
            this.FinishedTask1 = FinishedTask2 = FinishedTask3 = false;
        }

        private void Task1() 
        {
            if (!FinishedTask1)
            {
                this.FinishedTask1 = true;
                this.CurrentProgress1 = 0;
                this.PrepareTask1();
            }
        }

        private void Task2()
        {
            if (!FinishedTask2)
            {
                FinishedTask2 = true;
                CurrentProgress2 = 0;
                PrepareTask2();
            }
        }

        private void Task3()
        {
            if (!FinishedTask3)
            {
                FinishedTask3 = true;
                CurrentProgress3 = 0;
                PrepareTask3();
            }
        }
        public void PrepareTask1()
        {
            Task.Factory.StartNew(
              () =>
              {
                  for (int i = 0; i <= 100; i++)
                  {
                      Thread.Sleep(50); 
                      CurrentProgress1 = i;
                  }
              }).Wait();
            CurrentProgress1 = 0;
            TaskNameList1 += Environment.NewLine;
            TaskNameList1 += "Task 1 is Completed.";          
        }
        public void PrepareTask2()
        {
            Task.Factory.StartNew(
             () =>
             {
                 for (int i = 0; i <= 100; i++)
                 {
                     Thread.Sleep(50); 
                     CurrentProgress2 = i;
                 }
             }).Wait();
            CurrentProgress2 = 0;
            TaskNameList1 += Environment.NewLine;
            TaskNameList1 += "Task 2 is Completed.";           
        }

        public void PrepareTask3()
        {
            Task.Factory.StartNew(
             () =>
             {
                 for (int i = 0; i <= 100; i++)
                 {
                     Thread.Sleep(50); 
                     CurrentProgress3 = i;
                 }
             }).Wait();
            CurrentProgress3 = 0;
            TaskNameList1 += Environment.NewLine;
            TaskNameList1 += "Task 3 is Completed.";            
        }

        public async void ExecuteTasks()
        {
            while (true)
            {
                Action task = null;
                lock (queuelock)
                {
                    if (MyQueue.Count > 0)
                    {
                        task = MyQueue.Dequeue();
                    }
                }
                if (task != null)
                {
                    await Task.Run(() =>
                    {
                        task.Invoke();
                        Thread.Sleep(500);
                    });
                }
                else
                {
                    return;
                }
            }
        }

    }
}
