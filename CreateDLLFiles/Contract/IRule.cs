using System.ComponentModel;

namespace Contract
{
    public interface IRule : ICloneable, INotifyPropertyChanged
    {
        string Rename(string origin);
        IRule? Parse(string data);
        string Name { get; }
        string Label { get; }
        string textPreset { get; }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}