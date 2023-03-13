using System.ComponentModel.DataAnnotations.Schema;
using SunPortal.Communication.Parameters;

namespace SunPortal.Cloud.Service.Database.Data;

public class Parameter
{
    /// <summary>
    /// Coincides with Studer param number
    /// </summary>
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int ParameterId { get; set; }

    public string? Description { get; set; }

    public ParameterType Type { get; set; }
    public ParameterMode Mode { get; set; }
    public ParameterLevel Level { get; set; }

    public int ParameterGroupId { get; set; }
    public virtual ParameterGroup ParameterGroup { get; set; }

    public bool LogParameter { get; set; } = false; //TODO: useful?? 
}