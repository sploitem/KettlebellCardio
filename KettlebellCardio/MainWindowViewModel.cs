using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace KettlebellCardio
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private int counter;
        public int Counter
        {
            get
            {
                return counter;
            }
            set
            {
                counter = value;
                OnPropertyChanged(nameof(Counter));
            }
        }

        private TimeSpan totalTime;
        public TimeSpan TotalTime
        {
            get
            {
                return totalTime;
            }
            set
            {
                totalTime = value;
                OnPropertyChanged(nameof(TotalTime));
            }
        }

        private string exercise;
        public string Exercise
        {
            get
            {
                return exercise;
            }
            set
            {
                exercise = value;
                OnPropertyChanged(nameof(Exercise));
            }
        }

        private string nextExercise;
        public string NextExercise
        {
            get
            {
                return nextExercise;
            }
            set
            {
                nextExercise = value;
                OnPropertyChanged(nameof(NextExercise));
            }
        }

        DispatcherTimer timer;
        int exercisePosition = 0;

        public MainWindowViewModel()
        {
            TotalTime = TimeSpan.FromSeconds(0);
            timer = new DispatcherTimer();
            timer.Tick += Timer_Tick;
            timer.Interval = new TimeSpan(0, 0, 1);
            Exercise = Exercises.list[exercisePosition];
            NextExercise = exercisePosition == Exercises.list.Length ? "" : Exercises.list[exercisePosition + 1];
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (exercisePosition == Exercises.list.Length)
            {
                timer.Stop();
            }
            if (Counter == 30)
            {
                Counter = 0;
                exercisePosition++;
                Exercise = Exercises.list[exercisePosition];
                NextExercise = exercisePosition == Exercises.list.Length ? "" : Exercises.list[exercisePosition + 1];
            }
            TotalTime = TotalTime.Add(TimeSpan.FromSeconds(1));
            Counter++;
        }

        private Command startCommand;
        public Command StartCommand
        {
            get
            {
                return startCommand ??
                  (startCommand = new Command(obj =>
                  {
                      TotalTime = TimeSpan.FromSeconds(0);
                      Counter = 0;
                      exercisePosition = 0;
                      Exercise = Exercises.list[exercisePosition];
                      NextExercise = exercisePosition == Exercises.list.Length ? "" : Exercises.list[exercisePosition + 1];
                      timer.Start();
                  }));
            }
        }

        private Command stopCommand;
        public Command StopCommand
        {
            get
            {
                return stopCommand ??
                  (stopCommand = new Command(obj =>
                  {
                      timer.Stop();
                  }));
            }
        }
    }
}
