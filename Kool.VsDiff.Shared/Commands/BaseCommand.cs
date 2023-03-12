using Microsoft.VisualStudio.Shell;
using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using static Kool.VsDiff.Models.VS;
using static Kool.VsDiff.Package;

namespace Kool.VsDiff.Commands;

internal abstract class BaseCommand : OleMenuCommand
{
    protected BaseCommand(int cmdId) : base(OnBaseCommandEventHandler, null, OnBaseBeforeQueryStatus, new CommandID(Guid.Parse(Ids.CMD_SET), cmdId))
    {
    }

    private static void OnBaseBeforeQueryStatus(object sender, EventArgs e)
    {
        var command = sender as BaseCommand;

        Debug.WriteLine($"Command {command.GetType().Name} OnBeforeQueryStatus.");
        try
        {
            command.OnBeforeQueryStatus();
        }
        catch (Exception ex)
        {
            Debug.Fail(ex.Message);
        }
    }

    private static void OnBaseCommandEventHandler(object sender, EventArgs e)
    {
        var command = sender as BaseCommand;

        Debug.WriteLine($"Execute command {command.GetType().Name}.");

        try
        {
            command.OnExecute();
        }
        catch (Exception ex)
        {
            MessageBox.Error(VSPackage.ErrorMessageTitle, ex.Message);
        }
    }

    protected virtual void OnBeforeQueryStatus()
    {
    }

    protected abstract void OnExecute();

    public void Turn(bool featureOn)
    {
        if (!featureOn)
        {
            CommandService.RemoveCommand(this);
            return;
        }

        if (featureOn && CommandService.FindCommand(CommandID) == null)
        {
            CommandService.AddCommand(this);
        }
    }
}