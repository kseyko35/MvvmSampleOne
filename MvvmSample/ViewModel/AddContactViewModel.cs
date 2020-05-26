using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

using Xamarin.Essentials;
using Xamarin.Forms;

namespace MvvmSample.ViewModel
{
    public class AddContactViewModel : INotifyPropertyChanged
    {
        

        private string name= "Seyfi";
        private string website = "http://google.com";
        private bool bestFriend;
        private bool isBusy;
        private double firstNumber;
        private double secondNumber;
        private string selectedOperations;
        public List<string> operations;
        private string solutionNumber;
      
        public Command LaunchWebsiteCommand { get; }
        public Command SaveContactCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public AddContactViewModel()
        {
            LaunchWebsiteCommand = new Command(async () => await LaunchWebSiteAsync(), () => !isBusy);
            SaveContactCommand = new Command(async () => await SaveContactAsync(), () => !isBusy);
            operations = new List<string>
            {
                "+",
                "-",
                "*",
                "/"
            };
            
        }

        void onNotifyPropertyChanged([CallerMemberName] string name ="") // Callermembername ile member isimlerini otomatik atiyor.
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));   
        }

        public string Name { get => name; set
            {
                name = value;
                if (name == "Seyfi1")
                    IsBusy = true;
                else IsBusy = false;
                onNotifyPropertyChanged();
                onNotifyPropertyChanged(nameof(DisplayMessage));
            }
        }
        public string Website { get => website; set
            {
                website = value;
                onNotifyPropertyChanged();
            }
        }
        public bool BestFriend { get => bestFriend; set
            {
                bestFriend = value;
               
                onNotifyPropertyChanged();
                onNotifyPropertyChanged(nameof(DisplayMessage));
            }
        }
        public string SolutionAnswer()
        {
             switch (selectedOperations)
            {
                case "/":
                    if (secondNumber != 0)
                    {
                        solutionNumber = (firstNumber / secondNumber).ToString("N2");
                    }
                    else solutionNumber = "Undefined";
                    break;
                case "*":
                    solutionNumber = (firstNumber * secondNumber).ToString();
                    break;
                case "+":
                    solutionNumber = (firstNumber + secondNumber).ToString();
                    break;
                case "-":
                    solutionNumber = (firstNumber - secondNumber).ToString();
                    break;
            };
            return solutionNumber;
            
        }
       
        public bool IsBusy { get => isBusy; set
            {
                isBusy = value;
                onNotifyPropertyChanged();
                LaunchWebsiteCommand.ChangeCanExecute();
                SaveContactCommand.ChangeCanExecute();
            }
        }
        public double FirstNumber
        {
            get => firstNumber;
            set
            {
                firstNumber = value;
                SolutionAnswer();
                onNotifyPropertyChanged();
                onNotifyPropertyChanged(nameof(Solution));
            }
        }

        public double SecondNumber
        {
            get => secondNumber;
            set
            {
        
                secondNumber = value;
                SolutionAnswer();
                onNotifyPropertyChanged();
                onNotifyPropertyChanged(nameof(Solution));
            }
        }
        public string SelectedOperations
        {
            get => selectedOperations;
            set
            {
                    selectedOperations = value;
                    SolutionAnswer();
                    onNotifyPropertyChanged();
                    onNotifyPropertyChanged(nameof(Solution));
 
            }
        }
        public List<string> Operations
        {
            get => operations ;

        }

        public string DisplayMessage
        {
            get => $"Your new friend is named {Name} and " +
                       $"{(bestFriend ? "is" : "is not")} your best friend.";
            
        }
        public string Solution
        {
            get => $"{FirstNumber} " + $"{selectedOperations} " +
                       $"{SecondNumber} =" +$" {solutionNumber}";
        }

        async Task LaunchWebSiteAsync()
        {
            try
            {
                await Browser.OpenAsync(website, BrowserLaunchMode.SystemPreferred);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        async Task SaveContactAsync()
        {
            
            IsBusy = true;
            await Task.Delay(4000);
            IsBusy = false;
            
            await Application.Current.MainPage.DisplayAlert("Save", "Contanct has been saved", "Ok");
            
        }
    }
}
