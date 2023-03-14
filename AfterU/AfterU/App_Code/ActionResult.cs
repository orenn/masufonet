using System;
using System.Collections.Generic;
using System.Web;

/// <summary>
/// Summary description for ActionResult
/// </summary>
public class ActionResult
{
    public ActionResult()
    { }

    public ActionResult(ActionResultType resultType)
    {
        this.ResultType = resultType;
        this.Description = resultType.ToString();
    }

    public ActionResult(ActionResultType resultType, string description)
    {
        this.ResultType = resultType;
        this.Description = description;
    }

    public ActionResultType ResultType { get; set; }
    public string Description { get; set; }
    public long Id { get; set; }
}

public enum ActionResultType
{
    Ok = 1,
    Inserted = 2,
    Updated = 4,
    Deleted = 8,

    Failed = -1,
    Duplicate = -2
}