using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Proyecto_U2___Reposteria.Views
{
    /// <summary>
    /// Lógica de interacción para PostresView.xaml
    /// </summary>
    public partial class PostresView : Window
    {
        public PostresView()
        {
            InitializeComponent();
        }

        private void VerPostresView_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(MessageBox.Show("¿Estás seguro que deseas eliminar este postre?","Confirmar eliminación.",MessageBoxButton.YesNo,MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                pvm.EliminarCommand.Execute(null);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
