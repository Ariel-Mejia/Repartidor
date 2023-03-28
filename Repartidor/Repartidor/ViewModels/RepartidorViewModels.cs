using Repartidor.Proveedores;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Repartidor.ViewModels
{
    class RepartidorViewModels
    {
        #region Attributes
        private string userName;

        #endregion
        //==============================================================================

        //==============================================================================
        #region Properties
        public string UserName
        {
            get { return this.userName; }
        }
        #endregion
        //==============================================================================

        //==============================================================================
        #region Commands
        public ICommand LogOutCommand
        {
            get
            {
                return new RelayCommand(LogOut);
            }
        }
        #endregion
        //==============================================================================

        //==============================================================================
        #region Methods

        public void LogOut()
        {
            UsuarioProveedor obj = new UsuarioProveedor();
            obj.LogOut();
        }
        #endregion
    }
}
