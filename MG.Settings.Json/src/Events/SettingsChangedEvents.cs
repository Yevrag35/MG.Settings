using System;
using System.Collections;
using System.Linq;

namespace MG.Settings.Json
{
    /// <summary>
    /// An delegate to register for handling events when the <see cref="JsonSettingsManager"/> has it settings altered.
    /// </summary>
    /// <param name="sender">The <see cref="JsonSettingsManager"/> instance which experienced the alteration.</param>
    /// <param name="e">The event arguments associated with the triggered event.</param>
    public delegate void SettingsChangedEventHandler(object sender, SettingsChangedEventArgs e);

    /// <summary>
    /// A class representing the event data data stored when a setting is altered.
    /// </summary>
    public class SettingsChangedEventArgs : EventArgs
    {
        /// <summary>
        /// The action that was performed on the setting.
        /// </summary>
        public SettingChangedAction Action { get; }
        /// <summary>
        /// A list of settings and their values that were added or updated.
        /// </summary>
        public IList NewItems { get; }
        /// <summary>
        /// The whole instance of the new settings.
        /// </summary>
        public IJsonSettings NewSettings { get; }
        /// <summary>
        /// The replacement value for an altered setting.
        /// </summary>
        public object NewValue { get; }
        /// <summary>
        /// A list of settings and their values that were overwritten or removed.
        /// </summary>
        public IList OldItems { get; }
        /// <summary>
        /// The whole instance of the old settings.
        /// </summary>
        public IJsonSettings OldSettings { get; }
        /// <summary>
        /// The overwritten value for an altered setting.
        /// </summary>
        public object OldValue { get; }
        /// <summary>
        /// The name of the setting or property that was altered.
        /// </summary>
        public string PropertyName { get; }

        /// <summary>
        /// The default constructor specifying the action that took place.
        /// </summary>
        /// <param name="action">The specific action that triggered the event.</param>
        public SettingsChangedEventArgs(SettingChangedAction action) => this.Action = action;
        /// <summary>
        /// Initializes a <see cref="SettingsChangedEventArgs"/> instance with the specific action that altered
        /// the given property/setting, also specifying its old and new values.
        /// </summary>
        /// <param name="action">The specific action that triggered the event.</param>
        /// <param name="propertyName">The name of the setting or property that was altered.</param>
        /// <param name="oldValue">The setting's old value.</param>
        /// <param name="newValue">The setting's new value.</param>
        public SettingsChangedEventArgs(SettingChangedAction action, string propertyName, object oldValue, object newValue)
            : this(action)
        {
            this.NewValue = newValue;
            this.OldValue = oldValue;
        }
        /// <summary>
        /// Initializes a <see cref="SettingsChangedEventArgs"/> instance with the specific action with the 
        /// old settings and new settings.
        /// </summary>
        /// <param name="action">The specific action that triggered the event.</param>
        /// <param name="newItems">The list of settings and their values that were added or updated.</param>
        /// <param name="oldItems">The list of settings and their values that were overwritten or removed.</param>
        public SettingsChangedEventArgs(SettingChangedAction action, IList newItems, IList oldItems)
            : this(action)
        {
            this.NewItems = newItems;
            this.OldItems = oldItems;
        }
        /// <summary>
        /// Initializes a <see cref="SettingsChangedEventArgs"/> instance with the specific action.
        /// An entire copy of the old and new settings is also specified.
        /// </summary>
        /// <param name="action">The specific action that triggered the event.</param>
        /// <param name="newSettings">An object representing the new applied settings.</param>
        /// <param name="oldSettings">An object representing the old settings that were overwritten.</param>
        public SettingsChangedEventArgs(SettingChangedAction action, IJsonSettings newSettings, IJsonSettings oldSettings)
            : this(action)
        {
            this.NewSettings = newSettings;
            this.OldSettings = oldSettings;
        }
    }

    /// <summary>
    /// An enumeration for different types of alteration methods.
    /// </summary>
    public enum SettingChangedAction
    {
        /// <summary>
        /// A setting was added to the manager.
        /// </summary>
        Add,

        /// <summary>
        /// A setting was removed from the manager.
        /// </summary>
        Remove,

        /// <summary>
        /// A setting had its value updated.
        /// </summary>
        Replace,

        /// <summary>
        /// The manager had the current settings written to the file system.
        /// </summary>
        Save,

        /// <summary>
        /// The manager read the initial settings from the file system.
        /// </summary>
        Read,

        /// <summary>
        /// The manager reloaded its settings from the file system.
        /// </summary>
        Reload
    }
}