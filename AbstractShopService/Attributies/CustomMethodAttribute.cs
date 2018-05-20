using System;

namespace AbstractShopService.Attributies
{
    [AttributeUsage(AttributeTargets.Method)]
    public class CustomMethodAttribute : Attribute
    {
        public CustomMethodAttribute(string descript)
        {
            Description = string.Format("Описание метода: ", descript);
        }

        public string Description { get; private set; }
    }
}