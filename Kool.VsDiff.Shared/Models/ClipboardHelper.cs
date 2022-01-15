using System.Windows;

namespace Kool.VsDiff.Models;

internal static class ClipboardHelper
{
    public static bool TryGetClipboardText(out string text)
    {
        text = Clipboard.GetText();
        return text?.Length > 0;
    }
}