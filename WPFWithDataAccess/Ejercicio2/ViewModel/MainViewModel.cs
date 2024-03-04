using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using Model;
using Model.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ViewModel
{
    public class MainViewModel : NotificationObject
    {
        private DataAccess _dataAcces;
        private string _nombre;
        private string _apellidos;
        private decimal _promedio;
        private ICommand _agregarCommand;
        private List<Alumno> _alumnos;
        private Alumno _alumnoSeleccionado;

        public string Nombre
        {
            get { return _nombre; }
            set
            {
                if(value == _nombre) return;
                _nombre = value;
                RaisePropertyChanged(() => Nombre);
            }
        }
        public string Apellidos
        {
            get { return _apellidos; }
            set
            {
                if (value == _apellidos) return;
                _apellidos = value;
                RaisePropertyChanged(() => Apellidos);
            }
        }
        public decimal Promedio
        {
            get { return _promedio; }
            set
            {
                if (value == _promedio) return;
                _promedio = value;
                RaisePropertyChanged(() => Promedio);
            }
        }
        public List<Alumno> Alumnos
        {
            get { return _alumnos; }
            set
            {
                if (value == _alumnos) return;
                _alumnos = value;
                RaisePropertyChanged(() => Alumnos);
            }
        }
        public Alumno AlumnoSeleccionado
        {
            get { return _alumnoSeleccionado; }
            set
            {
                if (value == _alumnoSeleccionado) return;
                _alumnoSeleccionado = value;

                Nombre = _alumnoSeleccionado.Nombre;
                Apellidos = _alumnoSeleccionado.Apellidos;
                Promedio = _alumnoSeleccionado.Promedio;

                RaisePropertyChanged(() => AlumnoSeleccionado);
            }
        }
        public ICommand AgregarCommand
        {
            get
            {
                if(_agregarCommand == null)
                {
                    _agregarCommand = new DelegateCommand(AgregarAlumno, () => true);
                }
                return _agregarCommand;
            }
        }

        public MainViewModel()
        {
            Alumnos = new List<Alumno>();
            _dataAcces = new DataAccess();
            Alumnos = _dataAcces.GetAlumnosList();
        }



        public void AgregarAlumno()
        {
            Alumno alumno = new Alumno();
            alumno.Nombre = Nombre;
            alumno.Apellidos = Apellidos;
            alumno.Promedio = Promedio;
            _dataAcces.SaveAlumno(alumno);
            Alumnos = _dataAcces.GetAlumnosList();
            Nombre = String.Empty;
            Apellidos = String.Empty;
            Promedio = 0;
        }
    }
}
