using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce_MVC_Core.Code;

namespace Ecommerce_MVC_Core.BootstrapModal
{
    public class BootstrapModel
    {
        public string ID { get; set; }
        public string AreaLabeledId { get; set; }
        public ModalSize Size { get; set; }
        public string Message { get; set; }
        public string ModalSizeClass
        {
            get
            {
                switch (this.Size)
                {
                    case ModalSize.Small:
                        return "modal-sm";
                    case ModalSize.Large:
                        return "modal-lg";
                    case ModalSize.Medium:
                    default:
                        return "";
                }
            }
        }
    }
}
