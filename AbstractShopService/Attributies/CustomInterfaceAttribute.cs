using System;

namespace AbstractShopService.Attributies
{
    [AttributeUsage(AttributeTargets.Interface)]
    public class CustomInterfaceAttribute : Attribute
    {
        public CustomInterfaceAttribute(string descript)
        {
            Description = string.Format("Описание инетфейса: ", descript);
        }

        public string Description { get; private set; }
    }
}