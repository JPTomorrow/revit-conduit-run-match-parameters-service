using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace JPMorrow.UI.ViewModels
{
	public partial class ParentViewModel {
        // presenter model
    }

    /// <summary>
    /// Default Presenter: Just Presents a string value as a listbox item,
    /// can replace with an object for more complex listbox databindings
    /// </summary>
    public class ItemPresenter : Presenter
    {
        private readonly string _value;
        public ItemPresenter(string value) => _value = value;
    }

    #region Inherited Classes
    public abstract class Presenter : INotifyPropertyChanged
    {
         public event PropertyChangedEventHandler PropertyChanged;

         protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
         {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
         }
    }
    #endregion
}