using Repartidor.Models;
using Repartidor.Proveedores;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Repartidor.Views.RepartidorViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VentasPage : ContentPage
    {
        Ventas venta;
        public VentasPage(Ventas _venta)
        {
            InitializeComponent();

            venta = _venta;

            ProductListView.RefreshCommand = new Command(() =>
            {
                OnAppearing();
            });
        }

        protected override void OnAppearing()
        {
            ClientName.Text = venta.ClientName;
            ClientPhone.Text = venta.ClientPhone;
            ClientName.Text = venta.ClientName;
            TotalToPay.Text = venta.TotalToPay;
            PayWay.Text = venta.PayFormat;
            DateTimeText.Text = venta.Date;
            SaleState.Text = venta.State;

            if (venta.State == "Pendiente")
            {
                SaleState.TextColor = Color.DarkRed;
            }
            else
            {
                SaleState.TextColor = Color.Green;
            }

            var ShoppingList = JsonConvert.DeserializeObject<List<Shopping>>(venta.ShoppingCar);
            ProductListView.ItemsSource = null;
            ProductListView.ItemsSource = ShoppingList;
            ProductListView.IsRefreshing = false;

            LoadLocation();
        }

        async Task LoadLocation()
        {
            try
            {
                double latitude = Convert.ToDouble(venta.ClientLatitude);
                double longitude = Convert.ToDouble(venta.ClientLongitude);

                var placemarks = await Geocoding.GetPlacemarksAsync(latitude, longitude);
                var placemark = placemarks?.FirstOrDefault();

                ClientLocation.Text = $" ({placemark.CountryCode}) {placemark.CountryName}, {placemark.AdminArea}, {placemark.Locality}";
                Latitude.Text = venta.ClientLatitude;
                Longitude.Text = venta.ClientLongitude;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ubicacion Incorrecta", ex.Message, "OK");
            }
        }

        private async void BackButton_Clicked(object sender, EventArgs e)
        {
            Ventas updateVenta = new Ventas()
            {
                Id = venta.Id,
                ClientName = venta.ClientName,
                ClientMail = venta.ClientMail,
                ClientPhone = venta.ClientPhone,
                ClientLatitude = venta.ClientLatitude,
                ClientLongitude = venta.ClientLongitude,
                TotalToPay = venta.TotalToPay,
                Detail = venta.Detail,
                Date = venta.Date,
                PayFormat = venta.PayFormat,
                ShoppingCar = venta.ShoppingCar,
                State = "Entregado"
            };

            bool res = await VentasProvedores.UpdateState(updateVenta);

            if (res)
            {
                await DisplayAlert("Satisfactorio", "La entrega ha sido finalizada.", "OK");
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Error", "Un error ocurrio en el proceso.", "OK");
            }
        }
    }
}