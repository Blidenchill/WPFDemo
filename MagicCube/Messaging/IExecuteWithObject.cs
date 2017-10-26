using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicCube.Messaging
{
    public interface IExecuteWithObject
    {
        object Target
        {
            get;
        }
        void ExecuteWithObject(object parameter);
        void MarkForDeletion();
    }
}
