using Common;
using Common.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [ServiceContract]
    public interface IFileTransport
    {
        [OperationContract]
        bool ParseFile(FileManipulationOptions options,bool isForecast, out List<Audit> greske);


        [OperationContract]
        void Calculate();
    }
}