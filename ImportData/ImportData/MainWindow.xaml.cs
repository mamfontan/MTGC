using ImportData.domain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ImportData
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DataAccess _dataAccess;

        private bool _internetConnection;

        private bool _databaseConnection;

        public MainWindow()
        {
            InitializeComponent();
        }

        #region Event response methods
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _dataAccess = new DataAccess();

            _internetConnection = CheckInternetConnection();
            _databaseConnection = CheckDatabaseConnection();

            txtInfo.Text += _internetConnection ? " Internet connection available" : " Internet connextion is NOT available";
            txtInfo.Text += Environment.NewLine;

            txtInfo.Text += _databaseConnection ? " Database connection is available" : " Database connection is not available";
            txtInfo.Text += Environment.NewLine;

            txtInfo.CaretIndex = txtInfo.Text.Length;
            txtInfo.ScrollToEnd();
        }

        private void imgUpdate_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!_internetConnection)
            {
                txtInfo.Text += " Es necesario conectarse a internet para actualizar los datos";
                txtInfo.Text += Environment.NewLine;
                txtInfo.CaretIndex = txtInfo.Text.Length;
                txtInfo.ScrollToEnd();
            }
            else
            {
                if (!_databaseConnection)
                {
                    txtInfo.Text += " Es necesario conectarse a la base de datos para actualizar los datos";
                    txtInfo.Text += Environment.NewLine;
                    txtInfo.CaretIndex = txtInfo.Text.Length;
                    txtInfo.ScrollToEnd();
                }
                else
                {
                    var url = Convert.ToString(lblSetsUrl.Content);

                    try
                    {
                        using (HttpClient httpClient = new HttpClient())
                        {
                            var response = httpClient.GetAsync(url).GetAwaiter().GetResult();
                            var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                            //result = JsonConvert.DeserializeObject<T>(json);
                        }
                    }
                    catch (Exception error)
                    {
                        //Message = error.Message;
                        //result = default(T);
                    }

                }
            }
        }
        #endregion

        #region Private methods
        private bool CheckInternetConnection()
        {
            try
            {
                System.Net.IPHostEntry i = System.Net.Dns.GetHostEntry("www.google.com");
                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool CheckDatabaseConnection()
        {
            return _dataAccess.CheckDbConnection();
        }
        #endregion

    }
}
