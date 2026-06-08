using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ExchangeRateGUI
{
    public class ApiResponse
    {
        public string result { get; set; } = string.Empty;
        public string base_code { get; set; } = string.Empty;
        public Dictionary<string, float> rates { get; set; } = new Dictionary<string, float>();
    }

    public partial class MainWindow : Window
    {
        private const string ApiUrl = "https://open.er-api.com/v6/latest/USD";
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void FetchButton_Click(object sender, RoutedEventArgs e)
        {
            if (!(float.TryParse(UsdInput.Text, out float inputAmount) || inputAmount < 0))
            {
                MessageBox.Show("Please enter a valid positive number for the amount.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            StatusDisplay.Text = "Fetching live financial data from servers...";
            FetchButton.IsEnabled = false;

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string jsonString = await client.GetStringAsync(ApiUrl);

                    ApiResponse? data = JsonSerializer.Deserialize<ApiResponse>(jsonString);

                    if (data != null && data.result == "success")
                    {
                        StatusDisplay.Text = $"Rates Updated Successfully! Base: {data.base_code} ({DateTime.Now.ToShortTimeString()})";
                        string? selectedContent = ((ComboBoxItem)CurrencyDropdown.SelectedItem).Content.ToString();
                        string currencyCode = selectedContent.Substring(0, 3);

                        if (data.rates.TryGetValue(currencyCode, out float rate))
                        {
                            float convertedValue = inputAmount * rate;

                            ResultDisplay.Text = $"{inputAmount:F2} USD = {convertedValue:F2} {currencyCode}";
                        }
                    }
                    else
                    {
                        StatusDisplay.Text = "Error: API response signaled an operational failure.";
                    }
                }
                catch (Exception ex) 
                {
                    StatusDisplay.Text = "Connection Error.";
                    MessageBox.Show($"Failed to connect to exchange rate servers:\\n{ex.Message}\", \"Network Exception\", MessageBoxButton.OK, MessageBoxImage.Error);");
                }
                finally
                {
                    FetchButton.IsEnabled = true;
                }
            }
        }
    }
}