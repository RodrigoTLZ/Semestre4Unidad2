using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json;
using Proyecto_U2___Reposteria.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Proyecto_U2___Reposteria.ViewModels
{
    public class PostresViewModel : INotifyPropertyChanged
    {
        int PosicionPostreEditar;
        string vista;
        public ObservableCollection<Postre> ListaDePostres { get; set; } = new ObservableCollection<Postre>();

        private Postre? postre;

        public Postre Postre
        {
            get { return postre; }
            set {
                postre = value;
                PropertyChange();
            }
        }

        public string Vista { get; set; } = "Ver";
        public string? MensajeError { get; set; }

        public ICommand CambiarVistaCommand { get; set; }
        public ICommand AgregarCommand { get; set; }
        public ICommand EliminarCommand { get; set; }
        public ICommand ConfirmarGuardadoCommand { get; set; }
        public ICommand CancelarGuardadoCommand { get; set; }

        public PostresViewModel()
        {
            Cargar();
            CambiarVistaCommand = new RelayCommand<string>(CambiarVista);
            AgregarCommand = new RelayCommand(Agregar);
            EliminarCommand = new RelayCommand(Eliminar);
            ConfirmarGuardadoCommand = new RelayCommand(ConfirmarGuardado);
            CancelarGuardadoCommand = new RelayCommand(CancelarGuardado);
        }

        public void CambiarVista(string v)
        {
            MensajeError = "";
            Vista = v;

            if (Vista == "Agregar")
            {
                Postre = new Postre();
            }

            if(Vista == "Editar")
            {
                if (postre != null)
                {
                    Postre duplicado = new Postre()
                    {
                        Nombre = postre.Nombre,
                        Descripcion = postre.Descripcion,
                        Precio = postre.Precio,
                        Imagen = postre.Imagen,
                    };
                   
                    PosicionPostreEditar = ListaDePostres.IndexOf(postre);

                    Postre = duplicado;

                }
            }

            PropertyChange();
        }

        private void Agregar()
        {
            if(Postre !=null)
            {
                if(string.IsNullOrWhiteSpace(Postre.Nombre))
                {
                    MensajeError = "Escribe el nombre del postre.";
                    PropertyChange();
                    return;
                }

                if (string.IsNullOrWhiteSpace(Postre.Descripcion))
                {
                    MensajeError = "Escriba la descripción del postre.";
                    PropertyChange();
                    return;
                }

                if(!Uri.TryCreate(Postre.Imagen,UriKind.Absolute, out var uri))
                {
                    MensajeError = "Escriba una URL válida para la imagen";
                    PropertyChange();
                    return;
                }

                ListaDePostres.Add(Postre);
                CambiarVista("Ver");
                Guardar();
                MensajeError = "";
                
            }
        }

        private void Eliminar()
        {
            if(Postre != null)
            {
                ListaDePostres.Remove(Postre);
                Guardar();
                PropertyChange();
            }
        }

        private void ConfirmarGuardado()
        {
            if(Postre != null)
            {
                if(PosicionPostreEditar >= 0) 
                {
                    ListaDePostres[PosicionPostreEditar] = Postre;
                    Guardar();
                    CancelarGuardado();
                    MensajeError = "";
                   
                }
               
            }
            CambiarVista("Ver");
        }

        private void CancelarGuardado()
        {
            CambiarVista("Ver");
            Postre = null;
            MensajeError="";
        }

        private void Guardar()
        {
            var json = JsonConvert.SerializeObject(ListaDePostres);
            File.WriteAllText("postres.json", json);
        }

        public void Cargar()
        {
            if (File.Exists("postres.json"))
            {
                var json = File.ReadAllText("postres.json");
                var datos = JsonConvert.DeserializeObject<ObservableCollection<Postre>?>(json);

                if (datos != null)
                {
                    ListaDePostres = datos;
                }
                else
                {
                    ListaDePostres = new ObservableCollection<Postre>();
                }
            }
        }

        void PropertyChange()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

   
}
