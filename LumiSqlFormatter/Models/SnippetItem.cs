using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace LumiSqlFormatter.Models
{
    [Serializable]
    [JsonObject(MemberSerialization.OptIn)]
    public class SnippetItem : System.ComponentModel.INotifyPropertyChanged
    {
        private string _id = Guid.NewGuid().ToString();
        private string _prefix = string.Empty;
        private string _description = string.Empty;
        private string _body = string.Empty;

        [field: NonSerialized]
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }

        [JsonProperty("id", Order = 1)]
        public string Id
        {
            get => _id;
            set
            {
                if (_id != value)
                {
                    _id = value;
                    OnPropertyChanged();
                }
            }
        }

        [JsonProperty("prefix", Order = 2)]
        public string Prefix
        {
            get => _prefix;
            set
            {
                if (_prefix != value)
                {
                    _prefix = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(Shortcut));
                }
            }
        }

        [JsonProperty("description", Order = 3)]
        public string Description
        {
            get => _description;
            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged();
                }
            }
        }

        [JsonProperty("body", Order = 4)]
        public string Body
        {
            get => _body;
            set
            {
                if (_body != value)
                {
                    _body = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(Template));
                }
            }
        }

        // Backward compatibility for reading older JSON files with "shortcut"
        [JsonProperty("shortcut", NullValueHandling = NullValueHandling.Ignore)]
        private string LegacyShortcut
        {
            set
            {
                if (string.IsNullOrEmpty(Prefix))
                    Prefix = value;
            }
        }

        // Backward compatibility for reading older JSON files with "template"
        [JsonProperty("template", NullValueHandling = NullValueHandling.Ignore)]
        private string LegacyTemplate
        {
            set
            {
                if (string.IsNullOrEmpty(Body))
                    Body = value;
            }
        }

        public bool ShouldSerializeLegacyShortcut() => false;
        public bool ShouldSerializeLegacyTemplate() => false;

        // Aliases for C# code & XAML bindings compatibility
        [JsonIgnore]
        public string Shortcut
        {
            get => Prefix;
            set => Prefix = value;
        }

        [JsonIgnore]
        public string Template
        {
            get => Body;
            set => Body = value;
        }

        public SnippetItem()
        {
            if (string.IsNullOrEmpty(Id))
            {
                Id = Guid.NewGuid().ToString();
            }
        }

        public SnippetItem(string prefix, string description, string body, string id = null)
        {
            Id = string.IsNullOrWhiteSpace(id) ? Guid.NewGuid().ToString() : id;
            Prefix = prefix ?? string.Empty;
            Description = description ?? string.Empty;
            Body = body ?? string.Empty;
        }

        [OnDeserialized]
        internal void OnDeserializedMethod(StreamingContext context)
        {
            if (string.IsNullOrWhiteSpace(Id))
            {
                Id = Guid.NewGuid().ToString();
            }
        }

        public SnippetItem Clone()
        {
            return new SnippetItem(Prefix, Description, Body, Id);
        }
    }
}
