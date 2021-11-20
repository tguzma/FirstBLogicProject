using FirstBLogicProject.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirstBLogicProject.Models.ViewModels
{
    public class IndexViewModel
    {
        public IList<ContractItem> contractItems;
        public IList<AdvisorItem> advisorClientItems;
    }
}
