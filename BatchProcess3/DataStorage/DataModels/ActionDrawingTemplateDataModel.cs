using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BatchProcess3.DrawingTemplates;

namespace BatchProcess3.DataStorage.DataModels;

[Table("ActionDrawingTemplate")]
public class ActionDrawingTemplateDataModel : ActionDataModel
{
    public DrawingTemplateOperation Operation { get; set; }

    [MaxLength(1000)]
    public string? CurrentTemplatePath { get; set; }

    [MaxLength(1000)]
    public string? NewTemplatePath { get; set; }
}