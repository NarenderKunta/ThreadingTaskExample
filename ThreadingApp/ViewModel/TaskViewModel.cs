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
        //TaskScheduler uiScheduler;
        private static Queue<Action> MyQueue = new Queue<Action>();
        private static EventWaitHandle newtaskavailable = new AutoResetEvent(false);
        List<Action> TaskList = new List<Action>();
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



        public ICommand chkBox1Checked { get; set; }
        public ICommand chkBox2Checked { get; set; }
        public ICommand chkBox3Checked { get; set; }
        public ICommand StartTask { get; set; }
        public ICommand AddTask { get; set; }
        public ICommand ClearTask { get; set; }


        public TaskViewModel()
        {
            BtnTask1 = false;
            BtnTask2 = false;
            BtnTask3 = false;
            chkBox1Checked = new DelegateCommand(RunTask1);
            chkBox2Checked = new DelegateCommand(RunTask2);
            chkBox3Checked = new DelegateCommand(RunTask3);
            StartTask = new DelegateCommand(ButtonStartTaskClicked);
            AddTask = new DelegateCommand(ButtonAddTaskClicked);
            ClearTask = new DelegateCommand(ButtonClearTaskClicked);
            //TaskNameList = new List<string>();
        }

        public void RunTask1()
        {
            TaskList.Add(Task1);
            TaskNameList = "Task 1 Added.";
        }
        public void RunTask2()
        {
            //TaskName = Environment.NewLine;
            TaskNameList = "Task 2 Added.";
            TaskList.Add(Task2);
        }
        public void RunTask3()
        {
            //TaskName = Environment.NewLine;
            TaskNameList = "Task 3 Added.";
            TaskList.Add(Task3);
        }

        private void ButtonAddTaskClicked()
        {
            foreach (var item in TaskList)
            {
                MyQueue.Enqueue(item);
            }
            ExecuteTasks();
        }
        private void ButtonStartTaskClicked()
        {
            MyQueue.Enqueue(Task1);
            MyQueue.Enqueue(Task2);
            MyQueue.Enqueue(Task3);
            ExecuteTasks();
        }

        private void ButtonClearTaskClicked()
        {
            CurrentProgress1 = CurrentProgress2 = CurrentProgress3 = 0;
            FinishedTask1 = FinishedTask2 = FinishedTask3 = false;
        }

        private void Task1()
        {
            if (!FinishedTask1)
            {
                FinishedTask1 = true;
                CurrentProgress1 = 0;
                PrepareTask1();
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
                      Thread.Sleep(50); //simulateWork, do something with the data received
                      CurrentProgress1 = i;
                  }
              }).Wait();
            //var task = new Task(() =>
            //{
            //    for (int i = 0; i <= 100; i++)
            //    {
            //        Thread.Sleep(200);
            //        CurrentProgress1 = i;
            //    }
            //});
            //task.Start();
        }

        public void PrepareTask2()
        {
            Task.Factory.StartNew(
             () =>
             {
                 for (int i = 0; i <= 100; i++)
                 {
                     Thread.Sleep(50); //simulateWork, do something with the data received
                     CurrentProgress2 = i;
                 }
             }).Wait();
            //var task = new Task(() =>
            // {
            //     for (int i = 0; i <= 100; i++)
            //     {
            //         Thread.Sleep(500); 
            //        CurrentProgress2 = i;
            //     }
            // });
            //task.Start();
        }

        public void PrepareTask3()
        {
            Task.Factory.StartNew(
             () =>
             {
                 for (int i = 0; i <= 100; i++)
                 {
                     Thread.Sleep(50); //simulateWork, do something with the data received
                     CurrentProgress3 = i;
                 }
             }).Wait();
            //var task = new Task(() =>
            //{
            //    for (int i = 0; i <= 100; i++)
            //    {
            //        Thread.Sleep(500); 
            //        CurrentProgress3 = i;
            //    }
            //});
            //task.Start();
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
