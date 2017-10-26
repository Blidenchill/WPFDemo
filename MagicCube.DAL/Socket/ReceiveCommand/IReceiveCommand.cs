using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicCube.DAL
{
    public interface IReceiveCommand<T>
    {
         Task CommandCallback(T data);
    }
}
