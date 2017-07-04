using System;

namespace OchUploader.ViewModel
{
    [AttributeUsage(AttributeTargets.Method)]
    public class LastActionAttribute: Attribute
    {
        public string Description { get; set; }

        public LastActionAttribute(string description)
        {
            Description = description;
        }
    }
}
