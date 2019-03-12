namespace Modules.UI.Models.Views.Project
{
    public abstract class ProjectSettingsModelBase
    {
        private bool _read;

        public bool CanRead
        {
            get { return _read || CanWrite; }
            set { _read = value; }
        }

        public bool CanWrite { get; set; }
    }
}