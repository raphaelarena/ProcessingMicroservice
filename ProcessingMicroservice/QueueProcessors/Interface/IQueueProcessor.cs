using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessingMicroservice.QueueProcessors.Interface
{
    public interface IQueueProcessor
    {
        void ProcessSaveQueue();
        void ProcessUpdateQueue();
        void ProcessDeleteQueue();
    }
}
