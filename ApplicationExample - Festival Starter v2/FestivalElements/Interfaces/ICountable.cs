using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FestivalElements.Interfaces
{
    interface ICountable
    {
        long NumberOfElement
        {
            get;
        }
        long NextElementID
        {
            get;
        }
        void ResetElementCount();
    }
}
