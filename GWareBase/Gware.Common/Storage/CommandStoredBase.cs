﻿using Gware.Common.Storage.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Common.Storage
{
    public abstract class CommandStoredBase : LoadedFromAdapterBase
    {
        public abstract DataCommand CreateSaveCommand();
        public abstract DataCommand CreateDeleteCommand();
    }
}