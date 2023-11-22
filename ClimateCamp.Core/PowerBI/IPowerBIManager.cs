using ClimateCamp.PowerBI.Models;
using System;

namespace ClimateCamp.PowerBI
{
    public interface IPowerBIManager
    {
        EmbedParams GetEmbedParams(Guid workspaceId, Guid reportId);
    }
}
