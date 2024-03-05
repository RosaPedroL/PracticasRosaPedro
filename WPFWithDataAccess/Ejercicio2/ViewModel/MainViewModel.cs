using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using Model;
using Model.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace ViewModel
{
    public class MainViewModel : NotificationObject
    {
        private DataAccess _dataAcces;
        private int _id;
        private string _nombre;
        private string _apellidos;
        private decimal _promedio;
        private ICommand _agregarCommand; //comando para agregar 
        private ICommand _actualizarCommand;
        private ICommand _eliminarCommand;
        private ICommand _limpiarCommand; //comando para limpiar los campos
        private List<Alumno> _alumnos; //lista del alumno 
        private Alumno _alumnoSeleccionado; //alumno que selecciono 

        public int Id
        {
            get { return _id; }
            set
            {
                if (value == _id) return;
                _id = value;
                RaisePropertyChanged(() => Id);
            }
        }

        public string Nombre
        {
            get { return _nombre; }
            set
            {
                if (value == _nombre) return;
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
        public List<Alumno> Alumnos //envia los datos 
        {
            get { return _alumnos; }
            set
            {
                if (value == _alumnos) return;
                _alumnos = value;
                RaisePropertyChanged(() => Alumnos);
            }

        }
        public Alumno AlumnoSeleccionado //poner valor a los campos de la tabla seleccionado -> se utiliza en la vista esta propiedad   SelectedItem para seleecionar 
        {
            get { return _alumnoSeleccionado; }
            set
            {
                if (value == _alumnoSeleccionado) return;
                _alumnoSeleccionado = value;
                //no es necesario ponerlo en la vista aqui ya puedo utilizar el id sin especifica en la vista 
                if (_alumnoSeleccionado != null)
                {
                    Id = _alumnoSeleccionado.Id;
                    Nombre = _alumnoSeleccionado.Nombre;
                    Apellidos = _alumnoSeleccionado.Apellidos;
                    Promedio = _alumnoSeleccionado.Promedio;
                }
                else
                {
                    LimpiarCampos();
                }
                RaisePropertyChanged(() => AlumnoSeleccionado);
            }
        }

        public ICommand AgregarCommand
        {
            get
            {
                if (_agregarCommand == null)
                {
                    _agregarCommand = new DelegateCommand(AgregarAlumno, () => true);
                }
                return _agregarCommand;
            }
        }
        public ICommand LimpiarCommand
        {
            get
            {
                if (_limpiarCommand == null)
                {
                    _limpiarCommand = new DelegateCommand(LimpiarCampos, () => true);
                }
                return _limpiarCommand;
            }
        }

        public ICommand ActualizarCommand
        {
            get
            {
                if (_actualizarCommand == null)
                {
                    _actualizarCommand = new DelegateCommand(ActualizarAlumno, () => true);
                }
                return _actualizarCommand;
            }
        }
        public ICommand EliminarCommand
        {
            get
            {
                if (_eliminarCommand == null)
                {
                    _eliminarCommand = new DelegateCommand(EliminarAlumno, () => true);
                }
                return _eliminarCommand;
            }
        }

        public MainViewModel()
        {
            Alumnos = new List<Alumno>();
            _dataAcces = new DataAccess(); //crear un objeto donde accede a la base de datos 
            Alumnos = _dataAcces.GetAlumnosList(); //obtiene un inicio los datos 

        }



        public void AgregarAlumno()
        {
            DialogResult result;
            Alumno alumno = new Alumno(); //se crea un nuevo dato (1)
            alumno.Nombre = Nombre;
            //readyranche lista copiar 
            //ahora si el alumno compia temporar alumno agrega todo los elementos alumno 
            alumno.Apellidos = Apellidos;
            alumno.Promedio = Promedio;
            result = MessageBox.Show("¿Estás seguro de agregar este dato?", "Confirmar agregar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                if (string.IsNullOrEmpty(Nombre) || string.IsNullOrEmpty(Apellidos) || Promedio == 0)
                {
                    result = MessageBox.Show("Uno de los campos estan vació, por favor verifique que este lleno los campos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
                else
                {
                    if (_dataAcces.SearchFllAlumno(Nombre, Apellidos) == 0)
                    {
                        _dataAcces.SaveAlumno(alumno);//se guarda en la bd (2)
                        result = MessageBox.Show("El dato se agrego correctamente", "Inf", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Alumnos = _dataAcces.GetAlumnosList();//para actualizar la tabla con el nuevo datos (3)
                        LimpiarCampos();
                  

                    }
                    else
                    {
                        result = MessageBox.Show("Esta repetido el dato", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }


        public void LimpiarCampos()
        {
            Id = 0;
            Nombre = String.Empty;
            Apellidos = String.Empty;
            Promedio = 0;

            // String.Empty ->Es equivalente a ""(una cadena de longitud cero).Se utiliza
            // comúnmente en lugar de escribir una cadena vacía directamente en el código.
        }


        public void ActualizarAlumno()
        {
            DialogResult result = MessageBox.Show("¿Estás seguro de que deseas actualizar este dato?", "Confirmar actualizar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                if (string.IsNullOrEmpty(Nombre) || string.IsNullOrEmpty(Apellidos) || Promedio == 0)
                {
                    result = MessageBox.Show("Uno de los campos estan vació, por favor verifique que este lleno los campos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
                else
                {
                    //variables para los valores de salida
                    int existe;
                    int repetido;
                    Alumno alumno = new Alumno(); //se crea un nuevo dato (1)
                    alumno.Id = Id;
                    alumno.Nombre = Nombre;
                    alumno.Apellidos = Apellidos;
                    alumno.Promedio = Promedio;
                    //  método UpdateAlumno y pasar las variables de salida
                    _dataAcces.UpdateAlumno(alumno, out existe, out repetido);
                    if (existe == 1 & repetido == 0)
                    {
                        result = MessageBox.Show("El dato se modifico", "Inf", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Alumnos = _dataAcces.GetAlumnosList(); //para actualizar la tabla con el nuevo datos (3)   
                                                               //AlumnoSeleccionado = null;

                    }
                    else
                    {
                        if (existe == 0)
                        {
                            result = MessageBox.Show("El dato no existe", "Detalle", MessageBoxButtons.OK, MessageBoxIcon.Stop);

                        }
                        if (repetido == 1)
                        {
                            result = MessageBox.Show("Esta repetido el dato", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                        }
                    }
                }

            }

        }

        public void EliminarAlumno()
        {
            DialogResult result = MessageBox.Show("¿Estás seguro de que deseas eliminar este dato?", "Confirmar eliminar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                int existe = _dataAcces.DeleteAlumno(Id);
                if (existe == 1)
                {
                    result = MessageBox.Show("El dato se elimino correctament", "Inf", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Alumnos = _dataAcces.GetAlumnosList();

                }
                else
                {
                    result = MessageBox.Show("El dato no existe", "Detalle", MessageBoxButtons.OK, MessageBoxIcon.Stop);

                }

            }
        }












    }
}