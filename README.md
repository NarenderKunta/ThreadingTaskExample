# ThreadingTaskExample
Multi threading using Wpf
This TaskServicer is for 

(*) Adding of tasks - with the TaskAdder 

(*) Servicing/processing of tasks - with TaskQueue

	It contains BlockingCollection for storing Tasks



Both the TaskAdder & TaskQueue classes operate on the same Queue



Current settings:

Number of TaskAdders = 1

Number of TaskServicer(s) => based on the user-given value



This currently processes 3 tasks.
 showing the process with Progress Bar and clearing after the task is completed.


The user can cancel the processing by pressing the CTRL+C



Technologies used

(*) Blocking collection

(*) IProducerConsumerCollection

(*) Mutex for synchronisation of different servicer Tasks

(*) Logging mechanism with Serilog 
