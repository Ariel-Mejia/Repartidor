using Repartidor.Models;
using Repartidor.Proveedores;
using Repartidor.ViewModels;
using Repartidor.Views.RepartidorViews;
using Firebase.Auth;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Repartidor.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RepartidorPage : ContentPage
    {
        private string WebApiKey = "AIzaSyA33uUzHinDOOitFq-WNed3dlctMJJmjyk";
        public RepartidorPage()
        {
            InitializeComponent();
            BindingContext = new RepartidorViewModels();

            HistoryListView.RefreshCommand = new Command(() =>
            {
                OnAppearing();
            });

            GetProfileInformationAndRefreshToken();

            MenuButton.GestureRecognizers.Add(new TapGestureRecognizer((view) => OpenModal()));
            ClosePopUpModal.GestureRecognizers.Add(new TapGestureRecognizer((view) => CloseModal()));

            PopUpModal.IsVisible = false;
        }

        public async void GetProfileInformationAndRefreshToken()
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(WebApiKey));
            try
            {
                
                var savedfirebaseauth = JsonConvert.DeserializeObject<Firebase.Auth.FirebaseAuth>(Preferences.Get("MyFirebaseRefreshToken", ""));
                
                var RefreshedContent = await authProvider.RefreshAuthAsync(savedfirebaseauth);
                Preferences.Set("MyFirebaseRefreshToken", JsonConvert.SerializeObject(RefreshedContent));
               
                UserName.Text = Preferences.Get("Usuario", "UserNoDefined");
                UserEmail.Text = Preferences.Get("Correo", "NoEmailFounded");
                UserPhone.Text = Preferences.Get("Telefono", "00000000");
                UserImage.Source = Preferences.Get("Imagen", "https://i.ibb.co/vhh0Gkj/users.png");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await Application.Current.MainPage.DisplayAlert("Alerta", "Seccion Terminada", "OK");
            }
        }

        private void OpenModal()
        {
            PopUpModal.IsVisible = true;
        }

        private void CloseModal()
        {
            PopUpModal.IsVisible = false;
        }

        protected override async void OnAppearing()
        {
            var historyList = await VentasProvedores.GetPendingVentas();
            HistoryListView.ItemsSource = null;
            HistoryListView.ItemsSource = historyList;
            HistoryListView.IsRefreshing = false;
        }

        private void HistoryListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var venta = e.Item as Ventas;
            Navigation.PushAsync(new VentasPage(venta));
        }
    }
}