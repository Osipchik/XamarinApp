﻿using Xamarin.Forms;

namespace Labs.Helpers
{
    public static class Constants
    {
        public const string TestFolder = "Test";
        public const string TempFolder = "Temp";
        public const string TestName = "Test_";
        public const string TestTypeCheck = "Check";
        public const string TestTypeEntry = "Entry";
        public const string TestTypeStack = "Stack";
        public const string SettingsFileTxt = "settings.txt";

        public const string UploadTitles = "UploadTitles";
        public const string RunFirstTimer = "RunFirstTimer";
        public const string StopAllTimers = "StopAllTimers";
        public const string TimerIsEnd = "TimerIsEnd";
        public const string ReturnPages = "ReturnPages";
        public const string TimeZero = "00:00:00";

        public const int SymbolLength = 11;
        public const uint AnimationRate = 16;
        public const uint AnimationLength = 700;

        public const string Check = "check";
        public static class Colors
        {
            public static readonly Color ColorMaterialBlue = Color.FromHex("#03A9F4");
            public static readonly Color ColorMaterialRed = Color.FromHex("#f44336");
            public static readonly Color ColorMaterialBlueGray = Color.FromHex("#607D8B");
            public static readonly Color ColorMaterialGray = Color.FromHex("#B0BEC5");
            public static readonly Color ColorMaterialGrayText = Color.FromHex("##9E9E9E");
            public static readonly Color ColorMaterialGreen = Color.FromHex("#4CAF50");
        }
    }
}
