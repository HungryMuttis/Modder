using System.Runtime.InteropServices;

namespace Modder
{
    public static class Data
    {
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        public static string DefaultXml { get; } =
"""
<?xml version="1.0"?>
<!--The main configuration for "Modder"-->
<!--"DEFAULT" is a sumulated node at runtime having "PATH" and "HERE" nodes-->
<!--"PATH" is the folder where MODDER saves its files (saved in an enviorment variable "MODDER_PATH")-->
<!--"HERE" is the folder in which the EXE is located at runtime-->
<xml>
    <PATH>
        <Designs>{DEFAULT:PATH}Designs\</Designs>
        <Settings>{DEFAULT:PATH}Settings\</Settings>
        <Mods>{DEFAULT:PATH}Mods\</Mods>
        <ModData>{DEFAULT:PATH}ModData\</ModData>
        <ModLists>{PATH:ModData}ModLists\</ModLists>
        <Logs>{DEFAULT:PATH}Logs\</Logs>
    </PATH>
    <Logging>
        <WriteEnabled>true</WriteEnabled>
        <WriteToFile>true</WriteToFile>
        <WriteToTextBox>true</WriteToTextBox>
    </Logging>
    <Params>
        <Design>{DEFAULT:HERE}default-black.dll</Design>
        <ModList>{PATH:ModLists}default.xml</ModList>
    </Params>
</xml>
""";
        public static string Version { get; } = "1.0.0.1";
        public static uint VerstionINT { get; } = 1;
        public static short CMD_HIDE { get; } = 0;
        public static short CMD_SHOW { get; } = 5;
    }
}
