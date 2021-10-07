using System;
using System.IO;

namespace Dominator.Domain.Classes.Profiles
{
    public class ProfilesPathProvider
    {
        #region Properties & Consts
        public const string FOLDER_NAME = @"Alienware\OCControls\OCProfiles";
        public const string PREDEFINED_EXTENSION = "opp";
        public const string CUSTOM_EXTENSION = "ocp";
        public static readonly string STAGE1_PREDEFINED_PROFILENAME = "OC1";
        public static readonly string STAGE2_PREDEFINED_PROFILENAME = "OC2";
        public string ProfilesPath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), FOLDER_NAME);

        public string getPredefinedProfileExtension()
        {
            return PREDEFINED_EXTENSION;
        }

        public string getCustomProfileExtension()
        {
            return CUSTOM_EXTENSION;
        }
        #endregion
    }
}
