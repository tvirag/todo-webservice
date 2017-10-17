using System;
using System.Collections.Generic;
using System.Text;

namespace Todo.Interfaces
{
    public interface IDataContext: IDisposable
    {
        void Save();
    }
}
