[![Build Status](https://dev.azure.com/heku/vsix/_apis/build/status/heku.Kool.VsDiff?branchName=main)](https://dev.azure.com/heku/vsix/_build/latest?definitionId=17&branchName=main)
<br>
[![2019 marketplace](https://img.shields.io/visual-studio-marketplace/v/heku.vsdiff.svg?label=2019-Marketplace)](https://marketplace.visualstudio.com/items?itemName=heku.vsdiff)
[![2019 downloads](https://img.shields.io/visual-studio-marketplace/d/heku.vsdiff.svg?label=2019-Downloads)](https://marketplace.visualstudio.com/items?itemName=heku.vsdiff)
<br>
[![2022 marketplace](https://img.shields.io/visual-studio-marketplace/v/heku.vsdiff2022.svg?label=2022-Marketplace)](https://marketplace.visualstudio.com/items?itemName=heku.vsdiff2022)
[![2022 downloads](https://img.shields.io/visual-studio-marketplace/d/heku.vsdiff2022.svg?label=2022-Downloads)](https://marketplace.visualstudio.com/items?itemName=heku.vsdiff2022)

--------

Another open source Visual Studio extension to make file/code comparison easier.

You can install it via Visual Studio 2015/2017/2019/2022 'Extensions' or download it from
- [Marketplace for VS2022](https://marketplace.visualstudio.com/items?itemName=heku.VsDiff2022).
- [Marketplace for VS2019 and below](https://marketplace.visualstudio.com/items?itemName=heku.VsDiff).


### Kind Reminder

This extension was developed because the [official one](https://github.com/madskristensen/FileDiffer) was missing many features I need at that time, and I also wanted to learn
how to write a Visual Studio extension. Since the official one has added almost all the missing features, I recommend everyone to give preference to that extension and use this
extension only when the official one does not work for you. Refer to blog [comparing files in visual studio](https://devblogs.microsoft.com/visualstudio/comparing-files-in-visual-studio).


## Features

- Compare two selected files in Solution Explorer.

    ![CompareSelectedFiles.png](Screenshots/CompareSelectedFiles.png)

- Compare the selected file with Clipboard content.

    ![CompareSelectedFileWithClipboard.png](Screenshots/CompareSelectedFileWithClipboard.png)

- Compare the selected code with Clipboard content.

    ![CompareSelectedCodeWithClipboard.png](Screenshots/CompareSelectedCodeWithClipboard.png)

- Compare the active document with Clipboard content.

    ![CompareActiveDocumentWithClipboard.png](Screenshots/CompareActiveDocumentWithClipboard.png)

## Configurable

![Configuration.png](Screenshots/Configuration.png)

## Thanks

Before and during my development, I referred the following projects and documents,
and I am very grateful to these authors who have done a great job.

- [Clipboard Diff](https://github.com/einaregilsson/ClipboardDiff)
- [CodeMaid](https://github.com/codecadwallader/codemaid)
- [File Differ](https://github.com/madskristensen/FileDiffer)
- [Git Diff Margin](https://github.com/laurentkempe/GitDiffMargin)
- [Microsoft Docs](https://docs.microsoft.com/en-us/visualstudio/extensibility/)
- [VS.DiffAllFiles](https://github.com/deadlydog/VS.DiffAllFiles)


## License

- [MIT](LICENSE)


---------

I'm not a native English speaker, and I would appreciate it if you could correct any of my English mistakes.