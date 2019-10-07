using System;
using System.Collections;
using System.Linq;

namespace MG.Settings.Json
{
    public delegate void SettingsChangedEventHandler(object sender, SettingsChangedEventArgs e);

    public class SettingsChangedEventArgs : EventArgs
    {
        public SettingChangedAction Action { get; }
        public IList NewItems { get; }
        public IJsonSettings NewSettings { get; }
        public object NewValue { get; }
        public IList OldItems { get; }
        public IJsonSettings OldSettings { get; }
        public object OldValue { get; }
        public string PropertyName { get; }

        public SettingsChangedEventArgs(SettingChangedAction action) => this.Action = action;
        public SettingsChangedEventArgs(SettingChangedAction action, string propertyName, object oldValue, object newValue)
            : this(action)
        {
            this.NewValue = newValue;
            this.OldValue = oldValue;
        }
        public SettingsChangedEventArgs(SettingChangedAction action, IList newItems, IList oldItems)
            : this(action)
        {
            this.NewItems = newItems;
            this.OldItems = oldItems;
        }
        public SettingsChangedEventArgs(SettingChangedAction action, IJsonSettings newSettings, IJsonSettings oldSettings)
            : this(action)
        {
            this.NewSettings = newSettings;
            this.OldSettings = oldSettings;
        }
    }

    public enum SettingChangedAction
    {
        Add,
        Remove,
        Replace,
        Save,
        Read,
        Reload
    }
}